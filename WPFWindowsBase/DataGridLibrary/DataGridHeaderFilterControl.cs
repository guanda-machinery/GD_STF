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
    /// DataGrid標頭過濾器控件
    /// </summary>
    public class DataGridHeaderFilterControl : Control
    {
        static DataGridHeaderFilterControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridHeaderFilterControl), new FrameworkPropertyMetadata(typeof(DataGridHeaderFilterControl)));
        }
    }
}
