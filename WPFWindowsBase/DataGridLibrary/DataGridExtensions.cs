using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFWindowsBase.DataGridLibrary.Querying;

namespace WPFWindowsBase.DataGridLibrary
{
    /// <summary>
    /// DataGrid擴展
    /// </summary>
    public class DataGridExtensions
    {
        
        public static DependencyProperty DataGridFilterQueryControllerProperty =
            DependencyProperty.RegisterAttached("DataGridFilterQueryController",
                typeof(QueryController), typeof(DataGridExtensions));
        /// <summary>
        /// 獲取 <see cref="DataGrid"/> 過濾器查詢控制器
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static QueryController GetDataGridFilterQueryController(DependencyObject target)
        {
            return (QueryController)target.GetValue(DataGridFilterQueryControllerProperty);
        }
        /// <summary>
        /// 設置 <see cref="DataGrid"/> 過濾器查詢控制器
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static void SetDataGridFilterQueryController(DependencyObject target, QueryController value)
        {
            target.SetValue(DataGridFilterQueryControllerProperty, value);
        }

        public static DependencyProperty ClearFilterCommandProperty =
            DependencyProperty.RegisterAttached("ClearFilterCommand",
                typeof(DataGridFilterCommand), typeof(DataGridExtensions));
        /// <summary>
        /// 獲取清除過濾器命令
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DataGridFilterCommand GetClearFilterCommand(DependencyObject target)
        {
            return (DataGridFilterCommand)target.GetValue(ClearFilterCommandProperty);
        }
        /// <summary>
        /// 設置清除過濾器命令
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetClearFilterCommand(DependencyObject target, DataGridFilterCommand value)
        {
            target.SetValue(ClearFilterCommandProperty, value);
        }

        public static DependencyProperty IsFilterVisibleProperty =
            DependencyProperty.RegisterAttached("IsFilterVisible",
                typeof(bool?), typeof(DataGridExtensions),
                  new FrameworkPropertyMetadata(true));
        /// <summary>
        /// 獲取過濾器可見
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool? GetIsFilterVisible(
            DependencyObject target)
        {
            return (bool)target.GetValue(IsFilterVisibleProperty);
        }
        /// <summary>
        /// 設置過濾器可見
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetIsFilterVisible(
            DependencyObject target, bool? value)
        {
            target.SetValue(IsFilterVisibleProperty, value);
        }

        public static DependencyProperty UseBackgroundWorkerForFilteringProperty =
            DependencyProperty.RegisterAttached("UseBackgroundWorkerForFiltering",
                typeof(bool), typeof(DataGridExtensions),
                  new FrameworkPropertyMetadata(false));
        /// <summary>
        /// 獲取使用BackgroundWorker進行過濾
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GetUseBackgroundWorkerForFiltering(
            DependencyObject target)
        {
            return (bool)target.GetValue(UseBackgroundWorkerForFilteringProperty);
        }
        /// <summary>
        /// 設置使用BackgroundWorker進行過濾
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetUseBackgroundWorkerForFiltering(
            DependencyObject target, bool value)
        {
            target.SetValue(UseBackgroundWorkerForFilteringProperty, value);
        }

        public static DependencyProperty IsClearButtonVisibleProperty =
            DependencyProperty.RegisterAttached("IsClearButtonVisible",
                typeof(bool), typeof(DataGridExtensions),
                  new FrameworkPropertyMetadata(true));
        /// <summary>
        /// 獲取是清除按鈕可見
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GetIsClearButtonVisible(
            DependencyObject target)
        {
            return (bool)target.GetValue(IsClearButtonVisibleProperty);
        }
        /// <summary>
        /// 設置是清除按鈕可見
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetIsClearButtonVisible(
            DependencyObject target, bool value)
        {
            target.SetValue(IsClearButtonVisibleProperty, value);
        }
    }
}
