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
                String received = "";
                while (!received.Equals("END"))
                {
                    received = "";
                    /*var str = Console.ReadLine();
                    if (str == null)
                    {
                        Console.Write("\n");
                        client.Close();
                        return;
                    }
                    var asen = new ASCIIEncoding();
                    var toSend = asen.GetBytes(str);
                    stm.Write(toSend, 0, toSend.Length);*/
                    Stream stm = client.GetStream();
                    var buff = new byte[100];
                    var readStr = stm.Read(buff, 0, 100);
                    for (var i = 0; i < (readStr - 1); i++)
                        received += Convert.ToChar(buff[i]);
                    Console.Write(received + "\n");
                }
                Console.Write("Bye! :)\n");
                client.Close();
            } catch (SocketException) {
                Console.Write("Cannot connect to server!\n");
            } catch (Exception e) {
                Console.Write("An error occurred! Please see trace below for more information.\n" + e + "\n");
            }
        }
    }
}
