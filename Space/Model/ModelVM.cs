using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;
namespace Space
{
    /// <summary>
    /// 3D 視圖 ViewModell
    /// </summary>
    public class ModelVM : BaseViewModel
    {

        #region 公開屬性
        /// <summary>
        /// 實體列表
        /// </summary>
        public EntityList EntityList
        {
            get { return _EntityList; }
            set
            {
                _EntityList = value;
                //OnPropertyChanged("ViewportEntities");
                //TODO:這還不知道要幹嘛
                Debugger.Break();
            }
        }
        #endregion

        #region 私有屬性
        public EntityList _EntityList = new EntityList();
        #endregion
    }
}
