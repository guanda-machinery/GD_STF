using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105
{
    /// <summary>
    /// NC 實體列表
    /// </summary>
    [Serializable]
    public class NcTempList : List<NcTemp>
    {
        /// <summary>
        /// 取得設定檔，如過有取得到物件將會刪除。
        /// </summary>
        /// <param name="parNumber">編號</param>
        /// <returns></returns>
        public NcTemp GetData(string parNumber)
        {
            if (dataName == null)
            {
                throw new Exception("不可是空值");
            }
            int index = this.FindIndex(el => el.SteelAttr.PartNumber == parNumber); //取得設定檔
            if (index == -1)
            {
                return null;
            }
            NcTemp result = this[index];
            RemoveAt(index);//刪除物件

            return result;
        }
        /// <summary>
        /// 加入物件
        /// </summary>
        /// <param name="nc"></param>
        public new void Add(NcTemp nc)
        {
            int index = this.FindIndex(el => el.SteelAttr.PartNumber == nc.SteelAttr.PartNumber); //查詢是否有相同物件
            if (index == -1)//沒有相同物件
            {
                base.Add(nc); //加入物件
            }
            else
            {
                this[index] = nc;
            }
        }
    }
}
