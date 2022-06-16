using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase
{
    ///// <summary>
    ///// 有參數非同步命令
    ///// </summary>
    //public class AyncCommand<TResult> : AbsAyncCommand, INotifyPropertyChanged
    //{
    //    private readonly Func<Task<TResult>> _command;
    //    private NotifyTaskCompletion<TResult> _execution;
    //    public AsyncCommand(Func<Task<TResult>> command)
    //    {
    //        _command = command;
    //    }
    //    public override bool CanExecute(object parameter)
    //    {
    //        return true;
    //    }
    //    public override Task ExecuteAsync(object parameter)
    //    {
    //        Execution = new NotifyTaskCompletion(_command());
    //        return Execution.TaskCompletion;
    //    }
    //    // Raises PropertyChanged
    //    public NotifyTaskCompletion<TResult> Execution { get; private set; }
    //}
}
