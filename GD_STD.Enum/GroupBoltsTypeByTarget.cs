using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GD_STD.Enum
{
    /// <summary>
    /// 客製孔群 - 依附類別(系統客製、專案客製)(下拉或核選元件)
    /// </summary>
    public enum GroupBoltsTypeByTarget
    {
        [Image(), Display(Name = "系統客製"), Description("系統資料夾")]
        System,
        [Image(), Display(Name = "專案客製"), Description("專案資料夾")]
        Project
    }
}
