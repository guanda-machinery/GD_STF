using System;
using System.Collections.Generic;
using System.Text;

namespace WPFWindowsBase
{
    /// <summary>
    /// 查詢創建者
    /// </summary>
    public class QueryCreator
    {
        /// <summary>
        /// 參數
        /// </summary>
        private List<object> Parameters { get; set; }

        private readonly Dictionary<string, FilterData> filtersForColumns;
        private ParameterCounter paramCounter;
        /// <summary>
        /// 查詢創建者
        /// </summary>
        /// <param name="filtersForColumns">列過濾器</param>
        public QueryCreator( Dictionary<string, FilterData> filtersForColumns)
        {
            this.filtersForColumns = filtersForColumns;

            paramCounter = new ParameterCounter(0);
            Parameters = new List<object>();
        }
        /// <summary>
        /// 創建過濾器
        /// </summary>
        /// <param name="query"><see cref="Query"/>參數</param>
        public void CreateFilter(ref Query query)
        {
            StringBuilder filter = new StringBuilder();

            foreach (KeyValuePair<string, FilterData> kvp in filtersForColumns)
            {
                StringBuilder partialFilter = createSingleFilter(kvp.Value);

                if (filter.Length > 0 && partialFilter.Length > 0) filter.Append(" AND ");

                if (partialFilter.Length > 0)
                {
                    string valuePropertyBindingPath = String.Empty;
                    string[] paths = kvp.Value.ValuePropertyBindingPath.Split(new Char[] { '.' });

                    foreach (string p in paths)
                    {
                        if (valuePropertyBindingPath != String.Empty)
                        {
                            valuePropertyBindingPath += ".";
                        }

                        valuePropertyBindingPath += p;

                        filter.Append(valuePropertyBindingPath + " != null AND ");//消除：可空對象必須具有一個值，並且對象引用未設置為對象
                    }
                }

                filter.Append(partialFilter);
            }

            //初始化查詢
            query.FilterString = filter.ToString();
            query.QueryParameters = Parameters;
        }
        /// <summary>
        /// 創建單個過濾器
        /// </summary>
        /// <param name="filterData"></param>
        /// <returns></returns>
        private StringBuilder createSingleFilter(FilterData filterData)
        {
            StringBuilder filter = new StringBuilder();

            if (
                (filterData.Type == FilterType.NumericBetween || filterData.Type == FilterType.DateTimeBetween)
                &&
                (filterData.QueryString != String.Empty || filterData.QueryStringTo != String.Empty)
                )
            {
                if (filterData.QueryString != String.Empty)
                {
                    createFilterExpression(
                        filterData,
                        filterData.QueryString,
                        filter,
                        getOperatorString(FilterOperator.GreaterThanOrEqual));
                }
                if (filterData.QueryStringTo != String.Empty)
                {
                    if (filter.Length > 0) filter.Append(" AND ");

                    createFilterExpression(
                        filterData,
                        filterData.QueryStringTo,
                        filter,
                        getOperatorString(FilterOperator.LessThanOrEqual));
                }
            }
            else if (filterData.QueryString != String.Empty
                &&
                filterData.Operator != FilterOperator.Undefined)
            {
                if (filterData.Type == FilterType.Text)
                {
                    createStringFilterExpression(filterData, filter);
                }
                else
                {
                    createFilterExpression(
                        filterData, filterData.QueryString, filter, getOperatorString(filterData.Operator));
                }
            }

            return filter;
        }
        /// <summary>
        /// 創建過濾器表達式
        /// </summary>
        /// <param name="filterData"></param>
        /// <param name="queryString"></param>
        /// <param name="filter"></param>
        /// <param name="operatorString"></param>
        private void createFilterExpression(  FilterData filterData, string queryString, StringBuilder filter, string operatorString)
        {
            filter.Append(filterData.ValuePropertyBindingPath);

            object parameterValue = null;

            if (trySetParameterValue(out parameterValue, queryString, filterData.ValuePropertyType))
            {
                Parameters.Add(parameterValue);

                paramCounter.Increment();

                filter.Append(" " + operatorString + " @" + paramCounter.ParameterNumber);
            }
            else
            {
                filter = new StringBuilder();//不使用過濾器
            }
        }
        /// <summary>
        /// 嘗試設置參數值
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="stringValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool trySetParameterValue( out object parameterValue, string stringValue, Type type)
        {
            parameterValue = null;
            bool valueIsSet;

            try
            {
                if (type == typeof(Nullable<DateTime>) || type == typeof(DateTime))
                {
                    parameterValue = DateTime.Parse(stringValue);
                }
                else if (type == typeof(Enum) || type.BaseType == typeof(Enum))
                {
                    Parameters.Add(Enum.Parse(type, stringValue, true));
                }
                else if (type == typeof(Boolean) || type.BaseType == typeof(Boolean))
                {
                    parameterValue = Convert.ChangeType(stringValue, typeof(Boolean));
                }
                else
                {
                    parameterValue = Convert.ChangeType(stringValue, typeof(Double));//TODO use "real" number type
                }

                valueIsSet = true;
            }
            catch (Exception)
            {
                valueIsSet = false;
            }

            return valueIsSet;
        }
        /// <summary>
        /// 創建字符串過濾器表達式
        /// </summary>
        /// <param name="filterData"></param>
        /// <param name="filter"></param>
        private void createStringFilterExpression( FilterData filterData, StringBuilder filter)
        {
            StringFilterExpressionCreator
                creator = new StringFilterExpressionCreator(
                    paramCounter, filterData, Parameters);

            string filterExpression = creator.Create();

            filter.Append(filterExpression);
        }
        /// <summary>
        /// 獲取運算符字符串
        /// </summary>
        /// <param name="filterOperator"></param>
        /// <returns></returns>
        private string getOperatorString(FilterOperator filterOperator)
        {
            string op;

            switch (filterOperator)
            {
                case FilterOperator.Undefined:
                    op = String.Empty;
                    break;
                case FilterOperator.LessThan:
                    op = "<";
                    break;
                case FilterOperator.LessThanOrEqual:
                    op = "<=";
                    break;
                case FilterOperator.GreaterThan:
                    op = ">";
                    break;
                case FilterOperator.GreaterThanOrEqual:
                    op = ">=";
                    break;
                case FilterOperator.Equals:
                    op = "=";
                    break;
                case FilterOperator.Like:
                    op = String.Empty;
                    break;
                default:
                    op = String.Empty;
                    break;
            }

            return op;
        }
    }
}