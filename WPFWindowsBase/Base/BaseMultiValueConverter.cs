using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFWindowsBase
{

    /// <summary>
    /// 一個基本自訂邏輯器，允許直接使用XAML
    /// </summary>
    /// <typeparam name="T"> 該值轉換器的類型</typeparam>
    public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        #region 私有方法
        /// <summary>
        /// 此值轉換器的單個靜態實例
        /// </summary>
        private static T _Converter = null;
        #endregion

        #region 標記擴展方法
        /// <summary>
        /// 提供值轉換器的靜態實例
        /// </summary>
        /// <param name="serviceProvider">服務提供者</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _Converter ?? (_Converter = new T());
        }
        #endregion

        /// <summary>
        /// 將一種類型轉換為另一種類型的方法
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        /// <summary>
        /// 將值轉換回源類型的方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);

    }
}
