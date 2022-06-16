using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFWindowsBase
{
    /// <summary>
    /// TextBlock走馬燈自定義控件
    /// </summary>
    public class Marquee : TextBlock
    {
        /// <summary>
        /// 定時器
        /// </summary>
        Timer MarqueeTimer = new Timer();
        /// <summary>
        /// 滾動文字來源
        /// </summary>
        String _TextSource = "滾動文字來源";
        /// <summary>
        /// 輸出文字
        /// </summary>
        String _OutText = string.Empty;
        /// <summary>
        /// 文字暫存
        /// </summary>
        string _TempString = string.Empty;
        /// <summary>
        /// 文字的滾動速度
        /// </summary>
        double _RunSpeed = 300;
        /// <summary>
        /// 專案名稱
        /// </summary>
        string _PorjectName;

        Animation _AnimationType = Animation.NonStop;
        DateTime _SignTime;
        bool _IfFirst = true;

        /// <summary>
        /// 滾動一循環字幕停留的秒數,單位為毫秒,默認值停留3秒
        /// </summary>
        int _StopSecond = 3000;

        /// <summary>
        /// 滾動一循環字幕停留的秒數,單位為毫秒,默認值停留3秒
        /// </summary>
        public int StopSecond
        {
            get { return _StopSecond; }
            set
            {
                _StopSecond = value;
            }
        }

        /// <summary>
        /// 滾動的速度
        /// </summary>
        [Description("文字滾動的速度")]　//顯示在屬性設計視圖中的描述
        public double RunSpeed
        {
            get { return _RunSpeed; }
            set
            {
                _RunSpeed = value;
                MarqueeTimer.Interval = _RunSpeed;
            }
        }
        /// <summary>
        /// 動畫類型
        /// </summary>
        public enum Animation
        {
            /// <summary>
            /// 一次性
            /// </summary>
            OneTime,
            /// <summary>
            /// 永不停歇
            /// </summary>
            NonStop
        }

        /// <summary>
        /// 動畫類型值
        /// </summary>
        public Animation AnimationType
        {
            get { return _AnimationType; }
            set { _AnimationType = value; }
        }
        
        /// <summary>
        /// 滾動文字原始值
        /// </summary>
        [Description("滾動文字來源")]
        public string TextSource
        {
            get { return (string)GetValue(TagProperty); }
            set
            {
                SetValue(TagProperty, value);
            }
        }
        /// <summary>
        /// 設定跑馬燈內容
        /// </summary>
        public string SetContent
        {
            get { return (string)GetValue(TextProperty) ; }
            set 
            { 
                SetValue(TextProperty, value); 
            }
        }

        /// <summary>
        /// 建構函數
        /// </summary>
        public Marquee()
        {            
            Loaded += new RoutedEventHandler(ScrollingTextControl_Loaded);//綁定控件Loaded事件
            MarqueeTimer.Interval = _RunSpeed;//文字移動的速度
            MarqueeTimer.Enabled = true;      //開啟定時觸發事件
            MarqueeTimer.Elapsed += new ElapsedEventHandler(MarqueeTimer_Elapsed);//綁定定時事件
            //將觸發屬性綁在Tag以免被Text汙染
            var property = DependencyPropertyDescriptor.FromProperty(TextBlock.TagProperty, typeof(TextBlock));
            property.AddValueChanged(this, Marquee_SourceUpdated);
        }
        
        void Marquee_SourceUpdated(object sender, EventArgs e)
        {
            _PorjectName = TextSource;
        }

        void ScrollingTextControl_Loaded(object sender, RoutedEventArgs e)
        {
            _TextSource = TextSource;
            _TempString = _TextSource + "   ";
            _OutText = _TempString;
            _SignTime = DateTime.Now;
        }

        void MarqueeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {            
            if (string.IsNullOrWhiteSpace(_OutText)) 
                return;

            if (_PorjectName != _TextSource)
            {
                _TextSource = _PorjectName;
                _TempString = _TextSource + "   ";
                _OutText = _TempString;
                _SignTime = new DateTime();
                _SignTime = DateTime.Now;
            }

            if (_AnimationType == Animation.OneTime)
            {
                if (_OutText.Substring(1) + _OutText[0] == _TempString)
                {
                    if (_IfFirst)
                    {
                        _SignTime = DateTime.Now;
                    }

                    if ((DateTime.Now - _SignTime).TotalMilliseconds > _StopSecond)
                    {
                        _IfFirst = true;
                    }
                    else
                    {
                        _IfFirst = false;
                        return;
                    }
                }
            }

            _OutText = _OutText.Substring(1) + _OutText[0];     

            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetContent = _OutText;
            }));
        }
    }
}
