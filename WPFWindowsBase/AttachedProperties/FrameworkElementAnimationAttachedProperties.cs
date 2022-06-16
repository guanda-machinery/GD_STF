using System;
using System.Windows;

namespace WPFWindowsBase
{
    /// <summary>
    /// 當<see cref="bool"/>值設置為true時，運行任何動畫方法的基類
    /// </summary>
    public abstract class AbsAnimateBaseProperty<Parent> : BaseAttachedProperty<Parent, bool> where Parent : BaseAttachedProperty<Parent, bool>, new()
    {
        /// <summary>
        /// 第一次加載
        /// </summary>
        public bool FirstLoad { get; set; } = true;
        /// <inheritdoc/>
        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            //獲取框架元素
            if (!(sender is FrameworkElement element))
                return;

            //如果值不變則不觸發
            if (sender.GetValue(ValueProperty) == value && !FirstLoad)
                return;
            //第一次加載
            if (FirstLoad)
            {
                //創建一個自unhookable事件
                //對於元素加載事件
                RoutedEventHandler onLoaded = null;
                onLoaded = (s, e) =>
                {
                    //取消事件 onLoaded
                    element.Loaded -= onLoaded;
                    //做所需的動畫
                    DoAnimation(element, (bool)value);

                    //不再首先加載
                    FirstLoad = false;
                };
                //插入元素的Loaded事件
                element.Loaded += onLoaded;
            }
            else
            {
                DoAnimation(element, (bool)value);
            }
        }
        /// <summary>
        /// 觸發動畫 ， 並且元素進行更改
        /// </summary>
        /// <param name="element">UI 元素</param>
        /// <param name="value"></param>
        protected virtual void DoAnimation(FrameworkElement element, bool value)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///對顯示在頁面左側的框架元素進行動畫處理
    ///並向隱藏的左側滑動 
    /// </summary>
    public class AnimateSlideInFromLeftProperty : AbsAnimateBaseProperty<AnimateSlideInFromLeftProperty>
    {
        /// <inheritdoc/>
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            if (value)
                await element.SlideAndFadeInFromLeftAsync(FirstLoad ? 0 : 0.3f, KeepMargin: false);
            else
                await element.SlideAndFadeOutToLeftAsync(FirstLoad ? 0 : 0.3f, KeepMargin: false);
        }
    }
    /// <summary>
    ///對顯示在頁面左側的框架元素進行動畫處理
    ///並向隱藏的左側滑動 
    /// </summary>
    public class AnimateSlideInFromDownProperty : AbsAnimateBaseProperty<AnimateSlideInFromDownProperty>
    {
        /// <inheritdoc/>
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            if (value)
                await element.SlideAndFadeInFromDownAsync(FirstLoad ? 0 : 0.3f, KeepMargin: false);
            else
                await element.SlideAndFadeOutToDownAsync(FirstLoad ? 0 : 0.3f, KeepMargin: false);
        }
    }
    /// <summary>
    ///對顯示在頁面右側的框架元素進行動畫處理
    ///並向隱藏的右側滑動 
    /// </summary>
    public class AnimateSlideInFromRightProperty : AbsAnimateBaseProperty<AnimateSlideInFromRightProperty>
    {
        /// <inheritdoc/>
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            if (value)
                await element.SlideAndFadeInFromRightAsync(FirstLoad ? 0 : 0.3f, KeepMargin: false);
            else
                await element.SlideAndFadeOutToRightAsync(FirstLoad ? 0 : 0.3f, KeepMargin: false);
        }
    }
}
