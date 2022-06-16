using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWindowsBase.DataGridLibrary.Querying
{
    /// <summary>
    /// 參數計數器
    /// </summary>
    public class ParameterCounter
    {
        /// <summary>
        /// 陣列位置
        /// </summary>
        public int ParameterNumber { get => count - 1; }
        /// <summary>
        /// 索引
        /// </summary>
        private int count { get; set; }
        /// <summary>
        /// 增量
        /// </summary>
        public void Increment()
        {
            count++;
        }
        /// <summary>
        /// 減量
        /// </summary>
        public void Decrement()
        {
            count--;
        }
        /// <summary>
        /// 參數計數器
        /// </summary>
        public ParameterCounter()
        {
        }
        /// <summary>
        /// 參數計數器
        /// </summary>
        /// <param name="count">索引</param>
        public ParameterCounter(int count)
        {
            this.count = count;
        }
        public override string ToString()
        {
            return ParameterNumber.ToString();
        }
    }
}
