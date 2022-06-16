using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WPFSTD105.Attribute
{
    /// <summary>
    /// 一個抽象的自定義屬性檔案
    /// </summary>
    [Serializable]
    public abstract class AbsAttr : ICloneable, IModelObjectBase
    {
        /// <summary>
        /// 物件ID
        /// </summary>
        public Guid? GUID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 物件類型
        /// </summary>
        [Html("ProfileType")]
        public virtual OBJETC_TYPE Type { get; set; }
        /// <summary>
        /// 淺層複製
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 深層複製物件
        /// </summary>
        /// <returns></returns>
        public object DeepClone()
        {
            using (Stream stream = new MemoryStream())
            {

                IFormatter formatter = new BinaryFormatter(); //序列化物件格式
                formatter.Serialize(stream, this); //將自己所有資料序列化
                stream.Seek(0, SeekOrigin.Begin);//複寫資料流位置,返回最前端
                AbsAttr result = (AbsAttr)formatter.Deserialize(stream); //再將Stream反序列化回去 
                //result.GUID = Guid.NewGuid(); //產生新的 GUID
                return result;
            }
        }
    }
}
