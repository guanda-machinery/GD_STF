using DevExpress.Spreadsheet;
using DevExpress.Xpf.Spreadsheet;
using GD_STD;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Attribute;
using WPFWindowsBase;
using DevExpress.Data.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
//using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.Text;
using System.IO.Packaging;
using System.Xml;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Vml;
using System.Drawing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using Newtonsoft.Json;

namespace WPFSTD105
{
    public static class WordRender
    {
        private static void ReplaceParserTag(this OpenXmlElement elem, Dictionary<string, string> data)
        {
            var pool = new List<Run>();
            var matchText = string.Empty;
            var hiliteRuns = elem.Descendants<Run>() //找出鮮明提示
                .Where(o => o.RunProperties?.Elements<Highlight>().Any() ?? false).ToList();

            foreach (var run in hiliteRuns)
            {
                var t = run.InnerText;
                if (t.StartsWith("["))
                {
                    pool = new List<Run> { run };
                    matchText = t;
                }
                else
                {
                    matchText += t;
                    pool.Add(run);
                }
                if (t.EndsWith("]"))
                {
                    var m = Regex.Match(matchText, @"\[\$(?<n>\w+)\$\]");
                    if (m.Success && data.ContainsKey(m.Groups["n"].Value))
                    {
                        var firstRun = pool.First();
                        firstRun.RemoveAllChildren<Text>();
                        firstRun.RunProperties.RemoveAllChildren<Highlight>();
                        var newText = data[m.Groups["n"].Value];
                        var firstLine = true;
                        foreach (var line in Regex.Split(newText, @"\\n"))
                        {
                            if (firstLine) firstLine = false;
                            else firstRun.Append(new Break());
                            firstRun.Append(new Text(line));
                        }
                        pool.Skip(1).ToList().ForEach(o => o.Remove());
                    }
                }
            }
        }

        public static byte[] GenerateDocx(byte[] template, Dictionary<string, string> data)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(template, 0, template.Length);
                using (var docx = WordprocessingDocument.Open(ms, true))
                {
                    docx.MainDocumentPart.HeaderParts.ToList().ForEach(hdr =>
                    {
                        hdr.Header.ReplaceParserTag(data);
                    });
                    docx.MainDocumentPart.FooterParts.ToList().ForEach(ftr =>
                    {
                        ftr.Footer.ReplaceParserTag(data);
                    });
                    docx.MainDocumentPart.Document.Body.ReplaceParserTag(data);
                    docx.Save();
                }
                return ms.ToArray();
            }
        }
    }

    public class WordBuyService
    {
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
                             DistanceFromLeft = (UInt32Value)0U,
                             DistanceFromRight = (UInt32Value)0U,
                             EditId = "50D07946"
                         });
                return new Run(element);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public WordBuyService()
        {
        }
        #region 私有屬性
        #endregion
        /// <summary>
        /// 建立採購明細單
        /// </summary>
        /// <param name="path">儲存路徑</param>
        public void CreateFile(string ProjectName, string ProjectNumber, string DocTmplatePath, string DocPath, ObservableCollection<MaterialDataView> dataViews, double TotalLossBothSide, ObservableCollection<SteelAttr> steelAttrs = null)
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
                string tmplPath = DocTmplatePath;//"AllFileTemplate/BuyDocTemp.docx"; //模板DOC路徑
                //string aa = System.Environment.CurrentDirectory;
                //string destPath = "../TestDoc_Output.docx"; //模板DOC路徑
                //string destPath = @"D:\\0527-666瑞迪曼斯丙棟(斜角)\TestDoc_Output.docx"; //目標DOC路徑
                string destPath = DocPath; //模板DOC路徑

                WordTmplRendering(ProjectName, ProjectNumber, tmplPath, destPath, TotalLossBothSide);//替換模板DOC字串

                string[] ReportLogoList = { "ReportLogo" };
                ReplaceStringToSteelTypePicToWordDocument(destPath, "ReportLogo", ReportLogoList, 0.6m, 0.6m, 47000L);//以型鋼圖示群替換指定字串

                ReplaceStringToSteelTypePicToWordDocument(destPath, "SteelTypePicture", DistinctSteelType, 0.4m, 0.4m, 35000L);//以型鋼圖示群替換指定字串

                //將相同內容的零件合計
                ObservableCollection<MaterialDataView> Result_materialDataViews = FindSameMaterial(materialDataViews);

                //以型鋼型態分類建立個別表格
                Result_materialDataViews.GroupBy(el => el.Profile).ForEach(el =>
                {
                    AddHorizontalLine(destPath);
                    OpenAndAddTextToWordDocument(destPath, $"● 型鋼規格：{el.Key}", "22");
                    //table標頭加上型鋼圖示
                    //OpenAndAddTextToWordDocument(destPath, "TableHeader", "22");
                    //ReplaceStringToPictureToWordDocument(destPath, "TableHeader", 0.1m, 0.1m);
                    //OpenAndAddTextToWordDocument(destPath, "TableHeaderPicture_RH", "22");
                    //ReplaceStringToPictureToWordDocument(destPath, "TableHeaderPicture_RH", 0.4m, 0.4m);

                    //添加表格資料
                    //string[,] CurrentTableContent = { { "購料長(mm)", "數量", "切割長度組合(mm)", "材質", "加工長度(mm)", "損耗(%)", "購料重量(Kg)", "購料預設來源", "狀態" } };
                    //                                  { "12020", "1","3122*1, 3054*2, 2760*1","SN4908","12019","0.01","598.8","","" },
                    //                                  { "12020", "1","3122*1, 3054*2, 2760*1","SN4908","12019","0.01","598.8","","" }};
                    string[] TableColumnName = { "購料長(mm)", "數量", "切割長度組合(mm)", "材質", "加工長度(mm)", "損耗(%)", "購料重量(Kg)", "購料預設來源", "狀態" };
                    List<string[]> List_CurrentTableContent = new List<string[]>();
                    List_CurrentTableContent.Add(TableColumnName);

                    //將素材中相同切割長度的部分合計
                    double SumPartWeight = 0.0;
                    foreach (var item in el)
                    {
                        //steelAttrs = steelAttrs == null ? SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\RH.inp") : steelAttrs;
                        switch(item.ObjectType.ToString())
                        {
                            case "RH":
                                steelAttrs = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\RH.inp");
                                break;
                            case "H":
                                steelAttrs = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\H.inp");
                                break;
                            case "BH":
                                steelAttrs = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\BH.inp");
                                break;
                            case "LB":
                                steelAttrs = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\LB.inp");
                                break;
                            case "CH":
                                steelAttrs = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\CH.inp");
                                break;
                            default:
                                steelAttrs = SerializationHelper.Deserialize<ObservableCollection<SteelAttr>>($@"{ApplicationVM.DirectoryPorfile()}\RH.inp");
                                break;
                        }

                        string[] CurrentSteelPart = new string[9];
                        int index = steelAttrs.FindIndex(steel => steel.Profile == item.Profile);
                        CurrentSteelPart[0] = Convert.ToString(item.LengthStr);
                        CurrentSteelPart[1] = Convert.ToString(item.MeterialCount);

                        //CurrentSteelPart[2] = Convert.ToString(item.Parts.Select(part => $"{part.Length}").Aggregate((str1, str2) => $"{str1},{str2}"));
                        //收集該型鋼形態下各個part的長度
                        List<string> List_CurrentPart = new List<string>();
                        foreach (var part in item.Parts)
                        {
                            List_CurrentPart.Add(part.Length.ToString());
                        }
                        string[] Array_CurrentPart = List_CurrentPart.ToArray();

                        //計算每個長度有幾個
                        string string_LengthCombination = "";
                        foreach (var s in Array_CurrentPart.GroupBy(c => c))
                        {
                            //Console.WriteLine(s.Key + "重現了:" + s.Count() + "次");
                            string_LengthCombination += $"{s.Key}*{s.Count()} ,";
                        }
                        CurrentSteelPart[2] = string_LengthCombination.Remove(string_LengthCombination.Length - 2);

                        CurrentSteelPart[3] = Convert.ToString(item.Parts[0].Material);
                        CurrentSteelPart[4] = Convert.ToString(item.Loss);
                        CurrentSteelPart[5] = Convert.ToString(((item.LengthStr - item.Loss) / item.LengthStr * 100).ToString("#0.00"));
                        CurrentSteelPart[6] = Convert.ToString((index == -1 ? 0 : item.LengthStr / 1000 * steelAttrs[index].Kg).ToString("#0.00"));
                        CurrentSteelPart[7] = Convert.ToString(item.Sources);
                        CurrentSteelPart[8] = "";
                        List_CurrentTableContent.Add(CurrentSteelPart);

                        SumPartWeight += (index == -1 ? 0 : item.LengthStr / 1000 * steelAttrs[index].Kg);
                    }

                    AddTableToWordDocument(destPath, List_CurrentTableContent);

                    string[] b = { "合計", $"{DicProfileCount[el.Key]}", "", "", "", "", $"{SumPartWeight.ToString("#0.00")}(Kg)", "", "" };
                    AddTable_StatisticToWordDocument(destPath, b);

                    AddInvisibleParagraphToWordDocument(destPath);//加入不可見的paragraph，以斷開表格相連產生的欄位寬度錯誤
                });
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

        public static void ReplaceStringToSteelTypePicToWordDocument(string filepath, string location_string, string[] target_string, decimal PicWidth, decimal PicHeight, long ShapeTransFrom2DOffset)
        {
            //bool fAddSpace = false;
            //Run run = OpenAndAddTextToWordDocumentAndReturnRun(filepath, " ");

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

            using (WordprocessingDocument document = WordprocessingDocument.Open(filepath, true))
            {
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

                    var cat1Img = new ImageData(picture_path)
                    {
                        Width = PicWidth,
                        Height = PicHeight
                    };
                    var imgRun = DocxImgHelper.GenerateImageRun(document, cat1Img, ShapeTransFrom2DOffset);

                    Total_SteelTypeXml += imgRun.InnerXml;

                    if (SpaceList[j] == true)
                    {
                        picture_path = @"SteelSectionPng\Space.png";
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
            }
        }

        static void WordTmplRendering(string ProjectName, string ProjectNumber, string TmpDocPath, string DesDocPath, double TotalLossBothSide)
        {
            //計算ProjectNumber-TwoSideCut, ProjectName-CutLoss這兩行各自需要的空格數目
            //ProjectNumber-TwoSideCut
            int NeedSpace_ProjectNumber2TwoSideCut = (70 - ProjectNumber.Length) / 2;
            for (int i = 1; i < NeedSpace_ProjectNumber2TwoSideCut; i++) ProjectNumber += "  ";
            ProjectNumber += "雙邊切除：";
            //ProjectName-CutLoss
            int NeedSpace_ProjectName2CutLoss = (70 - ProjectName.Length) / 2;
            for (int i = 0; i < NeedSpace_ProjectName2CutLoss; i++) ProjectName += "  ";
            ProjectName += "切割損耗：";

            //取得報表上方資訊
            string current_date = DateTime.Now.ToString("yyyy-MM-dd");

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
                                                   new RunFonts() { Ascii = "Times New Roman" },
                                                   new FontSizeComplexScript { Val = font_size }),
                                                   new Text(txt)));
            //run.AppendChild(new Text(txt));

            // Close the handle explicitly.
            wordprocessingDocument.Close();
        }

        public static void AddInvisibleParagraphToWordDocument(string filepath)
        {
            // Open a WordprocessingDocument for editing using the filepath.
            WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(filepath, true);

            // Assign a reference to the existing document body.
            Body body = wordprocessingDocument.MainDocumentPart.Document.Body;

            // Add new text.
            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run(new RunProperties(
                                                   new FontSize { Val = "0" },
                                                   new RunFonts() { Ascii = "Calibri Light" },
                                                   new FontSizeComplexScript { Val = "0" }),
                                                   new Text("_")));

            // Close the handle explicitly.
            wordprocessingDocument.Save();
            wordprocessingDocument.Close();
        }

        // Take the data from a two-dimensional array and build a table at the 
        // end of the supplied document.
        public static void AddTableToWordDocument(string fileName, List<string[]> data)
        {
            using (var document = WordprocessingDocument.Open(fileName, true))
            {
                var doc = document.MainDocumentPart.Document;

                Table table = new Table();

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

                for (var i = 0; i < data.Count; i++)
                {
                    var tr = new TableRow(new TableRowProperties(
                                              new TableRowHeight() { Val = Convert.ToUInt32("0") }));

                    //if (i == 0)
                    //{
                    //    var tc = new TableCell();
                    //
                    //    tc.Append(new TableCellProperties(
                    //                  new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                    //                  new Paragraph(new ParagraphProperties(
                    //                                new SpacingBetweenLines { LineRule = LineSpacingRuleValues.AtLeast, BeforeLines = 0, AfterLines = 0 }),
                    //                                new Run(new RunProperties(
                    //                                        new FontSize { Val = "16" },
                    //                                        new RunFonts() { Ascii = "Calibri Light" },
                    //                                        new FontSizeComplexScript { Val = "16" }),
                    //                                        new Text("  "))));
                    //
                    //    tc.Append(new TableCellProperties(
                    //              new TableCellWidth { Type = TableWidthUnitValues.Auto}));
                    //
                    //    tr.Append(tc);
                    //}

                    for (var j = 0; j < data[i].Length; j++)
                    {
                        string cell_width = "0";
                        JustificationValues flag_JustificationValues = JustificationValues.Center;

                        switch (j)
                        {
                            case 0:
                                cell_width = "1100";//購料長
                                break;
                            case 1:
                                cell_width = "550";//數量
                                break;
                            case 2:
                                cell_width = "3100";//切割長度組合
                                flag_JustificationValues = JustificationValues.Left;
                                break;
                            case 3:
                                cell_width = "650";//材質
                                break;
                            case 4:
                                cell_width = "1300";//加工長度
                                break;
                            case 5:
                                cell_width = "850";//損耗
                                break;
                            case 6:
                                cell_width = "1200";//購料重量
                                break;
                            case 7:
                                cell_width = "1300";//購料預設來源
                                break;
                            case 8:
                                cell_width = "550";//狀態
                                break;
                        }
                        var tc = new TableCell();

                        tc.Append(new TableCellProperties(
                                      new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                                      new Paragraph(new ParagraphProperties(
                                                    new Justification() { Val = flag_JustificationValues },
                                                    new SpacingBetweenLines { LineRule = LineSpacingRuleValues.AtLeast, BeforeLines = 0, AfterLines = 0 }),
                                                    new Run(new RunProperties(
                                                            new FontSize { Val = "16" },
                                                            new RunFonts() { Ascii = "Calibri Light" },
                                                            new FontSizeComplexScript { Val = "16" }),
                                                            new Text(data[i][j]))));

                        tc.Append(new TableCellProperties(
                                  new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = cell_width }));
                        //TableCellProperties tcp = new TableCellProperties();
                        //tcp.AppendChild(string.IsNullOrEmpty(cell_width)
                        //                ? new TableCellWidth { Type = TableWidthUnitValues.Auto }
                        //                : new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = cell_width });
                        //tc.Append(tcp);

                        tr.Append(tc);

                        flag_JustificationValues = JustificationValues.Center;
                    }
                    table.Append(tr);
                }
                doc.Body.Append(table);
                doc.Save();
                document.Close();
            }
        }

        public static void AddTable_StatisticToWordDocument(string fileName, string[] data)
        {
            using (var document = WordprocessingDocument.Open(fileName, true))
            {
                var doc = document.MainDocumentPart.Document;

                Table table = new Table();

                TableProperties props = new TableProperties(
                    new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.Thick),
                        Size = 6
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 0
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


                for (var j = 0; j <= data.GetUpperBound(0); j++)
                {
                    string cell_width = "2000";
                    JustificationValues flag_JustificationValues = JustificationValues.Center;

                    switch (j)
                    {
                        case 0:
                            cell_width = "1100";
                            break;
                        case 1:
                            cell_width = "550";
                            break;
                        case 2:
                            cell_width = "3100";
                            flag_JustificationValues = JustificationValues.Left;
                            break;
                        case 3:
                            cell_width = "950";
                            break;
                        case 4:
                            cell_width = "1300";
                            break;
                        case 5:
                            cell_width = "750";
                            break;
                        case 6:
                            cell_width = "1200";
                            break;
                        case 7:
                            cell_width = "1200";
                            break;
                        case 8:
                            cell_width = "550";
                            break;
                    }
                    var tc = new TableCell();

                    tc.Append(new TableCellProperties(
                                      new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }),
                                  new Paragraph(new ParagraphProperties(
                                                    new Justification() { Val = flag_JustificationValues },
                                                    new SpacingBetweenLines { LineRule = LineSpacingRuleValues.AtLeast, BeforeLines = 0, AfterLines = 0 }),
                                                new Run(new RunProperties(
                                                            new Bold(),
                                                            new FontSize { Val = "20" },
                                                            new RunFonts() { Ascii = "Calibri Light" },
                                                            new FontSizeComplexScript { Val = "20" }),
                                                            //new Bold { Val = OnOffValue.FromBoolean(true) },//ISSUE:尚無法加入粗體
                                                            new Text(data[j]))));

                    tc.Append(new TableCellProperties(
                              new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = cell_width }));

                    tr.Append(tc);

                    flag_JustificationValues = JustificationValues.Center;
                }
                table.Append(tr);

                doc.Body.Append(table);
                doc.Save();
            }
        }

        public static void AddHorizontalLine(string fileName)
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

                string cell_width = "11000";
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

                //tc.Append(new TableCellProperties(
                //          new TableCellWidth { Type = TableWidthUnitValues.Dxa, Width = cell_width }));

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
