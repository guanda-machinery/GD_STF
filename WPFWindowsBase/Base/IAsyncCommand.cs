using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFWindowsBase
{
    /// <summary>
    /// 定義非同步命令
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// 運行非同步動作的基本命令(預設)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task ExecuteAsync(object parameter);
    }
}
