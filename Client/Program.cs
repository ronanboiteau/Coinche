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
                    if (received.StartsWith("MSG "))
                        Console.Write(received.Substring(4, received.Length) + "\n");
                    else if (received.StartsWith("DECK "))
                    {
                        /*    Scanner     scanner = new Scanner(received.substring(5, received.length()));
                            while (scanner.hasNextInt())
                                this.player.getDeck().addCard(modelDeck.getById(scanner.nextInt()));
                            Console.Write("Your cards:");
                            this.player.getDeck().printDeck();*/
                    }
                    else if (received.Equals("BID"))
                        //biddingChoice(chan);
                        ;
                    else if (received.Equals("BID OK"))
                        Console.Write("Illegal bid command!\n");
                    else if (received.Equals("BID KO"))
                        //biddingChoice(chan);
                        ;
                    else if (received.Equals("BID STOP")) {}
                    else if (received.Equals("BID RESET")) {
                        /*int     count = this.player.getDeck().size();
                        while (count != 0) {
                            this.player.getDeck().remove(0);
                            count -= 1;
                        }*/
                    }
                    else if (received.Equals("PLAY KO")) {}
                    else if (received.Equals("PLAY"))
                        //playACard(chan);
                        ;
                    else if (received.Equals("PLAY OK")) {
                        /*if (cardId != -1) {
                            this.player.putCard(cardId);
                            cardId = -1;
                        }*/
                    }
                    Stream stm = client.GetStream();
                    var buff = new byte[100];
                    var readStr = stm.Read(buff, 0, 100);
                    for (var i = 0; i < (readStr - 1); i++)
                        received += Convert.ToChar(buff[i]);
                    Console.Write(received + "\n");
                }
                Console.Write("The game is over. See you soon! :)\n");
                client.Close();
            } catch (SocketException) {
                Console.Write("Cannot connect to server!\n");
            } catch (Exception e) {
                Console.Write("An error occurred! Please see trace below for more information.\n" + e + "\n");
            }
        }
    }
}
