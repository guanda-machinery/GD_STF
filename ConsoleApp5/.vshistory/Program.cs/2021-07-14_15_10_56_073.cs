using ConsoleApp5.Phone;
using GD_STD.Enum;
using GD_STD.Phone;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class Program
    {
      
        public static Operating Operating = new Operating();
        static void Main(string[] args)
        {

            GD_STD.Phone.MemoryHelper.OpenSharedMemory(); //需要使用系統管理員
            Operating = SharedMemory<Operating>.GetValue(); ;
            Operating.OpenApp = true;
            Console.WriteLine("輸入要求 ...... ");
            Operating.Satus = (PHONE_SATUS)(Convert.ToInt16(Console.ReadLine()));
            SharedMemory<Operating>.SetValue(Operating);
            Task.Run(() =>
            {
                while (true)
                    Operating = SharedMemory<Operating>.GetValue();
            });
            Console.WriteLine("等待連線 ...... \n0 = 寫入, 1 = 不寫");
            while (true)
            {
                APP_Struct aPP = SharedMemory<APP_Struct>.GetValue();
                string vs = Console.ReadLine();
                aPP.OpenOil = true;
                switch (vs)
                {
                    case "0":
                        Operating.Satus = PHONE_SATUS.MONITOR;
                        SharedMemory<APP_Struct>.SetValue(aPP);
                        break;
                }
            }
        }
    }
}
