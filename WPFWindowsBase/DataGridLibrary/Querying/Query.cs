using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase.DataGridLibrary.Querying
{
    /// <summary>
    /// 詢問
    /// </summary>
    public class Query
    {
        /// <summary>
        /// 詢問方法
        /// </summary>
        public Query()
        {
            lastFilterString = String.Empty;
            lastQueryParameters = new List<object>();
        }
        /// <summary>
        /// 過濾字符串
        /// </summary>
        public string FilterString { get; set; }
        /// <summary>
        /// 詢問參數
        /// </summary>
        public List<object> QueryParameters { get; set; }
        /// <summary>
        /// 最後一次的過濾字串
        /// </summary>
        private string lastFilterString { get; set; }
        /// <summary>
        /// 最後一次的過濾參數
        /// </summary>
        private List<object> lastQueryParameters { get; set; }
        /// <summary>
        /// 過濾變更
        /// </summary>
        public bool IsQueryChanged
        {
            get
            {
                bool queryChanged = false;

                if (FilterString != lastFilterString)
                {
                    queryChanged = true;
                }
                else
                {
                    if (QueryParameters.Count != lastQueryParameters.Count)
                    {
                        queryChanged = true;
                    }
                    else
                    {
                        for (int i = 0; i < QueryParameters.Count; i++)
                        {
                            if (!QueryParameters[i].Equals(lastQueryParameters[i]))
                            {
                                queryChanged = true;
                                break;
                            }
                        }
                    }
                }

                return queryChanged;
            }
        }
        /// <summary>
        /// 存儲上次使用的值
        /// </summary>
        public void StoreLastUsedValues()
        {
            lastFilterString = FilterString;
            lastQueryParameters = QueryParameters;
        }

    }
}
