using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase.DataGridLibrary.Support
{
    /// <summary>
    /// 篩選數據
    /// </summary>
    [Serializable()]
    public class FilterData : BaseViewModel
    {
        /// <summary>
        /// 過濾類型
        /// </summary>
        public FilterType Type { get; set; }
        /// <summary>
        /// 值屬性綁定路徑
        /// </summary>
        public string ValuePropertyBindingPath { get; set; }
        /// <summary>
        /// 值屬性類型
        /// </summary>
        public Type ValuePropertyType { get; set; }
        /// <summary>
        /// 為TYPE初始化
        /// </summary>
        public bool IsTypeInitialized { get; set; }
        /// <summary>
        /// 是否區分大小寫
        /// </summary>
        public bool IsCaseSensitiveSearch { get; set; }
        /// <summary>
        /// 過濾更改的事件
        /// </summary>
        public event EventHandler<EventArgs> FilterChangedEvent;
        /// <summary>
        /// 是清除數據
        /// </summary>
        private bool isClearData;
        private void OnFilterChangedEvent()
        {
            EventHandler<EventArgs> temp = FilterChangedEvent;

            if (temp != null)
            {
                bool filterChanged = false;

                switch (Type)
                {
                    case FilterType.Numeric:
                    case FilterType.DateTime:

                        filterChanged = (Operator != FilterOperator.Undefined || QueryString != String.Empty);
                        break;

                    case FilterType.NumericBetween:
                    case FilterType.DateTimeBetween:

                        _operator = FilterOperator.Between;
                        filterChanged = true;
                        break;

                    case FilterType.Text:

                        _operator = FilterOperator.Like;
                        filterChanged = true;
                        break;

                    case FilterType.List:
                    case FilterType.Boolean:

                        _operator = FilterOperator.Equals;
                        filterChanged = true;
                        break;

                    default:
                        filterChanged = false;
                        break;
                }

                if (filterChanged && !isClearData) temp(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// 清除數據
        /// </summary>
        public void ClearData()
        {
            isClearData = true;

            Operator = FilterOperator.Undefined;
            if (QueryString != String.Empty) QueryString = null;
            if (QueryStringTo != String.Empty) QueryStringTo = null;

            isClearData = false;
        }

        private FilterOperator _operator;
        /// <summary>
        /// 過濾運算符
        /// </summary>
        public FilterOperator Operator
        {
            get { return _operator; }
            set
            {
                if (_operator != value)
                {
                    _operator = value;
                    OnPropertyChanged(nameof(Operator));
                    OnFilterChangedEvent();
                }
            }
        }

        private string queryString;
        /// <summary>
        /// 請求參數
        /// </summary>
        public string QueryString
        {
            get { return queryString; }
            set
            {
                if (queryString != value)
                {
                    queryString = value;

                    if (queryString == null) queryString = String.Empty;

                    OnPropertyChanged(nameof(QueryString));
                    OnFilterChangedEvent();
                }
            }
        }

        private string queryStringTo;
        /// <summary>
        /// 查詢字符串到
        /// </summary>
        public string QueryStringTo
        {
            get { return queryStringTo; }
            set
            {
                if (queryStringTo != value)
                {
                    queryStringTo = value;

                    if (queryStringTo == null) queryStringTo = String.Empty;

                    OnPropertyChanged(nameof(QueryStringTo));
                    OnFilterChangedEvent();
                }
            }
        }

        /// <summary>
        /// 正在執行搜索
        /// </summary>
        public bool IsSearchPerformed { get; set; }
        /// <summary>
        /// 刷新
        /// </summary>
        public bool IsRefresh { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Operator">過濾運算符</param>
        /// <param name="Type">篩選類型</param>
        /// <param name="ValuePropertyBindingPath">值屬性綁定路徑</param>
        /// <param name="ValuePropertyType">值屬性類型</param>
        /// <param name="QueryString">請求參數</param>
        /// <param name="QueryStringTo">查詢字串</param>
        /// <param name="IsTypeInitialized">為type初始化</param>
        /// <param name="IsCaseSensitiveSearch">是否分區大小寫</param>
        public FilterData(
         FilterOperator Operator,
         FilterType Type,
         string ValuePropertyBindingPath,
         Type ValuePropertyType,
         string QueryString,
         string QueryStringTo,
         bool IsTypeInitialized,
         bool IsCaseSensitiveSearch
         )
        {
            this.Operator = Operator;
            this.Type = Type;
            this.ValuePropertyBindingPath = ValuePropertyBindingPath;
            this.ValuePropertyType = ValuePropertyType;
            this.QueryString = QueryString;
            this.QueryStringTo = QueryStringTo;

            this.IsTypeInitialized = IsTypeInitialized;
            this.IsCaseSensitiveSearch = IsCaseSensitiveSearch;
        }

    }
}
