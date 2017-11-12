using System.Net.Sockets;
using System.Text;
using Shared;

namespace Client
{
    public class Player
    {
        private TcpClient _channel;
        private Deck _deck = new Deck(8);

        public Player(TcpClient channel)
        {
            _channel = channel;
        }
        
        public void SendMessage(string message)
        {
            var data = Encoding.ASCII.GetBytes(message + "\n");
            _channel.GetStream().Write(data, 0, data.Length);
        }

        public Deck GetDeck()
        {
            return _deck;
        }

        public void EmptyDeck()
        {
            var count = _deck.GetDeck().Count;
            while (count != 0) {
                _deck.GetDeck().RemoveAt(0);
                count -= 1;
            }
        }

        public void PutCard(Card card)
        {
            _deck.RemoveCard(card);
        }
    }
}
