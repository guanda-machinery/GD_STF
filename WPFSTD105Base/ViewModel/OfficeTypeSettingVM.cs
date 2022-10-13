using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.Printing;
using DevExpress.Xpf.Reports.UserDesigner;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraReports.UI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFWindowsBase;
using GD_STD.Data;
using static WPFSTD105.ViewLocator;
using System.Collections.Generic;
using System.Diagnostics;
//using WordToPdfService; //WORD to PDF by Office
using System.IO;

//using GrapeCity.Documents.Word; //by GrapeCity
//using System.Drawing;
//using GrapeCity.Documents.Word.Layout;
//using System.IO.Compression;

//using Syncfusion.DocIO; //Syncfusion
//using Syncfusion.DocIO.DLS;
//using Syncfusion.OfficeChart;
//using Syncfusion.DocToPDFConverter;
//using Syncfusion.OfficeChartToImageConverter;
//using Syncfusion.Pdf;

namespace WPFSTD105
{
    /// <summary>
    /// 辦公室軟體排版設定
    /// </summary>
    public class OfficeTypeSettingVM : AbsTypeSettingVM
    {
        /// <summary>
        /// 顯示報表清單
        /// </summary>
        public bool NotShowDetail { get; set; } = true;
        /// <summary>
        /// 辦公室軟體排版設定
        /// </summary>
        public OfficeTypeSettingVM()
        {
            //OpenPreviewCommand = OpenPreview();
            //ExpandTableDetailCommand = ExpandTableDetail();
            //SaveTypeSettingsModifyCommand = SaveTypeSettingsModify();
            //CutControlCommand = CutControl();
            //AmountControlCommand = AmountControl();
        }
        ///// <inheritdoc/>
        //protected override RelayCommand Auto()
        //{
        //    throw new NotImplementedException();
        //}
        /// <inheritdoc/>
        protected override RelayCommand Manual()
        {
            return new RelayCommand(() =>
            {

            });
        }

       
        /// <summary>
        /// 呼叫列印預覽視窗
        /// </summary>
       // public ICommand OpenPreviewCommand { get; set; }
        public RelayParameterizedCommand OpenPreviewCommand
        {
            get
            {
                return new RelayParameterizedCommand(el =>
                {
                    GridControl gridControl = el as GridControl;
                    string fileName = "TypeSettings_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    if (gridControl.ItemsSource != null)
                    {
                        gridControl.View.ShowPrintPreview(gridControl.View, fileName, "廣達國際機械有限公司");
                    }
                    else
                    {
                        WinUIMessageBox.Show(null,
                            "報表無任何資料",
                            "匯出錯誤",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error,
                            MessageBoxResult.None,
                            MessageBoxOptions.None);
                    }
                });
            }
        }
        /// <summary>
        /// 收折結果報表
        /// </summary>
        //public ICommand ExpandTableDetailCommand { get; set; }
        public RelayParameterizedCommand ExpandTableDetailCommand
        {
            get
            {
                return new RelayParameterizedCommand(el =>
                {
                    GridControl gridControl = el as GridControl;

                    if (!NotShowDetail)
                    {
                        for (int i = 0; i < gridControl.VisibleRowCount; i++)
                        {
                            var rowHandle = gridControl.GetRowHandleByVisibleIndex(i);
                            gridControl.ExpandMasterRow(rowHandle);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < gridControl.VisibleRowCount; i++)
                        {
                            var rowHandle = gridControl.GetRowHandleByVisibleIndex(i);
                            gridControl.CollapseMasterRow(rowHandle);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 儲存排版設定變更
        /// </summary>
        //public ICommand SaveTypeSettingsModifyCommand { get; set; }
        public RelayParameterizedCommand SaveTypeSettingsModifyCommand
        {
            get
            {
                return new RelayParameterizedCommand(el =>
                {
                    GridControl gridControl = el as GridControl;
                    ShowDesigner(gridControl.View as TableView);
                });
            }
        }

        /// <summary>
        /// 呼叫列印預覽視窗
        /// </summary>
        //public ICommand CutControlCommand { get; set; }
        public RelayParameterizedCommand CutControlCommand
        {
            get
            {
                return new RelayParameterizedCommand(el =>
                {
                    //ExcelCutService execl = new ExcelCutService();
                    //execl.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.xls", MaterialDataViews);

                    //20220624 張燕華 新增excel檔案儲存完成提示
                    //ExcelCutService execl = new ExcelCutService();
                    //var stringFilePath = $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.xls";
                    //execl.CreateFile(stringFilePath, MaterialDataViews);

                    //20220930 張燕華 改為word輸出報表
                    WordCutService word = new WordCutService();
                    word.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx", MaterialDataViews);

                    string current_time = DateTime.Now.ToString("yyyyMMddhhmmss");
                    try
                    {
                        var a = Process.Start(@"Word2Pdf\exe\word2pdf_test.exe", $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx {Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表_{current_time}.pdf");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    DeleteWordFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx", $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表_{current_time}.pdf");

                    WinUIMessageBox.Show(null,
                        $"切割明細表已下載",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Popup);

                    //try { File.Delete($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx"); }
                    //catch(Exception ex) { Console.WriteLine(ex.ToString()); }

                    ////WORD to PDF by Syncfusion
                    ////Loads an existing Word document
                    //WordDocument wordDocument = new WordDocument($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx", FormatType.Docx);
                    ////Initializes the ChartToImageConverter for converting charts during Word to pdf conversion
                    //wordDocument.ChartToImageConverter = new ChartToImageConverter();
                    ////Creates an instance of the DocToPDFConverter
                    //DocToPDFConverter converter = new DocToPDFConverter();
                    ////Converts Word document into PDF document
                    //PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);
                    ////Saves the PDF file 
                    //pdfDocument.Save($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.pdf");
                    ////Closes the instance of document objects
                    //pdfDocument.Close(true);
                    //wordDocument.Close();

                    ////word to pdf by Office
                    //using (var cvtr = new PdfConverter())
                    //{
                    //    var buff = cvtr.GetPdf(Path.Combine($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx"));
                    //    string current_time = DateTime.Now.ToString("yyyyMMddhhmmss");
                    //    File.WriteAllBytes($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表_{current_time}.pdf", buff);
                    //}




                });
            }
        }
        private static void ShowDesigner(IGridViewFactory<ColumnWrapper, RowBaseWrapper> factory)
        {
            XtraReport report = new XtraReport();
            ReportGenerationExtensions<ColumnWrapper, RowBaseWrapper>.Generate(report, factory);
            ReportDesigner reportDesigner = new ReportDesigner();
            reportDesigner.Loaded += (s, e) =>
            {
                reportDesigner.OpenDocument(report);
            };
            reportDesigner.ShowWindow(factory as FrameworkElement);
        }

        /// <summary>
        /// 呼叫列印預覽視窗
        /// </summary>
        //public ICommand BuyControlCommand { get; set; }
        public RelayParameterizedCommand BuyControlCommand
        {
            get
            {
                return new RelayParameterizedCommand(el =>
                {
                    //ExcelBuyService execl = new ExcelBuyService();
                    //execl.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.xls", MaterialDataViews);

                    //20220627 張燕華 新增excel檔案儲存完成提示
                    //ExcelBuyService execl = new ExcelBuyService();
                    //var stringFilePath = $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.xls";
                    //execl.CreateFile(stringFilePath, MaterialDataViews);

                    //20220928 張燕華 改為word輸出報表
                    WordBuyService word = new WordBuyService();
                    word.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx", MaterialDataViews);
                    
                    string current_time = DateTime.Now.ToString("yyyyMMddhhmmss");
                    try
                    {                        
                        var a = Process.Start(@"Word2Pdf\exe\word2pdf_test.exe", $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx {Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單_{current_time}.pdf");
                    }
                    catch (Exception ex)
                    { 
                        Console.WriteLine(ex.ToString()); 
                    }

                    DeleteWordFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx", $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單_{current_time}.pdf");

                    WinUIMessageBox.Show(null,
                        $"採購明細單已下載",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Popup);


                    ////WORD to PDF by Syncfusion
                    ////Loads an existing Word document
                    //WordDocument wordDocument = new WordDocument($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx", FormatType.Docx);
                    ////Initializes the ChartToImageConverter for converting charts during Word to pdf conversion
                    //wordDocument.ChartToImageConverter = new ChartToImageConverter();
                    ////Creates an instance of the DocToPDFConverter
                    //DocToPDFConverter converter = new DocToPDFConverter();
                    ////Converts Word document into PDF document
                    //PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);
                    ////Saves the PDF file 
                    //pdfDocument.Save($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.pdf");
                    ////Closes the instance of document objects
                    //pdfDocument.Close(true);
                    //wordDocument.Close();

                    ////WORD to PDF by GrapeCity
                    //var wordDoc = new GcWordDocument();
                    //var path = Path.Combine($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx");
                    //wordDoc.Load(path);
                    //using (var layout = new GcWordLayout(wordDoc))
                    //{
                    //    PdfOutputSettings pdfOutputSettings = new PdfOutputSettings();
                    //
                    //    pdfOutputSettings.CompressionLevel = CompressionLevel.Fastest;
                    //
                    //    pdfOutputSettings.ConformanceLevel = GrapeCity.Documents.Pdf.PdfAConformanceLevel.PdfA1a;
                    //
                    //    pdfOutputSettings.BackColor = Color.LightGray;
                    //
                    //    pdfOutputSettings.DocumentInfo = new GrapeCity.Documents.Pdf.DocumentInfo()
                    //
                    //    {
                    //        Author = "John",
                    //
                    //        Title = "Deforestation",
                    //
                    //        CreationDate = new GrapeCity.Documents.Pdf.PdfDateTime(DateTime.Now)
                    //
                    //    };
                    //
                    //    layout.SaveAsPdf($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.pdf", null, pdfOutputSettings);
                    //}

                    ////word to pdf by Office
                    //using (var cvtr = new PdfConverter())
                    //{
                    //    var buff = cvtr.GetPdf(Path.Combine($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx"));
                    //    string current_time = DateTime.Now.ToString("yyyyMMddhhmmss");
                    //    File.WriteAllBytes($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單_{current_time}.pdf", buff);
                    //}

                    //WinUIMessageBox.Show(null,
                    //    $"PDF已下載",
                    //    "通知",
                    //    MessageBoxButton.OK,
                    //    MessageBoxImage.Exclamation,
                    //    MessageBoxResult.None,
                    //    MessageBoxOptions.None,
                    //    FloatingMode.Popup);

                    //以Libreoffice把WORD轉換為PDF
                    //convertDocToPdf(@"C:\Program Files\LibreOffice\program", $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}", "採購明細單.docx");
                });
            }
        }
        /// <summary>
        /// 呼叫列印預覽視窗
        /// </summary>
        //public ICommand AmountControlCommand { get; set; }
        public RelayParameterizedCommand AmountControlCommand
        {
            get
            {
                return new RelayParameterizedCommand(el =>
                {
                ExcelAmountService execl = new ExcelAmountService();
                //execl.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購金額預算表.xls", MaterialDataViews);

                //20220627 張燕華 新增檔案儲存完成提示
                var stringFilePath = $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購金額預算表.xls";
                execl.CreateFile(stringFilePath, MaterialDataViews);

                WinUIMessageBox.Show(null,
                    $"檔案已下載",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                    FloatingMode.Popup);
                });
            }
        }
        /// <summary>
        /// 關閉長度配料參數
        /// </summary>
        public ICommand CloseLengthDodageCommand { get; set; } = new RelayCommand(() => 
        { 
            if (!OfficeViewModel.LengthDodageControl)
                OfficeViewModel.LengthDodageControl = true;
            else
                OfficeViewModel.LengthDodageControl = false;
        });


        /// <summary>
        /// 利用LibraOffice將Doc轉成PDF
        /// </summary>
        private void DeleteWordFile(string WordFilePath, string PdfFilePath)
        {
            //底線的寫法代表使用lambda時，如果不使用到傳入參數，就要打一個底線。
            System.Threading.Tasks.Task.Delay(4000).ContinueWith(_ => 
            {
                if (File.Exists(PdfFilePath))
                {
                    try { File.Delete(WordFilePath); }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
            });  
        }
    }


}
