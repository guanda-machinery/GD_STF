﻿using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static GD_STD.Phone.MemoryHelper;
namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {
            Marshal.SizeOf(typeof(MonitorWork));
            OpenSharedMemory();
            SharedMemory.GetValue<MonitorWork, MonitorWork>(10, 10);

        }
    }
}
