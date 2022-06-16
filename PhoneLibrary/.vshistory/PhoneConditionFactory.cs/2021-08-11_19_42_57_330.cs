using GD_STD.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.Phone.MemoryHelper;
namespace GD_STD.Phone
{
    /// <summary>
    /// 執行 <see cref="PhoneConditionAttribute"/> 驅動
    /// </summary>
    /// <typeparam name="T"><see cref="IPhoneSharedMemory"/></typeparam>
    public class PhoneConditionFactory<T> where T : struct, IPhoneSharedMemory
    {
        /// <summary>
        /// 寫入到記憶體的值
        /// </summary>
        private T _Compare { get; set; }
        /// <summary>
        /// 目前記憶體的值
        /// </summary>
        private T _Initial { get; set; }
        /// <summary>
        /// 處理 <typeparamref name="T"/> 類型內所有標記過 <see cref="PhoneConditionAttribute"/> 的所有欄位
        /// </summary>
        /// <param name="writeMemoryValue">要寫入到記憶體的值</param>
        /// <param name="readMemoryValue">讀取目前記憶體的值</param>
        public PhoneConditionFactory(T writeMemoryValue, T readMemoryValue)
        {
            _Compare = writeMemoryValue;
            _Initial = readMemoryValue;
        }
        /// <summary>
        /// 處理 <typeparamref name="T"/> 類型內被標記過 <see cref="PhoneConditionAttribute"/>
        /// 符合 <see cref="PhoneConditionAttribute.CONDITION.MEMORY"/> 或 <see cref="PhoneConditionAttribute.CONDITION.LIMITS"/> 的欄位。
        /// </summary>
        /// <param name="writeMemoryValue"></param>
        public PhoneConditionFactory(T writeMemoryValue)
        {
            _Compare = writeMemoryValue;
        }
        /// <summary>
        /// 處理所有邏輯判斷
        /// </summary>
        /// <returns>通過邏輯審核回傳 true，失敗則回傳 false</returns>
        /// <exception cref="MemoryException"></exception> 
        public bool Conditions()
        {
            FieldInfo[] fieldInfo = typeof(T).GetFields();//要寫入記憶體的欄位
            foreach (var field in fieldInfo)
            {
                Field(field.Name);
            }
            return true;
        }
        /// <summary>
        /// 判斷指定欄位所有的 <see cref="PhoneConditionAttribute"/> 條件運算
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
                      switch (el.Condition)
                      {
                          case PhoneConditionAttribute.CONDITION.FIELD:
                              ConditionField(el, field);
                              break;
                          case PhoneConditionAttribute.CONDITION.MEMORY:
                              //TODO:尚未加入邏輯
                              break;
                          case PhoneConditionAttribute.CONDITION.LIMITS:
                              ConditionLimits(el, field);
                              break;
                          default:
                              Debugger.Break();
                              break;
                      }
                  });
        }
        /// <summary>
        /// <see cref="PhoneConditionAttribute.CONDITION.LIMITS"/> 條件判斷
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="field"></param>
        private void ConditionLimits(PhoneConditionAttribute attribute, FieldInfo field)
        {
            double compare = Convert.ToDouble(field.GetValue(_Compare)); //寫入值
            double minValue = attribute.MinValue;//最小範圍
            double maxValue = attribute.MaxValue;//最大範圍
                                                 //判斷資料有沒有在指定範圍內
            if (compare >= minValue && compare <= maxValue)
            {
                return;
            }
            throw new MemoryException($"寫入 {field.Name} 失敗。因欄位 {attribute.FieldName} 的值，必須大於等於 {attribute.MinValue} && 小於等於 {attribute.MaxValue}");
        }
        /// <summary>
        /// <see cref="PhoneConditionAttribute.CONDITION.FIELD"/> 條件判斷
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="field"></param>
        private void ConditionField(PhoneConditionAttribute attribute, FieldInfo field)
        {
            string compareValue = GetValue(field.GetValue(_Compare));//準備寫入記憶體的值
            string initialValue = GetValue(field.GetValue(_Initial));//記憶體目前的值
            if (!attribute.WriteField)
            {
                if (compareValue != initialValue)
                {
                    throw new MemoryException($"寫入失敗。因 {field.Name} 不開放手機寫入。");
                }
                return;
            }
            //轉換多個欄位名稱 ForEach 執行動作
            attribute.FieldNames.Where(str => str != "").ToList().
                ForEach(referFieldName =>
                {
                    object additionalConditions = SelectedConditionsValue(referFieldName, attribute);//記憶體目前的值(條件判斷)
                    string conditionsValue = GetValue(attribute.Value); //可寫入記憶體的條件值
                    if (!additionalConditions.GetType().IsEnum)
                    {
                        string strAdditionalConditions = GetValue(additionalConditions);
                        //記憶體目前的值(條件判斷) != 可寫入記憶體的條件值 && 準備寫入記憶體的值 != initialValue
                        if (strAdditionalConditions != conditionsValue && compareValue != initialValue)
                        {
                            throw new MemoryException($"寫入 {field.Name} 失敗。因欄位 {attribute.FieldName} 的值，不符合 {attribute.Value} 條件");
                        }
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
            #region 使用受保護的方法 (Ban = 保護鎖)
#if !LogicYeh
            Ban = true;
#endif
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
#if !LogicYeh
            Ban = false;
#endif
            #endregion
            return result;
        }
    }
}
