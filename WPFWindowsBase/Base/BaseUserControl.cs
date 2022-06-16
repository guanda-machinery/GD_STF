using System.Windows.Controls;

namespace WPFWindowsBase
{
    /// <summary>
    /// 具有添加的ViewModel支持的<see cref="UserControl"/>
    /// </summary>
    /// <typeparam name="VM">類型</typeparam>
    public class BaseUserControl<VM> : UserControl where VM : BaseViewModel, new()
    {
        private VM _ViewModel { get; set; }
        #region 公用屬性
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'BaseUserControl<VM>.ViewModel' 的 XML 註解
        public VM ViewModel
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'BaseUserControl<VM>.ViewModel' 的 XML 註解
        {
            get
            {
                return _ViewModel;
            }
            set
            {
                if (_ViewModel == value)
                    return;
                //更新值
                _ViewModel = value;
                //自動更新資料繫結
                this.DataContext = _ViewModel;
            }
        }
        #endregion
        #region 建構式
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'BaseUserControl<VM>.BaseUserControl()' 的 XML 註解
        public BaseUserControl()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'BaseUserControl<VM>.BaseUserControl()' 的 XML 註解
        {
            this.ViewModel = new VM();
        }
        #endregion
    }
}
