using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase.DataGridLibrary.Support;

namespace WPFWindowsBase.DataGridLibrary.Querying
{
    /// <summary>
    /// 字符串過濾器表達式創建器
    /// </summary>
    internal class StringFilterExpressionCreator
    {
        /// <summary>
        /// 通配符任意字符串
        /// </summary>
        const string WildcardAnyString = "%";
        /// <summary>
        /// 字符串表達式功能
        /// </summary>
        private enum StringExpressionFunction
        {
            /// <summary>
            /// 未定義
            /// </summary>
            Undefined = 0,
            /// <summary>
            /// 以......開始
            /// </summary>
            StartsWith = 1,
            /// <summary>
            /// 指數
            /// </summary>
            IndexOf = 2,
            /// <summary>
            ///  以......結束
            /// </summary>
            EndsWith = 3
        }
        /// <summary>
        /// 篩選數據器
        /// </summary>
        FilterData filterData;
        /// <summary>
        /// 參數設定者
        /// </summary>
        List<object> paramseters;
        /// <summary>
        /// 參數計數器
        /// </summary>
        ParameterCounter paramCounter;
        /// <summary>
        /// 以建立的參數數量
        /// </summary>
        internal int ParametarsCrated { get { return paramseters.Count; } }
        /// <summary>
        /// 參數過濾表達創建
        /// </summary>
        /// <param name="paramCounter"></param>
        /// <param name="filterData"></param>
        /// <param name="paramseters"></param>
        internal StringFilterExpressionCreator(
            ParameterCounter paramCounter, FilterData filterData, List<object> paramseters)
        {
            this.paramCounter = paramCounter;
            this.filterData = filterData;
            this.paramseters = paramseters;
        }
        /// <summary>
        /// 創建
        /// </summary>
        /// <returns></returns>
        internal string Create()
        {
            StringBuilder filter = new StringBuilder();

            List<string> filterList = parse(this.filterData.QueryString);

            for (int i = 0; i < filterList.Count; i++)
            {
                if (i > 0) filter.Append(" and ");

                filter.Append(filterList[i]);
            }

            return filter.ToString();
        }
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="filterString">過濾字符串</param>
        /// <returns></returns>
        private List<string> parse(string filterString)
        {
            string token = null;
            int i = 0;
            bool expressionCompleted = false;
            List<string> filter = new List<string>();
            string expressionValue = String.Empty;
            StringExpressionFunction function = StringExpressionFunction.Undefined;

            do
            {
                token = i < filterString.Length ? filterString[i].ToString() : null;

                if (token == WildcardAnyString || token == null)
                {
                    if (expressionValue.StartsWith(WildcardAnyString) && token != null)
                    {
                        function = StringExpressionFunction.IndexOf;

                        expressionCompleted = true;
                    }
                    else if (expressionValue.StartsWith(WildcardAnyString) && token == null)
                    {
                        function = StringExpressionFunction.EndsWith;

                        expressionCompleted = false;
                    }
                    else
                    {
                        function = StringExpressionFunction.StartsWith;

                        if (filterString.Length - 1 > i) expressionCompleted = true;
                    }
                }

                if (token == null)
                {
                    expressionCompleted = true;
                }

                expressionValue += token;

                if (expressionCompleted
                    && function != StringExpressionFunction.Undefined
                    && expressionValue != String.Empty)
                {
                    string expressionValueCopy = String.Copy(expressionValue);

                    expressionValueCopy = expressionValueCopy.Replace(WildcardAnyString, String.Empty);

                    if (expressionValueCopy != String.Empty)
                    {
                        filter.Add(createFunction(function, expressionValueCopy));
                    }

                    function = StringExpressionFunction.Undefined;

                    expressionValue = expressionValue.EndsWith(WildcardAnyString) ? WildcardAnyString : String.Empty;

                    expressionCompleted = false;
                }

                i++;

            } while (token != null);

            return filter;
        }
        /// <summary>
        /// 創建功能
        /// </summary>
        /// <param name="function"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string createFunction(
            StringExpressionFunction function, string value)
        {
            StringBuilder filter = new StringBuilder();

            paramseters.Add(value);

            filter.Append(filterData.ValuePropertyBindingPath);

            if (filterData.ValuePropertyType.IsGenericType
                && filterData.ValuePropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                filter.Append(".Value");
            }

            paramCounter.Increment();
            paramCounter.Increment();

            filter.Append(".ToString()." + function.ToString() + "(@" + (paramCounter.ParameterNumber - 1) + ", @" + (paramCounter.ParameterNumber) + ")");

            if (function == StringExpressionFunction.IndexOf)
            {
                filter.Append(" != -1 ");
            }

            paramseters.Add(filterData.IsCaseSensitiveSearch
                ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            return filter.ToString();
        }
    }
}
