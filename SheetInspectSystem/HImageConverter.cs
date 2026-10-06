using System;
using System.Runtime.InteropServices;
using HalconDotNet;

namespace HFVisionInspect
{
    /// <summary>
    /// AcapLibPlus の画像バッファ（アンマネージドメモリ）から HALCON の HImage を生成するユーティリティ。
    /// GenImage1 / GenImageInterleaved は画素データを HALCON 側へ「コピー」するため、
    /// 生成後はリングバッファが次フレームで上書きされても HImage の内容は保持されます。
    /// </summary>
    internal static class HImageConverter
    {
        /// <summary>24bit カラー時の画素並び順（実機で色が入れ替わる場合は "rgb" に変更）</summary>
        public static string ColorOrder24 = "bgr";

        /// <summary>32bit カラー時の画素並び順（実機で色が入れ替わる場合は "rgbx" に変更）</summary>
        public static string ColorOrder32 = "bgrx";

        // 行単位のメモリコピー用（Win32 API）
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, UIntPtr count);

        /// <summary>
        /// 画像バッファから HImage を生成します。
        /// </summary>
        /// <param name="address">バッファ先頭アドレス（FrameEndEventArgs.Address）</param>
        /// <param name="width">横幅[画素]</param>
        /// <param name="height">行数</param>
        /// <param name="bitCount">1画素のビット幅（8 / 10～16 / 24 / 32）</param>
        /// <param name="stride">1行のバイト数（GetBufferStride で取得した値）</param>
        /// <returns>生成した HImage（呼び出し側で Dispose すること）</returns>
        public static HImage FromBuffer(IntPtr address, int width, int height, int bitCount, int stride)
        {
            if (address == IntPtr.Zero) throw new ArgumentException("バッファアドレスが NULL です。", nameof(address));
            if (width <= 0 || height <= 0) throw new ArgumentException($"画像サイズが不正です。({width}x{height})");

            // 1画素あたりのバイト数
            int bytesPerPixel = GetBytesPerPixel(bitCount);
            // パディングなしの1行バイト数
            int packedStride = width * bytesPerPixel;

            // ストライド未取得の場合はパディングなしとみなす
            if (stride <= 0) stride = packedStride;

            // ----- ストライドが詰まっている場合：そのまま HALCON に渡す（1回のコピーで済む） -----
            if (stride == packedStride)
            {
                return CreateImage(address, width, height, bytesPerPixel);
            }

            // ----- ストライドにパディングがある場合：一旦詰め直してから HALCON に渡す -----
            long packedSize = (long)packedStride * height;
            IntPtr temp = Marshal.AllocHGlobal(new IntPtr(packedSize));
            try
            {
                for (int y = 0; y < height; y++)
                {
                    IntPtr src = new IntPtr(address.ToInt64() + (long)y * stride);
                    IntPtr dst = new IntPtr(temp.ToInt64() + (long)y * packedStride);
                    CopyMemory(dst, src, new UIntPtr((uint)packedStride));
                }
                return CreateImage(temp, width, height, bytesPerPixel);
            }
            finally
            {
                // HALCON 側にコピー済みなので一時バッファは即解放してよい
                Marshal.FreeHGlobal(temp);
            }
        }

        /// <summary>
        /// 【メモリ増加対策版】画像バッファを一旦「作業用バッファ」へコピーしてから HImage を生成します。
        ///
        /// ボードの DMA バッファを直接 GenImage1 / GenImageInterleaved に渡すと、
        /// 連続取込時にプロセスのメモリ（Private）が増え続ける現象を確認したため、
        /// 「DMA バッファ → 自前メモリへコピー」「自前メモリ → HImage 生成」の2段階に分けています。
        /// （どちらの処理も単独ではメモリが増えないことを切り分けで確認済み）
        /// </summary>
        /// <param name="address">バッファ先頭アドレス（FrameEndEventArgs.Address）</param>
        /// <param name="width">横幅[画素]</param>
        /// <param name="height">行数</param>
        /// <param name="bitCount">1画素のビット幅（8 / 10～16 / 24 / 32）</param>
        /// <param name="stride">DMA バッファの1行のバイト数（GetBufferStride で取得した値）</param>
        /// <param name="staging">作業用バッファ（呼び出し側で確保し、フレーム間で使い回す）</param>
        /// <returns>生成した HImage（呼び出し側で Dispose すること）</returns>
        public static HImage FromBufferViaStaging(IntPtr address, int width, int height, int bitCount,
                                                  int stride, StagingBuffer staging)
        {
            if (address == IntPtr.Zero) throw new ArgumentException("バッファアドレスが NULL です。", nameof(address));
            if (width <= 0 || height <= 0) throw new ArgumentException($"画像サイズが不正です。({width}x{height})");
            if (staging == null) throw new ArgumentNullException(nameof(staging));

            // 1画素あたりのバイト数と、パディングなしの1行バイト数
            int bytesPerPixel = GetBytesPerPixel(bitCount);
            int packedStride = width * bytesPerPixel;
            long packedSize = (long)packedStride * height;

            // ストライド未取得の場合はパディングなしとみなす
            if (stride <= 0) stride = packedStride;

            // 作業用バッファのサイズ確認（足りなければ確保し直す：通常は初回のみ）
            staging.EnsureSize(packedSize);

            // ----- 1段目：DMA バッファ → 作業用バッファ（HALCON は使わない） -----
            if (stride == packedStride)
            {
                // パディングなし：一括コピー
                CopyMemory(staging.Pointer, address, new UIntPtr((ulong)packedSize));
            }
            else
            {
                // パディングあり：1行ずつ詰めてコピー
                for (int y = 0; y < height; y++)
                {
                    IntPtr src = new IntPtr(address.ToInt64() + (long)y * stride);
                    IntPtr dst = new IntPtr(staging.Pointer.ToInt64() + (long)y * packedStride);
                    CopyMemory(dst, src, new UIntPtr((uint)packedStride));
                }
            }

            // ----- 2段目：作業用バッファ → HImage（ボードのバッファは読まない） -----
            return CreateImage(staging.Pointer, width, height, bytesPerPixel);
        }

        /// <summary>ビット幅から1画素のバイト数を求めます。</summary>
        private static int GetBytesPerPixel(int bitCount)
        {
            if (bitCount <= 8) return 1;   // 8bit モノクロ
            if (bitCount <= 16) return 2;  // 10/12/14/16bit モノクロ（2バイト格納）
            if (bitCount == 24) return 3;  // 24bit カラー
            if (bitCount == 32) return 4;  // 32bit カラー
            throw new NotSupportedException($"未対応のビット幅です。(BitCount={bitCount})");
        }

        /// <summary>詰まったメモリから HImage を生成します（データは HALCON 側へコピーされる）。</summary>
        private static HImage CreateImage(IntPtr pixels, int width, int height, int bytesPerPixel)
        {
            HImage image = new HImage();
            try
            {
                switch (bytesPerPixel)
                {
                    case 1:
                        // 8bit モノクロ → byte 画像
                        image.GenImage1("byte", width, height, pixels);
                        break;
                    case 2:
                        // 10～16bit モノクロ → uint2 画像
                        image.GenImage1("uint2", width, height, pixels);
                        break;
                    case 3:
                        // 24bit カラー → 3チャンネル byte 画像
                        image.GenImageInterleaved(pixels, ColorOrder24, width, height, -1,
                                                  "byte", width, height, 0, 0, -1, 0);
                        break;
                    case 4:
                        // 32bit カラー → 3チャンネル byte 画像（x は捨てる）
                        image.GenImageInterleaved(pixels, ColorOrder32, width, height, -1,
                                                  "byte", width, height, 0, 0, -1, 0);
                        break;
                }
                return image;
            }
            catch
            {
                // 生成に失敗した場合は確保済みオブジェクトを解放してから再スロー
                image.Dispose();
                throw;
            }
        }
    }

    /// <summary>
    /// HImage 生成前に画像データを一時的に置く作業用バッファ（アンマネージドメモリ）。
    /// フレームごとに確保・解放せず、チャンネルごとに1つを使い回します。
    /// ※スレッドセーフではありません。同時に1スレッドからのみ使用すること。
    /// </summary>
    internal sealed class StagingBuffer : IDisposable
    {
        private IntPtr _ptr = IntPtr.Zero;   // 確保したメモリの先頭アドレス
        private long _size;                  // 確保済みサイズ[byte]

        /// <summary>バッファの先頭アドレス</summary>
        public IntPtr Pointer => _ptr;

        /// <summary>確保済みサイズ[byte]</summary>
        public long Size => _size;

        /// <summary>
        /// 指定サイズ以上のメモリを確保します（既に足りていれば何もしない）。
        /// </summary>
        public void EnsureSize(long size)
        {
            if (size <= _size && _ptr != IntPtr.Zero) return;

            // サイズ不足なら解放して確保し直す
            Free();
            _ptr = Marshal.AllocHGlobal(new IntPtr(size));
            _size = size;
        }

        /// <summary>メモリを解放します。</summary>
        public void Dispose()
        {
            Free();
        }

        private void Free()
        {
            if (_ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_ptr);
                _ptr = IntPtr.Zero;
                _size = 0;
            }
        }
    }
}
