using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    static class GameServer
    {
        private static TcpListener _listener;
        private static int _connectedClients;
        private static readonly List<Player> Players = new List<Player>();
        
        public static void StartServer(string ip, int port)
        {
            var address = IPAddress.Parse(ip);
            _listener = new TcpListener(address, port);
            _listener.Start();
            Console.Write($"Server up. Listening to TCP clients at {ip}:{port}\n");
        }

        public static void Listen()
        {
            try
            {
                while (_connectedClients < 4)
                {
                    Console.Write($"Waiting for more players... {_connectedClients} connected at the moment.\n"); 
                    _listen();
                }
                _listener.Stop();
                var team1 = new Team(Players[0], Players[2], "Team1");
                var team2 = new Team(Players[1], Players[3], "Team2");
                var game = new Game(Players, team1, team2);
                game.StartGame();
            } catch (Exception) {
                foreach (var player in Players)
                { 
                    player.GetChannel().GetStream().Close();
                    player.GetChannel().Close();
                }
            }
        }

        private static void _listen()
        {
            var task = _listener.AcceptTcpClientAsync();
            var client = task.Result;
            if (client == null)
                return ;
            Console.Write("A new client just joined!\n");
            _connectedClients++;
            var player = new Player(_connectedClients - 1, "Player" + _connectedClients, client);
            Players.Add(player);
        }
    }
}
