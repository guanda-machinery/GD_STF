using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
        static void Main(string[] args)
        {
            FreeConsole();
            string command = args[0];
            string[] value = command.Split(',');

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            if (value[0] == "0")
            {
                p.StandardInput.WriteLine($"netsh advfirewall firewall add rule name=\"檔案列印共用\" protocol=TCP dir=in localport={value[1].Trim()},{value[2].Trim()} action=block");
            }
            else
            {
                p.StandardInput.WriteLine($"netsh advfirewall firewall delete rule name=\"檔案列印共用\"");
            }
            p.StandardInput.WriteLine("exit"); //需要有這句，不然程式會掛機

        }
    }
}
