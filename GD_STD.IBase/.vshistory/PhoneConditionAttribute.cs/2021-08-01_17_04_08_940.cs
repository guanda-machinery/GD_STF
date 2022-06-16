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
        /// <param name="value">成功條件值</param>
        /// <param name="Operator">比較符號</param>
        public PhoneConditionAttribute(object value, OPERATOR Operator, params string[] fieldName)
        {
            this.Write = true;
            this.FieldName = fieldName;
            this.Value = value;
        }

        /// <summary>
        /// 欄位可寫入
        /// </summary>
        [DataMember]
        public bool Write { get; private set; }
        /// <summary>
        /// 判斷條件欄位
        /// </summary>
        [DataMember]
        public string[] FieldName { get; private set; } = null;
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
            foreach (var field in fieldInfo)
            {
                field.GetCustomAttributes<PhoneConditionAttribute>()?.ToList()
                    .ForEach(el =>
                    {
                        //TODO: 新增邏輯
                        string compareValue = GetValue(field.GetValue(compare));//比較值
                        string initialValue = GetValue(field.GetValue(initial));//初始值
                        if (el.Write && el.FieldName != null)
                        {
                            el.FieldName.ToList().
                                                    ForEach(fieldName => 
                                                    {
                                                        string additionalConditions = GetValue(compare.GetType().GetField(fieldName).GetValue(initial));//條件欄位比對值
                                                        string conditionsValue = GetValue(el.Value);//判斷欄位資料型態
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
            }
            //foreach (var field in fieldInfo)
            //{
            //IEnumerable<PhoneConditionAttribute> attributesFalse = field.GetCustomAttributes<PhoneConditionAttribute>().TakeWhile(el => el.Write == false);
            //IEnumerable<PhoneConditionAttribute> attributesTrue = field.GetCustomAttributes<PhoneConditionAttribute>().Where(el => el.Write == true);
            //if (attributesTrue.Count() > 0) //可寫入的參數
            //{
            //    foreach (var item in attributesTrue)
            //    {
            //        string compareValue = GetValue(field.GetValue(compare));//比較值
            //        string initialValue = GetValue(field.GetValue(initial));//初始值
            //        string additionalConditions = GetValue(compare.GetType().GetField(item.FieldName).GetValue(initial));//條件欄位比對值
            //        string conditionsValue = GetValue(item.Value);//判斷欄位資料型態
            //        if (additionalConditions != conditionsValue && compareValue != initialValue)
            //        {
            //            throw new MemoryException($"寫入失敗。因欄位 {item.FieldName} 的值，不符合 {item.Value} 條件");
            //        }
            //    }
            //}
            //else if (attributesFalse.Count() > 0)//不可寫入的參數
            //{
            //    foreach (var item in attributesFalse)
            //    {
            //        string compareValue = GetValue(field.GetValue(compare));//比較值
            //        string initialValue = GetValue(field.GetValue(initial));//初始值

            //        if (compareValue != initialValue)
            //        {
            //            throw new MemoryException($"寫入失敗。因 {field.Name} 不開放手機寫入。");
            //        }
            //    }
            //}
            //}

        }
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
