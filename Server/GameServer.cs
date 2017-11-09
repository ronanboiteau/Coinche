using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class GameServer
    {
        private static TcpListener Listener { get; set; }
        private static bool Accept { get; set; }
        private static int ConnectedClients { get; set; }
        private static List<Player> Players = new List<Player>();

        public static void StartServer(string ip, int port)
        {
            var address = IPAddress.Parse(ip);
            Listener = new TcpListener(address, port);
            Listener.Start();
            Accept = true;
            Console.Write($"Server up. Listening to TCP clients at {ip}:{port}\n");
        }

        public static void Listen()
        {
            if (Listener != null && Accept)
            {
                while (ConnectedClients < 4)
                {
                    Console.Write($"Waiting for more players... {ConnectedClients} connected at the moment.\n");
                    _listen();
                }
                var team1 = new Team(Players[0], Players[2], "Team1");
                var team2 = new Team(Players[1], Players[3], "Team2");
                var game = new Game(Players, team1, team2);
                game.StartGame();
            }
        }

        private static void _listen()
        {
            var task = Listener.AcceptTcpClientAsync();
            var client = task.Result;
            if (client == null)
                return ;
//            if (ConnectedClients >= 4)
//            {
//                var data = Encoding.ASCII.GetBytes("MSG The server is full, sorry! Try again later...\n");
//                client.GetStream().Write(data, 0, data.Length);
//                client.GetStream().Dispose();
//                return ;
//            }
            Console.Write("A new client just joined!\n");
            ConnectedClients++;
            var player = new Player(ConnectedClients - 1, "Player" + ConnectedClients, client);
            Players.Add(player);
        }
    }
}
