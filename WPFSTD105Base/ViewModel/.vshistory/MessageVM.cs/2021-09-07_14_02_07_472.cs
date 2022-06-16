//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using WPFBase = WPFWindowsBase;
//namespace WPFWindowsBase
//{
//    /// <summary>
//    /// 一個 <see cref="CustomMessage"/> 視圖模型
//    /// </summary>
//    public class MessageVM : WPFBase.BaseViewModel
//    {
//        /// <summary>
//        /// 標準建構式
//        /// </summary>
//        public MessageVM()
//        {

//        }
//        /// <summary>
//        ///  Message 完整設定
//        /// </summary>
//        /// <param name="title">Message 標題</param>
//        /// <param name="content">Message 內容</param>
//        /// <param name="leftButtonVisibility">左邊按鈕能見度</param>
//        /// <param name="rightButtonVisibility">右邊按鈕能見度</param>
//        /// <param name="leftButtonText">左邊按鈕文字內容</param>
//        /// <param name="rightButtonText">右邊按鈕文字內容</param>
//        /// <param name="autoClose">自動關閉 <see cref="CustomMessage"/></param>
//        /// <param name="s">倒數秒數</param>
//        /// <param name="comBoxVisibility">顯示下拉選單</param>
//        /// <param name="comBoxTitle">下拉選單標題</param>
//        /// <param name="comBoxContent">下拉選單內容</param>
//        /// 
//        public MessageVM(string title, string content, bool leftButtonVisibility, bool rightButtonVisibility, string leftButtonText, string rightButtonText, bool autoClose, int s, bool comBoxVisibility, string comBoxTitle = default(string), string[] comBoxContent = default(string[]))
//        {
//            Title = title;
//            Content = content;
//            LeftButtonVisibility = leftButtonVisibility;
//            RightButtonVisibility = rightButtonVisibility;
//            LeftButtonText = leftButtonText;
//            RightButtonText = rightButtonText;
//            CloseSecond = s;
//            AutoClose = autoClose;
//            ComBoxVisibility = comBoxVisibility;
//            ComBoxTitle = comBoxTitle;
//            ComBoxContent = comBoxContent;
//        }
//        /// <summary>
//        ///  Message 完整訊息包含要顯示的按鈕
//        /// </summary>
//        /// <param name="title">Message 標題</param>
//        /// <param name="content">Message 內容</param>
//        /// <param name="leftButtonVisibility">左邊按鈕能見度</param>
//        /// <param name="rightButtonVisibility">右邊按鈕能見度</param>
//        /// <param name="autoClose">自動關閉 <see cref="CustomMessage"/></param>
//        /// <param name="s">倒數秒數</param>
//        public MessageVM(string title, string content, bool leftButtonVisibility, bool rightButtonVisibility, bool autoClose = false, int s = 0)
//        {
//            Title = title;
//            Content = content;
//            LeftButtonVisibility = leftButtonVisibility;
//            RightButtonVisibility = rightButtonVisibility;
//            AutoClose = autoClose;
//            CloseSecond = s;
//        }
//        /// <summary>
//        ///  Message 訊息
//        /// </summary>
//        /// <param name="title">Message 標題</param>
//        /// <param name="content">Message 內容</param>
//        public MessageVM(string title, string content)
//        {
//            Title = title;
//            Content = content;
//        }
        
//        /// <summary>
//        /// 自動關閉視窗
//        /// </summary>
//        public bool AutoClose { get; set; } = true;
//        /// <summary>
//        /// 自動關閉秒數
//        /// </summary>
//        public int CloseSecond { get; set; } = 30;
//        /// <summary>
//        /// 標題
//        /// </summary>
//        public string Title { get; set; } = string.Empty;
//        /// <summary>
//        /// 內容
//        /// </summary>
//        public string Content { get; set; } = string.Empty;
//        /// <summary>
//        ///  指定使用者按下哪一個訊息方塊按鈕。
//        /// </summary>
//        public MessageBoxResult Result { get; set; }
//        /// <summary>
//        /// 右按鈕字樣
//        /// </summary>
//        public string RightButtonText { get; set; } = "YES";
//        /// <summary>
//        /// 左按鈕字樣
//        /// </summary>
//        public string LeftButtonText { get; set; } = "NO";
//        /// <summary>
//        /// 左按鈕能見度
//        /// </summary>
//        public bool LeftButtonVisibility { get; set; } = true;
//        /// <summary>
//        /// 右按鈕能見度
//        /// </summary>
//        public bool RightButtonVisibility { get; set; } = true;
//        /// <summary>
//        /// 下拉選單能見度
//        /// </summary>
//        public bool ComBoxVisibility { get; set; } = false;
//        /// <summary>
//        /// 下拉選單標題
//        /// </summary>
//        public string ComBoxTitle { get; set; } = "ComBox標題";
//        /// <summary>
//        /// 下拉選單內容
//        /// </summary>
//        public string[] ComBoxContent { get; set; } = new string[]{ "內容1", "內容2" };
//    }
//}
