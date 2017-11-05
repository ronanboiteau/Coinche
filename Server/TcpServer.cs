using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class TcpServer
    {
        private static TcpListener Listener { get; set; }
        private static bool Accept { get; set; }
        private static int ConnectedClients { get; set; }
        private static readonly List<Player> Players = new List<Player>();

        public static void StartServer(string ip, int port)
        {
            var address = IPAddress.Parse(ip);
            Listener = new TcpListener(address, port);
            Listener.Start();
            Accept = true;
            Console.Write($"Server started. Listening to TCP clients at {ip}:{port}\n");
        }

        public static void Listen()
        {
            if (Listener != null && Accept)
            {
                while (ConnectedClients < 4)
                {
                    Console.Write($"Waiting for client... {ConnectedClients} connected at the moment.\n");
                    _listen();
                }
            var team1 = new Team(Players[0], Players[2], "Team1");
            var team2 = new Team(Players[1], Players[3], "Team2");
            var game = new Game(team1, team2);
            game.StartGame();
            }
//                byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
//                client.GetStream().Write(data, 0, data.Length);
//                byte[] buffer = new byte[1024];
//                client.GetStream().Read(buffer, 0, buffer.Length);
//                String msg = Encoding.ASCII.GetString(buffer);
//                Console.Write(msg + "\n");
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
            Players.Add(player);
//            Console.Write("Closing connection. {connectedClients} connected at the moment.\n");
//            client.GetStream().Dispose();
//            clients.Remove(client);
//            connectedClients--;
            }

//        private async static void _listen() {
//            Task<TcpClient> clientTask = listener.AcceptTcpClientAsync(); // Get the client
//            clients.Add(clientTask);
//            if (clientTask.Result != null) {
//                connectedClients++;
//                Console.Write("Client connected. Waiting for data.\n");
//                var client = clientTask.Result;
//                string message = "";
//                await Task.Run(() => {
//                    while (message != null && !message.Equals("quit")) {
//                        byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
//                        client.GetStream().Write(data, 0, data.Length);
//                        byte[] buffer = new byte[1024];
//                        client.GetStream().Read(buffer, 0, buffer.Length);
//                        message = Encoding.ASCII.GetString(buffer);
//                        Console.Write(message + "\n");
//                    }    
//                });
//                Console.Write("Closing connection. {connectedClients} connected at the moment.\n");
//                client.GetStream().Dispose();
//                clients.Remove(clientTask);
//                connectedClients--;
//            }
    }
}
