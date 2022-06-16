using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Codesys_Fat_Baby
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            int i = 0;
            //Thread thread = new Thread(new ThreadStart(s));
            //thread.Start();
            while (true)
            {
                if (i > 50000000)
                {
                    Thread.Sleep(1);
                    i = 0;
                }
                else
                {
                    i++;
                }

            }
        }
        private static byte[] result = new byte[1024*1024*1024];
        static void s()
        {
            //設定伺服器IP地址
            IPAddress ip = IPAddress.Parse("192.168.31.153");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 80)); //配置伺服器IP與埠
                Console.WriteLine("連線伺服器成功");
            }
            catch
            {
                Console.WriteLine("連線伺服器失敗，請按回車鍵退出！");
                return;
            }
            //通過clientSocket接收資料
            //int receiveLength = clientSocket.Receive(result);
            //Console.WriteLine("接收伺服器訊息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));
            //通過 clientSocket 傳送資料
            while (true)
            {
                clientSocket.Send(result);
                Thread.Sleep(200);
            }
            Console.WriteLine("傳送完畢，按回車鍵退出");
            Console.ReadLine();
        }
    }
}
