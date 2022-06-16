using GD_STD.Base;
using GD_STD.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    /// <summary>
    /// Phone 條件判斷
    /// </summary>
    [DataContract]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class PhoneConditionAttribute : Attribute
    {

        public PhoneConditionAttribute()
        {
            Write = false;
        }
        ///// <summary>
        ///// 建構式
        ///// </summary>
        ///// <param name="fieldName">欄位可寫入</param>
        ///// <param name="value">成功條件的值</param>
        //public PhoneConditionAttribute( object value, params string[] fieldName)
        //{
        //    this.Write = true;
        //    this.FieldName = fieldName;
        //    this.Value = value;
        //}
        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="isConditionInitial">要比較目前記憶體的值回傳 true , 不要比較目前記憶體的值回傳 false。</param>
        /// <param name="value">成功條件值</param>
        /// <param name=""></param>
        public PhoneConditionAttribute(object value, string fieldName, bool isConditionInitial = true)
        {
            this.Write = true;
            this.FieldName = fieldName;
            this.Value = value;
            IsConditionInitial = isConditionInitial;
        }
        /// <summary>
        /// 要比較目前記憶體的值回傳 true , 不要比較目前記憶體的值回傳 false。
        /// </summary>
        public bool IsConditionInitial { get; private set; }
        /// <summary>
        /// 欄位可寫入
        /// </summary>
        [DataMember]
        public bool Write { get; private set; }
        /// <summary>
        /// 判斷條件欄位
        /// </summary>
        [DataMember]
        public string FieldName { get; private set; } = null;
        /// <summary>
        /// 成功條件的值
        /// </summary>
        [DataMember]
        public object Value { get; private set; }
        /// <summary>
        /// 條件判斷
        /// </summary>
        /// <param name="compare">包含 <see cref="PhoneConditionAttribute"/></param>
        /// <param name="initial">包含 <see cref="PhoneConditionAttribute"/></param>
        public static void Condition(object compare, object initial)
        {
            FieldInfo[] fieldInfo = compare.GetType().GetFields();//被比較
            List<string> fieldFinish = new List<string>();//完成的欄位名稱
            foreach (var field in fieldInfo)
            {

                field.GetCustomAttributes<PhoneConditionAttribute>().
                    Where(el => fieldFinish.Contains(el.FieldName) && !el.IsConditionInitial)?.
                        ToList().
                            ForEach(el =>
                            {

                            });




                //field.GetCustomAttributes<PhoneConditionAttribute>()?.ToList()
                //    .ForEach(el =>
                //    {
                //        string compareValue = GetValue(field.GetValue(compare));//比較值
                //        string initialValue = GetValue(field.GetValue(initial));//初始值
                //        if (el.Write && el.FieldName != null)
                //        {
                //            el.FieldName.Split(',').Where(str => str != "").ToList().
                //                                    ForEach(fieldName =>
                //                                    {
                //                                        string additionalConditions;
                //                                        //判斷是否要比較初始值並賦予相對應的值
                //                                        additionalConditions = el.IsConditionInitial ? 
                //                                        GetValue(compare.GetType().GetField(fieldName).GetValue(initial)) : 
                //                                        GetValue(compare.GetType().GetField(fieldName).GetValue(compare));

                //                                        string conditionsValue = GetValue(el.Value);//判斷預期的值
                //                                        if (additionalConditions != conditionsValue && compareValue != initialValue)
                //                                        {
                //                                            throw new MemoryException($"寫入失敗。因欄位 {el.FieldName} 的值，不符合 {el.Value} 條件");
                //                                        }
                //                                    });
                //        }
                //        else if (!el.Write)
                //        {
                //            if (compareValue != initialValue)
                //            {
                //                throw new MemoryException($"寫入失敗。因 {field.Name} 不開放手機寫入。");
                //            }
                //        }
                //    });
            }
        }
       /// <summary>
       /// 
       /// </summary>
        public static Func<List<PhoneConditionAttribute>, FieldInfo, object, object, List<string>> ActionCondition = (list, field, compare, initial) =>
        {
            List<string> result = new List<string>();
            list.ForEach(el =>
            {
                string compareValue = GetValue(field.GetValue(compare));//比較值
                string initialValue = GetValue(field.GetValue(initial));//初始值
                if (el.Write && el.FieldName != null)
                {
                    //返回上一層
                    el.FieldName.Split(',').Where(str => str != "").ToList().
                                            ForEach(fieldName =>
                                            {
                                                string additionalConditions;
                                                //判斷是否要比較初始值並賦予相對應的值
                                                additionalConditions = el.IsConditionInitial ?
                                            GetValue(compare.GetType().GetField(fieldName).GetValue(initial)) : GetValue(compare.GetType().GetField(fieldName).GetValue(compare));

                                                string conditionsValue = GetValue(el.Value);//判斷預期的值
                                                if (additionalConditions != conditionsValue && compareValue != initialValue)
                                                {
                                                    throw new MemoryException($"寫入失敗。因欄位 {el.FieldName} 的值，不符合 {el.Value} 條件");
                                                }
                                            });
                }
                else if (!el.Write)
                {
                    if (compareValue != initialValue)
                    {
                        throw new MemoryException($"寫入失敗。因 {field.Name} 不開放手機寫入。");
                    }
                }
            });
            return result;
        };

        internal static string GetValue(object obj)
        {
            string result = string.Empty;
            if (obj.GetType().IsEnum)
            {
                result = obj.ToString();
            }
            else if (obj.GetType().IsArray)
            {
                Array array = (Array)obj;
                for (int i = 0; i < array.Length; i++)
                {
                    result += BitConverter.ToString(array.GetValue(i).ToByteArray());
                }
            }
            else
            {
                result = BitConverter.ToString(obj.ToByteArray());
            }
            return result;
        }
    }
}
