using GD_STD;
using GD_STD.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBase = WPFWindowsBase;
using WPFSTD105.Properties;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 排版設定
    /// </summary>
    public class TypeSettingVM : AbsTypeSettingVM
    {
        /// <summary>
        /// 註冊命令
        /// </summary>
        public TypeSettingVM()
        {

        }

        protected override RelayCommand Auto()
        {
            throw new NotImplementedException();
        }

        protected override RelayCommand Manual()
        {
            throw new NotImplementedException();
        }
    }
}
