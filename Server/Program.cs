using System;

namespace Server
{
    public class Program
    {
        public static int Main()
        {
            try
            {
//                Console.Write("IP address: ");
//                var ip = Console.ReadLine();
                Console.Write("Port: ");
                var port = Convert.ToInt32(Console.ReadLine());
                GameServer.StartServer("127.0.0.1", port);
                GameServer.Listen();
            } catch (Exception e) {
                Console.Write("An error occurred! Please see trace below for more information.\n" + e);
                return 84;
            }
            return 0;
        }
    }
}
