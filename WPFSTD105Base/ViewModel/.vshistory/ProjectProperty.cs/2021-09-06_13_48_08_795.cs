using GD_STD.Data;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.Surrogate;
using WPFWindowsBase;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 專案屬性
    /// </summary>
    public class ProjectProperty : BaseViewModel, IProjectProperty
    {
        /// <summary>
        /// 轉換物件
        /// </summary>
        /// <param name="project"></param>
        public ProjectProperty(ProjectPropertySurrogate project)
        {
            Number = project.Number;
            BomProperties = project.BomProperties;
            Load = project.Load;
            Create = project.Create;
            Name = project.Name;
            Design = project.Design;
            Location = project.Location;
            NcLoad = project.NcLoad;
        }
        /// <summary>
        /// 標準建構式
        /// </summary>
        public ProjectProperty()
        {
            BomProperties = new ObservableCollection<BomProperty>();
        }
        /// <inheritdoc/>
        public string Number { get; set; }
        /// <inheritdoc/>
        public ObservableCollection<BomProperty> BomProperties { get; set; }
        /// <inheritdoc/>
        public bool Load { get; set; }
        /// <inheritdoc/>
        public DateTime Create { get; set; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public string Design { get; set; }
        /// <inheritdoc/>
        public string Location { get; set; }
        /// <inheritdoc/>
        public DateTime NcLoad { get; set; }
        /// <inheritdoc/>
        public DateTime Revise { get; set; }
    }
}
