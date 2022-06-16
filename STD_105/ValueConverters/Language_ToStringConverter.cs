using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Enum;

namespace STD_105
{
    public class Language_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<Language>
    {
        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
