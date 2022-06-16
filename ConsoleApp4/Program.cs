using GD_STD.Enum;
using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.MemoryHelper;
namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenSharedMemory();
            Operating operating = new Operating();
            operating.OpenApp = true;
            operating.Satus = PHONE_SATUS.MATCH;
            GD_STD.SharedMemory<Operating>.SetValue(operating);

            MonitorWork phone =  MonitorWork.Initialization();
            WorkMaterial  workMaterial=  WorkMaterial.Initialization();
            workMaterial.Finish = 10;
            phone.WorkMaterial[0] = workMaterial;
            SharedMemory<MonitorWork>.SetValue(phone);
            operating = new Operating();
            GD_STD.SharedMemory<Operating>.SetValue(operating);
            MonitorWork monitorWork = new MonitorWork(null);

        }
    }
}
