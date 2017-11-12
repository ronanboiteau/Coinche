using Shared;
using System;
using System.Data;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

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
            Console.Write("Enter 'PASS', 'COINCHE', 'SURCOINCHE' or bid: '<amount> <HEARTS|DIAMONDS|CLUBS|SPADES>' (ex: '80 SPADES')\n");
            if (_isAi)
            {
                player.SendMessage("BID Y 80 SPADES");
                return;
            }
            var input = Console.ReadLine();
            while (input == null)
            {
                Console.Write("Invalid bid!\n");
                Console.Write("Enter 'PASS', 'COINCHE', 'SURCOINCHE' or bid: '<amount> <HEARTS|DIAMONDS|CLUBS|SPADES>' (ex: '80 SPADES')\n");
                input = Console.ReadLine();
            }
            if (input.ToUpper().Equals("PASS"))
                player.SendMessage("BID N");
            else if (input.ToUpper().Equals("COINCHE"))
                player.SendMessage("BID COINCHE");
            else if (input.ToUpper().Equals("SURCOINCHE"))
                player.SendMessage("BID SURCOINCHE");
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
        
        private static string GetIp()
        {
            Console.Write("Server IP: ");
            var ip = Console.ReadLine();
            var match = Regex.Match(ip, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            while (ip == null || !match.Success)
            {
                Console.Write("Invalid IP address!\n");
                Console.Write("Server IP: ");
                ip = Console.ReadLine();
                match = Regex.Match(ip, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            }
            return ip;
        }

        private static int GetPort()
        {
            int port;
            Console.Write("Port: ");
            var input = Console.ReadLine();
            while (input == null || !int.TryParse(input, out port))
            {
                Console.Write("Invalid port!\n");
                Console.Write("Port: ");
                input = Console.ReadLine();
            }
            Console.Write("Trying to reach server...\n");
            return port;
        }

        public static int Main(string[] args)
        {
            try
            {
                ModelDeck.Generate32CardsDeck();
                var client = new TcpClient();
                var ip = GetIp();
                var port = GetPort();
                client.Connect(ip, port);
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
            } catch (Exception) {
                Console.Write("An error occurred!\n");
                return 84;
            }
            return 0;
        }
    }
}
