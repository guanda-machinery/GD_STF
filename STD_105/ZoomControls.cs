using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using WPFBase = WPFWindowsBase;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace STD_105
{
    class ZoomControls
    {
        // 原始的分割設定
        private static Array Row;
        private static Array Column;
        // 儲存上一次按的按鈕名
        private static string OriginalGrid;
        // 是否放大
        public static bool Zoom;
        // 儲存上上層物件
        private static Grid ParentParent;
        public static ICommand ZoomTarget
        { 
            get
            {
                return new WPFBase.RelayParameterizedCommand(ClickButton);
            }
        }

        public static void ClickButton(object e)
        {
            if (!(e is Button button))
                return;
            // 尋找最底層的Page
            var grandParent = VisualTreeHelper.GetParent(button);
            bool flag = false;
            Grid parent = new Grid() ;
            while (!(grandParent is Page))
            {
                if (grandParent is Grid && !flag)
                {
                    flag = true;
                    parent = grandParent as Grid;
                }
                grandParent = VisualTreeHelper.GetParent(grandParent);
            }
            // Button必須在Grid下層
            if (parent == null)
            {
                MessageBox.Show("Button必須包在Grid底下。");
                return;
            }
            
            if (!(((Page)grandParent).FindName("grid_Main") is Grid main))
            {
                MessageBox.Show("第一層Grid名稱必須設定為grid_Main。");
                return;
            }

            if (!(((Page)grandParent).FindName("grid_Popup") is Grid pop))
            {
                MessageBox.Show("快顯視窗名稱必須設定為grid_Popup");
                return;
            }

            if (!Zoom)
            {
                Zoom = true;
                OriginalGrid = parent.Name;
                ParentParent = parent.Parent as Grid;
                
                // 紀錄原有的Row跟Column設定
                Row = main.RowDefinitions.ToArray();
                Column = main.ColumnDefinitions.ToArray();
                // 清除原有的Row跟Column
                main.ColumnDefinitions.Clear();
                main.RowDefinitions.Clear();
                // 設置放大後的Row比例
                RowDefinition rowDefinition = new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                };
                RowDefinition rowDefinition1 = new RowDefinition
                {
                    Height = new GridLength(5, GridUnitType.Star)
                };
                RowDefinition rowDefinition2 = new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                };
                // 重新建立分割條件
                main.RowDefinitions.Add(rowDefinition);
                main.RowDefinitions.Add(rowDefinition1);
                main.RowDefinitions.Add(rowDefinition2);
                // 把目標Grid從父系控件中移除
                ParentParent.Children.Remove(parent);
                // 先清空快顯示窗的物件再將目標加入視窗
                pop.Children.Clear();
                pop.Children.Add(parent);
                pop.Visibility = Visibility.Visible;

                // Create a DoubleAnimation to animate the width of the button.
                DoubleAnimation myDoubleAnimation = new DoubleAnimation
                {
                    From = 200,
                    To = 970,
                    Duration = new Duration(TimeSpan.FromMilliseconds(300))
                };

                // Configure the animation to target the button's Width property.
                Storyboard.SetTargetName(myDoubleAnimation, pop.Name);
                Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Grid.HeightProperty));

                // Create a storyboard to contain the animation.
                Storyboard mystoryboard = new Storyboard();
                mystoryboard.Children.Add(myDoubleAnimation);

                pop.BeginStoryboard(mystoryboard);

                // 提高Z軸顯示層級
                pop.SetValue(Grid.ZIndexProperty, 99);
                // 更換顯示ICON
                Image image = new Image
                {
                    Source = ((Page)grandParent).FindResource("Minimize") as ImageSource
                };
                button.Content = image;
            }
            else if (Zoom && OriginalGrid == parent.Name)
            {
                // 回復原始的設定
                Zoom = false;
                parent.SetValue(Grid.ZIndexProperty, 1);
                main.ColumnDefinitions.Clear();
                main.RowDefinitions.Clear();

                for (int i = 0; i < Row.Length; i++)
                {
                    main.RowDefinitions.Add((RowDefinition)Row.GetValue(i));
                }
                for (int i = 0; i < Column.Length; i++)
                {
                    main.ColumnDefinitions.Add((ColumnDefinition)Column.GetValue(i));
                }
                pop.Children.Clear();
                ParentParent.Children.Add(parent);
                pop.Visibility = Visibility.Collapsed;

                pop.SetValue(Grid.ZIndexProperty, 1);
                // 更換顯示ICON
                Image image = new Image
                {
                    Source = ((Page)grandParent).FindResource("Maximize") as ImageSource
                };
                button.Content = image;
            }
            else
            {
                return;
            }
        }
        // 用這個Function才能在Grid裡搜尋Grid
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
