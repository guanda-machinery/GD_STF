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
using devDept.Eyeshot.Entities;
using WPFSTD105.Attribute;
using System.Diagnostics;

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
        /// <summary>
        /// 選擇的資料行
        /// </summary>
        public MaterialDataView SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _Model.Clear();
                STDSerialization ser = new STDSerialization();
                _SelectedItem = value;
                ReadFile file = ser.ReadMaterialModel(value.MaterialNumber);
                file.DoWork();
                file.AddToScene(_Model);
                _Model.Refresh();
                _Model.ZoomFit();//設置道適合的視口
                _Model.Invalidate();
            }
        }
        #endregion

        #region 私有屬性
        private devDept.Eyeshot.Model _Model { get; set; }
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
            _Model = mdoel;
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
                _Model.Clear();
                if (!File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{el.MaterialNumber}.dm"))
                {
                    _Model.AssemblyPart(el.MaterialNumber);
                    ser.SetMaterialModel(el.MaterialNumber, _Model);//儲存 3d 視圖
                }
                ReadFile file = ser.ReadMaterialModel(el.MaterialNumber);
                file.DoWork();
                file.AddToScene(_Model);
                _Model.Entities.ToList().ForEach(reference =>
                    {
                        BlockReference blockReference = (BlockReference)reference;
                        _Model.SetCurrent(blockReference);
                        _Model.Entities.ForEach(entity => 
                        {
                            if (entity.EntityData is BoltAttr boltAttr)
                            {
                                //Point3D boxMin, boxMax;
                                //Utility.ComputeBoundingBox(null, entity.Vertices, out boxMin, out boxMax);
                                //Point3D center = (boxMin + boxMax) / 2;
                                //Debug.WriteLine($"x={center.X}, y={center.Y}, z={center.Z}");
                            }
                        });
                        _Model.SetCurrent(null);
                    });
            });
            _Model.Clear();
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
