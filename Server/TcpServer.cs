using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TCPServer.App.AfzaalAhmadZeeshan
{
    class TcpServer
    {
        private static TcpListener listener { get; set; }
        private static bool accept { get; set; } = false;
        private static int connectedClients { get; set; } = 0;

        public static void StartServer(int port, string IP)
        {
            IPAddress address = IPAddress.Parse(IP);
            listener = new TcpListener(address, port);

            listener.Start();
            accept = true;

            Console.WriteLine($"Server started. Listening to TCP clients at {IP}:{port}");
        }

        public static void Listen()
        {
            if (listener != null && accept)
            {
                // Continue listening.
                while (true)
                {
                    Console.WriteLine($"Waiting for client... {connectedClients} connected at the moment.");
                    _listen(); // Helper to listen on the background and return.
                }
            }
        }

        private async static void _listen() {
            var clientTask = listener.AcceptTcpClientAsync(); // Get the client

            if (clientTask.Result != null)
            {
                connectedClients++;
                Console.WriteLine("Client connected. Waiting for data.");
                var client = clientTask.Result;
                string message = "";

                await Task.Run(() => {
                    while (message != null && !message.StartsWith("quit"))
                    {
                        byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
                        client.GetStream().Write(data, 0, data.Length);

                        byte[] buffer = new byte[1024];
                        client.GetStream().Read(buffer, 0, buffer.Length);

                        message = Encoding.ASCII.GetString(buffer);
                        Console.WriteLine(message);
                    }    
                });
                
                Console.WriteLine("Closing connection. {connectedClients} connected at the moment.");
                client.GetStream().Dispose();
                connectedClients--;
            }
        }
    }
}
