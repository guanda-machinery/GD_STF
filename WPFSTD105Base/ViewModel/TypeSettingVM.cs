using GD_STD;
using GD_STD.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBase = WPFWindowsBase;
using WPFSTD105.Properties;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using WPFWindowsBase;
using DevExpress.Xpf.Grid;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 排版設定
    /// </summary>
    public class TypeSettingVM : AbsTypeSettingVM
    {
        /// <summary>
        /// 顯示報表清單
        /// </summary>
        public bool NotShowDetail { get; set; } = true;
        /// <summary>
        /// 註冊命令
        /// </summary>
        public TypeSettingVM()
        {
            ExpandTableDetailCommand = ExpandTableDetail();
        }

        //protected override RelayCommand Auto()
        //{
        //    throw new NotImplementedException();
        //}

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'TypeSettingVM.Manual()' 的 XML 註解
        protected override RelayCommand Manual()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'TypeSettingVM.Manual()' 的 XML 註解
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 收折結果報表
        /// </summary>
        public ICommand ExpandTableDetailCommand { get; set; }
        private RelayParameterizedCommand ExpandTableDetail()
        {
            return new RelayParameterizedCommand(el =>
            {
                GridControl gridControl = el as GridControl;

                if (!NotShowDetail)
                {
                    for (int i = 0; i < gridControl.VisibleRowCount; i++)
                    {
                        var rowHandle = gridControl.GetRowHandleByVisibleIndex(i);
                        gridControl.ExpandMasterRow(rowHandle);
                    }
                }
                else
                {
                    for (int i = 0; i < gridControl.VisibleRowCount; i++)
                    {
                        var rowHandle = gridControl.GetRowHandleByVisibleIndex(i);
                        gridControl.CollapseMasterRow(rowHandle);
                    }
                }
            });
        }
    }
}
