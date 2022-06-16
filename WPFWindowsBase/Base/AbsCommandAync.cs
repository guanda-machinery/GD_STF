using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFWindowsBase
{
    /// <summary>
    /// 抽象的非同步命令
    /// </summary>
    public abstract class AbsCommandAync : IAsyncCommand
    {
        /// <inheritdoc/>
        public abstract bool CanExecute(object parameter);
        /// <inheritdoc/>
        public abstract Task ExecuteAsync(object parameter);
        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
        /// <inheritdoc/>
        /// <inheritdoc/>
        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        
        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
