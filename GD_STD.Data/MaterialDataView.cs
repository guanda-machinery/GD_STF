using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GD_STD.Data
{
    [AttributeUsageAttribute(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class ExcelAttribute : Attribute
    {
        public ExcelAttribute(string columnName, int index)
        {
            ColumnName=columnName;
            Index=index;
        }

        public string ColumnName { get; }
        public int Index { get; }
    }
    /// <summary>
    /// 素材組合
    /// </summary>
    [Serializable]
    public class MaterialDataView : WPFWindowsBase.BaseViewModel, IProfile, IMatchSetting
    {
        private double lengthStr;

        /// <summary>
        /// 長度列表
        /// </summary>
        public ObservableCollection<double> LengthList { get; set; } = new ObservableCollection<double>();
        /// <summary>
        /// 來源列表
        /// </summary>
        public string Sources { get; set; } = string.Empty;
        /// <summary>
        /// 素材編號
        /// </summary>
        [Excel("素材編號", 0)]
        public string MaterialNumber { get; set; }
        /// <inheritdoc/>
        [Excel("斷面規格", 1)]
        public string Profile { get; set; }
        /// <summary>
        /// <see cref="LengthList"/> 索引值
        /// </summary>
        public int LengthIndex { get; set; }
        /// <summary>
        /// 購料長
        /// </summary>
        [Excel("購料長", 3)]
        public double LengthStr { get; set; }
        //{
        //    get => lengthStr;
        //    set
        //    {
        //        if (lengthStr != value)
        //        {
        //            int index = LengthList.IndexOf(lengthStr);
        //            if (index >=0)
        //            {
        //                LengthList[index] = value;
        //            }
        //        }
        //        lengthStr =value;
        //    }
        //}
        /// <summary>
        /// 材質
        /// </summary>
        [Excel("材質", 2)]
        public string Material { get; set; }
        /// <summary>
        /// <see cref="Sources"/> 索引值
        /// </summary>
        [Excel("廠商", 4)]
        public int SourceIndex { get; set; }
        /// <summary>
        /// 零件列表
        /// </summary>
        public ObservableCollection<TypeSettingDataView> Parts { get; set; } = new ObservableCollection<TypeSettingDataView>();
        #region ButtonCommand

        private bool _buttoneable = false;
        public bool ButtonEnable
        {
            get
            {
                return _buttoneable;
            }
            set
            {
                if (_buttoneable != value)
                {
                    _buttoneable = value;
                    //↓使isenable可即時反應
                    OnPropertyChanged("ButtonEnable");
                }
            }
        }





        private enum CRotation
        {
            Clockwise,
            CounterClockwise
        }
        /// <summary>
        /// 零件正轉命令
        /// </summary>
        public System.Windows.Input.ICommand ClockwiseRotationComponentCommand
        {
            get
            {
                return RotationComponent(CRotation.Clockwise); 
            }
        }
        /// <summary>
        /// 零件逆轉命令
        /// </summary>
        public System.Windows.Input.ICommand CounterClockwiseRotationComponentCommand
        {
            get
            {
                return RotationComponent(CRotation.CounterClockwise);
            }
        }
        /// <summary>
        /// 正逆轉功能，當ClockwiseRotation為false時為逆時針
        /// </summary>
        /// <param name="_cRotation"></param> 
        private WPFWindowsBase.RelayParameterizedCommand RotationComponent(CRotation _cRotation = CRotation.Clockwise)
        {
            return new WPFWindowsBase.RelayParameterizedCommand(objArray =>
            {
                if (objArrayComponentCorrespond(objArray, out var SelectedCom, out var TabC))
                {
                    //找出目標及指向之顯示介面後，顯示零件
                    if (SelectedCom != null)
                    {
                        if (_cRotation == CRotation.Clockwise)
                        {
                            //正轉控制
                        }
                        else
                        {
                            //逆轉控制
                        }
                    }
                }
            });
        }
        private enum CDirection
        {
            Forward,
            Backward
        }
        /// <summary>
        /// 零件前進命令
        /// </summary>
        public System.Windows.Input.ICommand ForwardComponentCommand
        {
            get
            {
                return MoveComponent(CDirection.Forward); ;
            }
        }
        /// <summary>
        /// 零件後退命令
        /// </summary>
        public System.Windows.Input.ICommand BackwardComponentCommand
        {
            get
            {
                return MoveComponent(CDirection.Backward); ;
            }
        }
        /// <summary>
        /// 前進後退功能，當Direction為false時為後退
        /// </summary>
        /// <param name="_cDirection"></param>
        private WPFWindowsBase.RelayParameterizedCommand MoveComponent(CDirection _cDirection = CDirection.Forward)
        {
            return new WPFWindowsBase.RelayParameterizedCommand(objArray =>
            {
                if (objArrayComponentCorrespond(objArray, out var SelectedCom, out var TabC))
                {
                    //找出目標及指向之顯示介面後，顯示零件
                    if (SelectedCom != null)
                    {
                        if (_cDirection == CDirection.Forward)
                        {
                            //前進控制
                        }
                        else
                        {
                            //後退控制
                        }
                    }
                }
            });
        }
        private enum CArithmetic
        {
            Carry,
            Borrow
        }
        /// <summary>
        /// 零件進位命令
        /// </summary>
        /// <returns></returns>
        public System.Windows.Input.ICommand CarryComponentCommand
        {
            get
            {
                return ArithmeticComponent(CArithmetic.Carry); ;
            }
        }
        /// <summary>
        /// 零件退位命令
        /// </summary>
        /// <returns></returns>
        public System.Windows.Input.ICommand BorrowComponentCommand
        {
            get
            {
                return ArithmeticComponent(CArithmetic.Borrow); ;
            }
        }
        /// <summary>
        /// 進位退位功能，當CarryBorrow為false時為退位
        /// </summary>
        /// <param name="_cArithmetic"></param>
        private WPFWindowsBase.RelayParameterizedCommand ArithmeticComponent(CArithmetic _cArithmetic = CArithmetic.Carry)
        {
            return new WPFWindowsBase.RelayParameterizedCommand(objArray =>
            {
                if (objArrayComponentCorrespond(objArray, out var SelectedCom, out var TabC))
                {
                    //找出目標及指向之顯示介面後，顯示零件
                    if (SelectedCom != null)
                    {
                        if (_cArithmetic == CArithmetic.Carry)
                        {
                            //進位控制
                        }
                        else
                        {
                            //退位控制
                        }
                    }
                }
            });


        }
        //刪除零件命令
        public System.Windows.Input.ICommand DeletePartCommand
        {
            get
            {

                    return new WPFWindowsBase.RelayParameterizedCommand(obj =>
                    {
                        if (obj is DevExpress.Xpf.Grid.GridControl)
                        {
                            var MessageBoxReturn = WinUIMessageBox.Show(null,
                                "是否要刪除零件\r\nRetry the connection ? ",
                                "通知",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Exclamation,
                                MessageBoxResult.None,
                                MessageBoxOptions.None,
                                FloatingMode.Popup);

                            if (MessageBoxReturn == MessageBoxResult.Yes)
                            {
                                var OBJItemsSource = ((obj as DevExpress.Xpf.Grid.GridControl).ItemsSource as IEnumerable<GD_STD.Data.TypeSettingDataView>).ToList();
                                var GC_SelectedItem = (obj as DevExpress.Xpf.Grid.GridControl).SelectedItem as GD_STD.Data.TypeSettingDataView;

                                if (OBJItemsSource.Remove(GC_SelectedItem))
                                {
                                    (obj as DevExpress.Xpf.Grid.GridControl).ItemsSource = OBJItemsSource;
                                    (obj as DevExpress.Xpf.Grid.GridControl).RefreshData();

                                    WinUIMessageBox.Show(null,
                                        $"刪除成功！",
                                        "通知",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Exclamation,
                                        MessageBoxResult.None,
                                        MessageBoxOptions.None,
                                        FloatingMode.Popup);
                                }
                            }


                        }

                });
            }
        }
        //零件刪除命令




        private bool objArrayComponentCorrespond(object objArray, out GD_STD.Data.TypeSettingDataView SelectedComponent, out object TabControl)
        {
            SelectedComponent = null;
            TabControl = null;

            if (objArray.GetType().Equals(typeof(object[])))
            {
                foreach (var obj in (object[])objArray)
                {
                    //確認為TypeSettingDataView
                    if(obj.GetType().Equals(typeof(DevExpress.Xpf.Grid.GridControl)))
                    {
                       var GControl = (DevExpress.Xpf.Grid.GridControl)obj;
                       
                    }
                    else if (obj.GetType().Equals(typeof(GD_STD.Data.TypeSettingDataView)))
                    {
                        //取得已選擇的欄位
                        SelectedComponent = (GD_STD.Data.TypeSettingDataView)obj;
                    }

                    //連結到圖形顯示
                    //ex: obj.GetType().Equals(typeof(GD_STD.Data.TypeSettingDataView))
                    //else if (obj.GetType().Equals(typeof(   )))
                    else if (false)
                    {

                    }
                    else
                    {

                    }


                    if (SelectedComponent != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion


        /// <inheritdoc/>
        public double StartCut { get; set; }
        /// <inheritdoc/>
        public double EndCut { get; set; }
        /// <inheritdoc/>
        public double Cut { get; set; }
        /// <summary>
        /// 完成進度
        /// </summary>
        public double Schedule { get; set; }
        /// <summary>
        /// 完成
        /// </summary>
        public bool Finish { get; set; }
        /// <summary>
        /// 物件座標
        /// </summary>
        public float CurrentCoordinate { get; set; }



        private string _position;
        /// <summary>
        /// 目前位置
        /// </summary>        
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }
        /// <summary>
        /// 素材消耗
        /// </summary>
        [Excel("消耗", 5)]
        public double Loss
        {
            //get => Parts.Select(el => el.Length).Aggregate((l1, l2) => l1 + l2)  + Cut * (Parts.Count -1d > 0d ? Parts.Count -1d : 0d); 
            get
            {
                try
                {
                    var AllPartAggregate = Parts.Select(el => el.Length).Aggregate((l1, l2) => l1 + l2); //總零件長
                    var CutLoss = Cut * (Parts.Count - 1d > 0d ? Parts.Count - 1d : 0d); //鋸床切割損耗
                    var SE_Cut = StartCut + EndCut; //素材前後切割
                                                    //材料前後端切除
                    return AllPartAggregate + CutLoss + SE_Cut;
                }
                catch(Exception ex)
                {
                    log4net.LogManager.GetLogger("點選GO").Debug("未找到零件,無法計算總消耗長度");
                    return 0;
                }
            }
        }


        public bool LockMark
        {
            get;set;
        }

        /// <summary>
        /// 素材數量
        /// </summary>
        public double MeterialCount { get; set; }





    }
}
