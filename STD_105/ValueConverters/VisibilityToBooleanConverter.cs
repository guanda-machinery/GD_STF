using CalcBinding.Inversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace STD_105
{
    internal class VisibilityToBooleanConverter : MarkupExtension, IValueConverter
    {
        public bool Invert { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Visibility)
            {
                var VisBoolean = false;
                switch((Visibility)value)
                {
                    case Visibility.Visible:
                        VisBoolean = true;
                        break;
                    case Visibility.Hidden:
                        VisBoolean = false;
                        break;
                    case Visibility.Collapsed:
                        VisBoolean = false;
                        break;
                    default:
                        VisBoolean = false;
                        break;
                }

                return (!Invert) ?VisBoolean : !VisBoolean;
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
