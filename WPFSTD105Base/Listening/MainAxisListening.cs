using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
namespace WPFSTD105.Listening
{
    /// <summary>
    /// 聆聽 3D 追隨
    /// </summary>
    public class MainAxisListening : AbsListening
    {
        /// <inheritdoc/>
        protected override void ReadCodeSysMemory()
        {
            try
            {
                ReadMemoryCallbackHandler.LocationResetEvent.Reset();
                ReadDuplexMemory.GetMainAxisLocation();
                ReadMemoryCallbackHandler.LocationResetEvent.WaitOne(Timeout.Infinite);//等待完成訊號
            }
            catch (TimeoutException ex)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace, "嘗試重新連線");
                ReadDuplexMemory.ChannelFactory.CreateChannel();
                if (TimeoutNumber < 10)
                {
                    TimeoutNumber++;
                    return;
                }
                else
                {
                    throw;
                }
            }
            catch (FaultException ex)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                throw;
            }

        }
    }
}
