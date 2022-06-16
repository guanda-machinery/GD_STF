using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFWindowsBase
{

#pragma warning disable CS1723 // XML 註解具有參考類型參數的 cref 屬性 'T'
    /// <summary>
    /// 一個基本值轉換器，允許直接使用XAML
    /// </summary>
    /// <typeparam name="T"><see cref="T"/> = 該值轉換器的類型</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter where T : class, new()
#pragma warning restore CS1723 // XML 註解具有參考類型參數的 cref 屬性 'T'
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
        #region 值轉換器的方法
        /// <summary>
        /// 將一種類型轉換為另一種類型的方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        /// <summary>
        /// 將值轉換回源類型的方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
