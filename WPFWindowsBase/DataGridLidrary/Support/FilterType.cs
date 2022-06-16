using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWindowsBase
{
    /// <summary>
    ///對應於FilterCurrentData模板（DataTemplate）
   ///在Generic.xaml中定義的DataGridColumnFilter
    /// </summary>
    public enum FilterType
    {
        Numeric,
        NumericBetween,
        Text,
        List,
        Boolean,
        DateTime,
        DateTimeBetween
    }
}
