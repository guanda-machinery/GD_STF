using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GD_STD;
namespace Memor
{
    class Program
    {
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            MemoryHelper.IsOpen(true);//通知 Codesys 軟體準備關閉 
            MemoryHelper.Dispose();//關閉共享記憶體
            Console.WriteLine("已關閉 Codesys 通訊橋梁");
            Console.ReadLine();

            return false;
        }

        /// <summary>
        /// 進入點
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            MemoryHelper.IsOpen(true);//通知 Codesys 軟體開啟
            MemoryHelper.OpenSharedMemory();//開啟共享記憶體
            //開啟與 Codesys 共享記憶體
            Console.WriteLine("已開啟與 Codesys 通訊橋梁");
            SetConsoleCtrlHandler(cancelHandler, true);
            Console.ReadLine();
        }
    }
}
