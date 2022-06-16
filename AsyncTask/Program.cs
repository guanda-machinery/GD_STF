using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTask
{
    internal class Program
    {
        static Task task1, task2;
        static void Main(string[] args)
        {
            task1= Task.Factory.StartNew(GetTask1);
            task2= Task.Factory.StartNew(GetTask2);
            // .GetAwaiter();
            //.GetAwaiter();
            Console.WriteLine("完成");
            Console.ReadLine();
        }
        public static async void GetTask1()
        {
            Thread.Sleep(8000);
            Console.WriteLine("task 1");
            await Task.Yield();
        }
        public static async void GetTask2()
        {

            //Task.Yield();
            task1.Wait();
            await Task.Yield();
            Console.WriteLine("task 2");
        }
    }
}
