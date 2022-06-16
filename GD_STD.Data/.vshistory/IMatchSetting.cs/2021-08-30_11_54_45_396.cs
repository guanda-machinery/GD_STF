using System.Collections.Generic;

namespace GD_STD.Data
{
    public interface IMatchSetting
    {
        double Cut { get; set; }
        List<double> SecondaryLengths { get; set; }
        double StartCut { get; set; }
        double StartEnd { get; set; }
    }
}