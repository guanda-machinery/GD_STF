using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.Model
{
    [Serializable]
    public class MachiningTimeClass : INotifyPropertyChanged
    {
        public DateTime? MachiningStartTime { get; set; }
        public DateTime? MachiningEndTime { get; set; }

        public TimeSpan MachiningTimeSpan
        {
            get
            {
                if (MachiningStartTime.HasValue && MachiningEndTime.HasValue)

                    return MachiningEndTime.Value - MachiningStartTime.Value;

                else

                    return TimeSpan.Zero;

            }
        }

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

    }
}
