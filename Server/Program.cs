﻿using System;

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
//                Console.Write("Port: ");
//                int port = Convert.ToInt32(Console.ReadLine());
//                TcpServer.StartServer(ip, port);
                TcpServer.StartServer("127.0.0.1", 4242);
                TcpServer.Listen();
            } catch (Exception e) {
                Console.Write("An error occurred! Please see trace below for more information.\n" + e);
            }
        }
    }
}
