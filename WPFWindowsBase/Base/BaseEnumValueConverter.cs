using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFWindowsBase
{

    /// <summary>
    /// 一個基本自訂的 <see cref="Enum"/> 邏輯器，允許直接使用XAML <typeparamref name="T"/> 
    /// </summary>
    /// <typeparam name="T"> is <see cref="Enum"/></typeparam>
    public abstract class BaseEnumValueConverter<T> : MarkupExtension, IValueConverter where T : struct, IConvertible
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is T format)
            {
                return GetString(format);
            }
            return null;
        }
        /// <summary>
        /// 轉換 <typeparamref name="T"/> 列表
        /// </summary>
        public string[] Strings => GetStrings();
        /// <summary>
        /// 取得 <typeparamref name="T"/> 內容
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetString(T format)
        {
            return GetDescription(format);
        }
        /// <summary>
        /// 取得 <see cref="DescriptionAttribute.Description"/>
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetDescription(T format)
        {
            DescriptionAttribute descriptionAttribute = format.GetType().GetMember(format.ToString())[0].GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 轉換返回 <typeparamref name="T"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is T format)
            {
                return GetString(format);
            }
            return null;
        }
        /// <summary>
        /// 取得全部 <typeparamref name="T"/> 內容
        /// </summary>
        /// <returns></returns>
        public string[] GetStrings()
        {
            List<string> list = new List<string>();
            foreach (T format in Enum.GetValues(typeof(T)))
            {
                string str = GetString(format);
                if (str != null)
                {
                    list.Add(str);
                }
            }

            return list.ToArray();
        }
    }
}
