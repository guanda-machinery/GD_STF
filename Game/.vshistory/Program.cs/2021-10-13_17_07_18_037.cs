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
        static void Main(string[] args)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.StandardInput.WriteLine($"netsh advfirewall firewall add rule name=\"檔案列印共用213\" protocol=TCP dir=in localport=1111 action=block && netsh advfirewall firewall add rule name=\"檔案列印共用213\" protocol=TCP dir=out localport=1111 action=block");
            p.StandardInput.WriteLine("exit"); //需要有這句，不然程式會掛機

        }
    }
}
