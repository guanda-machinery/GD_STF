using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            foreach (ManagementObject MO in MOS.Get())
            {
                foreach (var item in MO.Properties)
                {
                    Console.WriteLine($"{item.Name} : {item.Value}");
                }
            }
            Console.ReadLine();
        }
    }
}
