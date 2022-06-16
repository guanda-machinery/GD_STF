using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;
using devDept.Eyeshot;
using WPFSTD105.Model;
using devDept.Eyeshot.Translators;
using System.IO;
using devDept.Geometry;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 加工監控VM
    /// </summary>
    public class ProcessingMonitorVM : BaseViewModel
    {
        #region 公開屬性
        /// <summary>
        /// 配料清單
        /// </summary>
        public ObservableCollection<MaterialDataView> DataView { get; set; }

        public MaterialDataView SelectedItem
        {
            get => _SelectedItem;
            set
            {
                Model.Clear();
                STDSerialization ser = new STDSerialization();
                _SelectedItem = value;
                ReadFile file = ser.ReadMaterialModel(value.MaterialNumber);
                file.DoWork();
                file.AddToScene(Model);
                Model.Refresh();
                Model.ZoomFit();//設置道適合的視口
                Model.Invalidate();
            }
        }
        #endregion

        #region 私有屬性
        private devDept.Eyeshot.Model Model { get; set; }
        private MaterialDataView _SelectedItem;
        #endregion

        #region 命令

        #endregion

        /// <summary>
        /// 建構式
        /// </summary>
        public ProcessingMonitorVM()
        {
            STDSerialization ser = new STDSerialization();
            DataView = ser.GetMaterialDataView();
        }
        /// <summary>
        /// 存取模型
        /// </summary>
        /// <param name="mdoel"></param>
        public void SetModel(devDept.Eyeshot.Model mdoel)
        {
            Model = mdoel;
            CreateFile();
        }
        /// <summary>
        /// 產生 <see cref="MaterialDataView"/> 的3D檔案
        /// </summary>
        public void CreateFile()
        {
            STDSerialization ser = new STDSerialization();
            DataView.ForEach(el =>
            {
                Model.Clear();
                MaterialDataView dataView = el;
                if (!File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{el.MaterialNumber}.dm"))
                {
                    Model.AssemblyPart(el.MaterialNumber);
                    ser.SetMaterialModel(el.MaterialNumber, Model);//儲存 3d 視圖
                }
                else
                {
                    ReadFile file = ser.ReadMaterialModel(el.MaterialNumber);
                    file.DoWork();
                    file.AddToScene(Model);
                }
                Model.Entities.ForEach(entity =>
                {
                    if (entity.EntityData is WPFSTD105.Attribute.GroupBoltsAttr)
                    {
                        Point3D boxMin, boxMax;
                        Utility.ComputeBoundingBox(new Transformation, entity.Vertices, out boxMin, out boxMax);
                        Point3D center = (boxMin + boxMax) / 2;

                    }
                });
            });
            Model.Clear();
            ser.SetMaterialDataView(DataView);
        }
        /// <summary>
        /// 轉換加工陣列
        /// </summary>
        private void AsDrill()
        {

        }
    }
}
