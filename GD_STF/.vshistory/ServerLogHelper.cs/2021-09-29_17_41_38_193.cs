//using log4net;
//using System;
//using System.IO;
//using System.Web;

//namespace GD_STD
//{
//    /// <summary>
//    /// Log 幫手
//    /// </summary>
//    public static class ServerLogHelper
//    {
//        /// <summary>
//        /// 讀取記憶體的日誌
//        /// </summary>
//        public static readonly ILog ReadMemorLog = LogManager.GetLogger("ReadMemor");
//        /// <summary>
//        /// 寫入記憶體的日誌
//        /// </summary>
//        public static readonly ILog WriteMemorLog = LogManager.GetLogger("WriteMemor");
//        /// <summary>
//        /// 收集訊息回 Server 的記錄檔
//        /// </summary>
//        public static readonly ILog CollectLog = LogManager.GetLogger("Collect");
//        static ServerLogHelper()
//        {
//            //使用指定的配置文件配置log4net。
//            if (HttpContext.Current == null)
//            {
//                log4net.Config.XmlConfigurator.Configure(new FileInfo($"{Path.GetDirectoryName(typeof(ServerLogHelper).Assembly.Location)}\\Log.config"));
//            }
//            else
//            {
//                log4net.Config.XmlConfigurator.Configure(new FileInfo(HttpContext.Current.Server.MapPath("~/Log.config")));
//            }

//        }
//        /// <summary>
//        /// 寫入 <see cref="log4net.ILog"/> 一般訊息
//        /// </summary>
//        /// <param name="log">日誌接口</param>
//        /// <param name="message">訊息說明</param>
//        /// <param name="par">物件參數</param>
//        public static void WriteInfo(ILog log, string message, string par = "")
//        {
//            ThreadContext.Properties["par"] = par;//自定義參數
//            log.Info(message);
//        }


//        /// <summary>
//        /// 寫入 <see cref="log4net.ILog"/> 一般訊息
//        /// </summary>
//        /// <param name="log">日誌接口</param>
//        /// <param name="ex">例外狀況</param>
//        public static void WriteError(ILog log, Exception ex)
//        {
//            ThreadContext.Properties["par"] = $"{ex.Message}\n堆疊框架：{ex.StackTrace}";//自定義參數
//            WriteMemorLog.Error(ex.Message);
//        }
//    }
//}