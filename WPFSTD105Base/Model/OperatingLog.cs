using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.Model
{
    public class OperatingLog
    {
        public string LogSource { get; set; }
        public DateTime LogDatetime { get; set; }
        public string LogString { get; set; }
        public bool IsAlert { get; set; }
    }
    public enum LogSourceEnum
    {
        Init ,
        Phone,
        Machine,
        Software
    }
}
