using System;
using WPFSTD105;
using WPFWindowsBase;
namespace STD_105
{
    /// <summary>
    /// 一個將 <see cref="MachineMode"/> 選項轉換為文字
    /// </summary>
    public class MchineModeToStringConverter : BaseEnumValueConverter<MachineMode>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
