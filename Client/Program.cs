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
                TcpClient tcpclnt = new TcpClient();
//                Console.Write("Server IP: ");
//                var ip = Console.ReadLine();
//                Console.Write("Server port: ");
//                int port = Convert.ToInt32(Console.ReadLine());
//                tcpclnt.Connect(ip, port);
                tcpclnt.Connect("127.0.0.1", 4242);
                Console.WriteLine("Connexion successful!");
                Console.Write("Message: ");
                String str = "";
                while (!str.Equals("quit"))
                {
                    str = Console.ReadLine();
                    if (str == null)
                    {
                        Console.Write("\n");
                        return;
                    }
                    Stream stm = tcpclnt.GetStream();
                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes(str);
                    stm.Write(ba, 0, ba.Length);
                    byte[] bb = new byte[100];
                    int k = stm.Read(bb, 0, 100);
                    for (int i = 0; i < k; i++)
                        Console.Write(Convert.ToChar(bb[i]));
                }
                tcpclnt.Close();
            } catch (SocketException) {
                Console.WriteLine("Cannot connect to server!");
            } catch (Exception e) {
                Console.WriteLine("An error occurred! Please see trace below for more information.\n" + e);
            }
        }
    }
}
