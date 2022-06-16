using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase
{
    /// <summary>
    /// 有參數非同步命令
    /// </summary>
    public class ParameterizedCommandAync : AbsCommandAync
    {
        private readonly Func<object,Task> _Command;
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="command"></param>
        public ParameterizedCommandAync(Func<object, Task> command)
        {
            _Command = command;
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override Task ExecuteAsync(object parameter)
        {
            return _Command(parameter);
        }
    }
}
