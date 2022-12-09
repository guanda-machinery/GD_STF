using DevExpress.Office.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STD_105
{
    internal class MaterialGridControlDataCheckConverter : WPFWindowsBase.BaseValueConverter<MaterialGridControlDataCheckConverter>
    {
        /// <summary>
        /// 未完成 已廢棄
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is IEnumerable<GD_STD.Data.TypeSettingDataView>)
           {
                //找到所有準備排版的資料
                var SoftList =(value as IEnumerable<GD_STD.Data.TypeSettingDataView>).ToList().FindAll(x => (x.SortCount > 0));

                //type為可見/不可見
                if (targetType == System.Windows.Visibility.Visible.GetType())
                {

                }

                //type為布林
                if (targetType == bool.TrueString.GetType())
                {

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
