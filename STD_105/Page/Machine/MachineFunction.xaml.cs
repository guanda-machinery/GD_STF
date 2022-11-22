using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WPFSTD105;
using WPFSTD105.ViewModel;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// MachineFunction.xaml 的互動邏輯
    /// </summary>
    public partial class MachineFunction : BasePage
    {

        public MachineFunction()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 離開頁面時清除所有按鈕
        /// </summary>
        ~MachineFunction()
        {
            GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
            PButton.ClampDown = false;
            PButton.SideClamp = false;
            PButton.EntranceRack = false;
            PButton.ExportRack = false;
            PButton.Hand = false;
            PButton.DrillWarehouse = false;
            PButton.Volume = false;
            PButton.MainAxisMode = false;
            CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
        }

    }
}
