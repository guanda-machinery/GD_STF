using DevExpress.XtraRichEdit.Model;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105.ViewModel;

namespace WPFSTD105.Surrogate
{
    /// <summary>
    /// 專案屬性序列化檔案
    /// </summary>
    [Serializable]
    public class ProjectPropertySurrogate : IProjectProperty
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        /// <param name="project"></param>
        public ProjectPropertySurrogate(ProjectProperty project)
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
        /// <inheritdoc/> 
        public ObservableCollection<BomProperty> BomProperties { get; set; }
        /// <inheritdoc/>
        public DateTime Create { get; set; }
        /// <inheritdoc/>
        public string Design { get; set; }
        /// <inheritdoc/>
        public bool Load { get; set; }
        /// <inheritdoc/>
        public string Location { get; set; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public string Number { get; set; }
        /// <inheritdoc/>
        public DateTime NcLoad { get; set; }
    }
}
