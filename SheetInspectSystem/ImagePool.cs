using System;
using System.Runtime.InteropServices;
using HalconDotNet;

namespace HFVisionInspect
{
    /// <summary>
    /// 事前確保した HImage を使い回すプール（モノクロ専用）。
    ///
    /// 目的:
    ///   フレームごとに GenImage1 で HImage を新規作成すると、大きい画像（8192x2000 以上）の
    ///   連続取込でメモリが増え続ける現象があったため、HALCON の画像メモリ確保を
    ///   「オープン時の1回だけ」にし、取込のたびにその中身へ画素データをコピーする方式にする。
    ///
    /// 注意:
    ///   ・プールの HImage は Dispose しないこと（本クラスが管理）
    ///   ・スロット数ぶん取り込むと同じ HImage が上書きされる
    ///     → 長く保持したい・保存したい場合は CopyImage() で複製してから使う
    /// </summary>
    internal sealed class ImagePool : IDisposable
    {
        // 画素データのコピー用（Win32 API）
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, UIntPtr count);

        private readonly HImage[] _images;     // 事前確保した HImage
        private readonly IntPtr[] _pointers;   // 各 HImage の画素データ先頭アドレス
        private int _next;                     // 次に書き込むスロット番号
        private bool _disposed;

        /// <summary>
        /// プールを生成します（オープン時に1回だけ呼ぶ）。
        /// </summary>
        /// <param name="slotCount">スロット数（表示中・処理中の画像が上書きされないよう 4 以上を推奨）</param>
        /// <param name="width">横幅[画素]</param>
        /// <param name="height">行数</param>
        /// <param name="bitCount">ビット幅（8 → byte、10～16 → uint2）</param>
        public ImagePool(int slotCount, int width, int height, int bitCount)
        {
            if (slotCount < 2) slotCount = 2;
            if (width <= 0 || height <= 0) throw new ArgumentException($"画像サイズが不正です。({width}x{height})");

            // --- ビット幅から HALCON の画像型と 1 画素バイト数を決定 ---
            string halconType;
            if (bitCount <= 8) { halconType = "byte"; BytesPerPixel = 1; }
            else if (bitCount <= 16) { halconType = "uint2"; BytesPerPixel = 2; }
            else throw new NotSupportedException($"ImagePool はモノクロ専用です。(BitCount={bitCount})");

            Width = width;
            Height = height;
            SlotCount = slotCount;

            _images = new HImage[slotCount];
            _pointers = new IntPtr[slotCount];

            try
            {
                for (int i = 0; i < slotCount; i++)
                {
                    // 指定サイズの空画像を作成（HALCON の画像メモリ確保はここだけ）
                    _images[i] = new HImage();
                    _images[i].GenImageConst(halconType, width, height);

                    // 画素データの先頭アドレスを取得して保持（毎フレーム取得しない）
                    string type;
                    int w, h;
                    _pointers[i] = _images[i].GetImagePointer1(out type, out w, out h);
                }
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        /// <summary>横幅[画素]</summary>
        public int Width { get; }

        /// <summary>行数</summary>
        public int Height { get; }

        /// <summary>1 画素のバイト数</summary>
        public int BytesPerPixel { get; }

        /// <summary>スロット数</summary>
        public int SlotCount { get; }

        /// <summary>
        /// DMA バッファの画素データを次のスロットの HImage へコピーし、その HImage を返します。
        /// 返した HImage は Dispose しないこと。
        /// </summary>
        /// <param name="address">DMA バッファの先頭アドレス（FrameEndEventArgs.Address）</param>
        /// <param name="stride">DMA バッファの 1 行のバイト数</param>
        public HImage Fill(IntPtr address, int stride)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ImagePool));
            if (address == IntPtr.Zero) throw new ArgumentException("バッファアドレスが NULL です。", nameof(address));

            // 次のスロットを選ぶ（リング状に使い回す）
            int slot = _next;
            _next = (_next + 1) % SlotCount;

            // プール画像が外部で Dispose されていないか確認
            // （Dispose されると画像メモリが解放され、ここでの書き込みが不正アクセスになるため）
            if (!_images[slot].IsInitialized())
            {
                throw new InvalidOperationException(
                    $"プール画像(スロット{slot})が外部で Dispose されています。IsPooled=true の画像は Dispose しないでください。");
            }

            IntPtr dst = _pointers[slot];
            int packedStride = Width * BytesPerPixel;   // HALCON 画像は行間パディングなし
            if (stride <= 0) stride = packedStride;

            if (stride == packedStride)
            {
                // パディングなし：一括コピー
                CopyMemory(dst, address, new UIntPtr((ulong)((long)packedStride * Height)));
            }
            else
            {
                // パディングあり：1 行ずつ詰めてコピー
                for (int y = 0; y < Height; y++)
                {
                    IntPtr src = new IntPtr(address.ToInt64() + (long)y * stride);
                    IntPtr d = new IntPtr(dst.ToInt64() + (long)y * packedStride);
                    CopyMemory(d, src, new UIntPtr((uint)packedStride));
                }
            }

            return _images[slot];
        }

        /// <summary>全ての HImage を解放します。</summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            if (_images == null) return;
            foreach (HImage img in _images) img?.Dispose();
        }
    }
}
