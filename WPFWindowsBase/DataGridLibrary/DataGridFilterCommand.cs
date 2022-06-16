using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFWindowsBase.DataGridLibrary
{
    /// <summary>
    /// <see cref="DataGrid"/>篩選器命令
    /// </summary>
    public class DataGridFilterCommand : ICommand
    {
        private readonly Action<object> action;

        public DataGridFilterCommand(Action<object> action)
        {
            this.action = action;
        }

        public void Execute(object parameter)
        {
            if (action != null) action(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
