using GD_STD.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GD_STD.Phone
{
    internal class PhoneConditionFactory<T> where T : struct, IPhoneSharedMemory
    {
        /// <summary>
        /// 寫入到記憶體的值
        /// </summary>
        private object _Compare { get; set; }
        /// <summary>
        /// 目前記憶體的值
        /// </summary>
        private object _Initial { get; set; }
        /// <summary>
        /// 處理 <typeparamref name="T"/> 類型所有 <see cref="PhoneConditionAttribute"/> 邏輯
        /// </summary>
        /// <param name="writeMemoryValue">要寫入到記憶體的值</param>
        /// <param name="readMemoryValue">讀取目前記憶體的值</param>
        public PhoneConditionFactory(object writeMemoryValue, object readMemoryValue)
        {
            if (writeMemoryValue.GetType() != readMemoryValue.GetType())
            {
                throw new Exception("傳入參數 Type 必須是相同的");
            }
        }
        /// <summary>
        /// 判斷 <typeparamref name="T"/> 類型，所有欄位附加的 <see cref="PhoneConditionAttribute.WriteField"/> 條件
        /// </summary>
        /// <returns>通過邏輯審核回傳 true，失敗則回傳 false</returns>
        /// <exception cref="MemoryException"></exception> 
        public bool Conditions()
        {
            FieldInfo[] fieldInfo = typeof(T).GetFields();//被比較
            foreach (var field in fieldInfo)
            {
                Field(field.Name);
            }
            return true;
        }
        /// <summary>
        /// 測試欄位指定欄位
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <exception cref="MemoryException"></exception> 
        public void Field(string fieldName)
        {
            FieldInfo field = typeof(T).GetField(fieldName);
            if (typeof(T).GetField(fieldName) == null)
            {
                throw new MemoryException($"在 {typeof(T)} 找不到 {fieldName} 欄位");
            }
            field.GetCustomAttributes<PhoneConditionAttribute>()?.ToList()
                  .ForEach(el =>
                  {
                      string _CompareValue = GetValue(field.GetValue(_Compare));//要寫入值
                      string _InitialValue = GetValue(field.GetValue(_Initial));//初始值
                      if (!el.WriteField)
                      {
                          if (_CompareValue != _InitialValue)
                          {
                              throw new MemoryException($"寫入失敗。因 {field.Name} 不開放手機寫入。");
                          }
                      }
                  });
        }

        private void TestConditionField(PhoneConditionAttribute attribute, FieldInfo field, string compareValue, string initialValue)
        {
            //轉換多個欄位名稱 ForEach 執行動作
            attribute.FieldName.Split('|').Where(str => str != "").ToList().
                ForEach(referFieldName =>
                {
                    //判斷是否要比較初始值並賦予相對應的值
                    object additionalConditions = SelectedConditionsValue(referFieldName, attribute);

                    string conditionsValue = GetValue(attribute.Value);//可寫入的條件值
                    if (!additionalConditions.GetType().IsEnum)
                    {
                        string strAdditionalConditions = GetValue(additionalConditions);
                        if (strAdditionalConditions != conditionsValue && compareValue != initialValue)
                        {
                            throw new MemoryException($"寫入 {field.Name} 失敗。因欄位 {attribute.FieldName} 的值，不符合 {attribute.Value} 條件");
                        }
                        ////判斷是否要比較最小值
                        //else if (attribute.MinValue != null)
                        //{
                        //    double compare = Convert.ToDouble(field.GetValue(_Compare));
                        //    double minValue = Convert.ToDouble(field.GetValue(el.MinValue));
                        //    //判斷寫入資料有沒有小於最小下限
                        //    if (compare < minValue)
                        //    {
                        //        throw new MemoryException($"寫入 {field.Name} 失敗。因欄位 {attribute.FieldName} 的值，必須大於等於 {attribute.Value}");
                        //    }
                        //}
                    }
                    else
                    {
                        if (!attribute.Value.ToString().Split(',').Contains(conditionsValue) && compareValue != initialValue)
                        {
                            throw new MemoryException($"寫入 {field.Name} 失敗。因欄位 {attribute.FieldName} 的值，不符合 {attribute.Value.ToString()} 條件");
                        }
                    }
                });
        }
        /// <summary>
        /// 選擇比較值
        /// </summary>
        /// <param name="referFieldName">參考的欄位名稱</param>
        /// <param name="attribute"></param>
        /// <returns>回傳</returns>
        private object SelectedConditionsValue(string referFieldName, PhoneConditionAttribute attribute)
        {
            return attribute.IsConditionInitial ? _Compare.GetType().GetField(referFieldName).GetValue(_Initial) : _Compare.GetType().GetField(referFieldName).GetValue(_Compare);
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
