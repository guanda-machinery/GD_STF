using GD_STD.Attribute;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace GD_STD.MS
{
    /// <summary>
    /// 是一個抽象的 MSSQL 資料表
    /// </summary>
    [DataContract]
    public class AbsMS : IMS
    {
        /// <summary>
        /// 抽象的 MSSQL 資料表
        /// </summary>
        /// <exception cref="TypeInitializationException">初始化失敗，造成的原因有可能是沒有附加 MSFieldAttribute Or MSTableAttribute。</exception>
        public AbsMS()
        {
            if (AllValue().Length == 0)
            {
                throw new TypeInitializationException(this.Type, new Exception("初始化失敗，造成的原因，尚未找到 MSFieldAttribute 的資料欄位"));
            }
            else if (GetType().GetCustomAttribute(typeof(MSTableAttribute)) == null)
            {
                throw new TypeInitializationException(this.Type, new Exception("初始化失敗，造成的原因，尚未找到 MSTableAttribute 的資料欄位"));
            }
            // MSFieldAttribute.Description 有重複
            bool repeatField = AllValue().GroupBy(x => ((MSFieldAttribute)x.GetCustomAttribute(typeof(MSFieldAttribute))).Description)
                                         .Any(x => x.Count() > 1);
            if (repeatField)
            {
                throw new TypeInitializationException(this.Type, new Exception("初始化失敗，造成的原因是位尚未找到 MSFieldAttribute.Description 名稱重複"));
            }

            Type = this.GetType().AssemblyQualifiedName;
        }
        /// <summary>
        /// 取得類型的組件限定名稱，包含載入 System.Type 的組件名稱。
        /// </summary>
        [DataMember]
        public string Type { get; set; }
        /// <summary>
        /// 公司名稱
        /// </summary>
        [MSField("公司名稱", "Company")]
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// 觸發時間
        /// </summary>
        [MSField("觸發日期", "DateTime")]
        [DataMember]
        public string DateTime { get; set; }
        /// <summary>
        /// 取得全部的欄位屬性
        /// </summary>
        /// <returns></returns>
        public PropertyInfo[] AllValue() => GetType().GetProperties().Where(el => el.GetCustomAttribute(typeof(MSFieldAttribute)) != null).ToArray();
    }
}
