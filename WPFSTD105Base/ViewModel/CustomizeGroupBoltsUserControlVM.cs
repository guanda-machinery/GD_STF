using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFSTD105.Attribute;

namespace WPFSTD105
{
    public class CustomizeGroupBoltsUserControlVM : WPFWindowsBase.BaseViewModel 
    {
        public CustomizeGroupBoltsUserControlVM()
        {
            _GroupBoltsAttr = new GroupBoltsAttr();
        }

        public GroupBoltsAttr _GroupBoltsAttr { get; set; } 

        public ICommand SaveGroupBoltsCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                  var b =  _GroupBoltsAttr;

                });
            }
        }
        public ICommand DeleteGroupBoltsCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {

                });
            }
        }


    }
}
