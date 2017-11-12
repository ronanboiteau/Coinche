using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    static class GameServer
    {
        private static TcpListener Listener { get; set; }
        private static bool Accept { get; set; }
        private static int ConnectedClients { get; set; }
        private static List<Player> _players = new List<Player>();

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
            if (!(Listener != null && Accept))
                return ;
            while (ConnectedClients < 4)
            {
                Console.Write($"Waiting for more players... {ConnectedClients} connected at the moment.\n");
                _listen();
            }
            var team1 = new Team(_players[0], _players[2], "Team1");
            var team2 = new Team(_players[1], _players[3], "Team2");
            var game = new Game(_players, team1, team2);
            game.StartGame();
        }

        private static void _listen()
        {
            var task = Listener.AcceptTcpClientAsync();
            var client = task.Result;
            if (client == null)
                return ;
            Console.Write("A new client just joined!\n");
            ConnectedClients++;
            var player = new Player(ConnectedClients - 1, "Player" + ConnectedClients, client);
            _players.Add(player);
        }
    }
}
