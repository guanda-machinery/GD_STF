using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Base
{
    internal class PhoneConditionFactory<T> where T : IPhoneSharedMemory
    {
        private Type _Type { get; set; }
        public PhoneConditionFactory(T obj)
        {
            _Type = obj.GetType();
        }
        /// <summary>
        /// 完成的欄位
        /// </summary>
        private List<string> fieldFinish = new List<string>();
        /// <summary>
        /// 條件判斷
        /// </summary>
        /// <param name="compare">包含 <see cref="PhoneConditionAttribute"/></param>
        /// <param name="initial">包含 <see cref="PhoneConditionAttribute"/></param>
        public bool Condition(object compare, object initial)
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
            return true;
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
