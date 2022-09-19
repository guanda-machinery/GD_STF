using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFSTD105
{
    #region 尋找所有子控制項
    /// <summary>
    /// 20220831 尋找在控制元件中找尋所有符合條件的控制項
    /// </summary>
    public class GetWpfLogicalChildClass
    {
        /// <summary>
        /// 設定控制元件內所有子控制元件checkBox全勾選/全取消 , parent為母控制元件
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool SetAllCheckBoxTrueOrFalse(object parent)
        {
            try
            {
                //勾選邏輯->checkbox中存在任意未勾選時，按鈕為全勾選
                //當checkbox為全勾選時，才執行全部取消勾選
                var AllCheckBoxList = GetWpfLogicalChildClass.GetLogicalChildCollection<System.Windows.Controls.CheckBox>(parent);
                var AllCheckboxBoolen = AllCheckBoxList.Exists(x => (x.IsChecked == false));
                foreach (var CBox in AllCheckBoxList)
                {
                    CBox.IsChecked = AllCheckboxBoolen;
                }
                return true;
            }
            catch (Exception EX)
            {
                return false;
            }
        }

        public static bool SetAllButtonEnabledTrueOrFalse(object parent, bool SetBoolen)
        {
            try
            {
                //勾選邏輯->checkbox中存在任意未勾選時，按鈕為全勾選
                //當checkbox為全勾選時，才執行全部取消勾選
                var AllCheckBoxList = GetWpfLogicalChildClass.GetLogicalChildCollection<System.Windows.Controls.Button>(parent);
                foreach (var CBox in AllCheckBoxList)
                {
                    CBox.IsEnabled = SetBoolen;
                }
                return true;
            }
            catch (Exception EX)
            {
                return false;
            }
        }

        /// <summary>
        /// 尋找控制項 T = 子控制元件的類型 , parent = 搜尋之母控制元件
        /// </summary>
        /// <typeparam name="T">子控制元件的類型</typeparam>
        /// <param name="parent">目標搜尋的控制元件名稱</param>
        /// <returns></returns>
        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            System.Collections.IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

    }
    #endregion


}
