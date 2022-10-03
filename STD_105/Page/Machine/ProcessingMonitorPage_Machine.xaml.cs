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

namespace STD_105
{
    /// <summary>
    /// 新加工監控 ProcessingMonitorPage_Machine.xaml 的互動邏輯
    /// </summary>
    public partial class ProcessingMonitorPage_Machine : BasePage<ProcessingMonitorVM>
    { 
        public ProcessingMonitorPage_Machine()
        {
            InitializeComponent();

         }
        private void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IDisposable disposable)
            {
                //model.Dispose();
                disposable.Dispose();
            }
        }


        
        private void ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if ((sender as DevExpress.Xpf.Grid.TableView).Name == MachiningCombinationl_List_TableView.Name)
            {
                IScrollInfo SoftCountTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(MachiningSchedule_List_TableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                if (SoftCountTableView_ScrollElement != null)
                    SoftCountTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
            }
            if ((sender as DevExpress.Xpf.Grid.TableView).Name == MachiningSchedule_List_TableView.Name)
            {
                IScrollInfo PartsTableView_ScrollElement = (DataPresenter)LayoutHelper.FindElement(LayoutHelper.FindElementByName(MachiningCombinationl_List_TableView, "PART_ScrollContentPresenter"), (el) => el is DataPresenter);
                if (PartsTableView_ScrollElement != null)
                    PartsTableView_ScrollElement.SetVerticalOffset(e.VerticalOffset);
            }
        }



    }
}
