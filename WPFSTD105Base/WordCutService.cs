using DevExpress.Spreadsheet;
using DevExpress.Xpf.Spreadsheet;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;
using ZXing;
using ZXing.QrCode;

using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace WPFSTD105
{
    public class WordCutService
    {
        public object EncodeTypes { get; private set; }

        public class ImageData
        {
            public string FileName = string.Empty;
            public byte[] BinaryData;
            public Stream DataStream => new MemoryStream(BinaryData);
            public ImagePartType ImageType
            {
                get
                {
                    var ext = System.IO.Path.GetExtension(FileName).TrimStart('.').ToLower();
                    switch (ext)
                    {
                        case "jpg":
                            return ImagePartType.Jpeg;
                        case "png":
                            return ImagePartType.Png;
                        case "":
                            return ImagePartType.Gif;
                        case "bmp":
                            return ImagePartType.Bmp;
                    }
                    throw new ApplicationException($"Unsupported image type: {ext}");
                }
            }
            public int SourceWidth;
            public int SourceHeight;
            public decimal Width;
            public decimal Height;
            public long WidthInEMU => Convert.ToInt64(Width * CM_TO_EMU);
            public long HeightInEMU => Convert.ToInt64(Height * CM_TO_EMU);
            private const decimal INCH_TO_CM = 2.54M;
            private const decimal CM_TO_EMU = 360000M;
            public string ImageName;
            public ImageData(string fileName, byte[] data, int dpi = 300)
            {
                FileName = fileName;
                BinaryData = data;
                Bitmap img = new Bitmap(new MemoryStream(data));
                SourceWidth = img.Width;
                SourceHeight = img.Height;
                Width = ((decimal)SourceWidth) / dpi * INCH_TO_CM;
                Height = ((decimal)SourceHeight) / dpi * INCH_TO_CM;
                ImageName = $"IMG_{Guid.NewGuid().ToString().Substring(0, 8)}";
            }
            public ImageData(string fileName, int dpi = 300) :
                this(fileName, File.ReadAllBytes(fileName), dpi)
            {
            }
        }
        public class DocxImgHelper
        {
            public static Run GenerateImageRun(WordprocessingDocument wordDoc, ImageData img, long ShapeTransFrom2DOffset)
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;

                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                var relationshipId = mainPart.GetIdOfPart(imagePart);
                imagePart.FeedData(img.DataStream);

                // Define the reference of the image.
                var element =
                     new Drawing(
                         new DW.Inline(
                             
                             new DW.Anchor()
                             {
                                 SimplePos = false,
                                 RelativeHeight = 251660288U,
                                 BehindDoc = false,
                                 Locked = false,
                                 LayoutInCell = true,
                                 AllowOverlap = true,
                                 AnchorId = "44803ED1",
                                 DistanceFromTop = (UInt32Value)0U,
                                 DistanceFromBottom = (UInt32Value)0U,
                                 DistanceFromLeft = (UInt32Value)114300U,
                                 DistanceFromRight = (UInt32Value)114300U,
                                 EditId = "50D07946"
                             },
                             new DW.SimplePosition()
                             {
                                 X = 0L,
                                 Y = 0L
                             },
                             new DW.HorizontalPosition() { RelativeFrom = DW.HorizontalRelativePositionValues.Column },
                             new DW.VerticalPosition() { RelativeFrom = DW.VerticalRelativePositionValues.Paragraph },
                             //Size of image, unit = EMU(English Metric Unit)
                             //1 cm = 360000 EMUs
                             new DW.Extent() { Cx = img.WidthInEMU, Cy = img.HeightInEMU },
                             new DW.EffectExtent()
                             {
                                 LeftEdge = 0L,
                                 TopEdge = 0L,
                                 RightEdge = 0L,
                                 BottomEdge = 0L
                             },
                             new DW.WrapTight()
                             {
                                 WrapText = DW.WrapTextValues.BothSides
                             },
                             new DW.WrapPolygon()
                             {
                                 Edited = false
                             },
                             new DW.StartPoint()
                             {
                                 X = 0L,
                                 Y = 0L
                             },
                             new DW.LineTo() { X = 21600L, Y = 1918L },
                             new DW.LineTo() { X = 1662L, Y = 1918L },
                             new DW.LineTo() { X = 1662L, Y = 21600L },
                             new DW.LineTo() { X = 1662L, Y = 21600L },
                             new DW.DocProperties()
                             {
                                 Id = (UInt32Value)1U,
                                 Name = img.ImageName
                             },
                             new DW.NonVisualGraphicFrameDrawingProperties(
                                 new A.GraphicFrameLocks() { NoChangeAspect = true }),
                             new A.Graphic(
                                 new A.GraphicData(
                                     new PIC.Picture(
                                         new PIC.NonVisualPictureProperties(
                                             new PIC.NonVisualDrawingProperties()
                                             {
                                                 Id = (UInt32Value)0U,
                                                 Name = img.FileName
                                             },
                                             new PIC.NonVisualPictureDrawingProperties()),
                                         new PIC.BlipFill(
                                             new A.Blip(
                                                 new A.BlipExtensionList(
                                                     new A.BlipExtension()
                                                     {
                                                         Uri =
                                                            "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                     })
                                             )
                                             {
                                                 Embed = relationshipId,
                                                 CompressionState =
                                                 A.BlipCompressionValues.Print
                                             },
                                             new A.Stretch(
                                                 new A.FillRectangle())),
                                         new PIC.ShapeProperties(
                                             new A.Transform2D(
                                                 new A.Offset() { X = 0L, Y = ShapeTransFrom2DOffset },
                                                 new A.Extents()
                                                 {
                                                     Cx = img.WidthInEMU,
                                                     Cy = img.HeightInEMU
                                                 }),
                                             new A.PresetGeometry(
                                                 new A.AdjustValueList()
                                             )
                                             { Preset = A.ShapeTypeValues.Rectangle }))
                                 )
                                 { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                         )
                         {
                             DistanceFromTop = (UInt32Value)0U,
                             DistanceFromBottom = (UInt32Value)0U,
                             DistanceFromLeft = (UInt32Value)114300U,
                             DistanceFromRight = (UInt32Value)114300U,
                             EditId = "50D07946"
                         });
                return new Run(element);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public WordCutService()
        {

        }
        #region 私有屬性

        #endregion

        /// <summary>
        /// 建立切割明細表
        /// </summary>
        /// <param name="DocPath">儲存路徑</param>
        public void CreateFile(string ProjectName, string ProjectNumber, string DocTmplatePath, string DocPath, ObservableCollection<MaterialDataView> dataViews, double TotalLossBothSide)
        {
            try
            {
                ReadOnlyCollection<MaterialDataView> materialDataViews = new ReadOnlyCollection<MaterialDataView>(dataViews);
                string a = ApplicationVM.DirectoryPorfile();

                //計算每種profile的數量
                Dictionary<string, int> DicProfileCount = new Dictionary<string, int>();
                foreach (var line in materialDataViews.GroupBy(info => info.Profile)
                        .Select(group => new
                        {
                            Metric = group.Key,
                            Count = group.Count()
                        })
                        .OrderBy(x => x.Metric))
                {
                    DicProfileCount.Add(line.Metric, line.Count);
                }

                //找出有幾種型鋼型態以顯示型鋼圖片
                string[] ListSteelType = new string[materialDataViews.Count];
                var pattern = @"[A-Z]+";
                for (int i = 0; i < materialDataViews.Count; i++)
                {
                    if (Regex.Match(materialDataViews[i].Profile, pattern).Value != "X")
                    {
                        ListSteelType[i] = Regex.Match(materialDataViews[i].Profile, pattern).Value;
                    }
                    else 
                    {
                        ListSteelType[i] = "CH"; //僅有槽鐵(CH)斷面規格不是英文字開始 ex:[200X80X7.5X11
                    }
                }
                string[] temp_DistinctSteelType = ListSteelType.Distinct().ToArray();//找出有幾種型鋼
                List<string> ListDistinctSteelType = temp_DistinctSteelType.ToList();
                for (int i = 0; i < (3 - temp_DistinctSteelType.Count()); i++)
                {
                    ListDistinctSteelType.Add("");
                }
                string[] DistinctSteelType = ListDistinctSteelType.ToArray();//要加多少空白型鋼圖片，可使"加工長度包含雙邊切除與切割損耗"文字貼齊外框

                //產生word
                string tmplPath = DocTmplatePath;//"AllFileTemplate/CutDocTemp.docx"; //模板DOC路徑
                string destPath = DocPath; //模板DOC路徑

                WordTmplRendering(ProjectName, ProjectNumber, tmplPath, destPath, TotalLossBothSide);//替換模板DOC字串

                string[] ReportLogoList = { "ReportLogo" };
                var document_ReportLogo = WordprocessingDocument.Open(destPath, true);
                ReplaceStringToSteelTypePicToWordDocument(document_ReportLogo, "ReportLogo", ReportLogoList, 0.6m, 0.6m, 47000L);//以型鋼圖示群替換指定字串
                document_ReportLogo.Close();

                var document_toPic = WordprocessingDocument.Open(destPath, true);
                ReplaceStringToSteelTypePicToWordDocument(document_toPic, "SteelTypePicture", DistinctSteelType, 0.4m, 0.4m, 35000L);//以型鋼圖示群替換指定字串
                document_toPic.Close();

                int ItemCount = 1;
                List<string[]> List_CurrentTableContent = new List<string[]>();
                string[] CurrentMaterial = new string[9];
                string[] CurrentSteelPart = new string[9];

                //表格內的格線：有為true，無為false
                bool InsideBorder = false;

                //將相同內容的零件合計
                //ObservableCollection<MaterialDataView> Result_materialDataViews = FindSameMaterial(materialDataViews);
                ObservableCollection<MaterialDataView> Result_materialDataViews = JsonConvert.DeserializeObject<ObservableCollection<MaterialDataView>>(JsonConvert.SerializeObject(materialDataViews));

                AddHorizontalLine_Bottom(destPath);
                foreach (string steel_type in temp_DistinctSteelType)//以型鋼型態分類
                {
                    //AddHorizontalLine_Bottom(destPath);
                    OpenAndAddTextToWordDocument(destPath, "_", "2");
                    OpenAndAddTextToWordDocument(destPath, $"型鋼型態：{steel_type}", "18");
                    //將表格標題型鋼文字替換成圖片
                    //var document_HeaderAddPic = WordprocessingDocument.Open(destPath, true);
                    //string[] HeaderPiclist = { $"{steel_type}" };
                    //ReplaceStringToSteelTypePicToWordDocument(document_HeaderAddPic, $"{steel_type}", HeaderPiclist, 0.4m, 0.4m, 35000L);//以型鋼圖示群替換指定字串
                    //document_HeaderAddPic.Close();
                    string[] TableColumnName = { "項目", "組合編號", "斷面規格", "材質", "購料長", "餘料長", "總數量", "購料來源", "狀態" };
                    List_CurrentTableContent.Add(TableColumnName);

                    var document = WordprocessingDocument.Open(destPath, true);
                    InsideBorder = true;
                    AddTableToWordDocument(document, List_CurrentTableContent, InsideBorder, "18","850","");
                    List_CurrentTableContent.Clear();

                    for (int i = 0; i < Result_materialDataViews.Count; i++)
                    {
                        string current_steel_type = "";
                        if (Regex.Match(Result_materialDataViews[i].Profile, pattern).Value != "X")
                        {
                            current_steel_type = Regex.Match(Result_materialDataViews[i].Profile, pattern).Value;
                        }
                        else 
                        {
                            current_steel_type = "CH"; //僅有槽鐵(CH)斷面規格不是英文字開始 ex:[200X80X7.5X11
                        }

                        if (steel_type == current_steel_type)
                        {
                            //單一素材資料
                            CurrentMaterial[0] = Convert.ToString(ItemCount);
                            CurrentMaterial[1] = Convert.ToString(Result_materialDataViews[i].MaterialNumber);
                            CurrentMaterial[2] = Convert.ToString(Result_materialDataViews[i].Profile);
                            CurrentMaterial[3] = Convert.ToString(Result_materialDataViews[i].Material);
                            CurrentMaterial[4] = Convert.ToString(Result_materialDataViews[i].LengthStr);
                            CurrentMaterial[5] = (Result_materialDataViews[i].LengthStr - Result_materialDataViews[i].Loss).ToString("#0.00");
                            //CurrentMaterial[6] = Convert.ToString(Result_materialDataViews[i].MeterialCount);
                            CurrentMaterial[6] = "1";//素材數量皆為1，因帶有素材編號的素材是唯一的
                            CurrentMaterial[7] = Convert.ToString("");
                            CurrentMaterial[8] = Convert.ToString("");

                            List_CurrentTableContent.Add(CurrentMaterial);
                            InsideBorder = true;
                            AddTableToWordDocument(document, List_CurrentTableContent, InsideBorder, "16","850","");
                            List_CurrentTableContent.Clear();
                            for (int ii = 0; ii < CurrentMaterial.Length; ii++) CurrentMaterial[ii] = "";

                            //string[] PartColumnName = { "QR_code", "", "構件編號", "零件編號", "長度", "數量", "Phase", "車次", "條碼" };//合併儲存格的起點儲存格內容，將覆蓋其他儲存格內容
                            string[] PartColumnName = { "QR_code", "", "構件編號", "零件編號", "長度", "數量", "標題一", "標題二", "條碼" };//合併儲存格的起點儲存格內容，將覆蓋其他儲存格內容
                            List_CurrentTableContent.Add(PartColumnName);
                            InsideBorder = false;
                            AddTableToWordDocument(document, List_CurrentTableContent, InsideBorder, "16","1400","START");
                            List_CurrentTableContent.Clear();

                            var part_group = Result_materialDataViews[i].Parts.GroupBy(el => (el.Length, el.AssemblyNumber, el.PartNumber, el.Count, el.Title1, el.Title2), el => (el.Length, el.AssemblyNumber, el.PartNumber, el.Count, el.Title1, el.Title2));
                            foreach (var part in part_group)
                            {
                                foreach (var part_item in part)
                                { 
                                    CurrentSteelPart[0] = Convert.ToString("");//這是被合併的儲存格
                                    CurrentSteelPart[1] = Convert.ToString("");//這是被合併的儲存格
                                    CurrentSteelPart[2] = Convert.ToString(part_item.AssemblyNumber);
                                    CurrentSteelPart[3] = Convert.ToString(part_item.PartNumber);
                                    CurrentSteelPart[4] = Convert.ToString(part_item.Length);

                                    int part_count = 0;
                                    for(int j=0;j< Result_materialDataViews[i].Parts.Count;j++)
                                    {
                                        if (Result_materialDataViews[i].Parts[j].AssemblyNumber == part_item.AssemblyNumber &&
                                            Result_materialDataViews[i].Parts[j].PartNumber == part_item.PartNumber &&
                                            Result_materialDataViews[i].Parts[j].AssemblyNumber == part_item.AssemblyNumber &&
                                            Result_materialDataViews[i].Parts[j].Length == part_item.Length &&
                                            //Result_materialDataViews[i].Parts[j].Phase == part_item.Phase &&
                                            //Result_materialDataViews[i].Parts[j].ShippingNumber == part_item.ShippingNumber
                                            Result_materialDataViews[i].Parts[j].Title1 == part_item.Title1 &&
                                            Result_materialDataViews[i].Parts[j].Title2 == part_item.Title2
                                            )
                                        {
                                            part_count ++;
                                        }
                                    }

                                    CurrentSteelPart[5] = Convert.ToString(part_count);
                                    //CurrentSteelPart[6] = "";//part_item.Phase
                                    //CurrentSteelPart[7] = "";//part_item.ShippingNumber
                                    CurrentSteelPart[6] = part_item.Title1;
                                    CurrentSteelPart[7] = part_item.Title2;

                                    CurrentSteelPart[8] = Convert.ToString("Bar_code");
                                    List_CurrentTableContent.Add(CurrentSteelPart);
                                    InsideBorder = false;
                                    AddTableToWordDocument(document, List_CurrentTableContent, InsideBorder, "16","1400", "CONTINUE");
                                    List_CurrentTableContent.Clear();
                                    for (int ii = 0; ii < CurrentSteelPart.Length; ii++) CurrentSteelPart[ii] = "";

                                    BarcodeWriter _writer = new BarcodeWriter();
                                    _writer.Format = BarcodeFormat.CODE_128;//由CODEBAR改成CODE_128，避免小於1000數字無法掃出的問題
                                    QrCodeEncodingOptions _qrCodeEncoding = new QrCodeEncodingOptions()
                                    {
                                        DisableECI = true,//設定內容編碼
                                        CharacterSet = "UTF-8", //設定二維碼的寬度和高度
                                        Width = 80,
                                        Height = 15,
                                        Margin = 0,//設定二維碼的邊距,單位不是固定畫素
                                    };
                                    _writer.Options = _qrCodeEncoding;
                                    _writer.Write(Convert.ToInt64(part_item.Length).ToString()).Save("Barcode.png"); ;

                                    string[] Barcodelist = { "Bar_code" };
                                    ReplaceStringToSteelTypePicToWordDocument(document, "Bar_code", Barcodelist, 1.5m, 0.3m, 0L);

                                    break;
                                }
                            }
                            BarcodeWriter _writerQR = new BarcodeWriter();
                            _writerQR.Format = BarcodeFormat.QR_CODE;//由CODEBAR改成CODE_128，避免小於1000數字無法掃出的問題
                            QrCodeEncodingOptions _qrCodeEncodingQR = new QrCodeEncodingOptions()
                            {
                                DisableECI = true,//設定內容編碼
                                CharacterSet = "UTF-8", //設定二維碼的寬度和高度
                                Width = 60,
                                Height = 60,
                                Margin = 0,//設定二維碼的邊距,單位不是固定畫素
                            };
                            _writerQR.Options = _qrCodeEncodingQR;
                            _writerQR.Write(Result_materialDataViews[i].MaterialNumber).Save("QRcode.png"); ;

                            string[] QRcodelist = { "QR_code" };
                            ReplaceStringToSteelTypePicToWordDocument(document, "QR_code", QRcodelist, 1.5m, 1.5m, 0L);

                            ItemCount++;
                        }
                    }
                    document.Close();

                    //測試連結不同寬度表格
                    //OpenAndAddTextToWordDocument(destPath, "_", "1");

                    //var document_test = WordprocessingDocument.Open(destPath, true);
                    //
                    //string[] TestPartColumnName = { "QR_code", "KKK", "構件編號", "零件編號", "長度", "數量", "Phase", "車次", "條碼" };//合併儲存格的起點儲存格內容，將覆蓋其他儲存格內容
                    //List_CurrentTableContent.Add(TestPartColumnName);
                    //InsideBorder = true;
                    //AddTableToWordDocument(document_test, List_CurrentTableContent, InsideBorder, "16", "400", "");
                    //List_CurrentTableContent.Clear();
                    //
                    //document_test.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static ObservableCollection<MaterialDataView> FindSameMaterial(ReadOnlyCollection<MaterialDataView> materialDataViews)
        {
            //複製資料materialDataViews, 因為統計過程中會需要異動資料而不要影響materialDataViews
            var temp_materialDataViews = JsonConvert.DeserializeObject<ObservableCollection<MaterialDataView>>(JsonConvert.SerializeObject(materialDataViews));
            //建立零件比對紀錄表 - 比對零件時若有比對到相同的則紀錄為false, 表示接下來不用再被比對
            bool[] NeedCheckDuplicateParts = new bool[temp_materialDataViews.Count];
            for (int i = 0; i < temp_materialDataViews.Count; i++) NeedCheckDuplicateParts[i] = true;
            //建立比對結果紀錄物件
            ObservableCollection<MaterialDataView> CheckResult_materialDataViews = new ObservableCollection<MaterialDataView>();

            //比對各個素材屬性是否相同, 以統計相同素材的數量
            for (int i = 0; i < temp_materialDataViews.Count; i++)
            {
                if (NeedCheckDuplicateParts[i] == true)//若該素材未被合併數量
                {
                    int current_material_count = 1;//紀錄當前素材統計數量
                    for (int j = i + 1; j < temp_materialDataViews.Count; j++)
                    {
                        if (NeedCheckDuplicateParts[j] == true)//若該素材只有一個(未超過2個以上)，則開始比對
                        {
                            if (temp_materialDataViews[i].Profile == temp_materialDataViews[j].Profile)//若斷面規格相同
                            {
                                if (temp_materialDataViews[i].LengthStr == temp_materialDataViews[j].LengthStr)//若購料長相同
                                {
                                    if (temp_materialDataViews[i].Parts.Count == temp_materialDataViews[j].Parts.Count)//若零件數量相同
                                    {
                                        double[] i_Part = new double[temp_materialDataViews[i].Parts.Count];
                                        double[] j_Part = new double[temp_materialDataViews[j].Parts.Count];
                                        for (int m = 0; m < temp_materialDataViews[i].Parts.Count; m++)
                                        {
                                            i_Part[m] = temp_materialDataViews[i].Parts[m].Length;
                                            j_Part[m] = temp_materialDataViews[j].Parts[m].Length;
                                        }
                                        Array.Sort(i_Part);
                                        Array.Sort(j_Part);

                                        if (Enumerable.SequenceEqual(i_Part, j_Part))//若各零件的切割長度相同
                                        {
                                            if (temp_materialDataViews[i].Material == temp_materialDataViews[j].Material &&
                                               temp_materialDataViews[i].Loss == temp_materialDataViews[i].Loss &&
                                               temp_materialDataViews[i].SourceIndex == temp_materialDataViews[i].SourceIndex)
                                            {
                                                current_material_count++;
                                                NeedCheckDuplicateParts[j] = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    temp_materialDataViews[i].MeterialCount = current_material_count;
                    CheckResult_materialDataViews.Add(temp_materialDataViews[i]);
                    NeedCheckDuplicateParts[i] = false;
                }
            }

            return CheckResult_materialDataViews;
        }

        /// <summary>
        /// 將錨定文字替換成指定圖片檔案，為使讀寫速度加快，開關WORD檔動作已搬移到方法之外
        /// </summary>
        public static void ReplaceStringToSteelTypePicToWordDocument(WordprocessingDocument document, string location_string, string[] target_string, decimal PicWidth, decimal PicHeight, long ShapeTransFrom2DOffset)
        {
            string[] NeedSpaceGroup = { "RH", "H", "BH", "BOX" };
            bool[] SpaceList = new bool[target_string.Length];
            for (int i = 0; i < target_string.Length; i++)
            {
                SpaceList[i] = false;

                if (i <= target_string.Length - 2)
                {
                    int FirstItem = Array.IndexOf(NeedSpaceGroup, target_string[i]);
                    int SecondItem = Array.IndexOf(NeedSpaceGroup, target_string[i + 1]);
                    if ((FirstItem != -1) && (SecondItem != -1))
                    {
                        SpaceList[i] = true;
                    }
                }
            }

            //using (WordprocessingDocument document = WordprocessingDocument.Open(filepath, true))
            //{
                //找到樣板中["SeelContentType"] = "SteelTypePicture"所在的 Run
                var runSteelTypePicture = document.MainDocumentPart.Document.Body.Descendants()
                    .Single(o => o.LocalName == "r" && o.InnerText == location_string);

                string Total_SteelTypeXml = "";
                for (int j = 0; j < target_string.Length; j++)
                {
                    string picture_path = "";
                    switch (target_string[j])
                    {
                        case "RH":
                        case "H":
                            picture_path = @"SteelSectionPng\RH.png";
                            break;
                        case "BH":
                            picture_path = @"SteelSectionPng\BH.png";
                            break;
                        case "L":
                            picture_path = @"SteelSectionPng\L.png";
                            break;
                        case "BOX":
                            picture_path = @"SteelSectionPng\BOX.png";
                            break;
                        case "U":
                        case "CH":
                            picture_path = @"SteelSectionPng\C.png";
                            break;
                        case "Bar_code":
                            picture_path = @"Barcode.png";
                            break;
                        case "QR_code":
                            picture_path = @"QRcode.png"; 
                            break;
                        case "ReportLogo":
                            picture_path = ApplicationVM.FileReportLogo() + @"\ReportLogo.png";
                            break;
                        default:
                            picture_path = @"SteelSectionPng\Space.png";
                            break;
                    }

                    string startup_path = System.Windows.Forms.Application.StartupPath;
                    if (target_string[j] != "ReportLogo")
                    {                        
                        picture_path = $@"{startup_path}\{picture_path}";
                    }
                    
                    var currentImg = new ImageData(picture_path)
                    {
                        Width = PicWidth,
                        Height = PicHeight
                    };
                    var imgRun = DocxImgHelper.GenerateImageRun(document, currentImg, ShapeTransFrom2DOffset);

                    Total_SteelTypeXml += imgRun.InnerXml;

                    //填入空白圖片(排版用)
                    if (SpaceList[j] == true)
                    {
                        picture_path = @"SteelSectionPng\Space.png";
                        picture_path = $@"{startup_path}\{picture_path}";
                        var SpaceImg = new ImageData(picture_path)
                        {
                            Width = 0.1m,
                            Height = PicHeight
                        };
                        var SpaceImgRun = DocxImgHelper.GenerateImageRun(document, SpaceImg, ShapeTransFrom2DOffset);

                        Total_SteelTypeXml += SpaceImgRun.InnerXml;
                    }
                }

                //將 InnerXML 置換成圖片 Run 的 InnerXML
                runSteelTypePicture.InnerXml = Total_SteelTypeXml;
            //}
        }

        static void WordTmplRendering(string ProjectName, string ProjectNumber, string TmpDocPath, string DesDocPath, double TotalLossBothSide)
        {
            //計算ProjectNumber-TwoSideCut, ProjectName-CutLoss這兩行各自需要的空格數目
            //計算ProjectName & ProjectNumber中分別有幾個中文字, 在下面填補空白時列入計算
            int count_ChineseCharacter_ProjectNumber = 0;
            Match match_ProjectNumber = Regex.Match(ProjectNumber, @"[\u4e00-\u9fa5]");
            while (match_ProjectNumber.Success)
            {
                count_ChineseCharacter_ProjectNumber += match_ProjectNumber.Captures.Count;
                match_ProjectNumber = match_ProjectNumber.NextMatch();
            }
            int count_ChineseCharacter_ProjectName = 0;
            Match match_ProjectName = Regex.Match(ProjectName, @"[\u4e00-\u9fa5]");
            while (match_ProjectName.Success)
            {
                count_ChineseCharacter_ProjectName += match_ProjectName.Captures.Count;
                match_ProjectName = match_ProjectName.NextMatch();
            }
            //ProjectNumber-TwoSideCut
            int NeedSpace_ProjectNumber2TwoSideCut = (70 - (ProjectNumber.Length- count_ChineseCharacter_ProjectNumber) - count_ChineseCharacter_ProjectNumber * 2) /2;
            for (int i = 1; i < NeedSpace_ProjectNumber2TwoSideCut; i++) ProjectNumber += "  ";
            ProjectNumber += "雙邊切除：";
            //ProjectName-CutLoss
            int NeedSpace_ProjectName2CutLoss = (70 - (ProjectName.Length- count_ChineseCharacter_ProjectName)- count_ChineseCharacter_ProjectName*2) /2;
            for (int i = 0; i < NeedSpace_ProjectName2CutLoss; i++) ProjectName += "  ";
            ProjectName += "切割損耗：";

            //取得報表上方資訊
            string current_date = DateTime.Now.ToString("yyyy-MM-dd");

            //替換報表上方資訊
            var docxBytes = WordRender.GenerateDocx(File.ReadAllBytes(TmpDocPath),
                new Dictionary<string, string>()
                {
                    ["ReportLogo"] = "ReportLogo",
                    ["PrintDate"] = current_date,
                    ["ProjectNo"] = ProjectNumber,
                    ["ProjectName"] = ProjectName,
                    ["TwoSideCut"] = TotalLossBothSide.ToString(),
                    ["CutLoss"] = "3",
                    ["SteelContentType"] = "SteelTypePicture"
                });
            File.WriteAllBytes(
                //Path.Combine(ResultFolder, $"套表測試-{DateTime.Now:HHmmss}.docx"),
                System.IO.Path.Combine(DesDocPath),
                docxBytes);
        }

        public static void OpenAndAddTextToWordDocument(string filepath, string txt, string font_size)
        {
            // Open a WordprocessingDocument for editing using the filepath.
            WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(filepath, true);

            // Assign a reference to the existing document body.
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            // Add new text.
            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run(new RunProperties(
                                                   new FontSize { Val = font_size },
                                                   //new RunFonts() { Ascii = "Calibri Light" },
                                                   new FontSizeComplexScript { Val = font_size }),
                                                   new Text(txt)));
            //run.AppendChild(new Text(txt));

            // Close the handle explicitly.
            wordprocessingDocument.Close();
        }

        /// <summary>
        /// 將給定字串陣列建立新的一列表格，為使讀寫速度加快，開關WORD檔動作已搬移到方法之外
        /// </summary>
        public static void AddTableToWordDocument(WordprocessingDocument document, List<string[]> data, bool InsideBorder, string CellFontSize, string state_cell_width, string vertical_merge)
        {
            //using (var document = WordprocessingDocument.Open(fileName, true))
            //{
                var doc = document.MainDocumentPart.Document;

                Table table = new Table();

                //設定表格框線
                if (InsideBorder == true)
                {
                    TableProperties props = new TableProperties(
                        new TableBorders(
                        new TopBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        },
                        new BottomBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        },
                        new LeftBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        },
                        new RightBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        },
                        new InsideHorizontalBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        },
                        new InsideVerticalBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        }),
                        new TableLayout() { Type = TableLayoutValues.Autofit });
                    table.AppendChild<TableProperties>(props);
                }
                else 
                {
                    TableProperties props = new TableProperties(
                        new TableCellSpacing() { Width = "20" },
                        new TableBorders(
                        new TopBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        },
                        new BottomBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        },
                        new LeftBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        },
                        new RightBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.Thick),
                            Size = 6
                        },
                        new InsideHorizontalBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.None),
                            Size = 0
                        },
                        new InsideVerticalBorder
                        {
                            Val = new EnumValue<BorderValues>(BorderValues.None),
                            Size = 0
                        }),
                        new TableLayout() { Type = TableLayoutValues.Autofit });
                    table.AppendChild<TableProperties>(props);
                }

                for (var i = 0; i < data.Count; i++)
                {
                    var tr = new TableRow(new TableRowProperties(
                                              new TableRowHeight() { Val = Convert.ToUInt32("0") }));

                    for (var j = 0; j < data[i].Length; j++)
                    {
                        string cell_width = "0";
                        JustificationValues flag_JustificationValues = JustificationValues.Center;
                        
                        switch (j)
                        {
                            case 0:
                                cell_width = "750"; //項目
                                break;
                            case 1:
                                cell_width = "1300"; //組合編號
                                break;
                            case 2:
                                cell_width = "2000"; //斷面規格
                                break;
                            case 3:
                                cell_width = "1050"; //材質
                                break;
                            case 4:
                                cell_width = "950"; //購料長
                                break;
                            case 5:
                                cell_width = "950"; //餘料長
                                break;
                            case 6:
                                cell_width = "950"; //總數量
                                break;
                            case 7:
                                cell_width = "1200"; //購料來源
                                break;
                            case 8:
                                cell_width = state_cell_width; //狀態
                                break;
                        }
                        var tc = new TableCell();

                        //測試
                        Paragraph par = new Paragraph();
                        Run run = new Run();

                        if (InsideBorder == false)
                        {
                            if (j == 0)
                            {
                                tc.Append(new TableCellProperties(
                                          new HorizontalMerge()
                                          {
                                              Val = MergedCellValues.Restart
                                          }));
                            }
                            if (j == 1)
                            {
                                tc.Append(new TableCellProperties(
                                          new HorizontalMerge()
                                          {
                                              Val = MergedCellValues.Continue
                                          }));
                            }
                            if (vertical_merge == "START")
                            {
                                if (j == 0 || j == 1)
                                {
                                    tc.Append(new TableCellProperties(
                                          new VerticalMerge()
                                          {
                                              Val = MergedCellValues.Restart
                                          }));
                                }
                            }
                            if (vertical_merge == "CONTINUE")
                            {
                                if (j == 0 || j == 1)
                                {
                                    tc.Append(new TableCellProperties(
                                          new VerticalMerge()
                                          {
                                              Val = MergedCellValues.Continue
                                          }));
                                }
                            }
                        }
                        tc.Append(new TableCellProperties(
                                      new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = cell_width },
                                      new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                                      new Paragraph(new ParagraphProperties(
                                                    new Justification() { Val = flag_JustificationValues },
                                                    new SpacingBetweenLines { LineRule = LineSpacingRuleValues.AtLeast, BeforeLines = 0, AfterLines = 0 }),
                                                    new Run(new RunProperties(
                                                            new FontSize { Val = CellFontSize },
                                                            new RunFonts() { Ascii = "Calibri Light" },
                                                            new FontSizeComplexScript { Val = CellFontSize }),
                                                            new Text(data[i][j]))));
                        tr.Append(tc);
                    }
                    table.Append(tr);
                }
                doc.Body.Append(table);
                doc.Save();
                //document.Close();
            //}
        }

        public static void AddHorizontalLine_Bottom(string fileName)
        {
            using (var document = WordprocessingDocument.Open(fileName, true))
            {
                var doc = document.MainDocumentPart.Document;

                Table table = new Table();

                TableProperties props = new TableProperties(
                    new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 0
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Thick),
                        Size = 6
                    },
                    new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 0
                    },
                    new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 0
                    },
                    new InsideHorizontalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 0
                    },
                    new InsideVerticalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 0
                    }));

                table.AppendChild<TableProperties>(props);


                var tr = new TableRow(new TableRowProperties(
                                          new TableRowHeight() { Val = Convert.ToUInt32("0") }));

                string cell_width = "10950";
                JustificationValues flag_JustificationValues = JustificationValues.Center;

                var tc = new TableCell();

                tc.Append(new TableCellProperties(
                                  new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                                  new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = cell_width },
                              new Paragraph(new ParagraphProperties(
                                                new Justification() { Val = flag_JustificationValues },
                                                new SpacingBetweenLines { LineRule = LineSpacingRuleValues.AtLeast, BeforeLines = 0, AfterLines = 0 }),
                                            new Run(new RunProperties(
                                                        new FontSize { Val = "2" },
                                                        new RunFonts() { Ascii = "Calibri Light" },
                                                        new FontSizeComplexScript { Val = "2" }),
                                                        new Text("_")
                                                        )));

                tr.Append(tc);

                flag_JustificationValues = JustificationValues.Center;

                table.Append(tr);

                doc.Body.Append(table);
                doc.Save();
                document.Close();
            }
        }

        // To search and replace content in a document part.
        public static void SearchAndReplace(string document)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText = new Regex("加工長度");
                docText = regexText.Replace(docText, "XXXX");

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }

    }
}
