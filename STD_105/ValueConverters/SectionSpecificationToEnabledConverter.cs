using DevExpress.Xpf.Gauges;
using DevExpress.XtraScheduler.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFWindowsBase;

namespace STD_105
{
    internal class SectionSpecificationToEnabledConverter : BaseValueConverter<SectionSpecificationToEnabledConverter>
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //分析字串 
            if (value is null)
                return value;

            if (value is WPFSTD105.Attribute.SteelAttr)
            {
             

               var SteelAttrHeight = (value as WPFSTD105.Attribute.SteelAttr).H  ;

                if (SteelAttrHeight > WPFSTD105.ViewLocator.CommonViewModel.SectionSpecificationMaxHeight
                    || SteelAttrHeight < WPFSTD105.ViewLocator.CommonViewModel.SectionSpecificationMinHeight)
                    return true;

                var SteelAttrWidth = (value as WPFSTD105.Attribute.SteelAttr).W;

                if (SteelAttrWidth > WPFSTD105.ViewLocator.CommonViewModel.SectionSpecificationMaxWidth 
                    || SteelAttrWidth < WPFSTD105.ViewLocator.CommonViewModel.SectionSpecificationMinWidth)
                    return true;

                return false;
            }
            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
