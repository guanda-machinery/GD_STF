using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPFWindowsBase
{
    /// <summary>
    /// Animation helpers for <see cref="Storyboard"/>
    /// </summary>
    public static class StoryboardHelpers
    {
        /// <summary>
        ///將幻燈片從正確的動畫添加到情節提要(下面淡入)
        /// </summary>
        /// <param name="storyboard">將動畫添加到的情節提要</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="offset">從右邊開始的距離</param>
        /// <param name="decelerationRatio">減速率</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        public static void AddSlideFromDown(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool KeepMargin = true)
        {

            // 從右邊創建邊距動畫
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0, -offset, 0, KeepMargin ? offset : 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            //設置目標屬性名稱
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            //將此添加到情節提要
            storyboard.Children.Add(animation);
        }
        /// <summary>
        ///將幻燈片從正確的動畫添加到情節提要(上面淡入)
        /// </summary>
        /// <param name="storyboard">將動畫添加到的情節提要</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="offset">從右邊開始的距離</param>
        /// <param name="decelerationRatio">減速率</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        public static void AddSlideFromUp(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool KeepMargin = true)
        {
            // 從右邊創建邊距動畫
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0, KeepMargin ? offset : 0, 0, -offset),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            //設置目標屬性名稱
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            //將此添加到情節提要
            storyboard.Children.Add(animation);
        }

        /// <summary>
        ///將幻燈片從正確的動畫添加到情節提要(右邊淡入)
        /// </summary>
        /// <param name="storyboard">將動畫添加到的情節提要</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="offset">從右邊開始的距離</param>
        /// <param name="decelerationRatio">減速率</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        public static void AddSlideFromRight(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool KeepMargin = true)
        {
            // 從右邊創建邊距動畫
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(KeepMargin ? offset : 0, 0, -offset, 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            //設置目標屬性名稱
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            //將此添加到情節提要
            storyboard.Children.Add(animation);
        }
        /// <summary>
        ///將幻燈片從正確的動畫添加到情節提要(左邊淡入)
        /// </summary>
        /// <param name="storyboard">將動畫添加到的情節提要</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="offset">從右邊開始的距離</param>
        /// <param name="decelerationRatio">減速率</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        public static void AddSlideFromLeft(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool KeepMargin = true)
        {
            // 從左邊創建邊距動畫
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(-offset, 0, KeepMargin ? offset : 0, 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            //設置目標屬性名稱
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            //將此添加到情節提要
            storyboard.Children.Add(animation);
        }
        /// <summary>
        ///向情節提要向下動畫添加幻燈片(淡出)
        /// </summary>
        /// <param name="storyboard">將動畫添加到的情節提要</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="offset">從右邊開始的距離</param>
        /// <param name="decelerationRatio">減速率</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        public static void AddSlideToDown(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool KeepMargin = true)
        {

            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(0, -offset, 0, KeepMargin ? offset : 0),
                DecelerationRatio = decelerationRatio
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            storyboard.Children.Add(animation);
        }

        /// <summary>
        ///向情節提要向左動畫添加幻燈片(左邊淡出)
        /// </summary>
        /// <param name="storyboard">將動畫添加到的情節提要</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="offset">從右邊開始的距離</param>
        /// <param name="decelerationRatio">減速率</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        public static void AddSlideToLeft(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool KeepMargin = true)
        {

            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(-offset, 0, KeepMargin ? offset : 0, 0),
                DecelerationRatio = decelerationRatio
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            storyboard.Children.Add(animation);
        }
        /// <summary>
        ///向情節提要向左動畫添加幻燈片(右邊淡出)
        /// </summary>
        /// <param name="storyboard">將動畫添加到的情節提要</param>
        /// <param name="seconds">動畫所需的時間</param>
        /// <param name="offset">從右邊開始的距離</param>
        /// <param name="decelerationRatio">減速率</param>
        /// <param name="KeepMargin">動畫期間是否將元素保持相同的寬度</param>
        public static void AddSlideToRight(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool KeepMargin = true)
        {
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(KeepMargin ? offset : 0, 0, -offset, 0),
                DecelerationRatio = decelerationRatio
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            storyboard.Children.Add(animation);

        }
        /// <summary>
        ///在情節提要中添加淡入淡出的動畫
        /// </summary>
        /// <param name="storyboard">將動畫添加到的情節提要</param>
        /// <param name="seconds">動畫所需的時間</param>
        public static void AddFadeIn(this Storyboard storyboard, float seconds)
        {
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1,
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            storyboard.Children.Add(animation);
        }

        /// <summary>
        ///將淡出動畫添加到情節提要
        /// </summary>
        /// <param name="storyboard">The storyboard to add the animation to</param>
        /// <param name="seconds">The time the animation will take</param>
        public static void AddFadeOut(this Storyboard storyboard, float seconds)
        {
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0,
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            storyboard.Children.Add(animation);
        }
    }
}
