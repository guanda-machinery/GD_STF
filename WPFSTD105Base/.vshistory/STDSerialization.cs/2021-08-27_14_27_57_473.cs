using GD_STD;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105
{
    /// <summary>
    /// <see cref="GD_STD.Data"/> 物件序列化處理器
    /// </summary>
    public class STDSerialization
    {

        /// <summary>
        /// 標準建構式
        /// </summary>
        public STDSerialization()
        {
        }
        /// <summary>
        /// 取得構件資訊列表 (壓縮)
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SteelAssembly> GetGZipAssemblies()
        {
            return SerializationHelper.GZipDeserialize<ObservableCollection<SteelAssembly>>(ApplicationVM.DirectorySteelAssembly());
        }
        /// <summary>
        /// 存取構件資訊列表
        /// </summary>
        /// <param name="steels"></param>
        public void SetSteelAssemblies(ObservableCollection<SteelAssembly> steels)
        {
            SerializationHelper.SerializeBinary(steels, ApplicationVM.DirectorySteelAssembly());
        }
        public Dictionary<string, ObservableCollection<object>> GetPart
    }
}
