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
        /// <summary>
        /// 邏輯處理的型態
        /// </summary>
        public enum CONDITION
        {
            /// <summary>
            /// 不需要判斷
            /// </summary>
            NULL,
            /// <summary>
            /// 欄位
            /// </summary>
            FIELD,
            /// <summary>
            /// 記憶體寫入
            /// </summary>
            MEMORY,
            /// <summary>
            /// 範圍限制
            /// </summary>
            LIMITS,
        }
        /// <summary>
        /// Phone 不可寫入
        /// </summary>
        public PhoneConditionAttribute()
        {
            Condition = CONDITION.FIELD;
            WriteField = false;
        }
        /// <summary>
        /// 定義欄位是 Phone 可以開放記憶體寫入。
        /// </summary>
        /// <param name="writeMemory"></param>
        public PhoneConditionAttribute(bool writeMemory)
        {
            if (writeMemory)
            {
                Condition = CONDITION.MEMORY;
                this.WriteField = true;
            }
        }
        /// <summary>
        /// 定義 Phone 寫入欄位否有符合最大最小值
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        public PhoneConditionAttribute(int minValue, int maxValue)
        {
            this.WriteField = true;
            this.IsConditionInitial = false;
            MinValue = minValue;
            MaxValue = maxValue;
            Condition = CONDITION.LIMITS;
        }
        /// <summary>
        /// 定義 Phone 寫入欄位否有符合最大最小值
        /// </summary>
        /// <param name="minField">要參考最小值的欄位</param>
        /// <param name="maxField">要參考最大值的欄位</param>
        public PhoneConditionAttribute(FieldInfo minField, FieldInfo maxField)
        {
            this.WriteField = true;
            this.IsConditionInitial = false;

            Condition = CONDITION.LIMITS;
            MinField = minField;
            MaxField = maxField;
        }
        /// <summary>
        /// 定義 Phone 寫入欄位否有符合最大最小值
        /// </summary>
        /// <param name="minField">要參考最小值的欄位</param>
        /// <param name="maxValue">最大值</param>
        public PhoneConditionAttribute(FieldInfo minField, int maxValue)
        {
            this.WriteField = true;
            this.IsConditionInitial = false;

            Condition = CONDITION.LIMITS;
            MinField = minField;
            MaxValue = maxValue;
        }
        /// <summary>
        /// 定義 Phone 寫入欄位否有符合最大最小值
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxField">要參考最大值的欄位</param>
        public PhoneConditionAttribute(int minValue, FieldInfo maxField)
        {
            this.WriteField = true;
            this.IsConditionInitial = false;

            Condition = CONDITION.LIMITS;
            MinValue = minValue;
            MaxField = maxField;
        }

        /// <summary>
        /// 定義 Phone 寫入欄位參數是否符合條件
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
            Condition = CONDITION.FIELD;
        }
        /// <summary>
        /// 判斷型態
        /// </summary>
        public CONDITION Condition { get; private set; }
        /// <summary>
        /// 寫入範圍最小值
        /// </summary>
        public int MinValue { get; private set; }
        /// <summary>
        /// 寫入範圍最大值
        /// </summary>
        public int MaxValue { get; private set; }
        /// <summary>
        /// 要比較目前記憶體的值回傳 true , 不要比較目前記憶體的值回傳 false。
        /// </summary>
        public bool IsConditionInitial { get; private set; }
        /// <summary>
        /// Phone 可寫入的欄位
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
        /// 判斷條件欄位列表
        /// </summary>
        public string[] FieldNames => FieldName.Split(',');
        /// <summary>
        /// 要參考最小值的欄位
        /// </summary>
        public FieldInfo MinField { get; private set; }
        /// <summary>
        /// 要參考最大值的欄位
        /// </summary>
        public FieldInfo MaxField { get; private set; }
    }
}
