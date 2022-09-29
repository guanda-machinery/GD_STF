using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFWindowsBase
{
    /// <summary>
    /// 所有頁面的基本功能
    /// </summary>
    public class BaseUserControl : Page
    {
        /// <inheritdoc/>
        public BaseUserControl()
        {
        }

    }
    /// <summary>
    /// 具有添加的ViewModel支持的<see cref="Page"/>
    /// </summary>
    /// <typeparam name="VM"></typeparam>
    public class BaseUserControl<VM> : BaseUserControl where VM : BaseViewModel, new()
    {
        #region 私用方法
        /// <summary>
        /// 與此頁面關聯的視圖模型
        /// </summary>
        private VM _ViewModel { get; set; }
        #endregion
        #region 公共屬性
        /// <summary>
        /// 與此頁面關聯的視圖模型
        /// </summary>
        public VM ViewModel
        {
            get
            {
                return _ViewModel;
            }
            set
            {
                //如果沒有任何變化，則返回
                if (_ViewModel == value)
                    return;
                //更新值
                _ViewModel = value;
                //設置此頁面的數據上下文
                this.DataContext = _ViewModel;
            }
        }
        #endregion

        #region 建構式
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseUserControl() : base()
        {
            this.ViewModel = new VM();
        }
        #endregion
    }
}
