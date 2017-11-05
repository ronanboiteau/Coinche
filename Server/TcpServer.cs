using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class TcpServer
    {
        private static TcpListener Listener { get; set; }
        private static bool Accept { get; set; }
        private static int ConnectedClients { get; set; }
        private static readonly List<TcpClient> Clients = new List<TcpClient>();

        public static void StartServer(string ip, int port)
        {
            IPAddress address = IPAddress.Parse(ip);
            Listener = new TcpListener(address, port);
            Listener.Start();
            Accept = true;
            Console.WriteLine($"Server started. Listening to TCP clients at {ip}:{port}");
        }

            public static void Listen()
            {
                if (Listener != null && Accept)
                {
                    while (ConnectedClients < 4)
                    {
                        Console.WriteLine($"Waiting for client... {ConnectedClients} connected at the moment.");
                        _listen();
                    }
                Console.WriteLine("Let's roll baby!!");
//                byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
//                client.GetStream().Write(data, 0, data.Length);
//                byte[] buffer = new byte[1024];
//                client.GetStream().Read(buffer, 0, buffer.Length);
//                String msg = Encoding.ASCII.GetString(buffer);
//                Console.WriteLine(msg);
            }
        }

        private static void _listen()
        {
            Task<TcpClient> task = Listener.AcceptTcpClientAsync();
            TcpClient client = task.Result;
            if (client == null)
                return ;
            Console.WriteLine("A new client just joined!");
            Clients.Add(client);
            ConnectedClients++;
//            Console.WriteLine("Closing connection. {connectedClients} connected at the moment.");
//            client.GetStream().Dispose();
//            clients.Remove(client);
//            connectedClients--;
            }

//        private async static void _listen() {
//            Task<TcpClient> clientTask = listener.AcceptTcpClientAsync(); // Get the client
//            clients.Add(clientTask);
//            if (clientTask.Result != null) {
//                connectedClients++;
//                Console.WriteLine("Client connected. Waiting for data.");
//                var client = clientTask.Result;
//                string message = "";
//                await Task.Run(() => {
//                    while (message != null && !message.Equals("quit")) {
//                        byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
//                        client.GetStream().Write(data, 0, data.Length);
//                        byte[] buffer = new byte[1024];
//                        client.GetStream().Read(buffer, 0, buffer.Length);
//                        message = Encoding.ASCII.GetString(buffer);
//                        Console.WriteLine(message);
//                    }    
//                });
//                Console.WriteLine("Closing connection. {connectedClients} connected at the moment.");
//                client.GetStream().Dispose();
//                clients.Remove(clientTask);
//                connectedClients--;
//            }
    }
}
