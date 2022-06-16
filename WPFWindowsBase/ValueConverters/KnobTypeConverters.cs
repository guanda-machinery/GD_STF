using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace WPFWindowsBase
{

    /// <summary>
    /// Converts the given color to a SolidColorBrush
    /// </summary>
    public class ColorToSolidColorBrushConverter : BaseValueConverter<ColorToSolidColorBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;
            return new SolidColorBrush(c);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>   
    /// A type converter for converting image offset into render transform  
    /// </summary>   
    public class ImageOffsetConverter : BaseValueConverter<ImageOffsetConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dblVal = (double)value;
            TranslateTransform tt = new TranslateTransform
            {
                Y = dblVal
            };
            return tt;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts radius to diameter
    /// </summary>
    public class RadiusToDiameterConverter : BaseValueConverter<RadiusToDiameterConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dblVal = (double)value;
            return (dblVal * 2);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Calculates the pointer position
    /// </summary>
    public class PointerCenterConverter : BaseValueConverter<PointerCenterConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dblVal = (double)value;
            TransformGroup tg = new TransformGroup();
            RotateTransform rt = new RotateTransform();
            TranslateTransform tt = new TranslateTransform
            {
                X = dblVal / 2
            };
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            return tg;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Calculates the range indicator light position
    /// </summary>
    public class RangeIndicatorLightPositionConverter : BaseValueConverter<RangeIndicatorLightPositionConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dblVal = (double)value;
            TransformGroup tg = new TransformGroup();
            RotateTransform rt = new RotateTransform();
            TranslateTransform tt = new TranslateTransform
            {
                Y = dblVal
            };
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            return tg;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts the given Size to height and width
    /// </summary>
    public class SizeConverter : BaseValueConverter<SizeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double i = 0;
            Size s = (Size)value;
            if (parameter.ToString() == "Height")
            {
                i = s.Height;
            }
            else if (parameter.ToString() == "Width")
            {
                i = s.Width;
            }

            return i;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Scaling factor for drawing the glass effect.
    /// </summary>
    public class GlassEffectWidthConverter : BaseValueConverter<GlassEffectWidthConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dbl = (double)value;
            return (dbl * 2) * 0.94;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts background color to Gradient effect
    /// </summary>
    public class BackgroundColorConverter : BaseValueConverter<BackgroundColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = (Color)value;
            RadialGradientBrush radBrush = new RadialGradientBrush();
            GradientStop g1 = new GradientStop
            {
                Offset = 0.982,
                Color = c
            };
            GradientStop g2 = new GradientStop
            {
                Color = Color.FromArgb(0xFF, 0xAF, 0xB2, 0xB0)
            };
            radBrush.GradientStops.Add(g1);
            radBrush.GradientStops.Add(g2);
            return radBrush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 轉盤起始角度換算成Arc起始角
    /// </summary>
    public class ScaleStartAngleToArcStartAngleConverter : BaseValueConverter<ScaleStartAngleToArcStartAngleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)value;
            angle += 90d;
            return angle;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轉盤起始角度換算成Arc結束角
    /// </summary>
    public class ScaleStartAngleToArcEndAngleConverter : BaseValueConverter<ScaleStartAngleToArcEndAngleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double angle = (double)value;            
            return - (angle + 90);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
