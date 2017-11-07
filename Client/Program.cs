using System;
using System.Net.Sockets;

namespace Client
{
    public class Client
    {
        private static readonly Deck _modelDeck = new Deck(32);
        private static int _idCardToPlay = -1;

        private static void CreateModelDeck()
        {
            _modelDeck.AddCard(new Card("7", Suit.DIAMONDS, 0, 0));
            _modelDeck.AddCard(new Card("8", Suit.DIAMONDS, 0, 1));
            _modelDeck.AddCard(new Card("9", Suit.DIAMONDS, 0, 2));
            _modelDeck.AddCard(new Card("J", Suit.DIAMONDS, 2, 3));
            _modelDeck.AddCard(new Card("Q", Suit.DIAMONDS, 3, 4));
            _modelDeck.AddCard(new Card("K", Suit.DIAMONDS, 4, 5));
            _modelDeck.AddCard(new Card("10", Suit.DIAMONDS, 10, 6));
            _modelDeck.AddCard(new Card("A", Suit.DIAMONDS, 11, 7));
            _modelDeck.AddCard(new Card("7", Suit.CLUBS, 0, 8));
            _modelDeck.AddCard(new Card("8", Suit.CLUBS, 0, 9));
            _modelDeck.AddCard(new Card("9", Suit.CLUBS, 0, 10));
            _modelDeck.AddCard(new Card("J", Suit.CLUBS, 2, 11));
            _modelDeck.AddCard(new Card("Q", Suit.CLUBS, 3, 12));
            _modelDeck.AddCard(new Card("K", Suit.CLUBS, 4, 13));
            _modelDeck.AddCard(new Card("10", Suit.CLUBS, 10, 14));
            _modelDeck.AddCard(new Card("A", Suit.CLUBS, 11, 15));
            _modelDeck.AddCard(new Card("7", Suit.HEARTS, 0, 16));
            _modelDeck.AddCard(new Card("8", Suit.HEARTS, 0, 17));
            _modelDeck.AddCard(new Card("9", Suit.HEARTS, 0, 18));
            _modelDeck.AddCard(new Card("J", Suit.HEARTS, 2, 19));
            _modelDeck.AddCard(new Card("Q", Suit.HEARTS, 3, 20));
            _modelDeck.AddCard(new Card("K", Suit.HEARTS, 4, 21));
            _modelDeck.AddCard(new Card("10", Suit.HEARTS, 10, 22));
            _modelDeck.AddCard(new Card("A", Suit.HEARTS, 11, 23));
            _modelDeck.AddCard(new Card("7", Suit.SPADES, 0, 24));
            _modelDeck.AddCard(new Card("8", Suit.SPADES, 0, 25));
            _modelDeck.AddCard(new Card("9", Suit.SPADES, 0, 26));
            _modelDeck.AddCard(new Card("J", Suit.SPADES, 2, 27));
            _modelDeck.AddCard(new Card("Q", Suit.SPADES, 3, 28));
            _modelDeck.AddCard(new Card("K", Suit.SPADES, 4, 29));
            _modelDeck.AddCard(new Card("10", Suit.SPADES, 10, 30));
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
        
        private static void     PlayACard(Player player) {
            Console.Write("Enter the ID of the card you want to play:\n");
            var input = Console.ReadLine();
            while (!Int32.TryParse(input, out _idCardToPlay))
            {
                Console.Write("Invalid card ID!\n");
                Console.Write("Enter the ID of the card you want to play:\n");
                input = Console.ReadLine();
            }
            player.SendMessage("PLAY " + input);
        }
        
        public static void Main()
        {
            try
            {
                CreateModelDeck();
                var client = new TcpClient();
//                Console.Write("Server IP: ");
//                var ip = Console.ReadLine();
                Console.Write("Server port: ");
                int port = Convert.ToInt32(Console.ReadLine());
                Console.Write("Connecting to server...\n");
                client.Connect("127.0.0.1", port);
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
                    if (received.StartsWith("MSG "))
                        Console.Write(received.Substring(4, received.Length - 4) + "\n");
                    else if (received.StartsWith("DECK "))
                    {
                        player.EmptyDeck();
                        var cardsId = received.Substring(5, received.Length - 5).Split();
                        for (var idx = 0; idx < cardsId.Length; idx += 1)
                            player.GetDeck().AddCard(_modelDeck.GetCardById(Int32.Parse(cardsId[idx])));
                        Console.Write("Your cards:");
                        player.GetDeck().PrintDeck();
                    }
                    else if (received.Equals("BID"))
                        BiddingChoice(player);
                    else if (received.Equals("BID KO"))
                        BiddingChoice(player);
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
