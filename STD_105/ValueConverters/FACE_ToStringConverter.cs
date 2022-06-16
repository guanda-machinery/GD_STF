using System;

namespace STD_105
{
    public class FACE_ToStringConverter : WPFWindowsBase.BaseEnumValueConverter<GD_STD.Enum.FACE>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
