using Shared;
using System;
using System.Net.Sockets;

namespace Client
{
    public class Program
    {
        private static readonly Deck ModelDeck = new Deck(32);
        private static Player _player;
        private static int _idCardToPlay = -1;
        private static bool _isAi;
        private static int _idxCardAi;

        private static void BiddingChoice(Player player)
        {
            Console.Write("Enter 'PASS' or bid: '<amount> <HEARTS|DIAMONDS|CLUBS|SPADES>' (ex: '80 SPADES')\n");
            if (_isAi)
            {
                player.SendMessage("BID Y 80 SPADES");
                return;
            }
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
        
        private static void PlayACard(Player player) {
            Console.Write("Enter the ID of the card you want to play:\n");
            if (_isAi)
            {
                if (_idxCardAi >= player.GetDeck().GetDeck().Count)
                    _idxCardAi = 0;
                Console.Write("Trying to play card nb " + _idxCardAi + "\n");
                _idCardToPlay = player.GetDeck().GetDeck()[_idxCardAi].GetId();
                player.SendMessage("PLAY " + _idCardToPlay);
                return;
            }
            var input = Console.ReadLine();
            while (!int.TryParse(input, out _idCardToPlay))
            {
                Console.Write("Invalid card ID!\n");
                Console.Write("Enter the ID of the card you want to play:\n");
                input = Console.ReadLine();
            }
            player.SendMessage("PLAY " + input);
        }

        private static void CheckBidding(string received)
        {
            if (received.Equals("BID"))
                BiddingChoice(_player);
            else if (received.Equals("BID KO"))
            {
                if (_isAi)
                    _player.SendMessage("BID N");
                else
                {
                    Console.Write("Invalid bid!\n");
                    BiddingChoice(_player);
                }
            }
            else if (received.Equals("BID RESET"))
                _player.EmptyDeck();
        }

        private static void CheckPlay(string received)
        {
            if (received.Equals("PLAY"))
                PlayACard(_player);
            else if (received.Equals("PLAY KO"))
            {
                if (_isAi)
                    _idxCardAi += 1;
                else
                    Console.Write("You cannot play this card!\n");
            }
            else if (received.Equals("PLAY OK"))
            {
                if (_idCardToPlay == -1)
                    return;
                if (_isAi)
                    Console.Write(_idCardToPlay + "\n");
                _player.PutCard(ModelDeck.GetDeck()[_idCardToPlay]);
                _idCardToPlay = -1;
            }
        }
        
        public static int Main(string[] args)
        {
            try
            {
                ModelDeck.Generate32CardsDeck();
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
                Console.Write("Connection successful! Waiting for more players...\n");
                var received = "";
                _player = new Player(client);
                if (args.Length == 1 && args[0].ToUpper().Equals("AI"))
                    _isAi = true;
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
                        _player.EmptyDeck();
                        var cardsId = received.Substring(5, received.Length - 5).Split();
                        for (var idx = 0; idx < cardsId.Length; idx += 1)
                            _player.GetDeck().AddCard(ModelDeck.GetCardById(int.Parse(cardsId[idx])));
                        Console.Write("Your cards:");
                        _player.GetDeck().PrintDeck();
                    }
                    else if (received.StartsWith("BID"))
                        CheckBidding(received);
                    else if (received.StartsWith("PLAY"))
                        CheckPlay(received);
                    
                }
                Console.Write("The game is over. See you soon! :)\n");
                client.Close();
            } catch (SocketException) {
                Console.Write("Cannot reach server!\n");
                return 84;
            } catch (Exception e) {
                Console.Write("An error occurred! Please see trace below for more information.\n" + e + "\n");
                return 84;
            }
            return 0;
        }
    }
}
