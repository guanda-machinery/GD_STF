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
            OpenPreviewCommand = OpenPreview();
            ExpandTableDetailCommand = ExpandTableDetail();
            SaveTypeSettingsModifyCommand = SaveTypeSettingsModify();
            CutControlCommand = CutControl();
            BuyControlCommand = BuyControl();
            AmountControlCommand = AmountControl();
        }
        ///// <inheritdoc/>
        //protected override RelayCommand Auto()
        //{
        //    throw new NotImplementedException();
        //}
        /// <inheritdoc/>
        protected override RelayCommand Manual()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 呼叫列印預覽視窗
        /// </summary>
        public ICommand OpenPreviewCommand { get; set; }
        private RelayParameterizedCommand OpenPreview()
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
        /// <summary>
        /// 收折結果報表
        /// </summary>
        public ICommand ExpandTableDetailCommand { get; set; }
        private RelayParameterizedCommand ExpandTableDetail()
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

        /// <summary>
        /// 儲存排版設定變更
        /// </summary>
        public ICommand SaveTypeSettingsModifyCommand { get; set; }
        private RelayParameterizedCommand SaveTypeSettingsModify()
        {
            return new RelayParameterizedCommand(el =>
            {
                GridControl gridControl = el as GridControl;
                ShowDesigner(gridControl.View as TableView);
            });
        }

        /// <summary>
        /// 呼叫列印預覽視窗
        /// </summary>
        public ICommand CutControlCommand { get; set; }
        private RelayParameterizedCommand CutControl()
        {
            return new RelayParameterizedCommand(el =>
            {
                ExcelCutService execl = new ExcelCutService();
                //execl.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.xls", MaterialDataViews);

                //20220624 張燕華 新增檔案儲存完成提示
                var stringFilePath = $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\切割明細表.xls";
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
        public ICommand BuyControlCommand { get; set; }
        private RelayParameterizedCommand BuyControl()
        {
            return new RelayParameterizedCommand(el =>
            {
                ExcelBuyService execl = new ExcelBuyService();
                //execl.CreateFile($@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.xls", MaterialDataViews);

                //20220627 張燕華 新增檔案儲存完成提示
                var stringFilePath = $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\採購明細單.xls";
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
        /// <summary>
        /// 呼叫列印預覽視窗
        /// </summary>
        public ICommand AmountControlCommand { get; set; }
        private RelayParameterizedCommand AmountControl()
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
    }
}
