using System;

namespace STD_105
{
    /// <summary>
    ///  一個效能控制的介面，因 PAC 效能過低，所以需要動畫進入前，頁面所有控制項必須隱藏，使用圖片代替控制項完成動畫，在顯示控制項
    /// </summary>
    internal interface IPerformAccele
    {
        /// <summary>
        /// 換頁效能加速方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PagePerformAccele(object sender, EventArgs e);
        /// <summary>
        /// 換頁完成處理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PageCompleteAccele(object sender, EventArgs e);
    }
}
