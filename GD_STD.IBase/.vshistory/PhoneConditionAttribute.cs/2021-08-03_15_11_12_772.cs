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
        internal PhoneConditionAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
        /// <summary>
        /// Phone 不可寫入
        /// </summary>
        public PhoneConditionAttribute()
        {
            WriteField = false;
        }
        /// <summary>
        /// 定義寫入欄位參數是否符合條件
        /// </summary>
        /// <param name="fieldName">欄位名稱。多個欄請用逗號區隔開來</param>
        /// <param name="offsetStart">記憶體範圍最小位置</param>
        /// <param name="offsetEnd"></param>
        public PhoneConditionAttribute(bool wrteMemory)
        {
            this.WriteField = true;
            this.FieldName = fieldName;
            this.Value = null;
            this.IsConditionInitial = false;
        }

        /// <summary>
        /// 定義寫入欄位參數是否符合條件
        /// </summary>
        /// <param name="fieldName">欄位名稱。多個欄請用逗號區隔開來</param>
        /// <param name="isConditionInitial">要比較目前記憶體的值回傳 true , 不要比較目前記憶體的值回傳 false。</param>
        /// <param name="value">符合寫入條件值。<see cref="FieldName"/>值必須符合 <see cref="Value"/></param>
        public PhoneConditionAttribute(object value, string fieldName, bool isConditionInitial = true)
        {
            this.WriteField = true;
            this.FieldName = fieldName;
            this.Value = value;
            this.IsConditionInitial = isConditionInitial;
        }
        /// <summary>
        /// 寫入範圍最小值
        /// </summary>
        public int? MinValue { get; private set; }
        /// <summary>
        /// 寫入範圍最大值
        /// </summary>
        public int? MaxValue { get; private set; }
        /// <summary>
        /// 要比較目前記憶體的值回傳 true , 不要比較目前記憶體的值回傳 false。
        /// </summary>
        public bool IsConditionInitial { get; private set; }
        /// <summary>
        /// 判斷欄位是否有開放 Phone 寫入
        /// </summary>
        /// <remarks>
        /// 開放寫入回傳 true，不開放則回傳 false。
        /// </remarks>
        [DataMember]
        public bool WriteField { get; private set; }
        /// <summary>
        /// 判斷條件欄位(多個欄請用逗號區隔開來)
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
        internal static void Condition(object compare, object initial)
        {
            FieldInfo[] fieldInfo = compare.GetType().GetFields();//被比較
            foreach (var field in fieldInfo)
            {
                field.GetCustomAttributes<PhoneConditionAttribute>()?.ToList()
                    .ForEach(el =>
                    {
                        string compareValue = GetValue(field.GetValue(compare));//比較值
                        string initialValue = GetValue(field.GetValue(initial));//初始值
                        if (el.WriteField && el.FieldName != null)
                        {
                            el.FieldName.Split(',').Where(str => str != "").ToList().
                                                    ForEach(fieldName =>
                                                    {
                                                        string additionalConditions;
                                                        //判斷是否要比較初始值並賦予相對應的值
                                                        additionalConditions = el.IsConditionInitial ?
                                                        GetValue(compare.GetType().GetField(fieldName).GetValue(initial)) :
                                                        GetValue(compare.GetType().GetField(fieldName).GetValue(compare));
                                                        string conditionsValue = GetValue(el.Value);//判斷欄位資料型態
                                                        if (additionalConditions != conditionsValue && compareValue != initialValue)
                                                        {
                                                            throw new MemoryException($"寫入失敗。因欄位 {el.FieldName} 的值，不符合 {el.Value} 條件");
                                                        }
                                                    });
                        }
                        else if (!el.WriteField)
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
