using System;
using System.Net.Sockets;

namespace Client
{
    public class Client
    {
        private static readonly Deck ModelDeck = new Deck(32);
        private static int _idCardToPlay = -1;

        private static void CreateModelDeck()
        {
            ModelDeck.AddCard(new Card("7", Suit.DIAMONDS, 0, 0));
            ModelDeck.AddCard(new Card("8", Suit.DIAMONDS, 0, 1));
            ModelDeck.AddCard(new Card("9", Suit.DIAMONDS, 0, 2));
            ModelDeck.AddCard(new Card("J", Suit.DIAMONDS, 2, 3));
            ModelDeck.AddCard(new Card("Q", Suit.DIAMONDS, 3, 4));
            ModelDeck.AddCard(new Card("K", Suit.DIAMONDS, 4, 5));
            ModelDeck.AddCard(new Card("10", Suit.DIAMONDS, 10, 6));
            ModelDeck.AddCard(new Card("A", Suit.DIAMONDS, 11, 7));
            ModelDeck.AddCard(new Card("7", Suit.CLUBS, 0, 8));
            ModelDeck.AddCard(new Card("8", Suit.CLUBS, 0, 9));
            ModelDeck.AddCard(new Card("9", Suit.CLUBS, 0, 10));
            ModelDeck.AddCard(new Card("J", Suit.CLUBS, 2, 11));
            ModelDeck.AddCard(new Card("Q", Suit.CLUBS, 3, 12));
            ModelDeck.AddCard(new Card("K", Suit.CLUBS, 4, 13));
            ModelDeck.AddCard(new Card("10", Suit.CLUBS, 10, 14));
            ModelDeck.AddCard(new Card("A", Suit.CLUBS, 11, 15));
            ModelDeck.AddCard(new Card("7", Suit.HEARTS, 0, 16));
            ModelDeck.AddCard(new Card("8", Suit.HEARTS, 0, 17));
            ModelDeck.AddCard(new Card("9", Suit.HEARTS, 0, 18));
            ModelDeck.AddCard(new Card("J", Suit.HEARTS, 2, 19));
            ModelDeck.AddCard(new Card("Q", Suit.HEARTS, 3, 20));
            ModelDeck.AddCard(new Card("K", Suit.HEARTS, 4, 21));
            ModelDeck.AddCard(new Card("10", Suit.HEARTS, 10, 22));
            ModelDeck.AddCard(new Card("A", Suit.HEARTS, 11, 23));
            ModelDeck.AddCard(new Card("7", Suit.SPADES, 0, 24));
            ModelDeck.AddCard(new Card("8", Suit.SPADES, 0, 25));
            ModelDeck.AddCard(new Card("9", Suit.SPADES, 0, 26));
            ModelDeck.AddCard(new Card("J", Suit.SPADES, 2, 27));
            ModelDeck.AddCard(new Card("Q", Suit.SPADES, 3, 28));
            ModelDeck.AddCard(new Card("K", Suit.SPADES, 4, 29));
            ModelDeck.AddCard(new Card("10", Suit.SPADES, 10, 30));
            ModelDeck.AddCard(new Card("A", Suit.SPADES, 11, 31));
        }

        private static void BiddingChoice(Player player)
        {
            Console.Write("Enter 'PASS' or bid: '<amount> <HEARTS|DIAMONDS|CLUBS|SPADES>' (ex: '80 SPADES')\n");
            var input = Console.ReadLine();
            while (input == null)
            {
                Console.Write("Invalid bid!\n");
                Console.Write("Enter 'PASS' or bid: '<amount> <HEARTS|DIAMONDS|CLUBS|SPADES>' (ex: '80 SPADES')\n");
                input = Console.ReadLine();
            }
            if (input.ToUpper().Equals("PASS"))
                player.SendMessage("BID N");
            else
                player.SendMessage("BID Y " + input);
        }
        
        private static void     PlayACard(Player player) {
            Console.Write("Enter the ID of the card you want to play:\n");
            var input = Console.ReadLine();
            while (!int.TryParse(input, out _idCardToPlay))
            {
                Console.Write("Invalid card ID!\n");
                Console.Write("Enter the ID of the card you want to play:\n");
                input = Console.ReadLine();
            }
            player.SendMessage("PLAY " + input);
        }
        
        public static int Main()
        {
            try
            {
                CreateModelDeck();
                var client = new TcpClient();
//                Console.Write("Server IP: ");
//                var ip = Console.ReadLine();
                int port;
                Console.Write("Port: ");
                var input = Console.ReadLine();
                while (!int.TryParse(input, out port))
                {
                    Console.Write("Port: ");
                    input = Console.ReadLine();
                }
                Console.Write("Trying to reach server...\n");
                client.Connect("127.0.0.1", port);
                Console.Write("Connexion successful! Waiting for more players...\n");
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
                    if (received.StartsWith("MSG "))
                        Console.Write(received.Substring(4, received.Length - 4) + "\n");
                    else if (received.StartsWith("DECK "))
                    {
                        player.EmptyDeck();
                        var cardsId = received.Substring(5, received.Length - 5).Split();
                        for (var idx = 0; idx < cardsId.Length; idx += 1)
                            player.GetDeck().AddCard(ModelDeck.GetCardById(int.Parse(cardsId[idx])));
                        Console.Write("Your cards:");
                        player.GetDeck().PrintDeck();
                    }
                    else if (received.Equals("BID"))
                        BiddingChoice(player);
                    else if (received.Equals("BID KO"))
                    {
                        Console.Write("Invalid bid!\n");
                        BiddingChoice(player);
                    }
                    else if (received.Equals("BID RESET"))
                        player.EmptyDeck();
                    else if (received.Equals("PLAY KO"))
                        Console.Write("You cannot play this card!\n");
                    else if (received.Equals("PLAY"))
                        PlayACard(player);
                    else if (received.Equals("PLAY OK"))
                    {
                        if (_idCardToPlay != -1)
                        {
                            player.PutCard(ModelDeck.GetDeck()[_idCardToPlay]);
                            _idCardToPlay = -1;
                        }
                    }
                }
                Console.Write("The game is over. See you soon! :)\n");
                client.Close();
            } catch (SocketException) {
                Console.Write("Cannot connect to server!\n");
                return (84);
            } catch (Exception e) {
                Console.Write("An error occurred! Please see trace below for more information.\n" + e + "\n");
                return 84;
            }
            return 0;
        }
    }
}
