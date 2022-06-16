using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Data.Properties
{
    /// <summary>
    /// 變更介面
    /// </summary>
    public interface IChange
    {
        /// <summary>
        /// 刪除物件
        /// </summary>
        /// <param name="id">Tekla ID</param>
        void Reduce(int id);
        /// <summary>
        /// 更新物件
        /// </summary>
        /// <param name="obj"></param>
        void Update(object obj);
    }
}
