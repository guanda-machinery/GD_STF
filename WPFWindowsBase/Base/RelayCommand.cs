using PropertyChanged;
using System;
using System.Windows.Input;

namespace WPFWindowsBase
{
    /// <summary>
    /// 運行動作的基本命令
    /// </summary>
    [ImplementPropertyChanged]
    public class RelayCommand : ICommand
    {
        #region 私有方法

        /// <summary>
        /// 運行的動作
        /// </summary>
        private Action mAction;

        #endregion

        #region  公開事件

        /// <summary>
        /// <see cref ="CanExecute(object)"/>值更改時觸發的事件
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Constructor

        /// <summary>
        ///  運行動作的基本命令(預設)
        /// </summary>
        public RelayCommand(Action action)
        {
            mAction = action;
        }

        #endregion

        #region 命令方法

        /// <summary>
        /// 中繼命令始終可以執行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 執行命令動作
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            mAction();
        }

        #endregion
    }
}
