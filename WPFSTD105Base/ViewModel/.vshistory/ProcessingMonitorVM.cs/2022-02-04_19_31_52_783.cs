﻿using GD_STD.Data;
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

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 加工監控VM
    /// </summary>
    public class ProcessingMonitorVM : BaseViewModel
    {
        /// <summary>
        /// 選擇 <see cref="DataView"/> 變更命令
        /// </summary>
        public WPFWindowsBase.ParameterizedCommandAync SelectedItemChangedCommand { get; set; }
        /// <summary>
        /// 選擇變更
        /// </summary>
        /// <returns></returns>
        public WPFWindowsBase.ParameterizedCommandAync SelectedItemChanged()
        {
            return new WPFWindowsBase.ParameterizedCommandAync(async el =>
            {
                Model.Clear();

                STDSerialization ser = new STDSerialization();
                ReadFile file = ser.ReadMaterialModel(dataView.MaterialNumber);
                file.DoWork();
                file.AddToScene(Model);

            });
        }
        /// <summary>
        /// 配料清單
        /// </summary>
        public ObservableCollection<MaterialDataView> DataView { get; set; }
        private devDept.Eyeshot.Model Model { get; set; }
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
                MaterialDataView dataView = el;
                if (!File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{el.MaterialNumber}.dm"))
                {
                    Model.Clear();
                    Model.AssemblyPart(el.MaterialNumber);
                    ser.SetMaterialModel(el.MaterialNumber, Model);//儲存 3d 視圖
                }
            });
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
