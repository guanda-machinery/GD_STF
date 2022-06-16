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
                string port = string.Empty;
                for (int i = 1; i < value.Length; i++)
                {
                    port += value[i].Trim()+",";
                }
                port = port.Substring(0, port.Length - 1);
                p.StandardInput.WriteLine($"netsh advfirewall firewall add rule name=\"檔案列印共用\" protocol=TCP dir=in localport={port} action=block && netsh advfirewall firewall add rule name=\"檔案列印共用\" protocol=TCP dir=out localport={port} action=block");
            }
            else
            {
                p.StandardInput.WriteLine($"netsh advfirewall firewall delete rule name=\"檔案列印共用\"");
            }
            p.StandardInput.WriteLine("exit"); //需要有這句，不然程式會掛機

        }
    }
}
