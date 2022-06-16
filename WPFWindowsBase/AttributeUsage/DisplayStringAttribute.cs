//using System;

//namespace WPFWindowsBase.AttributeUsage
//{
//    /// <summary>
//    /// 顯示字串屬性
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Field)]
//    public sealed class DisplayStringAttribute : Attribute
//    {
//        private readonly string value;

//#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'DisplayStringAttribute.Value' 的 XML 註解
//        public string Value { get => value; }
//#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'DisplayStringAttribute.Value' 的 XML 註解
//        /// <summary>
//        /// 取得或設定由這個靜態資源參考傳遞的索引鍵值。 該索引鍵用於傳回資源字典中與其相符的物件。
//        /// </summary>
//        public string ResourceKey { get; set; }
//        /// <summary>
//        /// 顯示字串屬性
//        /// </summary>
//        /// <param name="value">值</param>
//        public DisplayStringAttribute(string value)
//        {
//            this.value = value;
//        }
//        /// <inheritdoc/>
//        public DisplayStringAttribute()
//        {
//        }
//    }
//}
