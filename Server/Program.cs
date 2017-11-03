using System;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter the IP address where to run the TCP server: ");
            var ip = Console.ReadLine();
            Console.Write("Enter the port where to run the TCP server: ");
            int port = Convert.ToInt32(Console.ReadLine());

            // Start the server.
            TcpServer.StartServer(port, ip);
            TcpServer.Listen();
        }
    }
}
