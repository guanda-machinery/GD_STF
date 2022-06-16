using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace STD_105.Office
{
    class EnvironmentVM
    {
#pragma warning disable CS0649 // 從未指派欄位 'EnvironmentVM.IsCodesysOK'，會持續使用其預設值 false
        public static bool IsCodesysOK;
#pragma warning restore CS0649 // 從未指派欄位 'EnvironmentVM.IsCodesysOK'，會持續使用其預設值 false
        public static bool IsServerOK;
        /// <summary>
        /// 檢查更新
        /// </summary>
        public ICommand CheckUpdate { get; set; }
        /// <summary>
        /// 下載更新
        /// </summary>
        public ICommand DownLoadUpdate { get; set; }
    }

    public class StatusDataContext : DependencyObject
    {
        public EnvironmentStatus CurrentStatus
        {
            get => (EnvironmentStatus)GetValue(CurrectStateProperty);
            set => SetValue(CurrectStateProperty, value);
        }
        public static readonly DependencyProperty CurrectStateProperty =
            DependencyProperty.Register("CurrentStatus", typeof(EnvironmentStatus), typeof(StatusDataContext), new UIPropertyMetadata(EnvironmentStatus.CheckingUpdate));
    }
    public enum EnvironmentStatus
    {
        CheckingUpdate,
        Downloading,
        Installing,
        CheckingCodesys,
        CheckingServer,
    }
}
