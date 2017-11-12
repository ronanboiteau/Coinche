using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Server
{
    public class Program
    {
        private static string GetIp()
        {
            Console.Write("IP address: ");
            var ip = Console.ReadLine();
            var match = Regex.Match(ip, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            while (ip == null || !match.Success)
            {
                Console.Write("Invalid IP address!\n");
                Console.Write("IP address: ");
                ip = Console.ReadLine();
                match = Regex.Match(ip, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            }
            return ip;
        }

        private static int GetPort()
        {
            int port;
            Console.Write("Port: ");
            var input = Console.ReadLine();
            while (input == null || !int.TryParse(input, out port))
            {
                Console.Write("Invalid port!\n");
                Console.Write("Port: ");
                input = Console.ReadLine();
            }
            return port;
        }

        public static int Main()
        {
            try
            {
                var ip = GetIp();
                var port = GetPort();
                GameServer.StartServer(ip, port);
                GameServer.Listen();
            } catch (SocketException) {
                Console.Write("Failed to run server... Please check the IP address & port.\n");
                return 84;
            } catch (Exception) {
                Console.Write("An error occurred!\n");
                return 84;
            }
            return 0;
        }
    }
}
