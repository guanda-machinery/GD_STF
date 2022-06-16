using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GD_STF
{
    /// <summary>
    /// 二進制序列化介面
    /// </summary>
    public interface IBinaryData
    {
        /// <summary>
        /// 寫入二進制檔案
        /// </summary>
        void WriteBinary(object ob, string path);

    }
}