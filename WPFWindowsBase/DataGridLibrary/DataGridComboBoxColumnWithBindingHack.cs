using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFWindowsBase.DataGridLibrary
{
    /// <summary>
    /// 具有綁定Hack的DataGrid ComboBox列
    /// </summary>
    public class DataGridComboBoxColumnWithBindingHack : DataGridComboBoxColumn
    {
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement element = base.GenerateEditingElement(cell, dataItem);
            CopyItemsSource(element);
            return element;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement element = base.GenerateElement(cell, dataItem);
            CopyItemsSource(element);
            return element;
        }

        private void CopyItemsSource(FrameworkElement element)
        {
            BindingOperations.SetBinding(element, ComboBox.ItemsSourceProperty,
              BindingOperations.GetBinding(this, ComboBox.ItemsSourceProperty));
        }
    }
}
