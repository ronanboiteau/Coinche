using System.Net.Sockets;
using System.Text;
using Shared;

namespace Client
{
    public class Player
    {
        private TcpClient channel;
        private Deck deck = new Deck(8);

        public Player(TcpClient channel)
        {
            this.channel = channel;
        }

        
        public void SendMessage(string message)
        {
            var data = Encoding.ASCII.GetBytes(message + "\n");
            channel.GetStream().Write(data, 0, data.Length);
        }

        public Deck GetDeck()
        {
            return deck;
        }

        public void EmptyDeck()
        {
            var count = deck.GetDeck().Count;
            while (count != 0) {
                deck.GetDeck().RemoveAt(0);
                count -= 1;
            }
        }

        public void PutCard(Card card)
        {
            deck.RemoveCard(card);
        }
    }
}
