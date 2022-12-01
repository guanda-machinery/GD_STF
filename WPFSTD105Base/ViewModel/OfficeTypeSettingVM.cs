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
using Microsoft.Office.Interop.Word;
using WordApplication = Microsoft.Office.Interop.Word.Application;
using DevExpress.Mvvm;

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

        }
        /// <summary>
        /// 報表下載等待時間的提示
        /// </summary>
        private static SplashScreenManager manager = SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });

        public static void ActivateLoading()
        {
            manager.ViewModel.Status = "報表產生中";
            manager.Show(null, WindowStartupLocation.CenterScreen, true, InputBlockMode.Window);
        }

        public static void DeactivateLoading()
        {
            manager.Close();
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

        public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }
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
                    string current_time = DateTime.Now.ToString("yyyyMMdd");

                    //刪除目前已有WINWORD程序, 以避免發生不可預期的錯誤
                    try
                    {
                        DeleteWordFileAfterDelay(100, "", "");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    //ExcelCutService execl = new ExcelCutService();
                    //execl.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.xls", MaterialDataViews);

                    //20220624 張燕華 新增excel檔案儲存完成提示
                    //ExcelCutService execl = new ExcelCutService();
                    //var stringFilePath = $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.xls";
                    //execl.CreateFile(stringFilePath, MaterialDataViews);

                    //20220930 張燕華 改為word輸出報表
                    WordCutService word = new WordCutService();
                    double TotalLoss_BothSide = MatchSettingStartCut + MatchSettingEndCut;//素材前後端切割損耗

                    ActivateLoading();
                    try
                    {
                        word.CreateFile($@"{CommonViewModel.ProjectName}", $@"{CommonViewModel.ProjectProperty.Number}", $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx", MaterialDataViews, TotalLoss_BothSide);
                    }
                    catch (Exception ex)
                    {
                        DeactivateLoading();
                        Console.WriteLine(ex.ToString());

                        WinUIMessageBox.Show(null,
                        $"寫入切割明細表發生錯誤，請確認是否已關閉切割明細表",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Popup);
                    }

                    //string current_time = DateTime.Now.ToString("yyyyMMdd");

                    try 
                    { 
                        //Word轉PDF
                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                        wordDocument = appWord.Documents.Open($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx");
                        wordDocument.ExportAsFixedFormat($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表_{current_time}.pdf", WdExportFormat.wdExportFormatPDF);
                        wordDocument.Close(false);

                        DeactivateLoading();

                        WinUIMessageBox.Show(null,
                        $"切割明細表已下載",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Popup);
                    }
                    catch (Exception ex)
                    {
                        DeactivateLoading();
                        Console.WriteLine(ex.ToString());

                        WinUIMessageBox.Show(null,
                        $"產生切割明細表PDF檔發生錯誤",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Popup);
                    }

                    //刪除已產生的報表WORD檔&目前所有的WINWORD程序
                    try
                    {
                        DeleteWordFileAfterDelay(3000, $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx", $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表_{current_time}.pdf");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                    ////以python封裝exe執行轉檔
                    //try
                    //{
                    //    string para = $@"""{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.docx"" ""{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表_{current_time}.pdf""";
                    //    var a = Process.Start(@"Word2Pdf\exe\word2pdf_test.exe", para);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.ToString());
                    //}

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
                    string current_time = DateTime.Now.ToString("yyyyMMdd");

                    //刪除目前已有WINWORD程序, 以避免發生不可預期的錯誤
                    try
                    {
                        DeleteWordFileAfterDelay(100, "", "");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                    //ExcelBuyService execl = new ExcelBuyService();
                    //execl.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.xls", MaterialDataViews);

                    //20220627 張燕華 新增excel檔案儲存完成提示
                    //ExcelBuyService execl = new ExcelBuyService();
                    //var stringFilePath = $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.xls";
                    //execl.CreateFile(stringFilePath, MaterialDataViews);

                    //20220928 張燕華 改為word輸出報表
                    WordBuyService word = new WordBuyService();
                    double TotalLoss_BothSide = MatchSettingStartCut + MatchSettingEndCut;//素材前後端切割損耗

                    ActivateLoading();
                    try
                    {
                        word.CreateFile($@"{CommonViewModel.ProjectName}", $@"{CommonViewModel.ProjectProperty.Number}", $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx", MaterialDataViews, TotalLoss_BothSide);
                    }
                    catch (Exception ex)
                    {
                        DeactivateLoading();
                        Console.WriteLine(ex.ToString());

                        WinUIMessageBox.Show(null,
                        $"寫入採購明細單發生錯誤，請確認是否已關閉採購明細單",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Popup);
                    }

                    //string current_time = DateTime.Now.ToString("yyyyMMdd");

                    try
                    {
                        //Word轉PDF
                        Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                        wordDocument = appWord.Documents.Open($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx");
                        wordDocument.ExportAsFixedFormat($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單_{current_time}.pdf", WdExportFormat.wdExportFormatPDF);
                        wordDocument.Close(false);

                        DeactivateLoading();

                        WinUIMessageBox.Show(null,
                        $"採購明細單已下載",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Popup);
                    }
                    catch (Exception ex)
                    {
                        DeactivateLoading();
                        Console.WriteLine(ex.ToString());

                        WinUIMessageBox.Show(null,
                        $"產生採購明細單PDF檔發生錯誤",
                        "通知",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation,
                        MessageBoxResult.None,
                        MessageBoxOptions.None,
                        FloatingMode.Popup);
                    }

                    //刪除已產生的報表WORD檔&目前所有的WINWORD程序
                    try
                    {
                        DeleteWordFileAfterDelay(3000, $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx", $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單_{current_time}.pdf");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }


                    ////以python封裝exe執行轉檔                    
                    //try
                    //{
                    //    string para = $@"""{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx"" ""{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單_{current_time}.pdf""";
                    //    var a = Process.Start(@"Word2Pdf\exe\word2pdf_test.exe", para);
                    //}
                    //catch (Exception ex)
                    //{ 
                    //    Console.WriteLine(ex.ToString()); 
                    //}

                    ////word to pdf by Office
                    //using (var cvtr = new PdfConverter())
                    //{
                    //    var buff = cvtr.GetPdf(Path.Combine($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.docx"));
                    //    //string current_time = DateTime.Now.ToString("yyyyMMddhhmmss");
                    //    File.WriteAllBytes($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單_{current_time}.pdf", buff);
                    //}
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

        private WordApplication m_word;
        /// <summary>
        /// 背景執行緒等待設定時間後，檢查PDF已存在的話，則刪除原本的WORD檔
        /// </summary>
        private void DeleteWordFileAfterDelay(int Delay_ms, string WordFilePath, string PdfFilePath)
        {
            //底線的寫法代表使用lambda時，如果不使用到傳入參數，就要打一個底線。
            System.Threading.Tasks.Task.Delay(Delay_ms).ContinueWith(_ => 
            {
                if (WordFilePath != "")
                {
                    if (File.Exists(PdfFilePath))
                    {
                        //刪除word檔案
                        try
                        {
                            File.Delete(WordFilePath);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                    }
                }
                //刪除存留的word程序
                try
                {
                    Process[] processes = System.Diagnostics.Process.GetProcessesByName("WINWORD");

                    foreach (System.Diagnostics.Process process in processes)
                    {
                        if (process.MainWindowTitle == "")
                        {
                            process.Kill();

                            System.Threading.Thread.Sleep(500);
                        }
                        GC.Collect();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            });  
        }
    }


}
