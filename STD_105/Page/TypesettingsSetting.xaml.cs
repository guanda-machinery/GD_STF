using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;
using DevExpress.Data.Extensions;
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
using System.IO;
using GD_STD.Data;
using System.Collections.ObjectModel;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Core;

namespace STD_105.Office
{
    /// <summary>
    /// TypesettingsSetting.xaml 的互動邏輯
    /// </summary>
    public partial class TypesettingsSetting : BasePage<OfficeTypeSettingVM>
    {
        /// <summary>
        /// 220818 蘇冠綸 排版設定版面完成
        /// </summary>
        public TypesettingsSetting()
        {
            InitializeComponent();
        }


    }


    #region 測試
    /// <summary>
    /// 表格測試用綁定資料，完成後須刪除以下函式
    /// </summary>
    public class TestGridViewDataViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        #region Construction


        public TestGridViewDataViewModel()
        {

            TestData = new ObservableCollection<TestGridViewData>();
            TestData.Add(new TestGridViewData()
            {
                ExclamationMark = false,
                GearMark = true,
                LockMark = true,
                BuildTime = DateTime.Now,
                PieceLength = 100.5,
                PieceWeight = 200.05,
                MaterialPercent = 81,
                TypesettingNumber = "Piece1",
                EditTime = null,
                TestGridViewDataDetailList = new ObservableCollection<TestGridViewDataDetail>()
                {
                    new TestGridViewDataDetail() { Order = 1, StructNumber = "N100", PieceNumber = "M5", PieceLength = 100.5, PieceCount = 5 } ,
                    new TestGridViewDataDetail() { Order = 2, StructNumber = "N101", PieceNumber = "M6", PieceLength = 500.00, PieceCount = 1 }
                }
            });

            TestData.Add(new TestGridViewData()
            {
                ExclamationMark = false,
                GearMark = true,
                LockMark = true,
                BuildTime = DateTime.Now,
                PieceLength = 300,
                PieceWeight = 50,
                MaterialPercent = 50,
                TypesettingNumber = "Piece2",
                EditTime = null,
                TestGridViewDataDetailList = new ObservableCollection<TestGridViewDataDetail>()
                {
                    new TestGridViewDataDetail() { Order = 1, StructNumber = "N100", PieceNumber = "M5", PieceLength = 100.5, PieceCount = 5 } ,
                    new TestGridViewDataDetail() { Order = 2, StructNumber = "N101", PieceNumber = "M6", PieceLength = 500.00, PieceCount = 1 }
                }
            });

            _testdata = TestData;
        }
        #endregion


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        public class TestGridViewData
        {
            public bool ExclamationMark { get; set; }
            public bool GearMark { get; set; }
            public bool LockMark { get; set; }
            public DateTime BuildTime { get; set; }
            public DateTime? EditTime { get; set; }
            public string TypesettingNumber { get; set; }

            public double PieceLength { get; set; }
            public double PieceWeight { get; set; }
            public int MaterialPercent { get; set; }

            public ObservableCollection<TestGridViewDataDetail> TestGridViewDataDetailList { get; set; }

        }

        public class TestGridViewDataDetail
        {
            public int Order { get; set; }
            public string StructNumber { get; set; }
            public string PieceNumber { get; set; }
            public double PieceLength { get; set; }
            public int PieceCount { get; set; }
            public string Phase { get; set; }
        }



        private ObservableCollection<TestGridViewData> _testdata { get; set; }

        // 查詢結果
        public ObservableCollection<TestGridViewData> TestData
        {
            get { return _testdata; }
            set
            {
                _testdata = value;
                //RaisePropertyChanged("TestData");
            }
        }

    }
    #endregion
}








