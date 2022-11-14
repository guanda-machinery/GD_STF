using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 機台功能VM
    /// </summary>
    public class MachineFunctionVM : WPFWindowsBase.BaseViewModel
    {

        private const string Hsteel_Clamp_TabItemName = "HSteel_Clamp_TabItem";
        private const string HSteel_Grab_TabItemName = "HSteel_Grab_TabItem";
        private const string Transport_TabItemName = "Transport_TabItem";
        private const string Transport_by_Robot_TabItemName = "Transport_by_Robot_TabItem";
        private const string ToolMagazine_TabItemName = "ToolMagazine_TabItem";
        private const string Crumb_Conveyor_TabItemName = "Crumb_Conveyor_TabItem";

        public TabItem TabItemSelected
        {
            //會在切換頁面的時候切換CodesysMemor
            set
            {
                if (value != null)
                {
                    GD_STD.PanelButton PButton = ViewLocator.ApplicationViewModel.PanelButton;
                    ClearPButtonModeValue(ref PButton);

                    switch (value.Name)
                    {        
                        case Hsteel_Clamp_TabItemName:
                     
                            break;
                        case HSteel_Grab_TabItemName:

                            break;
                        case Transport_TabItemName:

                            break;
                        case Transport_by_Robot_TabItemName:

                            break;
                        case ToolMagazine_TabItemName:

                           
                            break;
                        case Crumb_Conveyor_TabItemName:

                            break;
                        default:

                            break;
                    }


                    CodesysIIS.WriteCodesysMemor.SetPanel(PButton);
                }
            }
        }


        private void ClearPButtonModeValue(ref GD_STD.PanelButton  PButton)
        {
            PButton.MainAxisMode = false;
            //PButton. = false;
        }

    }
}
