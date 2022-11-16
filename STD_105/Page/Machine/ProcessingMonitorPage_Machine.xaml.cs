using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using GD_STD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFWindowsBase;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using static WPFSTD105.CodesysIIS;
using devDept.Eyeshot;
using WPFSTD105.ViewModel;
using DevExpress.Xpf.Grid;
using System.Windows.Controls.Primitives;
using DevExpress.Xpf.Core.Native;
using MouseButton = devDept.Eyeshot.MouseButton;
using System.Web.UI.WebControls;
using DevExpress.XtraSplashScreen;

namespace STD_105
{
    /// <summary>
    /// 新加工監控 ProcessingMonitorPage_Machine.xaml 的互動邏輯
    /// </summary>
    public partial class ProcessingMonitorPage_Machine : BasePage<ProcessingMonitorVM>
    {


     //   public SplashScreenManager ScreenManager { get; set; } = SplashScreenManager.Create(() => new WaitIndicator(), new DevExpress.Mvvm.DXSplashScreenViewModel { });

        public ObSettingVM ViewModel { get; set; } = new ObSettingVM();
        /// <summary>
        /// nc 設定檔
        /// </summary>
        //public NcTemp NcTemp { get; set; }
        //public string DataPath { get; set; }
        public List<List<object>> DataList { get; set; }

        public ProcessingMonitorPage_Machine()
        {
            InitializeComponent();

            model.DataContext = ViewModel;
            drawing.DataContext = ViewModel;
            model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            drawing.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
            model.ActionMode = actionType.None;
            DataList = new List<List<object>>();
            drawing.LineTypes.Add(Steel2DBlock.LineTypeName, new float[] { 35, -35, 35, -35 });
            model.Secondary = drawing;
            drawing.Secondary = model;
            //ControlDraw3D();
            //CheckReportLogoExist();

        }
        private void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IDisposable disposable)
            {
                try
                {
                    //model.Dispose();
                    disposable.Dispose();
                }
                catch(Exception ex)
                {

                }
            }
        }


        private void Model3D_Loaded(object sender, RoutedEventArgs e)
        {
            #region Model 初始化
            //model.InitialView = viewType.Top;
            /*旋轉軸中心設定當前的鼠標光標位置。 如果模型全部位於相機視錐內部，
             * 它圍繞其邊界框中心旋轉。 否則它繞著下點旋轉鼠標。 如果在鼠標下方沒有深度，則旋轉發生在
             * 視口中心位於當前可見場景的平均深度處。*/
            //model.Rotate.RotationCenter = rotationCenterType.CursorLocation;
            //旋轉視圖 滑鼠中鍵 + Ctrl
            model.Rotate.MouseButton = new MouseButton(mouseButtonsZPR.Middle, modifierKeys.Ctrl);
            //平移滑鼠中鍵
            model.Pan.MouseButton = new MouseButton(mouseButtonsZPR.Middle, modifierKeys.None);
            model.ActionMode = actionType.SelectByBox;
            if (ViewModel.Reductions == null)
            {
                ViewModel.Reductions = new ReductionList(model, drawing); //紀錄使用找操作
            }

            #endregion
        }


        private void ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if ((sender as DevExpress.Xpf.Grid.TableView).Name == MachiningCombinational_List_TableView.Name)
            {
                IScrollInfo SoftCountTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(MachiningSchedule_List_TableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                if (SoftCountTableView_ScrollElement != null)
                    SoftCountTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
            }
            if ((sender as DevExpress.Xpf.Grid.TableView).Name == MachiningSchedule_List_TableView.Name)
            {
                IScrollInfo PartsTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(MachiningCombinational_List_TableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                if (PartsTableView_ScrollElement != null)
                    PartsTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
            }
        }



    }
}
