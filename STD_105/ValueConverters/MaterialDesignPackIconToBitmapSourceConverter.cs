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

    public class MaterialDesignPackIconToBitmapSourceConverter : BaseValueConverter<MaterialDesignPackIconToBitmapSourceConverter>
    {


        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MaterialDesignThemes.Wpf.PackIcon PackIconSource = (MaterialDesignThemes.Wpf.PackIcon)value;
         
            if (PackIconSource.Width is double.NaN)
            {
                PackIconSource.Width = 50;
            }
            if (PackIconSource.Height is double.NaN)
            {
                PackIconSource.Height = 50;
            }

            GeometryDrawing geoDrawing = new GeometryDrawing();
            geoDrawing.Brush =  Brushes.Black;
            geoDrawing.Pen = new Pen(geoDrawing.Brush, 0.00);
            geoDrawing.Geometry = Geometry.Parse(PackIconSource.Data);

            var geoDrawingWhite = geoDrawing.Clone();
            geoDrawingWhite.Pen = new Pen(Brushes.White, 0.75);

            var drawingGroup = new DrawingGroup { Children = { geoDrawing,geoDrawing , geoDrawingWhite } };

            var DrawImage = new DrawingImage { Drawing = drawingGroup };
           // return DrawImage;

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
