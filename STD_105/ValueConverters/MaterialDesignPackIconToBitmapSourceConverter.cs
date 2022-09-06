using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// 將MaterialDesignPackIcon轉為Bitmap
    /// </summary>
    public class MaterialDesignPackIconToBitmapSourceConverter : BaseValueConverter<MaterialDesignPackIconToBitmapSourceConverter>
    {


        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MaterialDesignThemes.Wpf.PackIcon PackIconSource = (MaterialDesignThemes.Wpf.PackIcon)value;

            if (PackIconSource.Width is double.NaN)
            {
                PackIconSource.Width = 48;
            }
            if (PackIconSource.Height is double.NaN)
            {
                PackIconSource.Height = 48;
            }



            //外框線
            GeometryDrawing geoDrawing = new GeometryDrawing(Brushes.Black, new Pen(Brushes.White, 0.75), Geometry.Parse(PackIconSource.Data));

            var RectGeo = new RectangleGeometry(new Rect());
            if (parameter is Brush)
            {
                PackIconSource.Background = (parameter as Brush);
                // var RectGeo = new RectangleGeometry(new Rect(0, 0, geoDrawing.Bounds.Width, geoDrawing.Bounds.Height));
                RectGeo = new RectangleGeometry(new Rect(0, 0, geoDrawing.Bounds.Width, geoDrawing.Bounds.Height));
            }
            GeometryDrawing BackgroundDrawing = new GeometryDrawing(Brushes.Blue, new Pen(), RectGeo);



            var drawingGroup = new DrawingGroup { Children = { BackgroundDrawing , geoDrawing  } };
            var DrawImage = new DrawingImage { Drawing = drawingGroup };

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            //DrawingImage source = (DrawingImage)value;
            drawingContext.DrawImage(DrawImage, new Rect(new Point(0, 0), new Size(PackIconSource.Width, PackIconSource.Height)));
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)PackIconSource.Width, (int)PackIconSource.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
