using System;
using System.Threading;
namespace SocketServerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🚀 WebSocket Server эхлүүлж байна...");

            var server = WebSocketServer.Instance;
            server.Start();

            Console.WriteLine("✅ WebSocket Server ажиллаж байна.");
            Console.WriteLine("📡 WinForms клиентүүд холбогдох боломжтой");
            Console.WriteLine("🔄 SignalR-рүү мэдэгдэл илгээх боломжтой");
            Console.WriteLine("Зогсоохын тулд 'q' дарна уу.");

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                {
                    break;
                }
            }

            Console.WriteLine("🛑 WebSocket Server зогсож байна...");
            server.Stop();
            Console.WriteLine("✅ WebSocket Server зогслоо.");
        }
    }
}