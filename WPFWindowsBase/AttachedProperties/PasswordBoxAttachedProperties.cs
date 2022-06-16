using System.Windows;
using System.Windows.Controls;

namespace WPFWindowsBase
{
    /// <summary>
    /// <see cref="PasswordBox"/> MonitorPassword附加属性
    /// </summary>
    public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool>
    {
        /// <summary>
        ///更改此類型的任何附加屬性時調用的方法
        /// </summary>
        /// <param name="sender">更改此屬性的UI元素</param>
        /// <param name="e">此事件的參數</param>
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            //確保它是一個密碼框
            if (passwordBox == null)
                return;

            //刪除任何以前的事件
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            //如果調用者將MonitorPassword設置為true ...
            if ((bool)e.NewValue)
            {
                //設定默認值
                HasTextProperty.SetValue(passwordBox);

                //開始偵聽密碼更改
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        /// <summary>
        ///密碼框密碼值更改時觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //設置附加的HasText值
            HasTextProperty.SetValue((PasswordBox)sender);
        }
    }


    /// <summary>
    /// <see cref ="PasswordBox"/>的HasText附加屬性
    /// </summary>
    public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool>
    {
        /// <summary>
        /// 根據調用方<see cref ="PasswordBox" />是否包含任何文本來設置HasText屬性
        /// </summary>
        /// <param name="sender"></param>
        public static void SetValue(DependencyObject sender)
        {
            SetValue(sender, ((PasswordBox)sender).SecurePassword.Length > 0);
        }
    }
}
