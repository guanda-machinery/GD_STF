using devDept.Eyeshot.Translators;
using devDept.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devDept.Eyeshot;
using ProtoBuf.Meta;
using WPFSTD105.Attribute;
using WPFSTD105.Surrogate;
using devDept.Eyeshot.Entities;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 為Eyeshot專有文件格式定義擴展名。
    /// eyeshot的檔案是binary format，所以得用二進制的檔案儲存方式
    /// https://devdept.zendesk.com/hc/en-us/articles/360003318873-Eyeshot-Proprietary-File-Format
    /// </summary>
    public class FileSerializerExt : FileSerializer
    {
        /// <summary>
        /// 與<see cref ="WriteFile" />類一起使用的空構造函數，該類接受<see cref ="devDept.Eyeshot.Model" />作為參數。
        /// </summary>
        /// <remarks>
        /// 使用此構造函數為<see cref ="WriteFile"/>類定義序列化模型，該類接受<see cref ="devDept.Eyeshot.Model" />作為參數。
        /// </remarks>
        public FileSerializerExt()
        { 
        }
        /// <summary>
        /// 與<see cref ="ReadFile"/>類結合使用的構造方法。
        /// </summary>
        /// <param name="contentType"></param>
        /// <exception cref="EyeshotException">如果內容類型為<see cref ="contentType.None" />，則拋出該異常。</exception>
        /// <remarks>
        /// 使用此構造函數為<see cref ="ReadFile" />類定義序列化模型。
        /// </remarks>
        public FileSerializerExt(contentType contentType) : base(contentType)
        {
        }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'FileSerializerExt.FillModel()' 的 XML 註解
        protected override void FillModel()
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'FileSerializerExt.FillModel()' 的 XML 註解
        {
            base.FillModel();

            /*將自訂義資料格式添加到protobuf模型並定義其代理。*/
            AddSurrogate(typeof(CutPoint), typeof(CutPointSurrogate));
            AddSurrogate(typeof(CutList), typeof(CutListSurrogate));
            AddSurrogate(typeof(SteelAttr), typeof(SteelAttrSurrogate));
            AddSurrogate(typeof(GroupBoltsAttr), typeof(GroupBoltsAttrSurrogate));
            AddSurrogate(typeof(BoltAttr), typeof(BoltAttrSurrogate));
        }
        /// <summary>
        /// 加入序列化資料格式
        /// </summary>
        /// <param name="nativeData">原生物件</param>
        /// <param name="nativeSurrogate">原生資料格式</param>
        /// <param name="customData">自訂物件</param>
        /// <param name="customSurrogate">自訂資料格式</param>
        /// <param name="id"></param>
        /// <remarks>
        /// 將 customData 添加為 nativeData 的子類型
        /// 將子類型添加到Eyeshot對象時，必須使用id> 1000。
        /// </remarks>
        private void AddSurrogate(Type nativeData, Type nativeSurrogate, Type customData, Type customSurrogate, int id)
        {
            /*將子類型添加到Eyeshot對象時，必須使用id> 1000。*/
            Model[nativeData].AddSubType(id, customData);
            Model[nativeSurrogate].AddSubType(id, customSurrogate);
        }
        /// <summary>
        /// 加入序列化資料格式
        /// </summary>
        /// <param name="data"><see cref="Entity.EntityData"/> 物件</param>
        /// <param name="surrogate"><see cref="Entity.EntityData"/> 格式</param>
        private void AddSurrogate(Type data, Type surrogate)
        {
            Model.Add(data, false).SetSurrogate(surrogate);
            string[] vs = (from el in surrogate.GetProperties() where el.Name != "Log" select el.Name).ToArray();
            MetaType meta = Model[surrogate];
            for (int i = 0; i < vs.Length; i++)
            {
                meta.Add(i + 1, vs[i]);
            }
            //meta.SetCallbacks(null, null, data.FullName, null);
            meta.SetCallbacks(null, null, "BeforeDeserialize", null);
            meta.UseConstructor = false; // 避免使用無參數構造函數。
        }
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'FileSerializerExt.GetTypeForObject(string)' 的 XML 註解
        protected override Type GetTypeForObject(string typeName)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'FileSerializerExt.GetTypeForObject(string)' 的 XML 註解
        {
            Type t = Type.GetType(typeName, false, true);
            if (t != null)
                return t;
            return base.GetTypeForObject(typeName);
        }
    }
}
