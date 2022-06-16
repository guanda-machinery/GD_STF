using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFWindowsBase
{
    /// <summary>
    /// 非同步命令
    /// </summary>
    [ImplementPropertyChanged]
    public class CommandAync : AbsCommandAync
    {
        private readonly Func<Task> _Command;
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="command"></param>
        public CommandAync(Func<Task> command)
        {
            _Command = command;
        }
        /// <inheritdoc/>
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        /// <inheritdoc/>
        public override Task ExecuteAsync(object parameter)
        {
            return _Command();
        }
    }
}
