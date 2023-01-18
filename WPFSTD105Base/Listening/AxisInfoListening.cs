using System;
using System.ServiceModel;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.ViewLocator;
namespace WPFSTD105
{
    /// <summary>
    /// 聆聽機台目前主軸的位置與速度
    /// </summary>
    public class AxisInfoListening : AbsListening
    {
        /// <inheritdoc/>
        protected override void ReadCodeSysMemory()
        {
            try
            {
                ApplicationViewModel.AxisInfo = ReadCodesysMemor.GetAxisInfo();
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
                    //throw;
                }
            }
            catch (FaultException ex)
            {
                log4net.LogManager.GetLogger("嚴重錯誤").ErrorFormat(ex.Message, ex.StackTrace);
                
               // throw;
            }
            catch(Exception ex)
            {
                TimeoutNumber++;
            }

        }
    }
}
