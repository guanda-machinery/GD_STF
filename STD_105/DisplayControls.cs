using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFWindowsBase;

namespace STD_105
{
    class DisplayControls
    {
        static int Count = 0;
        static int Index = 0;
        static readonly int[,] Sort = new int[4, 3] { { 1, 0, 0 }, { 1, 1, 0 }, { 2, 0, 0 }, { 2, 1, 0 } };
        static bool StatusSwitch = false;

        public static ICommand DisplayTarget
        {
            get
            {
                return new RelayParameterizedCommand(ClickButton);
            }
        }

        private static void BoolSwitch()
        {
            if (StatusSwitch)
                StatusSwitch = false;
            else
                StatusSwitch = true;
        }

        private static void ClickButton(object e)
        {
            if (!(e is Button button))
                return;

            string btnName = button.Name;
            string gridName = btnName.Replace("btn", "grid");
            bool display = Convert.ToBoolean(button.Tag);
            var grandParent = VisualTreeHelper.GetParent(button);

            while (!(grandParent is Page))
            {
                grandParent = VisualTreeHelper.GetParent(grandParent);
            }
            if (!(((Page)grandParent).FindName("grid_Display") is Grid main))
            {
                MessageBox.Show("請把分割Grid的名稱設定為grid_Display。");
                return;
            }
            if (!(((Page)grandParent).FindName(gridName) is Grid grid))
            {
                MessageBox.Show("請把欲顯示的Grid名稱跟Button名稱設定一致。");
                return;
            }

            if (!display)
            {
                if (Count == 4)
                    return;
                Count++;
                Index++;
                button.Tag = true;
                button.Visibility = Visibility.Collapsed;
                GridSort(grid);
            }
            else
            {
                Count--;
                button.Tag = false;
                button.Visibility = Visibility.Visible;
                Sort[(int)grid.Tag, 2] = 0;
                GridIInitialization(grid);
            }
            DisplaySeparation(main);
        }
        // 排序目標Grid
        private static void GridSort(Grid grid)
        {
            GridIInitialization(grid);
            grid.Visibility = Visibility.Visible;
            for (int i = 0; i < Sort.GetLength(0); i++)
            {
                bool flag = Convert.ToBoolean(Sort[i, 2]);
                if (!flag)
                {
                    grid.SetValue(Grid.RowProperty, Sort[i, 0]);
                    grid.SetValue(Grid.ColumnProperty, Sort[i, 1]);
                    grid.SetValue(Grid.ZIndexProperty, Index * 2);
                    Sort[i, 2] = 1;
                    grid.Tag = i;
                    if (i >= 2)
                        BoolSwitch();
                    break;
                }
            }
        }

        // 初始化顯示配置
        private static void GridIInitialization(Grid grid)
        {
            grid.Visibility = Visibility.Collapsed;
            grid.SetValue(Grid.RowProperty, 0);
            grid.SetValue(Grid.ColumnProperty, 0);
            grid.SetValue(Grid.RowSpanProperty, 1);
            grid.SetValue(Grid.ColumnSpanProperty, 1);
            grid.SetValue(Grid.ZIndexProperty, 1);
        }
        // 分割主要顯示的Grid
        private static void DisplaySeparation(Grid main)
        {
            // 清除Grid的分割設定
            main.ColumnDefinitions.Clear();
            main.RowDefinitions.Clear();
            if (Count == 1)
            {
                // 設置分割後的Row比例
                for (int i=0; i<2; i++)
                {
                    RowDefinition rowDefinition = new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    };
                    // 重新建立分割條件
                    main.RowDefinitions.Add(rowDefinition);
                }
            }
            else if (Count == 2)
            {
                // 設置分割後的Row比例
                for (int i = 0; i < 2; i++)
                {
                    RowDefinition rowDefinition = new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    };
                    ColumnDefinition columnDefinition = new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    };
                    // 重新建立Row&Column分割條件
                    main.RowDefinitions.Add(rowDefinition);
                    main.ColumnDefinitions.Add(columnDefinition);
                }
            }
            else if (Count > 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    RowDefinition rowDefinition = new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    };
                    // 重新建立Row分割條件
                    main.RowDefinitions.Add(rowDefinition);
                }
                for (int i = 0; i < 2; i++)
                {
                    ColumnDefinition columnDefinition = new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    };
                    // 重新建立Column分割條件
                    main.ColumnDefinitions.Add(columnDefinition);
                }
            }
        }
        public static ICommand CloseTarget
        {
            get
            {
                return new RelayParameterizedCommand(ClickCloseButton);
            }
        }
        // Only for CloseButton
        private static void ClickCloseButton(object e)
        {
            if (!(e is CloseButton closebutton))
                return;

            if (ZoomControls.Zoom)
            {
                MessageBox.Show("請先縮小視窗再關閉。", "通知", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                return;
            }

            string cbtnName = closebutton.Name;
            string btnName = cbtnName.Replace("cbtn", "btn");
            string gridName = cbtnName.Replace("cbtn", "grid");
            var grandParent = VisualTreeHelper.GetParent(closebutton);

            while (!(grandParent is Page))
            {
                grandParent = VisualTreeHelper.GetParent(grandParent);
            }
            
            Grid main = ((Page)grandParent).FindName("grid_Display") as Grid;
            Grid grid = ((Page)grandParent).FindName(gridName) as Grid;
            Button button = ((Page)grandParent).FindName(btnName) as Button;
            Count--;
            button.Tag = false;
            button.Visibility = Visibility.Visible;
            Sort[(int)grid.Tag, 2] = 0;
            GridIInitialization(grid);
            if ((int)grid.Tag == 2 || (int)grid.Tag == 3)
                BoolSwitch();
            if (Count == 2 && StatusSwitch)
                return;
            DisplaySeparation(main);
        }
    }
}
