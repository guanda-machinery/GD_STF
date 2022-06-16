using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using WPFWindowsBase.AttributeUsage;
namespace WPFWindowsBase.DataGridLibrary.Support
{
    /// <summary>
    /// <see cref="Enum"/>顯示
    /// </summary>
    [ContentProperty("OverriddenDisplayEntries")]
    public class EnumDisplayer : IValueConverter
    {
        #region 私有
        /// <summary>
        /// 類型
        /// </summary>
        private Type type;
        /// <summary>
        /// 顯示列表
        /// </summary>
        private IDictionary displayValues { get; set; }
        /// <summary>
        /// 逆轉列表
        /// </summary>
        private IDictionary reverseValues { get; set; }

        private List<EnumDisplayEntry> overriddenDisplayEntries;
        #endregion

        public EnumDisplayer()
        {

        }
        public EnumDisplayer(Type type)
        {
            this.Type = type;
        }
        /// <summary>
        /// 類型
        /// </summary>
        public Type Type
        {
            get { return type; }
            set
            {
                if (!value.IsEnum)
                    throw new ArgumentException("參數不是列舉類型", "value");
                this.type = value;
            }
        }
        public ReadOnlyCollection<string> DisplayName
        {
            get
            {
                Type type = typeof(Dictionary<,>).GetGenericTypeDefinition().MakeGenericType(this.type, typeof(string));
                this.displayValues = (IDictionary)Activator.CreateInstance(type);

                this.reverseValues = (IDictionary)Activator.CreateInstance(
                    typeof(Dictionary<,>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(typeof(string), this.type));

                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

                foreach (var field in fields)
                {
                    DisplayStringAttribute[] a = (DisplayStringAttribute[])
                                                field.GetCustomAttributes(typeof(DisplayStringAttribute), false);

                    string displayString = GetDisplayStringValue(a);
                    object enumValue = field.GetValue(null);

                    if (displayString == null)
                    {
                        displayString = GetBackupDisplayStringValue(enumValue);
                    }
                    if (displayString != null)
                    {
                        displayValues.Add(enumValue, displayString);
                        reverseValues.Add(displayString, enumValue);
                    }
                }
                return new List<string>((IEnumerable<string>)displayValues.Values).AsReadOnly();
            }
        }
        /// <summary>
        /// 獲取顯示字串值
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private string GetDisplayStringValue(DisplayStringAttribute[] a)
        {
            if (a == null || a.Length == 0) return null;
            DisplayStringAttribute dsa = a[0];
            if (!string.IsNullOrEmpty(dsa.ResourceKey))
            {
                ResourceManager rm = new ResourceManager(type);
                return rm.GetString(dsa.ResourceKey);
            }
            return dsa.Value;
        }
        /// <summary>
        /// 獲取備份顯示字符串值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        private string GetBackupDisplayStringValue(object enumValue)
        {
            if (overriddenDisplayEntries != null && overriddenDisplayEntries.Count > 0)
            {
                EnumDisplayEntry foundEntry = overriddenDisplayEntries.Find(delegate (EnumDisplayEntry entry)
                {
                    object e = Enum.Parse(type, entry.EnumValue);
                    return enumValue.Equals(e);
                });
                if (foundEntry != null)
                {
                    if (foundEntry.ExcludeFromDisplay) return null;
                    return foundEntry.DisplayString;

                }
            }
            return Enum.GetName(type, enumValue);
        }
        /// <summary>
        /// 複寫顯示項
        /// </summary>
        public List<EnumDisplayEntry> OverriddenDisplayEntries
        {
            get
            {
                if (overriddenDisplayEntries == null)
                    overriddenDisplayEntries = new List<EnumDisplayEntry>();
                return overriddenDisplayEntries;
            }
        }
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return displayValues[value];
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return reverseValues[value];
        }
    }
    public class EnumDisplayEntry
    {
        public string EnumValue { get; set; }
        public string DisplayString { get; set; }
        public bool ExcludeFromDisplay { get; set; }
    }
}
