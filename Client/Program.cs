using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Client {

        public static void Main() {
        
            try {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");
            
                Console.Write("Enter the IP address of the server: ");
                var ip = Console.ReadLine();
                Console.Write("Enter the port of the server: ");
                int port = Convert.ToInt32(Console.ReadLine());
                
                tcpclnt.Connect(ip,port);
                // use the ipaddress as in the server program
            
                Console.WriteLine("Connected");
                Console.Write("Enter the string to be transmitted : ");
                String str = "";
                while (!str.Equals("quit"))
                {
                    str = Console.ReadLine();
                    Stream stm = tcpclnt.GetStream();

                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] ba = asen.GetBytes(str);
                    Console.WriteLine("Transmitting.....");

                    stm.Write(ba, 0, ba.Length);

                    byte[] bb = new byte[100];
                    int k = stm.Read(bb, 0, 100);

                    for (int i = 0; i < k; i++)
                        Console.Write(Convert.ToChar(bb[i]));
                }
                tcpclnt.Close();
            }
        
            catch (Exception e) {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

    }
}

/*using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hey you! Someday I'll be a coinche client!");
        }
    }
}*/