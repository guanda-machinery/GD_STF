using System.Runtime.Serialization;

namespace GD_STD
{
    /// <summary>
    /// 代表 Server 執行期間所發生的錯誤
    /// </summary>
    [DataContract]
    public class ServerError
    {
        /// <summary>
        /// 使用指定的錯誤訊息以及造成此例外狀況的內部例外狀況的參考，初始化 System.Exception 類別的新執行個體。
        /// </summary>
        /// <param name="message">解釋例外狀況原因的錯誤訊息</param>
        /// <param name="stackTrace">例外狀況呼叫堆疊上即時運算框架的字串表示</param>
        public ServerError(string message, string stackTrace)
        {
            Message = message;
            StackTrace = stackTrace;
        }
        /// <summary>
        /// 取得描述目前例外狀況的訊息
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// 取得呼叫堆疊上即時運算框架的字串表示
        /// </summary>
        public string StackTrace { get; private set; }
    }
}
