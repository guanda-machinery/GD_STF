using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace STD_105
{
    internal class DatetimeArrayToLastestDatetimeConverter : BaseValueConverter<DatetimeArrayToLastestDatetimeConverter>
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsReviseTime = false;
            if(parameter is string)
            {
                if((parameter as string).ToLower().Contains("revise"))
                    IsReviseTime = true;
            }



            if (value == null)
                return null;

            if (value is IEnumerable<GD_STD.Data.TypeSettingDataView>)
            {
                if ((value as IEnumerable<GD_STD.Data.TypeSettingDataView>).Count() == 0)
                {
                    return null;
                }

                var CreationTime = new DateTime();
                var ReviseTime = new DateTime();
                foreach (var dt in value as IEnumerable<GD_STD.Data.TypeSettingDataView>)
                {
                    if (dt.Creation > CreationTime)
                    {
                        CreationTime = dt.Creation;
                    }

                    if (dt.Revise > ReviseTime)
                    {
                        ReviseTime = dt.Revise;
                    }
                }


                var RetrunTime =  IsReviseTime ?  ReviseTime: CreationTime;

                if(RetrunTime == new DateTime())
                {
                    return null;
                }
                else
                {
                    return RetrunTime.ToShortDateString();
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
