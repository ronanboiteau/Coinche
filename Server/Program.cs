using System;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
//                Console.Write("IP address: ");
//                var ip = Console.ReadLine();
                Console.Write("Port: ");
                int port = Convert.ToInt32(Console.ReadLine());
                TcpServer.StartServer("127.0.0.1", port);
                TcpServer.Listen();
            } catch (Exception e) {
                Console.Write("An error occurred! Please see trace below for more information.\n" + e);
            }
        }
    }
}
