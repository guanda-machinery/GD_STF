using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFWindowsBase.DataGridLibrary
{
    /// <summary>
    /// 數據網格列擴展
    /// </summary>
    public class DataGridColumnExtensions
    {
        public static DependencyProperty IsCaseSensitiveSearchProperty =
          DependencyProperty.RegisterAttached("IsCaseSensitiveSearch",
              typeof(bool), typeof(DataGridColumn));
        /// <summary>
        /// 取得區分大小寫的搜索
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GetIsCaseSensitiveSearch(DependencyObject target)
        {
            return (bool)target.GetValue(IsCaseSensitiveSearchProperty);
        }
        /// <summary>
        /// 設置區分大小寫的搜索
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetIsCaseSensitiveSearch(DependencyObject target, bool value)
        {
            target.SetValue(IsCaseSensitiveSearchProperty, value);
        }

        public static DependencyProperty IsBetweenFilterControlProperty =
            DependencyProperty.RegisterAttached("IsBetweenFilterControl",
                typeof(bool), typeof(DataGridColumn));
        /// <summary>
        /// 取得介於過濾器控制之間
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GetIsBetweenFilterControl(DependencyObject target)
        {
            return (bool)target.GetValue(IsBetweenFilterControlProperty);
        }
        /// <summary>
        /// 設置介於過濾器控制之間
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetIsBetweenFilterControl(DependencyObject target, bool value)
        {
            target.SetValue(IsBetweenFilterControlProperty, value);
        }
   
        public static DependencyProperty DoNotGenerateFilterControlProperty =
            DependencyProperty.RegisterAttached("DoNotGenerateFilterControl",
                typeof(bool), typeof(DataGridColumn), new PropertyMetadata(false));
        /// <summary>
        /// 獲取不生成過濾器控件
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GetDoNotGenerateFilterControl(DependencyObject target)
        {
            return (bool)target.GetValue(DoNotGenerateFilterControlProperty);
        }
        /// <summary>
        /// 設置不生成過濾器控件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetDoNotGenerateFilterControl(DependencyObject target, bool value)
        {
            target.SetValue(DoNotGenerateFilterControlProperty, value);
        }
    }
}
