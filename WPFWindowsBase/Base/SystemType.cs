using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WPFWindowsBase
{
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SystemType' 的 XML 註解
    public class SystemType : MarkupExtension
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SystemType' 的 XML 註解
    {
        private object parameter;

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SystemType.Bool' 的 XML 註解
        public bool Bool { set { parameter = value; } }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SystemType.Bool' 的 XML 註解
        // add more as needed here

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'SystemType.ProvideValue(IServiceProvider)' 的 XML 註解
        public override object ProvideValue(IServiceProvider serviceProvider)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'SystemType.ProvideValue(IServiceProvider)' 的 XML 註解
        {
            return parameter;
        }
    }
}
