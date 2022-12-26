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

                if (SteelAttrHeight > 1050
                    || SteelAttrHeight < 150)
                    return true;

                var SteelAttrWidth = (value as WPFSTD105.Attribute.SteelAttr).W;

                if (SteelAttrWidth > 500 
                    || SteelAttrWidth < 75)
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

    internal class SectionTypeToEnabledConverter : BaseValueConverter<SectionTypeToEnabledConverter>
    {
        public int fSectionTypeTillBOX = 0;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (fSectionTypeTillBOX == 0)
            {
                if ((string)value == "BOX") 
                { fSectionTypeTillBOX = 1; }
                return true;
            }
            else
            {
                if ((string)value == "CH") 
                { fSectionTypeTillBOX = 0; }
                return false;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
