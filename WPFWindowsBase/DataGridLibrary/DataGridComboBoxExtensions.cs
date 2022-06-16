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
    /// DataGrid ComboBox擴展
    /// </summary>
    public class DataGridComboBoxExtensions
    {
        public static DependencyProperty IsTextFilterProperty =
            DependencyProperty.RegisterAttached("IsTextFilter",
                typeof(bool), typeof(DataGridComboBoxColumn));

        /// <summary>
        /// 取得Text篩選器
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GetIsTextFilter(DependencyObject target)
        {
            return (bool)target.GetValue(IsTextFilterProperty);
        }

        public static void SetIsTextFilter(DependencyObject target, bool value)
        {
            target.SetValue(IsTextFilterProperty, value);
        }


        /// <summary>
        ///  如果為true，則ComboBox.IsEditable為true，而ComboBox.IsReadOnly為false
        ///  否則
        ///  ComboBox.IsEditable 為 false and ComboBox.IsReadOnly 為 true
        /// </summary>
        public static DependencyProperty UserCanEnterTextProperty =
            DependencyProperty.RegisterAttached("UserCanEnterText",
                typeof(bool), typeof(DataGridComboBoxColumn));

        public static bool GetUserCanEnterText(DependencyObject target)
        {
            return (bool)target.GetValue(UserCanEnterTextProperty);
        }

        public static void SetUserCanEnterText(DependencyObject target, bool value)
        {
            target.SetValue(UserCanEnterTextProperty, value);
        }
    }
}
