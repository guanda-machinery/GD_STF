using GD_STD.Data;
using System;
using System.Collections.ObjectModel;

namespace WPFSTD105
{
    /// <summary>
    /// 專案屬性介面
    /// </summary>
    public interface IProjectProperty
    {
        /// <summary>
        /// 報表屬性設定
        /// </summary>
        ObservableCollection<BomProperty> BomProperties { get; set; }
        /// <summary>
        /// 專案建置日期
        /// </summary>
        DateTime Create { get; set; }
        /// <summary>
        /// 設計人員名稱
        /// </summary>
        string Design { get; set; }
        /// <summary>
        /// 有加載 nc 檔
        /// </summary>
        bool IsNcLoad { get; set; }
        /// <summary>
        /// 有加載 Bom 檔與報表
        /// </summary>
        bool IsBomLoad { get; set; }
        /// <summary>
        /// 地理位置
        /// </summary>
        string Location { get; set; }
        /// <summary>
        /// 專案名稱
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 工程號碼
        /// </summary>
        string Number { get; set; }
        /// <summary>
        /// NC 檔案載入時間
        /// </summary>
        DateTime NcLoad { get; set; }
        /// <summary>
        /// BOM 檔案載入時間
        /// </summary>
        DateTime BomLoad { get; set; }
        /// <summary>
        /// 修改專案日期
        /// </summary>
        DateTime Revise { get; set; }
    }
}