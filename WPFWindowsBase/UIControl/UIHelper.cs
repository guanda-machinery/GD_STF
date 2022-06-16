using System.Windows;
using System.Windows.Media;

namespace WPFWindowsBase.UIControl
{
    /// <summary>
    /// WPF UI 幫手
    /// </summary>
    public static class UIHelper
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'UIHelper.FindVisualParent<T>(DependencyObject)' 的 XML 註解
        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'UIHelper.FindVisualParent<T>(DependencyObject)' 的 XML 註解
        {
            //獲取父項
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //我們到了節點的盡頭
            if (parentObject == null) return null;

            //檢查父母是否與我們尋找的類型匹配
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //使用迴遞進行下一個級別
                return FindVisualParent<T>(parentObject);
            }
        }
    }
}
