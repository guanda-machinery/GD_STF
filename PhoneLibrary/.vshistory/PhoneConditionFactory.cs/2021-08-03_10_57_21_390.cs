using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// <param name="write">要寫入到記憶體的值</param>
        /// <param name="initial">讀取目前記憶體的值</param>
        public PhoneConditionFactory(object write, object initial)
        {
            if (write.GetType() != initial.GetType())
            {
                throw new Exception("傳入參數 Type 必須是相同的");
            }
        }
        /// <summary>
        /// 測試 <typeparamref name="T"/> 類型，所有欄位附加的 <see cref="PhoneConditionAttribute"/> 條件
        /// </summary>
        /// <returns>通過邏輯審核回傳 true，失敗則回傳 false</returns>
        /// <exception cref="MemoryException"></exception> 
        public bool TestConditions()
        {
            FieldInfo[] fieldInfo = typeof(T).GetFields();//被比較
            foreach (var field in fieldInfo)
            {
                TestField(field.Name);
            }
            return true;
        }
        /// <summary>
        /// 測試欄位指定欄位
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <exception cref="MemoryException"></exception> 
        public void TestField(string fieldName)
        {
            FieldInfo field = typeof(T).GetField(fieldName);
            if (typeof(T).GetField(fieldName) == null)
            {
                throw new MemoryException($"在 {typeof(T)} 找不到 {fieldName} 欄位");
            }
            field.GetCustomAttributes<PhoneConditionAttribute>()?.ToList()
                  .ForEach(el =>
                  {
                      string _CompareValue = GetValue(field.GetValue(_Compare));//比較值
                      string _InitialValue = GetValue(field.GetValue(_Initial));//初始值
                      if (el.Write && el.FieldName != null)
                      {
                          //轉換多個欄位名稱 ForEach 執行動作
                          el.FieldName.Split(',').Where(str => str != "").ToList().
                                                  ForEach(referFieldName =>
                                                  {
                                                      string additionalConditions;
                                                      //判斷是否要比較初始值並賦予相對應的值
                                                      additionalConditions = el.IsConditionInitial ?
                                                      GetValue(_Compare.GetType().GetField(referFieldName).GetValue(_Initial)) :
                                                      GetValue(_Compare.GetType().GetField(referFieldName).GetValue(_Compare));
                                                      string conditionsValue = GetValue(el.Value);//判斷欄位資料型態
                                                      if (additionalConditions != conditionsValue && _CompareValue != _InitialValue)
                                                      {
                                                          throw new MemoryException($"寫入 {field.Name} 失敗。因欄位 {el.FieldName} 的值，不符合 {el.Value} 條件");
                                                      }
                                                      //if (additionalConditions != conditionsValue && _CompareValue != _InitialValue)
                                                      //{
                                                      //    throw new MemoryException($"寫入失敗。因欄位 {el.FieldName} 的值，不符合 {el.Value} 條件");
                                                      //}
                                                  });
                      }
                      else if (!el.Write)
                      {
                          if (_CompareValue != _InitialValue)
                          {
                              throw new MemoryException($"寫入失敗。因 {field.Name} 不開放手機寫入。");
                          }
                      }
                  });
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
