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
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"); foreach (ManagementObject MO in MOS.Get())
            {
                Console.WriteLine("Processor Details"); 
                Console.WriteLine("==============================================================="); 
                Console.WriteLine("AddressWidth :" + MO["AddressWidth"]); Console.WriteLine("Architecture :" + MO["Architecture"]); 
                Console.WriteLine("Availability :" + MO["Availability"]); Console.WriteLine("Caption      :" + MO["Caption"]); 
                Console.WriteLine("ConfigManagerErrorCode       :" + MO["ConfigManagerErrorCode"]); 
                Console.WriteLine("ConfigManagerUserConfig      :" + MO["ConfigManagerUserConfig"]); 
                Console.WriteLine("CpuStatus    :" + MO["CpuStatus"]); 
                Console.WriteLine("CreationClassName            :" + MO["CreationClassName"]); 
                Console.WriteLine("CurrentClockSpeed            :" + MO["CurrentClockSpeed"]); 
                Console.WriteLine("CurrentVoltage               :" + MO["CurrentVoltage"]); 
                Console.WriteLine("DataWidth    :" + MO["DataWidth"]); 
                Console.WriteLine("Description  :" + MO["Description"]); 
                Console.WriteLine("DeviceID     :" + MO["DeviceID"]); 
                Console.WriteLine("ErrorCleared :" + MO["ErrorCleared"]); 
                Console.WriteLine("InstallDate  :" + MO["InstallDate"]); 
                Console.WriteLine("LoadPercentage               :" + MO["LoadPercentage"]); 
                Console.WriteLine("Name         :" + MO["Name"]); 
                Console.WriteLine("NumberOfCores                :" + MO["NumberOfCores"]); 
                Console.WriteLine("NumberOfLogicalProcessors    :" + MO["NumberOfLogicalProcessors"]);
                Console.WriteLine("ProcessorID  :" + MO["ProcessorID"]); 
                Console.WriteLine("ProcessorType                :" + MO["ProcessorType"]); 
                Console.WriteLine("OtherFamilyDescription       :" + MO["OtherFamilyDescription"]); 
                Console.WriteLine("PNPDeviceID  :" + MO["PNPDeviceID"]); 
                Console.WriteLine("PowerManagementSupported     :" + MO["PowerManagementSupported"]); 
                Console.WriteLine("Revision     :" + MO["Revision"]); Console.WriteLine("Role         :" + MO["Role"]); 
                Console.WriteLine("SocketDesignation            :" + MO["SocketDesignation"]);
                Console.WriteLine("Status       :" + MO["Status"]);
                Console.WriteLine("StatusInfo   :" + MO["StatusInfo"]); 
                Console.WriteLine("Stepping     :" + MO["Stepping"]); 
                Console.WriteLine("SystemCreationClassName      :" + MO["SystemCreationClassName"]); 
                Console.WriteLine("SystemName   :" + MO["SystemName"]); 
                Console.WriteLine("UniqueId     :" + MO["UniqueId"]); 
                Console.WriteLine("UpgradeMethod                :" + MO["UpgradeMethod"]); 
                Console.WriteLine("Version      :" + MO["Version"]); 
                Console.WriteLine("VoltageCaps  :" + MO["VoltageCaps"]);

            }
        }
    }
