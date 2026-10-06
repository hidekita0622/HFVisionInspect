// ============================================================================
// 中空糸外観検査 ルールベース処理（HDevelopサンプルからのC#移植版）
//   元コード : 中空糸外観検査_ルールベース_HALCON24.11_sample.hdev
//   対象     : HALCON Progress 24.11 / 8192 x 2040 / 12本固定分割
//   方針     : HOperatorSet は使用せず、HImage / HRegion / HFunction1D の
//              オブジェクト指向APIと C# 配列処理で実装する。
//   範囲     : 固定分割、輝度正規化、糸二値化、糸揺れ補正、
//              エッジ判定、糸内部欠陥二値化、計測時間
//   除外     : Deep Learning分類、装置I/O、非同期CSV/画像保存
//   注意     : しきい値は仮値。実画像評価でレシピ値を確定すること。
//
// 【HDevからの主な変更点（処理結果は同等）】
//   1. 行ごとの tuple_find(O(行数×ラン数)) を、ランを1回走査する方式(O(ラン数))に変更。
//   2. 揺れ補正Map（gen_image_surface_first_order / add_image / max_image /
//      min_image 等）は C# の float 配列で直接生成し、HImage 化は1回だけにした。
//   3. 行Map（行方向の参照座標）は糸幅ごとにキャッシュして再利用する。
//   4. 使用後の HImage / HRegion は必ず Dispose する（メモリリーク防止）。
//
// 【使い方の例（WinForms）】
//   // 起動時に1回だけ（HDevの set_system と同じ）
//   HSystem.SetSystem("store_empty_region", "true");
//
//   using (var inspector = new HollowFiberRuleInspector(new RuleRecipe()))
//   using (var image = new HImage(@"C:\data\241011_050957_0000201679_&Cam1Img.bmp"))
//   {
//       RuleInspectionResult result = inspector.Inspect(image);
//       foreach (var y in result.Yarns) { Debug.WriteLine(y.ToCsvLine("xxx.bmp", result.TotalMs)); }
//   }
// ============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using HalconDotNet;

namespace HFVisionInspect
{
    // ------------------------------------------------------------------------
    // レシピ（検査パラメータ）
    //   HDevの「入力と固定条件」「レシピ仮値」に対応。
    // ------------------------------------------------------------------------
    public class RuleRecipe
    {
        // ---- 入力と固定条件 ----
        public int InputWidth { get; set; } = 8192;     // 入力画像の幅
        public int InputHeight { get; set; } = 2048;    // 入力画像の高さ
        public int YarnCount { get; set; } = 12;        // 固定分割本数

        // ---- サンプル画像用の分割位置調整（HDevの OffsetCol / ColStart > 6800 に対応）----
        public int OffsetCol { get; set; } = 616;       // 分割開始位置の横オフセット

        public int firstYarnX { get; set; } = 890;
        public int lastYarnX { get; set; } = 6990;
        public int ColStartLimit { get; set; } = 6800;  // これを超える開始列の糸は処理しない

        // ---- 輝度正規化 ----
        public bool NormalizeEnable { get; set; } = true;
        public double TargetGray { get; set; } = 180.0; // 正規化後の糸平均輝度の目標値

        // ---- 糸抽出二値化 ----
        public double YarnGrayMin { get; set; } = 50;
        public double YarnGrayMax { get; set; } = 255;

        // ---- 糸揺れ平滑化 ----
        public int MovingAverageRows { get; set; } = 31; // 移動平均の行数

        // ---- エッジ判定 ----
        public double WidthMin { get; set; } = 50.0;          // 平均糸幅の下限
        public double WidthMax { get; set; } = 160.0;         // 平均糸幅の上限
        public double WidthDeviationMax { get; set; } = 8.0;  // 糸幅の標準偏差の上限
        public double SymmetryTolerance { get; set; } = 8.0;  // 左右対称誤差の上限
        public int MissingRowMax { get; set; } = 20;          // 連続欠落行数の上限

        // ---- 糸内部欠陥 ----
        public int TopDeadBand { get; set; } = 20;      // 上側不感帯（行）
        public int BottomDeadBand { get; set; } = 20;   // 下側不感帯（行）
        public int BorderMargin { get; set; } = 4;      // 糸エッジから除外する幅（片側）
        public double DefectGrayMin { get; set; } = 0;
        public double DefectGrayMax { get; set; } = 80;
        public double DefectAreaMin { get; set; } = 3.0;
        public double DefectAreaMax { get; set; } = 2000.0;
    }

    // ------------------------------------------------------------------------
    // 糸1本分の検査結果（HDevの Result*** タプル1要素分に対応）
    // ------------------------------------------------------------------------
    public class YarnRuleResult
    {
        public int YarnNo { get; set; }                 // 糸番号（1始まり）
        public bool EdgeOK { get; set; }                // エッジ判定
        public bool InternalDefectOK { get; set; }      // 内部欠陥判定
        public bool RuleOK { get; set; }                // 総合判定（Edge && Internal）
        public double MeanWidth { get; set; }           // 補正後の平均糸幅
        public double WidthDeviation { get; set; }      // 補正後の糸幅標準偏差
        public double MaxSymmetryError { get; set; }    // 最大左右対称誤差
        public int MaxMissingRows { get; set; }         // 最大連続欠落行数（補正前後の大きい方）
        public int DefectCount { get; set; }            // 欠陥個数
        public double DefectAreaSum { get; set; }       // 欠陥面積合計
        public double SwayMs { get; set; }              // 揺れ補正までの処理時間[ms]
        public double RuleMs { get; set; }              // 判定処理時間[ms]

        // 欠陥位置（糸画像座標系）。表示やNG画像保存で使う想定。
        public double[] DefectRows { get; set; } = new double[0];
        public double[] DefectColumns { get; set; } = new double[0];
        public double[] DefectAreas { get; set; } = new double[0];
        public HRegion[] DefectRegion { get; set; } = new HRegion[0];

		// 分割位置（元画像の列範囲）。欠陥位置を元画像座標へ戻すときに使う。
		public int ColStart { get; set; }
        public int ColEnd { get; set; }

        /// <summary>
        /// 本番CSVの1行を生成する。
        /// 列: Filename,YarnNo,EdgeResult,InternalDefectResult,RuleResult,
        ///     MeanWidth,WidthDeviation,MaxSymmetryError,MaxMissingRows,
        ///     DefectCount,DefectAreaSum,SwayMs,RuleMs,TotalMs
        /// </summary>
        public string ToCsvLine(string fileName, double totalMs)
        {
            CultureInfo ci = CultureInfo.InvariantCulture; // 小数点を常に '.' にする
            return string.Join(",",
                fileName,
                YarnNo.ToString(ci),
                EdgeOK ? "OK" : "NG",
                InternalDefectOK ? "OK" : "NG",
                RuleOK ? "OK" : "NG",
                MeanWidth.ToString("F3", ci),
                WidthDeviation.ToString("F3", ci),
                MaxSymmetryError.ToString("F3", ci),
                MaxMissingRows.ToString(ci),
                DefectCount.ToString(ci),
                DefectAreaSum.ToString("F1", ci),
                SwayMs.ToString("F2", ci),
                RuleMs.ToString("F2", ci),
                totalMs.ToString("F2", ci));
        }
       
        public void Dispose()
        {
		}
    }

    // ------------------------------------------------------------------------
    // 画像1枚分の検査結果
    // ------------------------------------------------------------------------
    public class RuleInspectionResult
    {
        public bool InputError { get; set; }            // 入力サイズ不一致など
        public string ErrorMessage { get; set; } = "";
        public List<YarnRuleResult> Yarns { get; } = new List<YarnRuleResult>();
        public double TotalMs { get; set; }             // ルールベース処理全体の時間[ms]
    }

    // ------------------------------------------------------------------------
    // ルールベース検査本体
    //   ・スレッドセーフではない（内部バッファ/キャッシュを持つため）。
    //     カメラごと・スレッドごとに1インスタンスを持つこと。
    // ------------------------------------------------------------------------
    public sealed class HollowFiberRuleInspector : IDisposable
    {
        public HImage ImageProc { get; set; }

        private readonly RuleRecipe _recipe;

        // 行Map（出力画素が参照する入力行座標）のキャッシュ。キー=糸幅。
        // 分割幅は682/683の2種類なので、実質2枚だけ常駐する。
        private readonly Dictionary<int, HImage> _rowMapCache = new Dictionary<int, HImage>();

        // 列Map生成用の作業バッファ（毎回newしないように使い回す）
        private float[] _colMapBuffer = new float[0];

        private bool _disposed;

        public HollowFiberRuleInspector(RuleRecipe recipe)
        {
            _recipe = recipe ?? throw new ArgumentNullException(nameof(recipe));
        }

        /// <summary>
        /// 画像1枚分のルールベース検査を行う（HDev main の for ループ全体に相当）。
        /// </summary>
        /// <param name="image">入力画像（8192x2040想定）</param>
        public RuleInspectionResult Inspect(HImage image)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(HollowFiberRuleInspector));
            if (image == null) throw new ArgumentNullException(nameof(image));

            RuleInspectionResult result = new RuleInspectionResult();
            Stopwatch totalWatch = Stopwatch.StartNew();   // count_seconds(TotalStart)

            // ---- 入力サイズチェック（HDevでは stop() していた箇所）----
            int width, height;
            image.GetImageSize(out width, out height);
            if (width != _recipe.InputWidth || height != _recipe.InputHeight)
            {
                result.InputError = true;
                result.ErrorMessage = string.Format(
                    "入力画像サイズ不一致: {0}x{1}（期待値 {2}x{3}）",
                    width, height, _recipe.InputWidth, _recipe.InputHeight);
                result.TotalMs = totalWatch.Elapsed.TotalMilliseconds;
                return result;
            }


			HImage imgProc = new HImage(image);
            // ---- 糸ごとのループ ----
            // HDevは YarnCount-2 までループしているが、ColStartLimit による
            // スキップで同じ糸だけが処理されるため、ここでは全本数を回して判定する。
            for (int yarnIndex = 0; yarnIndex < _recipe.YarnCount; yarnIndex++)
            {
                int intW = (_recipe.lastYarnX - _recipe.firstYarnX) / (_recipe.YarnCount - 1);
				// ---- 1. 固定12分割 ----
				//int colStart = (int)Math.Floor(yarnIndex * _recipe.InputWidth / (1.0 * _recipe.YarnCount)) + _recipe.OffsetCol;
				//int colEnd = (int)Math.Floor((yarnIndex + 1) * _recipe.InputWidth / (1.0 * _recipe.YarnCount)) - 1 + _recipe.OffsetCol;
				int colStart = yarnIndex * intW + _recipe.OffsetCol;
				int colEnd = colStart + intW;

				// サンプル画像用の除外条件＋画像外へのはみ出し防止
				if (colStart > _recipe.lastYarnX) continue;
                if (colEnd >= width) continue;

                HImage partImage = new HImage();
                YarnRuleResult yarn = InspectYarn(image, yarnIndex, colStart, colEnd, height, out partImage);

                imgProc = PasteImage(imgProc, partImage, 0, colStart, height - 1, colEnd);

				result.Yarns.Add(yarn);
            }

			if (ImageProc != null)
				ImageProc.Dispose();
			ImageProc = imgProc.CopyImage();


			totalWatch.Stop();
            result.TotalMs = totalWatch.Elapsed.TotalMilliseconds;   // TotalRuleBasedMs
            return result;
        }

        /// <summary>
        /// 糸1本分の検査（HDev の for ループ本体 1〜8 に相当）
        /// </summary>
        private YarnRuleResult InspectYarn(HImage image, int yarnIndex, int colStart, int colEnd, int height, out HImage imageProc)
        {
            RuleRecipe r = _recipe;
            YarnRuleResult yarn = new YarnRuleResult
            {
                YarnNo = yarnIndex + 1,
                ColStart = colStart,
                ColEnd = colEnd
            };

            Stopwatch yarnWatch = Stopwatch.StartNew();   // count_seconds(YarnStart)

            // この糸の処理で生成したHALCONオブジェクトをまとめて破棄するための入れ物
            using (HalconObjectBag bag = new HalconObjectBag())
            {
                // ---- 1. 固定分割（切り出し）----
                HImage yarnImage = bag.Add(image.CropRectangle1(0, colStart, height - 1, colEnd));
                int yarnWidth, yarnHeight;
                yarnImage.GetImageSize(out yarnWidth, out yarnHeight);
                double targetCenter = (yarnWidth - 1) * 0.5;   // 糸を寄せる目標中心列

                // ---- 2. 仮糸抽出と輝度正規化 ----
                HImage normalizedImage = NormalizeBrightness(yarnImage, bag);

                // ---- 3. 糸抽出二値化と行別中心検出 ----
                HRegion yarnRegion = bag.Add(normalizedImage.Threshold(r.YarnGrayMin, r.YarnGrayMax));

                int[] rawLeft = new int[yarnHeight];
                int[] rawRight = new int[yarnHeight];
                FindLongestRunPerRow(yarnRegion, yarnHeight, rawLeft, rawRight);

                double[] centerRaw = new double[yarnHeight];
                double lastCenter = targetCenter;   // 糸が見つからない行は直前の中心で補間
                int missingRows = 0;
                int maxMissingRows = 0;
                for (int row = 0; row < yarnHeight; row++)
                {
                    if (rawLeft[row] >= 0)
                    {
                        // 最長ランの中心を糸中心とする
                        double center = (rawLeft[row] + rawRight[row]) * 0.5;
                        centerRaw[row] = center;
                        lastCenter = center;
                        missingRows = 0;
                    }
                    else
                    {
                        // 短い欠落だけ直前中心で補間。最終判定では連続欠落長を必ず評価する。
                        centerRaw[row] = lastCenter;
                        missingRows++;
                        if (missingRows > maxMissingRows) maxMissingRows = missingRows;
                    }
                }

                // ---- 4. 糸揺れ量平滑化 ----
                // map_image は「出力画素が参照する入力座標」を要求するため、
                // Offset = 検出中心 - 目標中心 とする。
                double[] offsetRaw = new double[yarnHeight];
                for (int row = 0; row < yarnHeight; row++)
                {
                    offsetRaw[row] = centerRaw[row] - targetCenter;
                }
                double[] offsetSmooth = SmoothMean(offsetRaw, r.MovingAverageRows);

                // ---- 5. 絶対座標Map生成と一括補正 ----
                HImage rowMap = GetRowMap(yarnWidth, yarnHeight);                     // キャッシュ（bagには入れない）
                HImage colMap = bag.Add(CreateColumnMap(offsetSmooth, yarnWidth, yarnHeight));
                HImage swayMap = bag.Add(rowMap.RealToVectorField(colMap, "vector_field_absolute"));
                HImage correctedImage = bag.Add(normalizedImage.MapImage(swayMap));

                double swayEndMs = yarnWatch.Elapsed.TotalMilliseconds;   // count_seconds(SwayEnd)

                // ---- 6. 補正後エッジ評価 ----
                HRegion correctedYarnRegion = bag.Add(correctedImage.Threshold(r.YarnGrayMin, r.YarnGrayMax));

                int[] corLeft = new int[yarnHeight];
                int[] corRight = new int[yarnHeight];
                FindLongestRunPerRow(correctedYarnRegion, yarnHeight, corLeft, corRight);

                List<double> correctedWidths = new List<double>(yarnHeight);
                double maxSymmetryError = 0.0;
                int correctedMissingRows = 0;
                int correctedMaxMissingRows = 0;
                for (int row = 0; row < yarnHeight; row++)
                {
                    if (corLeft[row] >= 0)
                    {
                        int left = corLeft[row];
                        int right = corRight[row];
                        correctedWidths.Add(right - left + 1);

                        // 目標中心から左右エッジまでの距離差 = 左右対称誤差
                        double symmetryError = Math.Abs((targetCenter - left) - (right - targetCenter));
                        if (symmetryError > maxSymmetryError) maxSymmetryError = symmetryError;

                        correctedMissingRows = 0;
                    }
                    else
                    {
                        correctedMissingRows++;
                        if (correctedMissingRows > correctedMaxMissingRows) correctedMaxMissingRows = correctedMissingRows;
                    }
                }

                double meanWidth;
                double widthDeviation;
                if (correctedWidths.Count > 0)
                {
                    meanWidth = Mean(correctedWidths);
                    widthDeviation = Deviation(correctedWidths, meanWidth);
                }
                else
                {
                    // 糸が1行も見つからない場合は必ずNGになる値にする
                    meanWidth = 0.0;
                    widthDeviation = 0.0;
                    maxSymmetryError = 999999.0;
                }

                bool edgeOK = true;
                if (meanWidth < r.WidthMin || meanWidth > r.WidthMax) edgeOK = false;
                if (widthDeviation > r.WidthDeviationMax || maxSymmetryError > r.SymmetryTolerance) edgeOK = false;
                if (maxMissingRows > r.MissingRowMax || correctedMaxMissingRows > r.MissingRowMax) edgeOK = false;

                // ---- 7. 糸内部欠陥の二値化 ----
                // 境界・上下不感帯を除いてから、糸内部専用しきい値を適用する。
                HRegion innerYarnRegion = bag.Add(correctedYarnRegion.ErosionRectangle1(2 * r.BorderMargin + 1, 1));
                HRegion validRowsRegion = bag.Add(new HRegion(
                    (double)r.TopDeadBand, 0.0,
                    (double)(yarnHeight - 1 - r.BottomDeadBand), (double)(yarnWidth - 1)));
                HRegion inspectionRegion = bag.Add(innerYarnRegion.Intersection(validRowsRegion));
                HImage inspectionImage = bag.Add(correctedImage.ReduceDomain(inspectionRegion));

                //inspectionImage.WriteImage("bmp", 0, $"D:\\data\\inspectionImage{yarnIndex}.bmp");
                imageProc = inspectionImage.CopyImage();

				HRegion defectRegion = bag.Add(inspectionImage.Threshold(r.DefectGrayMin, r.DefectGrayMax));
                HRegion defectConnected = bag.Add(defectRegion.Connection());
                HRegion defectSelected = bag.Add(defectConnected.SelectShape("area", "and", r.DefectAreaMin, r.DefectAreaMax));

                int defectCount = defectSelected.CountObj();
                double defectAreaSum = 0.0;
                if (defectCount > 0)
                {
                    HTuple defectRows, defectCols;
                    HTuple defectAreas = defectSelected.AreaCenter(out defectRows, out defectCols);

                    yarn.DefectAreas = ToDoubleArray(defectAreas);
                    yarn.DefectRows = ToDoubleArray(defectRows);
                    yarn.DefectColumns = ToDoubleArray(defectCols, colStart);
					//yarn.DefectRegion = ToRegionArray(defectSelected);


					HRegion blobRegion = new HRegion();
					yarn.DefectRegion = new HRegion[defectCount];
					for (int i = 0; i < defectCount; i++)
                    {
                        blobRegion = defectSelected.SelectObj(i + 1).MoveRegion(0, (int)colStart);
						//blobRegion = blobRegion.MoveRegion((int)yarn.DefectRows[i], (int)yarn.DefectColumns[i]);  // 元画像座標に戻す

						yarn.DefectRegion[i] = new HRegion(blobRegion);
						blobRegion.Dispose();
					}
					foreach (double a in yarn.DefectAreas) defectAreaSum += a;
                }

                bool internalDefectOK = (defectCount == 0);
                bool ruleOK = edgeOK && internalDefectOK;

                double ruleEndMs = yarnWatch.Elapsed.TotalMilliseconds;   // count_seconds(RuleEnd)

                // ---- 8. 糸別結果 ----
                yarn.EdgeOK = edgeOK;
                yarn.InternalDefectOK = internalDefectOK;
                yarn.RuleOK = ruleOK;
                yarn.MeanWidth = meanWidth;
                yarn.WidthDeviation = widthDeviation;
                yarn.MaxSymmetryError = maxSymmetryError;
                yarn.MaxMissingRows = Math.Max(maxMissingRows, correctedMaxMissingRows);
                yarn.DefectCount = defectCount;
                yarn.DefectAreaSum = defectAreaSum;
                yarn.SwayMs = swayEndMs;
                yarn.RuleMs = ruleEndMs - swayEndMs;
            }

            return yarn;
        }

        // ====================================================================
        // 以下、補助メソッド
        // ====================================================================

        /// <summary>
        /// 2. 仮糸抽出と輝度正規化。
        ///   仮二値化した糸領域の平均輝度が TargetGray になるように倍率をかける。
        ///   戻り値の HImage は bag に登録済み。
        /// </summary>
        private HImage NormalizeBrightness(HImage yarnImage, HalconObjectBag bag)
        {
            RuleRecipe r = _recipe;

            HRegion regionBeforeNorm = bag.Add(yarnImage.Threshold(r.YarnGrayMin, r.YarnGrayMax));
            double dummyRow, dummyCol;
            int areaBeforeNorm = regionBeforeNorm.AreaCenter(out dummyRow, out dummyCol);

            if (r.NormalizeEnable && areaBeforeNorm > 0)
            {
                double deviationGray;
                double meanGray = regionBeforeNorm.Intensity(yarnImage, out deviationGray);

                // 平均輝度がほぼ0の場合はゼロ除算を避けて等倍
                double grayScale = (meanGray > 1.0) ? r.TargetGray / meanGray : 1.0;

                // byte画像の場合、255を超える値は scale_image 内でクリップされる
                return bag.Add(yarnImage.ScaleImage(grayScale, 0.0));
            }

            return bag.Add(yarnImage.CopyImage());
        }

        /// <summary>
        /// 領域のランレングス情報から、行ごとに「最長ラン」の左端・右端列を求める。
        ///   HDevの tuple_find + tuple_sort_index による処理と同じ結果になる。
        ///   糸が無い行は left = right = -1。
        ///   同じ長さのランが複数ある場合は、HDevと同様に右側（後のラン）を採用する。
        /// </summary>
        private static void FindLongestRunPerRow(HRegion region, int height, int[] left, int[] right)
        {
            // 初期化（-1 = その行に糸なし）
            for (int i = 0; i < height; i++)
            {
                left[i] = -1;
                right[i] = -1;
            }

            // get_region_runs : 行, 開始列, 終了列 のタプル
            HTuple colBeginT, colEndT;
            HTuple rowsT;
            region.GetRegionRuns(out rowsT, out colBeginT, out colEndT);
            if (rowsT.Length == 0) return;   // 空領域

            int[] rows = rowsT.ToIArr();
            int[] colBegin = colBeginT.ToIArr();
            int[] colEnd = colEndT.ToIArr();

            // ランは1回だけ走査する（行ごとに検索しないので高速）
            for (int i = 0; i < rows.Length; i++)
            {
                int row = rows[i];
                if (row < 0 || row >= height) continue;

                int length = colEnd[i] - colBegin[i] + 1;
                int currentLength = (left[row] >= 0) ? (right[row] - left[row] + 1) : -1;

                // ">=" で後のランを優先（tuple_sort_index の最後の要素を採るHDevと合わせる）
                if (length >= currentLength)
                {
                    left[row] = colBegin[i];
                    right[row] = colEnd[i];
                }
            }
        }

        /// <summary>
        /// 4. 糸揺れ量の移動平均（create_funct_1d_array + smooth_funct_1d_mean）。
        ///   端部の扱いをHDevと完全に一致させるため、HALCONの HFunction1D を使う。
        /// </summary>
        private static double[] SmoothMean(double[] values, int smoothSize)
        {
            HFunction1D function = new HFunction1D(new HTuple(values));
            HFunction1D smoothed = function.SmoothFunct1dMean(smoothSize, 1);

            HTuple xValues, yValues;
            smoothed.Funct1dToPairs(out xValues, out yValues);

            double[] result = yValues.ToDArr();

            // 念のため長さを保証（通常は入力と同じ長さ）
            if (result.Length != values.Length)
            {
                double[] fixedResult = new double[values.Length];
                Array.Copy(result, fixedResult, Math.Min(result.Length, values.Length));
                return fixedResult;
            }
            return result;
        }

        /// <summary>
        /// 5. 列Map（出力画素が参照する入力列座標）を生成する。
        ///   HDevの以下の処理を C# 配列で1パスにまとめたもの:
        ///     gen_image_const + set_grayval + zoom_image_size('constant')  … 行ごとのオフセット画像
        ///     gen_image_surface_first_order(ColMapBase) + add_image          … 列座標 + オフセット
        ///     max_image(ZeroMap) + min_image(MaxColMap)                       … 0 ～ 幅-2 にクリップ
        /// </summary>
        private HImage CreateColumnMap(double[] offsetSmooth, int width, int height)
        {
            int size = width * height;
            if (_colMapBuffer.Length < size) _colMapBuffer = new float[size];

            float maxCol = width - 2.0f;   // map_image の補間で範囲外を参照しないよう幅-2まで
            for (int row = 0; row < height; row++)
            {
                float offset = (float)offsetSmooth[row];
                int baseIndex = row * width;
                for (int col = 0; col < width; col++)
                {
                    float v = col + offset;
                    if (v < 0.0f) v = 0.0f;
                    else if (v > maxCol) v = maxCol;
                    _colMapBuffer[baseIndex + col] = v;
                }
            }

            return CreateRealImage(_colMapBuffer, width, height);
        }

        /// <summary>
        /// 5. 行Map（出力画素が参照する入力行座標）を取得する。
        ///   値は min(行番号, 高さ-2) で、糸揺れに依存しないため糸幅ごとにキャッシュする。
        ///   戻り値はキャッシュなので呼び出し側で Dispose しないこと。
        /// </summary>
        private HImage GetRowMap(int width, int height)
        {
            HImage cached;
            if (_rowMapCache.TryGetValue(width, out cached))
            {
                return cached;
            }

            float[] buffer = new float[width * height];
            float maxRow = height - 2.0f;
            for (int row = 0; row < height; row++)
            {
                float v = Math.Min(row, maxRow);
                int baseIndex = row * width;
                for (int col = 0; col < width; col++)
                {
                    buffer[baseIndex + col] = v;
                }
            }

            HImage rowMap = CreateRealImage(buffer, width, height);
            _rowMapCache[width] = rowMap;
            return rowMap;
        }

        /// <summary>
        /// float配列から 'real' 型の HImage を生成する（gen_image1 相当）。
        ///   HALCON側に画素がコピーされるので、生成後に配列の固定を解除してよい。
        /// </summary>
        private static HImage CreateRealImage(float[] data, int width, int height)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return new HImage("real", width, height, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>平均値（tuple_mean 相当）</summary>
        private static double Mean(List<double> values)
        {
            double sum = 0.0;
            foreach (double v in values) sum += v;
            return sum / values.Count;
        }

        /// <summary>
        /// 標準偏差（tuple_deviation 相当）。
        ///   HALCONの tuple_deviation は (n-1) で割る標本標準偏差。要素1個のときは0。
        /// </summary>
        private static double Deviation(List<double> values, double mean)
        {
            if (values.Count < 2) return 0.0;
            double sq = 0.0;
            foreach (double v in values)
            {
                double d = v - mean;
                sq += d * d;
            }
            return Math.Sqrt(sq / (values.Count - 1));
        }

        /// <summary>HTuple（整数/実数混在可）を double 配列へ変換する</summary>
        private static double[] ToDoubleArray(HTuple tuple, double offset = 0)
        {
            double[] result = new double[tuple.Length];
            for (int i = 0; i < tuple.Length; i++)
            {
                result[i] = Convert.ToDouble(tuple[i].O, CultureInfo.InvariantCulture) + offset;
            }
            return result;
        }

		private static HRegion[] ToRegionArray(HRegion tuple)
		{
			HRegion[] result = new HRegion[tuple.CountObj()];
			for (int i = 0; i < tuple.CountObj(); i++)
			{
				result[i] = new HRegion(tuple[i]);
			}
			return result;
		}

		/// <summary>キャッシュしている行Mapを解放する</summary>
		public void Dispose()
        {
            if (_disposed) return;
            foreach (HImage img in _rowMapCache.Values)
            {
                img.Dispose();
            }
            _rowMapCache.Clear();
            _disposed = true;
        }

		/// <summary>
		/// 貼り付け先画像の指定矩形領域に、別の画像を貼り付ける
		/// </summary>
		/// <param name="destImage">貼り付け先の画像</param>
		/// <param name="srcImage">貼り付ける画像</param>
		/// <param name="row1">貼り付け先矩形の左上 Row</param>
		/// <param name="col1">貼り付け先矩形の左上 Column</param>
		/// <param name="row2">貼り付け先矩形の右下 Row</param>
		/// <param name="col2">貼り付け先矩形の右下 Column</param>
		/// <returns>貼り付け後の画像（新しいオブジェクト）</returns>
		private HImage PasteImage(HImage destImage, HImage srcImage,  int row1, int col1, int row2, int col2)
		{
			// 貼り付け先画像のサイズを取得
			destImage.GetImageSize(out int destWidth, out int destHeight);

			// 貼り付け先矩形のサイズ
			int pasteWidth = col2 - col1 + 1;
			int pasteHeight = row2 - row1 + 1;

			// ソース画像のサイズを取得
			srcImage.GetImageSize(out int srcWidth, out int srcHeight);

			// --- 1. 変換行列を作成（拡大縮小 → 平行移動）---
			HHomMat2D homMat = new HHomMat2D();   // 単位行列
												  // ソース画像を貼り付け先矩形サイズに合わせて拡大縮小（原点基準）
			homMat = homMat.HomMat2dScale(
				(double)pasteHeight / srcHeight,
				(double)pasteWidth / srcWidth,
				0.0, 0.0);
			// 貼り付け位置へ平行移動
			homMat = homMat.HomMat2dTranslate((double)row1, (double)col1);

			// --- 2. 貼り付け先と同じサイズの画像キャンバス上にソースを配置 ---
			//  "constant" 補間は高速。画質重視なら "bilinear" などに変更
			HImage movedSrc = srcImage.AffineTransImageSize(
				homMat, "constant", destWidth, destHeight);

			// --- 3. 貼り付けたい領域だけに定義域を絞る ---
			HRegion pasteRegion = new HRegion();
			pasteRegion.GenRectangle1((double)row1, (double)col1, (double)row2, (double)col2);
			HImage reducedSrc = movedSrc.ReduceDomain(pasteRegion);

			// --- 4. 定義域内の画素値を貼り付け先へコピー ---
			//  PaintGray は元画像を変更せず、新しい画像を返す
			HImage result = reducedSrc.PaintGray(destImage);

			// 一時オブジェクトの解放
			movedSrc.Dispose();
			reducedSrc.Dispose();
			pasteRegion.Dispose();

			return result;
		}
	}

    // ------------------------------------------------------------------------
    // HALCONオブジェクトの一括破棄用ヘルパー
    //   C# 7.3（.NET Framework）でも using のネストを深くせずに済むようにする。
    // ------------------------------------------------------------------------
    internal sealed class HalconObjectBag : IDisposable
    {
        private readonly List<IDisposable> _items = new List<IDisposable>();

        /// <summary>オブジェクトを登録してそのまま返す</summary>
        public T Add<T>(T obj) where T : class, IDisposable
        {
            if (obj != null) _items.Add(obj);
            return obj;
        }

        /// <summary>登録順と逆順に破棄する</summary>
        public void Dispose()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                _items[i].Dispose();
            }
            _items.Clear();
        }
    }


}
