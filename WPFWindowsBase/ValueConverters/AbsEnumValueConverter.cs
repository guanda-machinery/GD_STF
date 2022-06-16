using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFWindowsBase
{
   public abstract  class AbsEnumValueConverter : IValueConverter
    {
        public abstract Type GetType();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum format)
            {
                return GetString(format);
            }
            return null;
        }
        public string[] Strings => GetStrings();

        public static string GetString(Enum format)
        {
            return GetDescription(format);
        }

        public static string GetDescription(Enum format)
        {
            return format.GetType().GetMember(format.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description;
        }
        public string[] GetStrings()
        {
            List<string> list = new List<string>();
            foreach (Enum format in Enum.GetValues(GetType()))
            {
                list.Add(GetString(format));
            }

            return list.ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
