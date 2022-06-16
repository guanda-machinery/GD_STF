using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFSTD105;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.ViewModel;
using WPFWindowsBase;
using static devDept.Eyeshot.Entities.Mesh;
using static devDept.Eyeshot.Environment;
using BlockReference = devDept.Eyeshot.Entities.BlockReference;
using MouseButton = devDept.Eyeshot.MouseButton;

namespace STD_105.Office
{
    /// <summary>
    /// ObSettingsPage_Office.xaml 的互動邏輯
    /// </summary>
    public partial class ObSettingsPage_Office : BasePage
    {
        public ObSettingsPage_Office()
        {
            InitializeComponent();
            DataContext = new ObSettingVM();
        }
        
       //// <summary>
       //// 當項目從已載入項目的項目樹狀結構中移除時發生。
       //// </summary>
       //// <param name="sender"></param>
       //// <param name="e"></param>
       //rivate void BasePage_Unloaded(object sender, RoutedEventArgs e)
       //
       //  model.Dispose();//釋放資源
       //  drawing.Dispose();//釋放資源
       //  drawing.Loaded -= drawing_Loaded;
       //  GC.Collect();
       //
       //
       //rivate void drawing_Loaded(object sender, RoutedEventArgs e)
       //
       //  //平移滑鼠中鍵
       //  drawing.Pan.MouseButton = new MouseButton(mouseButtonsZPR.Middle, modifierKeys.None);
       //  drawing.ActionMode = actionType.SelectByBox;
       //  drawing.ZoomFit();//設置道適合的視口
       //  drawing.Refresh();//刷新模型
       //
       //rivate void TabControlSelectedIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
       //
       //  if (tabControl.SelectedIndex == 1)
       //      drawing.CurrentModel = true;
       //  else
       //      drawing.CurrentModel = false;
       //
       //// <summary>
       //// 選擇面
       //// </summary>
       //// <param name="sender"></param>
       //// <param name="e"></param>
       //rivate void model_SelectionChanged(object sender, devDept.Eyeshot.Environment.SelectionChangedEventArgs e)
       //
       //  // 計算選定的實體
       //  object[] selected = new object[e.AddedItems.Count];
       //
       //   int selectedCount = 0;
       //
       //  // 填充選定的數組
       //  for (int index = 0; index < e.AddedItems.Count; index++)
       //  {
       //      var item = e.AddedItems[index];
       //
       //      if (item is SelectedFace)
       //      {
       //          var faceItem = (SelectedFace)item;
       //          var ent = faceItem.Item;
       //
       //          if (ent is Mesh)
       //          {
       //              var mesh = (Mesh)ent;
       //              selected[selectedCount++] = mesh.Faces[faceItem.Index];
       //              List<int> faceElement = ((FaceElement)selected[0]).Triangles;
       //              Plane plane = new Plane(mesh.Vertices[mesh.Triangles[faceElement[0]].V2], mesh.Vertices[mesh.Triangles[faceElement[0]].V1], mesh.Vertices[mesh.Triangles[faceElement[0]].V3]);
       //              model.SetDrawingPlan(plane);
       //              model.ClearAllPreviousCommandData();
       //              model.ActionMode = actionType.None;
       //              model.objectSnapEnabled = true;
       //              model.drawingLinearDim = true;
       //          }
       //      }
       //  }
       //
       //// <summary>
       //// 修改螺栓狀態
       //// </summary>
       //ublic bool modifyHole { get; set; } = false;
       //// <summary>
       //// 3D Model 載入
       //// </summary>
       //// <param name="sender"></param>
       //// <param name="e"></param>
       //rivate void Model3D_Loaded(object sender, RoutedEventArgs e)
       //
       //  #region Model 初始化
       //  //model.InitialView = viewType.Top;
       //  /*旋轉軸中心設定當前的鼠標光標位置。 如果模型全部位於相機視錐內部，
       //   * 它圍繞其邊界框中心旋轉。 否則它繞著下點旋轉鼠標。 如果在鼠標下方沒有深度，則旋轉發生在
       //   * 視口中心位於當前可見場景的平均深度處。*/
       //  //model.Rotate.RotationCenter = rotationCenterType.CursorLocation;
       //  //旋轉視圖 滑鼠中鍵 + Ctrl
       //  model.Rotate.MouseButton = new MouseButton(mouseButtonsZPR.Middle, modifierKeys.Ctrl);
       //
       //  //平移滑鼠中鍵
       //  model.Pan.MouseButton = new MouseButton(mouseButtonsZPR.Middle, modifierKeys.None);
       //  model.ActionMode = actionType.SelectByBox;
       //  if (ViewModel.Reductions == null)
       //  {
       //      ViewModel.Reductions = new ReductionList(model, drawing); //紀錄使用找操作
       //  }
       //  #endregion
        //}
    }
}
