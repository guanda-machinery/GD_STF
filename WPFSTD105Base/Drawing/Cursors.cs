//using devDept.Geometry;

//namespace WPFSTD105
//{
//    public partial class DrawingsHelper
//    {
//        /// <summary>
//        /// 在當前鼠標位置繪製一個加號（+）
//        /// </summary>
//        private void DrawPositionMark(Point3D current, double crossSize = 20.0)
//        {
//            Point3D currentScreen = _drawings.WorldToScreen(current);

//            // 計算屏幕上的水平線方向
//            Point2D left = _drawings.WorldToScreen(current.X - 1, current.Y, 0);
//            Vector2D dirHorizontal = Vector2D.Subtract(left, currentScreen);
//            dirHorizontal.Normalize();

//            // 計算屏幕上水平線端點的位置
//            left = currentScreen + dirHorizontal * crossSize;
//            Point2D right = currentScreen - dirHorizontal * crossSize;

//            _drawings.renderContext.DrawLine(left, right);

//            // 計算屏幕上的垂直線方向
//            Point2D bottom = _drawings.WorldToScreen(current.X, current.Y - 1, 0);
//            Vector2D dirVertical = Vector2D.Subtract(bottom, currentScreen);
//            dirVertical.Normalize();

//            // 計算屏幕上的水平線方向
//            bottom = currentScreen + dirVertical * crossSize;
//            Point2D top = currentScreen - dirVertical * crossSize;

//            _drawings.renderContext.DrawLine(bottom, top);

//            _drawings.renderContext.SetLineSize(1);
//        }
//        /// <summary>
//        /// 在當前鼠標位置繪製選擇框
//        /// </summary>
//        public void DrawSelectionMark(System.Drawing.Point current)
//        {
//            // 取取框的大小
//            double size = _drawings.PickBoxSize;
//            _drawings.renderContext.EnableXOR(true);

//#if WINFORMS
//            double height = _drawings.Height;
//#else
//            double height = _drawings.Size.Height;
//#endif
//            double x1 = current.X - (size / 2);
//            double y1 = height - current.Y - (size / 2);
//            double x2 = current.X + (size / 2);
//            double y2 = height - current.Y + (size / 2);

//            Point3D bottomLeftVertex = new Point3D(x1, y1);
//            Point3D bottomRightVertex = new Point3D(x2, y1);
//            Point3D topRightVertex = new Point3D(x2, y2);
//            Point3D topLeftVertex = new Point3D(x1, y2);

//            // 畫盒子
//            _drawings.renderContext.DrawLineLoop(new Point3D[]
//            {
//                bottomLeftVertex,
//                bottomRightVertex,
//                topRightVertex,
//                topLeftVertex
//            });

//            _drawings.renderContext.SetLineSize(1);
//            _drawings.renderContext.EnableXOR(false);
//        }
//    }
//}
