//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Windows.Input;
//using devDept.Eyeshot;
//using devDept.Eyeshot.Entities;
//using devDept.Geometry;
//using devDept.Graphics;
//using WPFSTD105.Attribute;
//using MouseButton = System.Windows.Input.MouseButton;

//namespace WPFSTD105
//{
//    /// <summary>
//    /// 這是Drawing，它將擴展繪圖應用程序所需的行為。
//    /// </summary>
//    public class DrawingExt : devDept.Eyeshot.Drawings
//    {
//        #region 私有屬性
//        private readonly Dictionary<string, string> _formatBlockNames = new Dictionary<string, string>();
//        #endregion
//        #region 公開屬性
//        public DrawingsHelper Helper { get; private set; }
//        /// <summary>
//        /// 3D 模型
//        /// </summary>
//        public ModelExt Model { get; set; } = null;
//        /// <summary>
//        /// 為視圖實體計算幾何的類。
//        /// </summary>
//        public ViewBuilder ViewBuilder;
//        /// <summary>
//        /// 指示是否修改了原始場景。
//        /// </summary>
//        public bool IsModified = true;
//        /// <summary>
//        /// 指示當前場景是否已導入。
//        /// </summary>
//        /// <remarks>導入後，將禁用示例的某些按鈕。</remarks>
//        public bool IsImported = false;
//        /// <summary>
//        /// 指示是否必須重新加載當前工程圖。
//        /// </summary>
//        public bool IsToReload = true;
//        #endregion  
//        /// <summary>
//        /// 標準構造函數。
//        /// </summary>
//        public DrawingExt()
//        {
//            Helper = new DrawingsHelper(this);

//            formatType.A0_ISO.SetDisplayName("A0");
//            formatType.A1_ISO.SetDisplayName("A1");
//            formatType.A2_ISO.SetDisplayName("A2");
//            formatType.A3_ISO.SetDisplayName("A3");
//            formatType.A4_ISO.SetDisplayName("A4");
//            formatType.A4_LANDSCAPE_ISO.SetDisplayName("A4");
//            formatType.A_ANSI.SetDisplayName("A");
//            formatType.A_LANDSCAPE_ANSI.SetDisplayName("A");
//            formatType.B_ANSI.SetDisplayName("B");
//            formatType.C_ANSI.SetDisplayName("C");
//            formatType.D_ANSI.SetDisplayName("D");
//            formatType.E_ANSI.SetDisplayName("E");
//        }
//        #region 受保護的方法
//        /// <summary>
//        /// 繪製疊加的UI元素。
//        /// </summary>
//        /// <param name="data"></param>
//        protected override void DrawOverlay(DrawSceneParams data)
//        {
//            Helper.DrawOverlay(DrawText);
//            base.DrawOverlay(data);
//        }
//        /// <summary>
//        /// 鼠標輸入
//        /// </summary>
//        /// <param name="e"></param>
//        protected override void OnMouseEnter(EventArgs e)
//        {
//            Helper.OnMouseEnter();
//            base.OnMouseEnter(e);
//        }
//        /// <summary>
//        /// 鼠標離開
//        /// </summary>
//        /// <param name="e"></param>
//        protected override void OnMouseLeave(EventArgs e)
//        {
//            Helper.OnMouseLeave();
//            base.OnMouseLeave(e);
//        }
//        /// <summary>
//        /// 鼠標按下
//        /// </summary>
//        /// <param name="e"></param>
//        protected override void OnMouseDown(MouseButtonEventArgs e)
//        {
//            if (!Helper.OnMouseDown(e))
//                base.OnMouseDown(e);
//        }
//        /// <summary>
//        /// 屬標移動
//        /// </summary>
//        /// <param name="e"></param>
//        protected override void OnMouseMove(MouseEventArgs e)
//        {
//            System.Drawing.Point mouseLocation = RenderContextUtility.ConvertPoint(GetMousePosition(e));
//            Helper.OnMouseMove(mouseLocation, PaintBackBuffer, SwapBuffers);
//            base.OnMouseMove(e);
//        }
//        #endregion

//        #region 公開方法
//        /// <summary>
//        /// 將視圖和塊添加到工程圖控件。
//        /// </summary>
//        public void ViewBuilderAddToDrawing()
//        {
//            ViewBuilder.AddToDrawings(this);
//        }
//        /// <summary>
//        /// 開始繪製視圖
//        /// </summary>
//        /// <param name="singleView"></param>
//        /// <param name="dirtyOnly"></param>
//        public void StartViewBuilder(View singleView = null, bool dirtyOnly = true)
//        {
//            bool initView = ViewBuilder == null; //查看繪製視圖是否為 null 

//            if (initView)
//            {
//                /*摘要：
//                 *標準構造函數。
//                 *參數：
//                 *模型：用於構建視圖的模型。
//                 *圖紙：包含包含要重建視圖的圖紙的工程圖控件。
//                 *dirtyOnly：如果為true，則僅構建丟失/骯髒的視圖。
//                 *operatingType用於重建視圖的devDept.Eyeshot.ViewBuilder.operatingType。*/
//                ViewBuilder = new ViewBuilder(Model, this, dirtyOnly, ViewBuilder.operatingType.Queue);
//            }

//            if (singleView != null)
//            {
//                if (initView)
//                    ViewBuilder.ClearQueue();
//                //將視圖添加到ViewBuilder的隊列中
//                ViewBuilder.AddToQueue(singleView, ActiveSheet);
//            }
//            else
//            {
//                if (!initView)
//                    ViewBuilder.AddToQueue(this, dirtyOnly, ActiveSheet);
//            }
//            //如果ViewBuilder尚未運行，那麼我將異步啟動它。
//            if (!IsBusy)
//            {
//                StartWork(ViewBuilder);
//            }
//        }
//        /// <summary>
//        /// 重載圖紙
//        /// </summary>
//        public void ReloadDrawings()
//        {

//            if (Sheets.Count > 0)
//            {
//                //重建
//                StartViewBuilder();
//            }

//            IsToReload = true;
//        }
//        /// <summary>
//        /// 初始化圖紙
//        /// </summary>
//        public void InitializeDrawings()
//        {
//            Clear();
//            AddSheet("Sheet1", linearUnitsType.Millimeters, formatType.A4_LANDSCAPE_ISO, false);
//        }
//        #endregion

//        #region 私有方法
//        /// <summary>
//        /// 根據格式類型創建具有一些默認視圖的新工作表。
//        /// </summary>
//        /// <param name="name">工作表的名稱。</param>
//        /// <param name="units">圖紙的測量系統類型。</param>
//        /// <param name="formatType"><see cref="formatType"/></param>
//        /// <param name="addFrame">加入圖框</param>
//        /// <param name="addDefaultView">加入預設視圖</param>
//        private void AddSheet(string name, linearUnitsType units, formatType formatType, bool addFrame, bool addDefaultView = true)
//        {
//            Tuple<double, double> size = GetFormatSize(units, formatType); //紙張尺寸
//            Sheet sheet = new Sheet(units, size.Item1, size.Item2, name); //工作表格

//            Block block;
//            BlockReference br;
//            //如果要加入圖框
//            if (addFrame)
//            {
//                br = CreateFormatBlock(formatType, sheet, out block);
//                Blocks.Add(block);
//                sheet.Entities.Add(br);//無法將實體添加到圖形中，因為尚未創建控件句柄。 當此工作表被設置為活動工作表時，將添加它。
//            }
//            Sheets.Add(sheet);
//            Helper.AddDimensionsLayer();
//            if (addDefaultView)
//            {
//                AddSampleViews(sheet);
//            }
//        }
//        /// <summary>
//        /// 設定主要鋼結構物件設定檔
//        /// </summary>
//        /// <returns></returns>
//        private SteelAttr GetMainSteelAttr()
//        {
//            BlockReference blockReference = Model.CurrentBlockReference; //存取目前 BlockReference
//            Model.SetCurrent(null); //返回最上層
//            Model.SetCurrent((BlockReference)Model.Entities[0]);//設置主件 BlockReference 是當前 BlockReference
//            SteelAttr result = (SteelAttr)Model.Entities[0].EntityData; //取得設定檔資訊
//            Model.SetCurrent(blockReference);
//            return result;
//        }

//        /// <summary>
//        /// 取得適合比例
//        /// </summary>
//        /// <returns></returns>
//        private double GetScale(Sheet sheet)
//        {
//            SteelAttr steelAttr = GetMainSteelAttr();
//            double result1 = (sheet.Width - 50) / steelAttr.Length / 1000, result2 = (sheet.Height - 50) / (steelAttr.W * 2 + steelAttr.H) / 1000;

//            return result1 < result2 ? result1 : result2;
//        }
//        /// <summary>
//        /// 添加樣本視圖。
//        /// </summary>
//        /// <param name="sheet">The sheet where the views will be added.</param>
//        private void AddSampleViews(Sheet sheet)
//        {
//            // 該樣本使用以毫米為單位的值來添加視圖，並且使用此因子來獲取轉換後的值。
//            double unitsConversionFactor = Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, sheet.Units);
//            //比例計算
//            //double scaleFactor = GetScale(sheet);//計算視圖比例
//            double scaleFactor = 1;//計算視圖比例
//            //視圖位置+
//            double viewCenterX = sheet.Width / 2 * unitsConversionFactor; //Sheet 中心 X
//            double viewCenterY = sheet.Height / 2 * unitsConversionFactor;//Sheet 中心 Y
//            SteelAttr steelAttr = GetMainSteelAttr();
//            double backSideY = viewCenterY + (steelAttr.H / 2 * (scaleFactor * 1000)) + (steelAttr.W / 2 * (scaleFactor * 1000)) + 15;//back Y
//            double frontSideY = viewCenterY - (steelAttr.H / 2 * (scaleFactor * 1000)) - (steelAttr.W / 2 * (scaleFactor * 1000)) - 15;//front Y
//            //視圖設置
//            double extensionAmount = Math.Min(sheet.Width, sheet.Height) / 200;//設置中心線延伸量
//            //產生視圖
//            var back = new VectorView(0, 0, viewType.Rear, scaleFactor, GetViewName(sheet, viewType.Rear))
//            {
//                CenterlinesExtensionAmount = extensionAmount,
//                HiddenSegments = true,//虛線
//            };//背視圖
//            back.Rotate(Math.PI, Vector3D.AxisZ);
//            back.Translate(viewCenterX, backSideY);

//            sheet.AddViewPlaceHolder(back, Model, this, back.BlockName.Replace(sheet.Name, String.Empty));//加入背視圖
//            var top = new VectorView(viewCenterX, viewCenterY, viewType.Top, scaleFactor, GetViewName(sheet, viewType.Top))
//            {
//                CenterlinesExtensionAmount = extensionAmount,
//                HiddenSegments = true,//虛線
//            };//上視圖
//            sheet.AddViewPlaceHolder(top, Model, this, top.BlockName.Replace(sheet.Name, String.Empty));//加入上視圖
//            var front = new VectorView(viewCenterX, frontSideY, viewType.Front, scaleFactor, GetViewName(sheet, viewType.Front))
//            {
//                CenterlinesExtensionAmount = extensionAmount,
//                HiddenSegments = true,//虛線
//            };//前視圖

//            sheet.AddViewPlaceHolder(front, Model, this, front.BlockName.Replace(sheet.Name, String.Empty));//加入前視圖
//            Transformation accumulatedParentTransform =
//                            top.GetFullTransformation(Blocks);

//        }
//        /// <summary>
//        /// 取得視圖名稱
//        /// </summary>
//        /// <param name="sheet"></param>
//        /// <param name="viewType"></param>
//        /// <param name="isRaster"></param>
//        /// <returns></returns>
//        private static string GetViewName(Sheet sheet, viewType viewType, bool isRaster = false)
//        {
//            return String.Format("{0} {1} {2}", sheet.Name, viewType.ToString(), isRaster ? "Raster View" : "Vector View");
//        }
//        ///// <summary>
//        ///// 創建一個新工作表，其中包含帶有隱藏對象的視圖和另一個包含對象詳細信息的視圖。
//        ///// </summary>
//        ///// <param name="sheet">將在其中添加視圖的工作表。</param>
//        //private void AddViewWithHiddenAndDetailsObjects(Sheet sheet)
//        //{
//        //    // 該樣本使用以毫米為單位的值來添加視圖，並且使用該因子來獲取轉換後的值。
//        //    double unitsConversionFactor = Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, sheet.Units);
//        //    //TODO: 自動計算比例
//        //    double scaleFactor = 50;
//        //    double extensionAmount = Math.Min(sheet.Width, sheet.Height) / 200;
//        //    // 設置相機以構圖細節
//        //    Camera cam = new Camera(new Point3D(35, 0, 0), 0, Viewport.GetCameraRotation(viewType.Front), projectionType.Orthographic, 0, 1);
//        //    //添加細節視圖佔位符
//        //    VectorView detailView = new VectorView(327 * unitsConversionFactor, 140 * unitsConversionFactor, cam, 20 * scaleFactor, GetViewName(sheet, viewType.Front), unitsConversionFactor * 40, unitsConversionFactor * 100) { CenterlinesExtensionAmount = extensionAmount };
//        //    sheet.AddViewPlaceHolder(detailView, Model, this, detailView.BlockName.Replace(sheet.Name, String.Empty));
//        //    //adds a description
//        //    MultilineText detailDescription = new MultilineText(0, 0, "Detailed view" + System.Environment.NewLine + "Scale: 1:5", 10, 0.4, 0.7);
//        //    detailDescription.Alignment = devDept.Eyeshot.Entities.Text.alignmentType.MiddleCenter;
//        //    // sheet.Entities.Add(detailDescription);
//        //    Blocks[detailView.BlockName].Entities.Add(detailDescription);
//        //    //here we create a trimetric view with one hidden instance 
//        //    List<Tuple<Stack<BlockReference>, Entity>> entitiesToHide = new List<Tuple<Stack<BlockReference>, Entity>>();
//        //    entitiesToHide.Add(new Tuple<Stack<BlockReference>, Entity>(new Stack<BlockReference>(), Model.Entities[0]));
//        //    VectorView vector = new VectorView(130 * unitsConversionFactor, 180 * unitsConversionFactor, viewType.Trimetric, 3 * scaleFactor, GetViewName(sheet, viewType.Top))
//        //    { CenterlinesExtensionAmount = extensionAmount };
//        //    //設置要隱藏在視圖中的實體實例
//        //    vector.EntitiesToHide = entitiesToHide;
//        //    // 添加三角矢量視圖
//        //    sheet.AddViewPlaceHolder(vector, Model, this, vector.BlockName.Replace(sheet.Name, String.Empty));
//        //    Text hiddenInstanceDescription = new Text(-45, -45, 0, "Hidden instance view", 4);
//        //    Blocks[vector.BlockName].Entities.Add(hiddenInstanceDescription);
//        //    vector.HiddenSegments = false;
//        //}
//        /// <summary>
//        /// 創建
//        /// </summary>
//        /// <param name="formatType">圖紙大小</param>
//        /// <param name="sheet">工作葉面</param>
//        /// <param name="block">要輸出的圖塊</param>
//        /// <param name="titleName">標題名稱</param>
//        /// <returns></returns>
//        /// <remarks>
//        /// 圖框如果存在，它將刪除工作表的先前格式塊。
//        /// </remarks>
//        public BlockReference CreateFormatBlock(formatType formatType, Sheet sheet, out Block block, string titleName = "")
//        {
//            if (_formatBlockNames.ContainsKey(sheet.Name))
//            {
//                Blocks.Remove(_formatBlockNames[sheet.Name]); //刪除塊和相關塊的引用。
//                _formatBlockNames.Remove(sheet.Name);
//            }

//            BlockReference br = BuildFormatBlock(formatType, sheet, out block);

//            _formatBlockNames.Add(sheet.Name, br.BlockName); //將格式塊名稱添加到字典中

//            // 初始化屬性
//            br.Attributes["標題"] = new AttributeReference(titleName);
//            br.Attributes["紙張大小"] = new AttributeReference(formatType.GetDisplayName());
//            int sheetNumber = Sheets.IndexOf(ActiveSheet) + 1;
//            int sheetsCount = Sheets.Count;
//            if (sheetsCount == 0)
//            {
//                sheetNumber++;
//                sheetsCount++;
//            }
//            br.Attributes["Sheet"] = new AttributeReference(string.Format("SHEET {0} OF {1}", sheetNumber, sheetsCount));

//            return br;
//        }
//        /// <summary>
//        /// 圖塊構建格式
//        /// </summary>
//        /// <param name="formatType">圖紙大小</param>
//        /// <param name="sheet">工作表</param>
//        /// <param name="block">要輸出的圖塊</param>
//        /// <returns></returns>
//        private BlockReference BuildFormatBlock(formatType formatType, Sheet sheet, out Block block)
//        {
//            BlockReference br = null;
//            block = null;

//            switch (formatType)
//            {
//                case formatType.A0_ISO:
//                    br = sheet.BuildA0ISO(out block, "T", System.Drawing.Color.Black);
//                    break;
//                case formatType.A1_ISO:
//                    br = sheet.BuildA1ISO(out block);
//                    break;
//                case formatType.A2_ISO:
//                    br = sheet.BuildA2ISO(out block);
//                    break;
//                case formatType.A3_ISO:
//                    br = sheet.BuildA3ISO(out block);
//                    break;
//                case formatType.A4_ISO:
//                    br = sheet.BuildA4ISO(out block);
//                    break;
//                case formatType.A4_LANDSCAPE_ISO:
//                    br = sheet.BuildA4LANDSCAPEISO(out block);
//                    break;
//                case formatType.A_ANSI:
//                    br = sheet.BuildAANSI(out block);
//                    break;
//                case formatType.A_LANDSCAPE_ANSI:
//                    br = sheet.BuildALANDSCAPEANSI(out block);
//                    break;
//                case formatType.B_ANSI:
//                    br = sheet.BuildBANSI(out block);
//                    break;
//                case formatType.C_ANSI:
//                    br = sheet.BuildCANSI(out block);
//                    break;
//                case formatType.D_ANSI:
//                    br = sheet.BuildDANSI(out block);
//                    break;
//                case formatType.E_ANSI:
//                    br = sheet.BuildEANSI(out block);
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }

//            return br;
//        }
//        /// <summary>
//        /// 獲取指定格式類型的紙張尺寸。
//        /// </summary>
//        /// <param name="units">單位</param>
//        /// <param name="formatType">紙張格式</param>
//        /// <returns></returns>
//        private Tuple<double, double> GetFormatSize(linearUnitsType units, formatType formatType)
//        {
//            double conversionFactor = Utility.GetLinearUnitsConversionFactor(linearUnitsType.Millimeters, units);

//            switch (formatType)
//            {
//                case formatType.A0_ISO:
//                    return new Tuple<double, double>(1189 * conversionFactor, 841 * conversionFactor);
//                case formatType.A1_ISO:
//                    return new Tuple<double, double>(841 * conversionFactor, 594 * conversionFactor);
//                case formatType.A2_ISO:
//                    return new Tuple<double, double>(594 * conversionFactor, 420 * conversionFactor);
//                case formatType.A3_ISO:
//                    return new Tuple<double, double>(420 * conversionFactor, 297 * conversionFactor);
//                case formatType.A4_ISO:
//                    return new Tuple<double, double>(210 * conversionFactor, 297 * conversionFactor);
//                case formatType.A4_LANDSCAPE_ISO:
//                    return new Tuple<double, double>(297 * conversionFactor, 210 * conversionFactor);
//                case formatType.A_ANSI:
//                    return new Tuple<double, double>(215.9 * conversionFactor, 279.4 * conversionFactor);
//                case formatType.A_LANDSCAPE_ANSI:
//                    return new Tuple<double, double>(279.4 * conversionFactor, 215.9 * conversionFactor);
//                case formatType.B_ANSI:
//                    return new Tuple<double, double>(431.8 * conversionFactor, 279.4 * conversionFactor);
//                case formatType.C_ANSI:
//                    return new Tuple<double, double>(558.8 * conversionFactor, 431.8 * conversionFactor);
//                case formatType.D_ANSI:
//                    return new Tuple<double, double>(863.6 * conversionFactor, 558.8 * conversionFactor);
//                case formatType.E_ANSI:
//                    return new Tuple<double, double>(1117.6 * conversionFactor, 863.6 * conversionFactor);
//                default:
//                    return new Tuple<double, double>(210 * conversionFactor, 297 * conversionFactor);
//            }
//        }
//        #endregion
//    }
//}