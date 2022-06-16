using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWindowsBase
{
    /// <summary>
    /// 參數計數器
    /// </summary>
    public class ParameterCounter
    {
        /// <summary>
        /// 陣列位置
        /// </summary>
        public int ParameterNumber { get { return count - 1; } }
        /// <summary>
        /// 記數
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
        /// <param name="count">記數</param>
        public ParameterCounter(int count)
        {
            this.count = count;
        }
        /// <summary>
        /// 陣列位置轉字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ParameterNumber.ToString();
        }
    }
}
