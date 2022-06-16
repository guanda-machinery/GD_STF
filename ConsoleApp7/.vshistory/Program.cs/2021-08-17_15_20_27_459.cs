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
            String strQuery = "select * from meta_class";
            //2.引用ManagementObjectSearcher
            ManagementObjectSearcher Query = new ManagementObjectSearcher(strQuery);
            //3.擷取各種WMI集合
            ManagementObjectCollection EnumWMI = Query.Get();
            //4.列舉WMI類別
            foreach (ManagementObject searcher in EnumWMI)
            {
                Debug.WriteLine(searcher.ToString());
            }
        }
    }
}
