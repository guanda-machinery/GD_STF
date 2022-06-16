using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Expression.Shapes;
using System.Windows.Media.Animation;

namespace WPFWindowsBase
{
    /// <summary>
    /// 旋鈕繪製控制項
    /// </summary>
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "GraphRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "Pointer", Type = typeof(Path))]
    [TemplatePart(Name = "RangeIndicatorLight", Type = typeof(Ellipse))]
    //[TemplatePart(Name = "PointerCap", Type = typeof(Ellipse))]
    [TemplatePart(Name = "KnobButton", Type = typeof(Button))]
    [TemplatePart(Name = "Arc", Type = typeof(Arc))]
    public class KnobAddControl : Control
    {
        #region 私有變數
        private Grid rootGrid;
        private Grid graphGrid;
        //private Path rangeIndicator;
        private Path pointer;
        //private Ellipse pointerCap;
        private Ellipse lightIndicator;
        /// <summary>RangeIndicatorRadius
        /// 中心旋鈕
        /// </summary>
        private Button knobButton;
        private bool isInitialValueSet = false;
        //private Double arcradius1;
        //private Double arcradius2;
        private int animatingSpeedFactor = 3;
        private Arc arc;
        #endregion

        #region 依賴屬性
        /// <summary>
        /// 設定當前數值
        /// </summary>
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(double), typeof(KnobAddControl),
            new PropertyMetadata(double.MinValue, new PropertyChangedCallback(KnobAddControl.OnCurrentValuePropertyChanged)));

        /// <summary>
        /// 設定刻度最小值
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定刻度最大值
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定轉盤外框大小
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定中心旋鈕大小
        /// </summary>
        public static readonly DependencyProperty PointerCapRadiusProperty =
            DependencyProperty.Register("PointerCapRadius", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定指針長度
        /// </summary>
        public static readonly DependencyProperty PointerLengthProperty =
            DependencyProperty.Register("PointerLength", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定刻度距離中心點大小
        /// </summary>
        public static readonly DependencyProperty ScaleRadiusProperty =
            DependencyProperty.Register("ScaleRadius", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定初始值起點角度
        /// </summary>
        public static readonly DependencyProperty ScaleStartAngleProperty =
            DependencyProperty.Register("ScaleStartAngle", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定總刻度的扇形角度
        /// </summary>
        public static readonly DependencyProperty ScaleSweepAngleProperty =
            DependencyProperty.Register("ScaleSweepAngle", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定大刻度總數
        /// </summary>
        public static readonly DependencyProperty MajorDivisionsCountProperty =
            DependencyProperty.Register("MajorDivisionsCount", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定小刻度總數
        /// </summary>
        public static readonly DependencyProperty MinorDivisionsCountProperty =
            DependencyProperty.Register("MinorDivisionsCount", typeof(double), typeof(KnobAddControl), null);
        /*
        /// <summary>
        /// 設定轉盤顏色區間結束點
        /// </summary>
        public static readonly DependencyProperty OptimalRangeEndValueProperty =
           DependencyProperty.Register("OptimalRangeEndValue", typeof(double), typeof(KnobAddControl), new PropertyMetadata(new PropertyChangedCallback(KnobAddControl.OnOptimalRangeEndValuePropertyChanged)));

        /// <summary>
        /// 設定轉盤顏色區間起始點
        /// </summary>
        public static readonly DependencyProperty OptimalRangeStartValueProperty =
           DependencyProperty.Register("OptimalRangeStartValue", typeof(double), typeof(KnobAddControl), new PropertyMetadata(new PropertyChangedCallback(KnobAddControl.OnOptimalRangeStartValuePropertyChanged)));
        */
        /// <summary>
        /// LOGO來源圖檔
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
          DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定圖像偏移量
        /// </summary>
        public static readonly DependencyProperty ImageOffsetProperty =
          DependencyProperty.Register("ImageOffset", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定指示燈偏移量
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorLightOffsetProperty =
          DependencyProperty.Register("RangeIndicatorLightOffset", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定圖片大小
        /// </summary>
        public static readonly DependencyProperty ImageSizeProperty =
          DependencyProperty.Register("ImageSize", typeof(Size), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定指示燈距離中心點大小
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorRadiusProperty =
          DependencyProperty.Register("RangeIndicatorRadius", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定指示燈外框線條大小
        /// </summary>
        public static readonly DependencyProperty RangeIndicatorThicknessProperty =
         DependencyProperty.Register("RangeIndicatorThickness", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定刻度文字距離中心點大小
        /// </summary>
        public static readonly DependencyProperty ScaleLabelRadiusProperty =
            DependencyProperty.Register("ScaleLabelRadius", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定刻度文字大小
        /// </summary>
        public static readonly DependencyProperty ScaleLabelSizeProperty =
         DependencyProperty.Register("ScaleLabelSize", typeof(Size), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定刻度文字字體大小
        /// </summary>
        public static readonly DependencyProperty ScaleLabelFontSizeProperty =
            DependencyProperty.Register("ScaleLabelFontSize", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定刻度文字顏色
        /// </summary>

        public static readonly DependencyProperty ScaleLabelForegroundProperty =
            DependencyProperty.Register("ScaleLabelForeground", typeof(Color), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定主要刻度大小
        /// </summary>
        public static readonly DependencyProperty MajorTickSizeProperty =
          DependencyProperty.Register("MajorTickRectSize", typeof(Size), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定子刻度大小
        /// </summary>
        public static readonly DependencyProperty MinorTickSizeProperty =
          DependencyProperty.Register("MinorTickSize", typeof(Size), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定主要刻度顏色
        /// </summary>
        public static readonly DependencyProperty MajorTickColorProperty =
           DependencyProperty.Register("MajorTickColor", typeof(Color), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定子刻度顏色
        /// </summary>
        public static readonly DependencyProperty MinorTickColorProperty =
          DependencyProperty.Register("MinorTickColor", typeof(Color), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定轉盤背景顏色
        /// </summary>
        public static readonly DependencyProperty GaugeBackgroundColorProperty =
          DependencyProperty.Register("GaugeBackgroundColor", typeof(Color), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定指針框線大小
        /// </summary>
        public static readonly DependencyProperty PointerThicknessProperty =
        DependencyProperty.Register("PointerThickness", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定Arc大小
        /// </summary>
        public static readonly DependencyProperty ArcRadiusProperty =
        DependencyProperty.Register("ArcRadius", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定Arc顏色
        /// </summary>
        public static readonly DependencyProperty ArcColorProperty =
        DependencyProperty.Register("ArcColor", typeof(Color), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定是否初始化
        /// </summary>
        public static readonly DependencyProperty ResetPointerOnStartUpProperty =
        DependencyProperty.Register("ResetPointerOnStartUp", typeof(bool), typeof(KnobAddControl), new PropertyMetadata(true, null));

        /// <summary>
        /// 設定刻度精準度
        /// </summary>
        public static readonly DependencyProperty ScaleValuePrecisionProperty =
        DependencyProperty.Register("ScaleValuePrecision", typeof(int), typeof(KnobAddControl), null);

        /*
        /// <summary>
        /// 設定下方顏色區間的顏色
        /// </summary>

        public static readonly DependencyProperty BelowOptimalRangeColorProperty =
            DependencyProperty.Register("BelowOptimalRangeColor", typeof(Color), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定顏色區間的顏色
        /// </summary>

        public static readonly DependencyProperty OptimalRangeColorProperty =
            DependencyProperty.Register("OptimalRangeColor", typeof(Color), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定上方顏色區間的顏色
        /// </summary>

        public static readonly DependencyProperty AboveOptimalRangeColorProperty =
            DependencyProperty.Register("AboveOptimalRangeColor", typeof(Color), typeof(KnobAddControl), null);
        */
        /// <summary>
        /// 設定說明文字的內容
        /// </summary>

        public static readonly DependencyProperty DialTextProperty =
            DependencyProperty.Register("DialText", typeof(string), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定標題文字的內容
        /// </summary>

        public static readonly DependencyProperty TitleTextProperty =
            DependencyProperty.Register("TitleText", typeof(string), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定副標題文字的內容
        /// </summary>

        public static readonly DependencyProperty SubTitleTextProperty =
            DependencyProperty.Register("SubTitleText", typeof(string), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定說明文字的顏色
        /// </summary>

        public static readonly DependencyProperty DialTextColorProperty =
            DependencyProperty.Register("DialTextColor", typeof(Color), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定說明文字大小
        /// </summary>

        public static readonly DependencyProperty DialTextFontSizeProperty =
            DependencyProperty.Register("DialTextFontSize", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定說明文字偏移量
        /// </summary>

        public static readonly DependencyProperty DialTextOffsetProperty =
            DependencyProperty.Register("DialTextOffset", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 設定指示燈距離中心點大小
        /// </summary>

        public static readonly DependencyProperty RangeIndicatorLightRadiusProperty =
            DependencyProperty.Register("RangeIndicatorLightRadius", typeof(double), typeof(KnobAddControl), null);

        /// <summary>
        /// 旋鈕事件
        /// </summary>
        public static readonly DependencyProperty KnobButtonCommandProperty =
            DependencyProperty.Register("KnobButtonCommand", typeof(ICommand), typeof(KnobAddControl), null);
        /*
        /// <summary>
        /// Arc結束角度
        /// </summary>
        public static readonly DependencyProperty ArcEndAngleProperty =
            DependencyProperty.Register("ArcEndAngle", typeof(double), typeof(KnobAddControl),null);*/
        #endregion

        #region 容器屬性
        /// <summary>
        /// 獲取當前值
        /// </summary>
        public double CurrentValue
        {
            get
            {
                return (double)GetValue(CurrentValueProperty);
            }
            set
            {
                SetValue(CurrentValueProperty, value);
            }
        }

        /// <summary>
        /// 獲取最小值
        /// </summary>
        public double MinValue
        {
            get
            {
                return (double)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        /// <summary>
        /// 獲取最大值
        /// </summary>
        public double MaxValue
        {
            get
            {
                return (double)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        /// <summary>
        /// 獲取外框大小值
        /// </summary>
        public double Radius
        {
            get
            {
                return (double)GetValue(RadiusProperty);
            }
            set
            {
                SetValue(RadiusProperty, value);
            }
        }

        /// <summary>
        /// 獲取旋鈕大小值
        /// </summary>
        public double PointerCapRadius
        {
            get
            {
                return (double)GetValue(PointerCapRadiusProperty);
            }
            set
            {
                SetValue(PointerCapRadiusProperty, value);
            }
        }

        /// <summary>
        /// 獲取指針長度
        /// </summary>
        public double PointerLength
        {
            get
            {
                return (double)GetValue(PointerLengthProperty);
            }
            set
            {
                SetValue(PointerLengthProperty, value);
            }
        }

        /// <summary>
        /// 獲取指針外框線條大小
        /// </summary>
        public double PointerThickness
        {
            get
            {
                return (double)GetValue(PointerThicknessProperty);
            }
            set
            {
                SetValue(PointerThicknessProperty, value);
            }
        }

        /// <summary>
        /// 設定Arc大小
        /// </summary>
        public double ArcRadius
        {
            get
            {
                return (double)GetValue(ArcRadiusProperty);
            }
            set
            {
                SetValue(ArcRadiusProperty, value);
            }
        }

        /// <summary>
        /// 設定Arc顏色
        /// </summary>
        public Color ArcColor
        {
            get
            {
                return (Color)GetValue(ArcColorProperty);
            }
            set
            {
                SetValue(ArcColorProperty, value);
            }
        }

        /// <summary>
        /// 獲取刻度距離中心值
        /// </summary>
        public double ScaleRadius
        {
            get
            {
                return (double)GetValue(ScaleRadiusProperty);
            }
            set
            {
                SetValue(ScaleRadiusProperty, value);
            }
        }

        /// <summary>
        /// 獲取起始角度
        /// </summary>
        public double ScaleStartAngle
        {
            get
            {
                return (double)GetValue(ScaleStartAngleProperty);
            }
            set
            {
                SetValue(ScaleStartAngleProperty, value);
            }
        }

        /// <summary>
        /// 獲取最大刻度扇形角
        /// </summary>
        public double ScaleSweepAngle
        {
            get
            {
                return (double)GetValue(ScaleSweepAngleProperty);
            }
            set
            {
                SetValue(ScaleSweepAngleProperty, value);
            }
        }

        /// <summary>
        /// 獲取主要刻度的總數
        /// </summary>
        public double MajorDivisionsCount
        {
            get
            {
                return (double)GetValue(MajorDivisionsCountProperty);
            }
            set
            {
                SetValue(MajorDivisionsCountProperty, value);
            }
        }

        /// <summary>
        /// 獲取子刻度的總數
        /// </summary>
        public double MinorDivisionsCount
        {
            get
            {
                return (double)GetValue(MinorDivisionsCountProperty);
            }
            set
            {
                SetValue(MinorDivisionsCountProperty, value);
            }
        }
        /*
        /// <summary>
        /// 獲取最佳值區間結束點
        /// </summary>
        public double OptimalRangeEndValue
        {
            get
            {
                return (double)GetValue(OptimalRangeEndValueProperty);
            }
            set
            {
                SetValue(OptimalRangeEndValueProperty, value);
            }
        }
        /// <summary>
        /// 獲取最佳值區間起始點
        /// </summary>
        public double OptimalRangeStartValue
        {
            get
            {
                return (double)GetValue(OptimalRangeStartValueProperty);
            }
            set
            {
                SetValue(OptimalRangeStartValueProperty, value);
            }
        }*/

        /// <summary>
        /// 獲取圖片來源
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        /// <summary>
        /// 獲取圖片位置偏移量
        /// </summary>
        public double ImageOffset
        {
            get
            {
                return (double)GetValue(ImageOffsetProperty);
            }
            set
            {
                SetValue(ImageOffsetProperty, value);
            }
        }

        /// <summary>
        /// 獲取指示燈偏移量
        /// </summary>
        public double RangeIndicatorLightOffset
        {
            get
            {
                return (double)GetValue(RangeIndicatorLightOffsetProperty);
            }
            set
            {
                SetValue(RangeIndicatorLightOffsetProperty, value);
            }
        }

        /// <summary>
        /// 獲取圖片的長寬值
        /// </summary>
        public Size ImageSize
        {
            get
            {
                return (Size)GetValue(ImageSizeProperty);
            }
            set
            {
                SetValue(ImageSizeProperty, value);
            }
        }

        /// <summary>
        /// 獲取指示燈半徑
        /// </summary>
        public double RangeIndicatorRadius
        {
            get
            {
                return (double)GetValue(RangeIndicatorRadiusProperty);
            }
            set
            {
                SetValue(RangeIndicatorRadiusProperty, value);
            }
        }

        /// <summary>
        /// 獲取指示燈外框線條大小
        /// </summary>
        public double RangeIndicatorThickness
        {
            get
            {
                return (double)GetValue(RangeIndicatorThicknessProperty);
            }
            set
            {
                SetValue(RangeIndicatorThicknessProperty, value);
            }
        }
        /// <summary>
        /// 獲取刻度文字半徑
        /// </summary>
        public double ScaleLabelRadius
        {
            get
            {
                return (double)GetValue(ScaleLabelRadiusProperty);
            }
            set
            {
                SetValue(ScaleLabelRadiusProperty, value);
            }
        }
        /// <summary>
        /// 獲取刻度文字大小
        /// </summary>
        public Size ScaleLabelSize
        {
            get
            {
                return (Size)GetValue(ScaleLabelSizeProperty);
            }
            set
            {
                SetValue(ScaleLabelSizeProperty, value);
            }
        }
        /// <summary>
        /// 獲取刻度文字字體大小
        /// </summary>
        public double ScaleLabelFontSize
        {
            get
            {
                return (double)GetValue(ScaleLabelFontSizeProperty);
            }
            set
            {
                SetValue(ScaleLabelFontSizeProperty, value);
            }
        }
        /// <summary>
        /// 獲取刻度文字顏色
        /// </summary>
        public Color ScaleLabelForeground
        {
            get
            {
                return (Color)GetValue(ScaleLabelForegroundProperty);
            }
            set
            {
                SetValue(ScaleLabelForegroundProperty, value);
            }
        }
        /// <summary>
        /// 獲取主要刻度大小
        /// </summary>
        public Size MajorTickSize
        {
            get
            {
                return (Size)GetValue(MajorTickSizeProperty);
            }
            set
            {
                SetValue(MajorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// 獲取子刻度大小
        /// </summary>
        public Size MinorTickSize
        {
            get
            {
                return (Size)GetValue(MinorTickSizeProperty);
            }
            set
            {
                SetValue(MinorTickSizeProperty, value);
            }
        }

        /// <summary>
        /// 獲取主刻度顏色
        /// </summary>
        public Color MajorTickColor
        {
            get
            {
                return (Color)GetValue(MajorTickColorProperty);
            }
            set
            {
                SetValue(MajorTickColorProperty, value);
            }
        }

        /// <summary>
        /// 獲取子刻度顏色
        /// </summary>
        public Color MinorTickColor
        {
            get
            {
                return (Color)GetValue(MinorTickColorProperty);
            }
            set
            {
                SetValue(MinorTickColorProperty, value);
            }
        }

        /// <summary>
        /// 獲取背景顏色
        /// </summary>
        public Color GaugeBackgroundColor
        {
            get
            {
                return (Color)GetValue(GaugeBackgroundColorProperty);
            }
            set
            {
                SetValue(GaugeBackgroundColorProperty, value);
            }
        }

        /// <summary>
        /// 獲取初始化指針，默認值為是
        /// </summary>
        public bool ResetPointerOnStartUp
        {
            get
            {
                return (bool)GetValue(ResetPointerOnStartUpProperty);
            }
            set
            {
                SetValue(ResetPointerOnStartUpProperty, value);
            }
        }

        /// <summary>
        /// 獲取刻度精度值
        /// </summary>
        public int ScaleValuePrecision
        {
            get
            {
                return (int)GetValue(ScaleValuePrecisionProperty);
            }
            set
            {
                SetValue(ScaleValuePrecisionProperty, value);
            }
        }
        /*
        /// <summary>
        /// 獲取下位區間值顏色
        /// </summary>
        public Color BelowOptimalRangeColor
        {
            get
            {
                return (Color)GetValue(BelowOptimalRangeColorProperty);
            }
            set
            {
                SetValue(BelowOptimalRangeColorProperty, value);
            }
        }

        /// <summary>
        /// 獲取區間值顏色
        /// </summary>
        public Color OptimalRangeColor
        {
            get
            {
                return (Color)GetValue(OptimalRangeColorProperty);
            }
            set
            {
                SetValue(OptimalRangeColorProperty, value);
            }
        }

        /// <summary>
        /// 獲取上位區間值顏色
        /// </summary>
        public Color AboveOptimalRangeColor
        {
            get
            {
                return (Color)GetValue(AboveOptimalRangeColorProperty);
            }
            set
            {
                SetValue(AboveOptimalRangeColorProperty, value);
            }
        }*/
        /// <summary>
        /// 獲取說明文字內容
        /// </summary>
        public string DialText
        {
            get
            {
                return (string)GetValue(DialTextProperty);
            }
            set
            {
                SetValue(DialTextProperty, value);
            }
        }

        /// <summary>
        /// 獲取標題文字內容
        /// </summary>
        public string TitleText
        {
            get
            {
                return (string)GetValue(TitleTextProperty);
            }
            set
            {
                SetValue(TitleTextProperty, value);
            }
        }

        /// <summary>
        /// 獲取標題文字內容
        /// </summary>
        public string SubTitleText
        {
            get
            {
                return (string)GetValue(SubTitleTextProperty);
            }
            set
            {
                SetValue(SubTitleTextProperty, value);
            }
        }

        /// <summary>
        /// 獲取說明文字顏色
        /// </summary>
        public Color DialTextColor
        {
            get
            {
                return (Color)GetValue(DialTextColorProperty);
            }
            set
            {
                SetValue(DialTextColorProperty, value);
            }
        }

        /// <summary>
        /// 獲取說明文字字體大小
        /// </summary>
        public double DialTextFontSize
        {
            get
            {
                return (double)GetValue(DialTextFontSizeProperty);
            }
            set
            {
                SetValue(DialTextFontSizeProperty, value);
            }
        }

        /// <summary>
        /// 獲取說明文字偏移量
        /// </summary>
        public double DialTextOffset
        {
            get
            {
                return (double)GetValue(DialTextOffsetProperty);
            }
            set
            {
                SetValue(DialTextOffsetProperty, value);
            }
        }

        /// <summary>
        /// 獲取指示燈偏移量
        /// </summary>
        public double RangeIndicatorLightRadius
        {
            get
            {
                return (double)GetValue(RangeIndicatorLightRadiusProperty);
            }
            set
            {
                SetValue(RangeIndicatorLightRadiusProperty, value);
            }
        }

        /// <summary>
        /// 旋鈕命令
        /// </summary>
        public ICommand KnobButtonCommand
        {
            get
            {
                return (ICommand)GetValue(KnobButtonCommandProperty);
            }
            set
            {
                SetValue(KnobButtonCommandProperty, value);
            }
        }
        /*
        /// <summary>
        /// 旋鈕命令
        /// </summary>
        public double ArcEndAngle
        {
            get
            {
                return (double)GetValue(ArcEndAngleProperty);
            }
            set
            {
                SetValue(ArcEndAngleProperty, value);
            }
        }*/
        #endregion

        #region 建構函式
        static KnobAddControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KnobAddControl), new FrameworkPropertyMetadata(typeof(KnobAddControl)));
        }
        #endregion

        #region 私有方法
        private static void OnCurrentValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       {
            // 更改當前值
            KnobAddControl gauge = d as KnobAddControl;
            gauge.OnCurrentValueChanged(e);
        }
        /*
        private static void OnOptimalRangeEndValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 更改最佳值區間結束點
            KnobAddControl gauge = d as KnobAddControl;
            if ((double)e.NewValue > gauge.MaxValue)
            {
                gauge.OptimalRangeEndValue = gauge.MaxValue;
            }

        }
        private static void OnOptimalRangeStartValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //更改最佳值區間起始點
            KnobAddControl gauge = d as KnobAddControl;
            if ((double)e.NewValue < gauge.MinValue)
            {
                gauge.OptimalRangeStartValue = gauge.MinValue;
            }
        }*/
        /// <summary>
        /// 變更當前值函式
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnCurrentValueChanged(DependencyPropertyChangedEventArgs e)
        {
            //驗證並設置新值
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;

            if (newValue > this.MaxValue)
            {
                newValue = this.MaxValue;
            }
            else if (newValue < this.MinValue)
            {
                newValue = this.MinValue;
            }

            if (oldValue > this.MaxValue)
            {
                oldValue = this.MaxValue;
            }
            else if (oldValue < this.MinValue)
            {
                oldValue = this.MinValue;
            }

            double gap, oldcurr_realworldunit, newcurr_realworldunit;
            //每次轉角的區間
            double realworldunit = (ScaleSweepAngle / (MaxValue - MinValue));
            //重置舊值為最小值
            if (!isInitialValueSet)
            {
                oldValue = this.MinValue;
                isInitialValueSet = true;
            }
            if (oldValue < 0)
            {
                gap = MinValue + Math.Abs(oldValue);
                oldcurr_realworldunit = ((double)(Math.Abs(gap * realworldunit)));
            }
            else
            {
                gap = oldValue - MinValue;
                oldcurr_realworldunit = ((double)(gap * realworldunit));
            }
            if (newValue < 0)
            {
                gap = MinValue + Math.Abs(newValue);
                newcurr_realworldunit = ((double)(Math.Abs(gap * realworldunit)));
            }
            else
            {
                gap = newValue - MinValue;
                newcurr_realworldunit = ((double)(gap * realworldunit));
            }

            Double oldcurrentvalueAngle = (ScaleStartAngle + oldcurr_realworldunit);
            Double newcurrentvalueAngle = (ScaleStartAngle + newcurr_realworldunit);

            if (pointer != null)
            {
                //轉動指針從舊值到新值
                AnimatePointer(oldcurrentvalueAngle, newcurrentvalueAngle);
            }
            if (arc != null)
            {
                AnimateArc(oldcurrentvalueAngle + 90, newcurrentvalueAngle + 90);
            }
        }

        /// <summary>
        /// 將指向當前值的指針設置為新值
        /// </summary>
        /// <param name="oldcurrentvalueAngle"></param>
        /// <param name="newcurrentvalueAngle"></param>
        void AnimatePointer(double oldcurrentvalueAngle, double newcurrentvalueAngle)
        {
            if (pointer != null)
            {
                DoubleAnimation da = new DoubleAnimation();
                da.From = oldcurrentvalueAngle;
                da.To = newcurrentvalueAngle;

                double animDuration = Math.Abs(oldcurrentvalueAngle - newcurrentvalueAngle) * animatingSpeedFactor;
                da.Duration = new Duration(TimeSpan.FromMilliseconds(animDuration));

                Storyboard sb = new Storyboard();
                //b.Completed += new EventHandler(sb_Completed);
                sb.Children.Add(da);
                Storyboard.SetTarget(da, pointer);
                Storyboard.SetTargetProperty(da, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));

                if (newcurrentvalueAngle != oldcurrentvalueAngle)
                {
                    sb.Begin();
                }
            }
        }

        /// <summary>
        /// Arc動畫
        /// </summary>
        /// <param name="oldcurrentvalueAngle"></param>
        /// <param name="newcurrentvalueAngle"></param>
        void AnimateArc(double oldcurrentvalueAngle, double newcurrentvalueAngle)
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = oldcurrentvalueAngle,
                To = newcurrentvalueAngle
            };

            double animDuration = Math.Abs(oldcurrentvalueAngle - newcurrentvalueAngle) * animatingSpeedFactor * 0.75;
            da.Duration = new Duration(TimeSpan.FromMilliseconds(animDuration));

            Storyboard sb = new Storyboard();
            //sb.Completed += new EventHandler(sb_Completed);
            sb.Children.Add(da);
            Storyboard.SetTarget(da, arc);
            Storyboard.SetTargetProperty(da, new PropertyPath("EndAngle"));

            if (newcurrentvalueAngle != oldcurrentvalueAngle)
            {
                sb.Begin();
            }
        }

        /// <summary>
        /// 在沒有動畫的情況下移動指針
        /// </summary>
        /// <param name="angleValue"></param>
        void ResetPointer(double angleValue)
        {
            if (pointer != null)
            {
                TransformGroup tg = pointer.RenderTransform as TransformGroup;
                RotateTransform rt = tg.Children[0] as RotateTransform;
                rt.Angle = angleValue;                
            }
            if (arc != null)
            {
                double currentAngle = (CurrentValue - MinValue) / (MaxValue - MinValue) * ScaleSweepAngle;
                arc.EndAngle = arc.StartAngle + currentAngle;
            }
        }
        /*
        /// <summary>
        /// 指針完成動畫後打開最佳值區間指示燈
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sb_Completed(object sender, EventArgs e)
        {
            if (this.CurrentValue > OptimalRangeEndValue)
            {
                lightIndicator.Fill = GetRangeIndicatorGradEffect(AboveOptimalRangeColor);
            }
            else if (this.CurrentValue <= OptimalRangeEndValue && this.CurrentValue >= OptimalRangeStartValue)
            {
                lightIndicator.Fill = GetRangeIndicatorGradEffect(OptimalRangeColor);

            }
            else if (this.CurrentValue < OptimalRangeStartValue)
            {
                lightIndicator.Fill = GetRangeIndicatorGradEffect(BelowOptimalRangeColor);
            }
        }
        */
        /// <summary>
        /// 獲取範圍指示燈的漸變效果
        /// </summary>
        /// <param name="gradientColor"></param>
        /// <returns></returns>
        private GradientBrush GetRangeIndicatorGradEffect(Color gradientColor)
        {
            LinearGradientBrush gradient = new LinearGradientBrush();
            gradient.StartPoint = new Point(0, 0);
            gradient.EndPoint = new Point(1, 1);
            GradientStop color1 = new GradientStop();
            if (gradientColor == Colors.Transparent)
            {
                color1.Color = gradientColor;
            }
            else
                color1.Color = Colors.LightGray;

            color1.Offset = 0.2;
            gradient.GradientStops.Add(color1);
            GradientStop color2 = new GradientStop();
            color2.Color = gradientColor; color2.Offset = 0.5;
            gradient.GradientStops.Add(color2);
            GradientStop color3 = new GradientStop();
            color3.Color = gradientColor; color3.Offset = 0.8;
            gradient.GradientStops.Add(color3);
            return gradient;
        }
        /// <summary>
        /// 模板應用
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //獲取對控件模板上已知元素的引用
            rootGrid = GetTemplateChild("LayoutRoot") as Grid;
            graphGrid = GetTemplateChild("GraphRoot") as Grid;
            //pointerCap = GetTemplateChild("PointerCap") as Ellipse;
            lightIndicator = GetTemplateChild("RangeIndicatorLight") as Ellipse;
            knobButton = GetTemplateChild("KnobButton") as Button;
            pointer = GetTemplateChild("Pointer") as Path;
            knobButton.Command = KnobButtonCommand;
            arc = GetTemplateChild("Arc") as Arc;
            //繪製刻度和範圍指示器
            DrawScale();
            //DrawRangeIndicator();

            //將指針和指針帽的Z軸設置為一個非常高的數字，以便它保持在頂部
            //刻度和範圍指示器
            Canvas.SetZIndex(pointer, 100001);
            Canvas.SetZIndex(knobButton, 100001);
            Canvas.SetZIndex(arc, 99999);

            if (ResetPointerOnStartUp)
            {
                //重置指針
                ResetPointer(ScaleStartAngle);
            }
        }

        //使用比例半徑繪製刻度
        private void DrawScale()
        {
            //計算主要刻度角
            Double majorTickUnitAngle = ScaleSweepAngle / MajorDivisionsCount;

            //計算子刻度角
            Double minorTickUnitAngle = ScaleSweepAngle / MinorDivisionsCount;

            //獲得主要刻度值
            Double majorTicksUnitValue = (MaxValue - MinValue) / MajorDivisionsCount;
            majorTicksUnitValue = Math.Round(majorTicksUnitValue, ScaleValuePrecision);

            Double minvalue = MinValue; ;

            //繪製主刻度
            for (Double i = ScaleStartAngle; i <= (ScaleStartAngle + ScaleSweepAngle); i += majorTickUnitAngle)
            {
                //主刻度繪製為矩形
                System.Windows.Shapes.Rectangle majortickrect = new System.Windows.Shapes.Rectangle();
                majortickrect.Height = MajorTickSize.Height;
                majortickrect.Width = MajorTickSize.Width;
                majortickrect.Fill = new SolidColorBrush(MajorTickColor);
                Point p = new Point(0.5, 0.5);
                majortickrect.RenderTransformOrigin = p;
                majortickrect.HorizontalAlignment = HorizontalAlignment.Center;
                majortickrect.VerticalAlignment = VerticalAlignment.Center;

                TransformGroup majortickgp = new TransformGroup();
                RotateTransform majortickrt = new RotateTransform();

                //獲取用於計算點的弧度角度
                Double i_radian = (i * Math.PI) / 180;
                majortickrt.Angle = i;
                majortickgp.Children.Add(majortickrt);
                TranslateTransform majorticktt = new TranslateTransform();

                //找到標尺上繪製主要刻度的點
                //這裡繪製中心為0,0的點
                majorticktt.X = (int)((ScaleRadius) * Math.Cos(i_radian));
                majorticktt.Y = (int)((ScaleRadius) * Math.Sin(i_radian));

                //保存刻度文字的座標
                TranslateTransform majorscalevaluett = new TranslateTransform();
                //here drawing the points with center as (0,0)
                majorscalevaluett.X = (int)((ScaleLabelRadius) * Math.Cos(i_radian));
                majorscalevaluett.Y = (int)((ScaleLabelRadius) * Math.Sin(i_radian));

                //定義刻度文字的屬性
                TextBlock tb = new TextBlock
                {
                    Height = ScaleLabelSize.Height,
                    Width = ScaleLabelSize.Width,
                    FontSize = ScaleLabelFontSize,
                    Foreground = new SolidColorBrush(ScaleLabelForeground),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                //寫入和附加比例值
                //檢查最小值<最大值w.r.t比例精度值
                if (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision))
                {
                    minvalue = Math.Round(minvalue, ScaleValuePrecision);
                    tb.Text = minvalue.ToString();
                    minvalue = minvalue + majorTicksUnitValue;
                }
                else
                {
                    break;
                }
                majortickgp.Children.Add(majorticktt);
                majortickrect.RenderTransform = majortickgp;
                tb.RenderTransform = majorscalevaluett;
                rootGrid.Children.Add(majortickrect);
                rootGrid.Children.Add(tb);


                //繪製短軸刻度
                Double onedegree = ((i + majorTickUnitAngle) - i) / (MinorDivisionsCount);

                if ((i < (ScaleStartAngle + ScaleSweepAngle)) && (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision)))
                {
                    //繪製子刻度
                    for (Double mi = i + onedegree; mi < (i + majorTickUnitAngle); mi = mi + onedegree)
                    {
                        //將子刻度定義為矩形
                        System.Windows.Shapes.Rectangle mr = new System.Windows.Shapes.Rectangle();
                        mr.Height = MinorTickSize.Height;
                        mr.Width = MinorTickSize.Width;
                        mr.Fill = new SolidColorBrush(MinorTickColor);
                        mr.HorizontalAlignment = HorizontalAlignment.Center;
                        mr.VerticalAlignment = VerticalAlignment.Center;
                        Point p1 = new Point(0.5, 0.5);
                        mr.RenderTransformOrigin = p1;

                        TransformGroup minortickgp = new TransformGroup();
                        RotateTransform minortickrt = new RotateTransform();
                        minortickrt.Angle = mi;
                        minortickgp.Children.Add(minortickrt);
                        TranslateTransform minorticktt = new TranslateTransform();

                        //獲取用於計算點的弧度角度
                        Double mi_radian = (mi * Math.PI) / 180;
                        //在刻度上找到繪製小刻度的座標
                        minorticktt.X = (int)((ScaleRadius) * Math.Cos(mi_radian));
                        minorticktt.Y = (int)((ScaleRadius) * Math.Sin(mi_radian));

                        minortickgp.Children.Add(minorticktt);
                        mr.RenderTransform = minortickgp;
                        rootGrid.Children.Add(mr);
                    }
                }
            }
        }

        /// <summary>
        /// 獲取圓周上的座標(x,y)
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Point GetCircumferencePoint(Double angle, Double radius)
        {
            Double angle_radian = (angle * Math.PI) / 180;
            //Radius-- is the Radius of the gauge
            Double X = (Double)((Radius) + (radius) * Math.Cos(angle_radian));
            Double Y = (Double)((Radius) + (radius) * Math.Sin(angle_radian));
            Point p = new Point(X, Y);
            return p;
        }
        /*
        /// <summary>
        /// 繪製指示燈
        /// </summary>
        private void DrawRangeIndicator()
        {
            Double realworldunit = ScaleSweepAngle / (MaxValue - MinValue);
            Double optimalStartAngle;
            Double optimalEndAngle;
            double db;

            if (graphGrid != null && graphGrid.Children.Count > 0)
            {
                graphGrid.Children.Clear();
            }            

            if (OptimalRangeStartValue < 0)
            {
                db = MinValue + OptimalRangeStartValue;
                optimalStartAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = MinValue + OptimalRangeStartValue;
                optimalStartAngle = ((double)(db * realworldunit));
            }

            if (OptimalRangeEndValue < 0)
            {
                db = MinValue + OptimalRangeEndValue;
                optimalEndAngle = ((double)(Math.Abs(db * realworldunit)));
            }
            else
            {
                db = MinValue + OptimalRangeEndValue;
                optimalEndAngle = ((double)(db * realworldunit));
            }
            //計算顏色區間的角度
            Double optimalStartAngleFromStart = ScaleStartAngle + optimalStartAngle;
            Double optimalEndAngleFromStart = ScaleStartAngle + optimalEndAngle;

            //計算區間的弧半徑
            arcradius1 = RangeIndicatorRadius + RangeIndicatorThickness;
            arcradius2 = RangeIndicatorRadius;

            double endAngle = ScaleStartAngle + ScaleSweepAngle;

            double intervalAngle = ScaleSweepAngle / Math.Abs(MaxValue - MinValue);
            
            for (int i=0;i<Math.Abs(CurrentValue - MinValue); i++)
            {
                //從中心計算下位顏色區間的座標                                
                Point A = GetCircumferencePoint(ScaleStartAngle + i * intervalAngle, arcradius1);
                Point B = GetCircumferencePoint(ScaleStartAngle + i * intervalAngle,  arcradius2);
                Point C = GetCircumferencePoint(ScaleStartAngle + (i * intervalAngle) + intervalAngle, arcradius2);
                Point D = GetCircumferencePoint((ScaleStartAngle + (i * intervalAngle) + intervalAngle), arcradius1);
                bool isReflexAngle = Math.Abs(optimalStartAngleFromStart - ScaleStartAngle) > 180.0;
                DrawSegment(A, B, C, D, isReflexAngle, BelowOptimalRangeColor);
            }
            
            //從中心計算下位顏色區間的座標
            Point A = GetCircumferencePoint(ScaleStartAngle, arcradius1);
            Point B = GetCircumferencePoint(ScaleStartAngle, arcradius2);
            Point C = GetCircumferencePoint(optimalStartAngleFromStart, arcradius2);
            Point D = GetCircumferencePoint(optimalStartAngleFromStart, arcradius1);

            bool isReflexAngle = Math.Abs(optimalStartAngleFromStart - ScaleStartAngle) > 180.0;
            DrawSegment(A, B, C, D, isReflexAngle, BelowOptimalRangeColor);
            
            //從中心計算中位顏色區間的座標
            Point A1 = GetCircumferencePoint(optimalStartAngleFromStart, arcradius1);
            Point B1 = GetCircumferencePoint(optimalStartAngleFromStart, arcradius2);
            Point C1 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius2);
            Point D1 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius1);
            bool isReflexAngle1 = Math.Abs(optimalEndAngleFromStart - optimalStartAngleFromStart) > 180.0;
            DrawSegment(A1, B1, C1, D1, isReflexAngle1, OptimalRangeColor);

            //從中心計算上位顏色區間的座標
            Point A2 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius1);
            Point B2 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius2);
            Point C2 = GetCircumferencePoint(endAngle, arcradius2);
            Point D2 = GetCircumferencePoint(endAngle, arcradius1);
            bool isReflexAngle2 = Math.Abs(endAngle - optimalEndAngleFromStart) > 180.0;
            DrawSegment(A2, B2, C2, D2, isReflexAngle2, AboveOptimalRangeColor);
    }    
    //用兩條弧線和兩條線繪製線段
    private void DrawSegment(Point p1, Point p2, Point p3, Point p4, bool reflexangle, Color clr)
        {
            PathSegmentCollection segments = new PathSegmentCollection();

            segments.Add(new LineSegment() { Point = p2 });

            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius2, arcradius2),
                Point = p3,
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = reflexangle
            });
            segments.Add(new LineSegment() { Point = p4 });

            segments.Add(new ArcSegment()
            {
                Size = new Size(arcradius1, arcradius1),
                Point = p1,
                SweepDirection = SweepDirection.Counterclockwise,
                IsLargeArc = reflexangle
            });

            Color rangestrokecolor;
            if (clr == Colors.Transparent)
            {
                rangestrokecolor = clr;
            }
            else
                rangestrokecolor = Colors.White;

            rangeIndicator = new Path()
            {
                StrokeLineJoin = PenLineJoin.Round,
                Stroke = new SolidColorBrush(rangestrokecolor),
                Fill = new SolidColorBrush(clr),
                Opacity = 0.65,
                StrokeThickness = 0.2,
                Data = new PathGeometry()
                {
                    Figures = new PathFigureCollection()
                     {
                        new PathFigure()
                        {
                            IsClosed = true,
                            StartPoint = p1,
                            Segments = segments
                        }
                    }
                }
            };
            rangeIndicator.SetValue(Canvas.ZIndexProperty, 100002);
            graphGrid.Children.Add(rangeIndicator);
        }*/
        #endregion
    }
}
