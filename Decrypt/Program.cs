using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFWindowsBase;

namespace Decrypt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run();
        }
        public static void Run()
        {
            Console.Write("Number : ");
            string number = Console.ReadLine();
            Console.Write("Code : ");
            string code = Console.ReadLine();
            MD5DES mD = new MD5DES(number, code);
            WPFSTD105.SystemVerification verification = new WPFSTD105.SystemVerification(mD);
            Console.WriteLine($"Key : {verification.GetKey()}");
            Console.WriteLine("------------------------------------------");
            Run();
        }
    }
}
