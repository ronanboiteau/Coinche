using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Client
    {
        public static void Main()
        {
            try
            {
                TcpClient client = new TcpClient();
//                Console.Write("Server IP: ");
//                var ip = Console.ReadLine();
//                Console.Write("Server port: ");
//                int port = Convert.ToInt32(Console.ReadLine());
//                tcpclnt.Connect(ip, port);
                Console.Write("Connecting to server...\n");
                client.Connect("127.0.0.1", 4242);
                Console.Write("Connexion successful!\n");
                Console.Write("Message: ");
                while (true)
                {
                    var str = Console.ReadLine();
                    if (str == null)
                    {
                        Console.Write("\n");
                        client.Close();
                        return;
                    }
                    Stream stm = client.GetStream();
                    var asen = new ASCIIEncoding();
                    var ba = asen.GetBytes(str);
                    stm.Write(ba, 0, ba.Length);
                    var bb = new byte[100];
                    var k = stm.Read(bb, 0, 100);
                    for (var i = 0; i < k; i++)
                        Console.Write(Convert.ToChar(bb[i]));
                }
            } catch (SocketException) {
                Console.Write("Cannot connect to server!\n");
            } catch (Exception e) {
                Console.Write("An error occurred! Please see trace below for more information.\n" + e + "\n");
            }
        }
    }
}
