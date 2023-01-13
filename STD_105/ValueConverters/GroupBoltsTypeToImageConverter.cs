using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using WPFWindowsBase;

namespace STD_105
{
    internal class GroupBoltsTypeToImageConverter : BaseValueConverter<GroupBoltsTypeToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var FunctionLock = App.Current.TryFindResource("FunctionLock");

            if (value is DevExpress.Mvvm.EnumMemberInfo)
            {
                if ((value as DevExpress.Mvvm.EnumMemberInfo).Id is GroupBoltsType)
                {
                    GroupBoltsType groupBoltsType= (GroupBoltsType)(value as DevExpress.Mvvm.EnumMemberInfo).Id;
                    switch (groupBoltsType)
                    {
                        case (GroupBoltsType.Rectangle):
                            return FunctionLock;
                        case (GroupBoltsType.DisalignmentLeft):
                            return null;
                        case (GroupBoltsType.DisalignmentRight):
                            return null;
                        case (GroupBoltsType.HypotenuseLeft):
                            return null;
                        case (GroupBoltsType.HypotenuseRight):
                            return null;
                        default:
                            return null;
                    }
                }
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
