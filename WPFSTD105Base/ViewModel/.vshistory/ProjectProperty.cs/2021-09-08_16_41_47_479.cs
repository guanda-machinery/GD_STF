using GD_STD.Data;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFSTD105.Surrogate;
using WPFWindowsBase;

namespace WPFSTD105
{
    /// <summary>
    /// 專案屬性 VM
    /// </summary>
    [Serializable]
    public class ProjectProperty : BaseViewModel, IProjectProperty
    {
        /// <inheritdoc/>
        public ObservableCollection<BomProperty> BomProperties { get; set; }
        /// <inheritdoc/>
        public DateTime Create { get; set; }
        /// <inheritdoc/>
        public string Design { get; set; }
        /// <inheritdoc/>
        public bool IsNcLoad { get; set; }
        /// <inheritdoc/>
        public bool IsBomLoad { get; set; }
        /// <inheritdoc/>
        public string Location { get; set; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public string Number { get; set; }
        /// <inheritdoc/> 
        public DateTime NcLoad { get; set; }
        /// <inheritdoc/>
        public DateTime BomLoad { get; set; }
        /// <inheritdoc/>
        public DateTime Revise { get; set; }

        ///// <summary>
        ///// 轉換物件
        ///// </summary>
        ///// <param name="project"></param>
        //public ProjectProperty(ProjectPropertySurrogate project)
        //{
        //    Number = project.Number;
        //    BomProperties = project.BomProperties;
        //    Create = project.Create;
        //    Name = project.Name;
        //    Design = project.Design;
        //    Location = project.Location;
        //    NcLoad = project.NcLoad;
        //    Revise = project.Revise;
        //    IsNcLoad = project.IsNcLoad;
        //    IsBomLoad = project.IsBomLoad;
        //    BomLoad = project.BomLoad;
        //}
        /// <summary>
        /// 標準建構式
        /// </summary>
        public ProjectProperty()
        {
            BomProperties = new ObservableCollection<BomProperty>();
        }
    }
}
