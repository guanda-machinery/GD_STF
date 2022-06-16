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
            CommonViewModel.AxisInfo = ReadCodesysMemor.GetAxisInfo();
        }
    }
}
