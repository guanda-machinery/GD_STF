using System;
using System.Windows;

namespace WPFWindowsBase
{
    /// <summary>
    ///一個基本附加屬性，以替換 windows WPF附加屬性
    /// </summary>
    /// <typeparam name="Parent">父類將作為附加屬性</typeparam>
    /// <typeparam name="Property">此附加屬性的類型</typeparam>
    public abstract class BaseAttachedProperty<Parent, Property>
        where Parent : new()
    {
        #region 公用事件

        /// <summary>
        /// 值更改時觸發
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        /// <summary>
        /// 值更改時觸發，即使值相同
        /// </summary>
        public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };

        #endregion

        #region 公用屬性

        /// <summary>
        /// 父類的單例實例
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();

        #endregion

        #region 附加屬性定義

        /// <summary>
        /// 此類的附加屬性
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(Property),
            typeof(BaseAttachedProperty<Parent, Property>),
            new UIPropertyMetadata(
                default(Property),
                new PropertyChangedCallback(OnValuePropertyChanged),
                new CoerceValueCallback(OnValuePropertyUpdated)
                ));

        /// <summary>
        /// 更改<see cref =" ValueProperty" />時的回調事件
        /// </summary>
        /// <param name="d">屬性已更改的UI元素</param>
        /// <param name="e">事件的參數</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (Instance as BaseAttachedProperty<Parent, Property>)?.OnValueChanged(d, e);

            (Instance as BaseAttachedProperty<Parent, Property>)?.ValueChanged(d, e);
        }

        /// <summary>
        /// 更改<see cref ="ValueProperty"/>時的回調事件，即使其值相同
        /// </summary>
        /// <param name="d">屬性已更改的UI元素</param>
        /// <param name="value">事件的參數</param>
        private static object OnValuePropertyUpdated(DependencyObject d, object value)
        {
            (Instance as BaseAttachedProperty<Parent, Property>)?.OnValueUpdated(d, value);

            (Instance as BaseAttachedProperty<Parent, Property>)?.ValueUpdated(d, value);

            return value;
        }

        /// <summary>
        /// 獲取附加屬性
        /// </summary>
        /// <param name="d">從中獲取屬性的元素</param>
        /// <returns></returns>
        public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);

        /// <summary>
        /// 設置附加屬性
        /// </summary>
        /// <param name="d">從中獲取屬性的元素</param>
        /// <param name="value">將該屬性設置為的值</param>
        public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);

        #endregion

        #region 事件方法

        /// <summary>
        /// 更改此類型的任何附加屬性時調用的方法
        /// </summary>
        /// <param name="sender">更改此屬性的UI元素</param>
        /// <param name="e">此事件的參數</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        /// <summary>
        ///設置附加屬性
        /// </summary>
        /// <param name="sender">更改此屬性的UI元素</param>
        /// <param name="value">將該屬性設置為的值</param>
        public virtual void OnValueUpdated(DependencyObject sender, object value) { }

        #endregion
    }
}
