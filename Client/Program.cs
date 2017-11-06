using System;
using System.Net.Sockets;

namespace Client
{
    public class Client
    {
        private static Deck _modelDeck = new Deck(32);
        private static int _idCardToPlay = -1;

        private static void CreateModelDeck()
        {
            _modelDeck.AddCard(new Card("7", Suit.DIAMONDS, 0, 0));
            _modelDeck.AddCard(new Card("8", Suit.DIAMONDS, 0, 1));
            _modelDeck.AddCard(new Card("9", Suit.DIAMONDS, 0, 2));
            _modelDeck.AddCard(new Card("10", Suit.DIAMONDS, 10, 3));
            _modelDeck.AddCard(new Card("J", Suit.DIAMONDS, 2, 4));
            _modelDeck.AddCard(new Card("Q", Suit.DIAMONDS, 3, 5));
            _modelDeck.AddCard(new Card("K", Suit.DIAMONDS, 4, 6));
            _modelDeck.AddCard(new Card("A", Suit.DIAMONDS, 11, 7));
            _modelDeck.AddCard(new Card("7", Suit.CLUBS, 0, 8));
            _modelDeck.AddCard(new Card("8", Suit.CLUBS, 0, 9));
            _modelDeck.AddCard(new Card("9", Suit.CLUBS, 0, 10));
            _modelDeck.AddCard(new Card("10", Suit.CLUBS, 10, 11));
            _modelDeck.AddCard(new Card("J", Suit.CLUBS, 2, 12));
            _modelDeck.AddCard(new Card("Q", Suit.CLUBS, 3, 13));
            _modelDeck.AddCard(new Card("K", Suit.CLUBS, 4, 14));
            _modelDeck.AddCard(new Card("A", Suit.CLUBS, 11, 15));
            _modelDeck.AddCard(new Card("7", Suit.HEARTS, 0, 16));
            _modelDeck.AddCard(new Card("8", Suit.HEARTS, 0, 17));
            _modelDeck.AddCard(new Card("9", Suit.HEARTS, 0, 18));
            _modelDeck.AddCard(new Card("10", Suit.HEARTS, 10, 19));
            _modelDeck.AddCard(new Card("J", Suit.HEARTS, 2, 20));
            _modelDeck.AddCard(new Card("Q", Suit.HEARTS, 3, 21));
            _modelDeck.AddCard(new Card("K", Suit.HEARTS, 4, 22));
            _modelDeck.AddCard(new Card("A", Suit.HEARTS, 11, 23));
            _modelDeck.AddCard(new Card("7", Suit.SPADES, 0, 24));
            _modelDeck.AddCard(new Card("8", Suit.SPADES, 0, 25));
            _modelDeck.AddCard(new Card("9", Suit.SPADES, 0, 26));
            _modelDeck.AddCard(new Card("10", Suit.SPADES, 10, 27));
            _modelDeck.AddCard(new Card("J", Suit.SPADES, 2, 28));
            _modelDeck.AddCard(new Card("Q", Suit.SPADES, 3, 29));
            _modelDeck.AddCard(new Card("K", Suit.SPADES, 4, 30));
            _modelDeck.AddCard(new Card("A", Suit.SPADES, 11, 31));
        }

        private static void BiddingChoice(Player player)
        {
            Console.Write("Enter 'PASS' or bid: '<amount> <HEARTS|DIAMONDS|CLUBS|SPADES>' (ex: '80 SPADES')\n");
            var  input = Console.ReadLine();
            if (input.ToUpper().Equals("PASS"))
                player.SendMessage("BID N");
            else
                player.SendMessage("BID Y " + input);
        }
        
        public static void     PlayACard(Player player) {
            Console.Write("Enter the ID of the card you want to play:\n");
            var  input = Console.ReadLine();
            player.SendMessage("PLAY " + input);
            _idCardToPlay = Int16.Parse(input);
        }
        
        public static void Main()
        {
            try
            {
                CreateModelDeck();
                TcpClient client = new TcpClient();
//                Console.Write("Server IP: ");
//                var ip = Console.ReadLine();
//                Console.Write("Server port: ");
//                int port = Convert.ToInt32(Console.ReadLine());
//                tcpclnt.Connect(ip, port);
                Console.Write("Connecting to server...\n");
                client.Connect("127.0.0.1", 4242);
                Console.Write("Connexion successful!\n");
                var received = "";
                var player = new Player(client);
                while (!received.Equals("END"))
                {
                    var buffer = "";
                    while (buffer.IndexOf('\n') < 0)
                    {
                        var stream = client.GetStream();
                        var buff = new byte[1];
                        var readStr = stream.Read(buff, 0, 1);
                        for (var i = 0; i < readStr; i++)
                            buffer += Convert.ToChar(buff[i]);
                    }
                    received = buffer.Substring(0, buffer.IndexOf('\n'));
                    buffer = buffer.Substring(buffer.IndexOf('\n') + 1, buffer.Length - (buffer.IndexOf('\n') + 1));
                    Console.Write("RECEIVED: " + received + "\n");
                    if (received.StartsWith("MSG "))
                        Console.Write(received.Substring(4, received.Length - 4) + "\n");
                    else if (received.StartsWith("DECK "))
                    {
                        var cardsId = received.Substring(5, received.Length - 5).Split();
                        for (var idx = 0; idx < cardsId.Length; idx += 1)
                        {
                            Console.Write("Trying to add card with ID " + cardsId[idx] + " to your deck...\n");
                            player.GetDeck().AddCard(_modelDeck.GetCardById(Int16.Parse(cardsId[idx])));
                            Console.Write("Card added successfully!\n");
                        }
                        Console.Write("Your cards:");
                        player.GetDeck().PrintDeck();
                    }
                    else if (received.Equals("BID"))
                        BiddingChoice(player);
                    /*else if (received.Equals("BID OK"))
                        ;*/
                    else if (received.Equals("BID KO"))
                        BiddingChoice(player);
                    //else if (received.Equals("BID STOP")) {}
                    else if (received.Equals("BID RESET"))
                    {
                        int     count = player.GetDeck().Size();
                        while (count != 0) {
                            player.GetDeck().GetDeck().RemoveAt(0);
                            count -= 1;
                        }
                    }
                    else if (received.Equals("PLAY KO"))
                    {
                        Console.Write("You cannot play this card!\n");
                        PlayACard(player);
                    }
                    else if (received.Equals("PLAY"))
                        PlayACard(player);
                    else if (received.Equals("PLAY OK"))
                    {
                        if (_idCardToPlay != -1)
                        {
                            player.PutCard(_modelDeck.GetDeck()[_idCardToPlay]);
                            _idCardToPlay = -1;
                        }
                    }
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
