using System;
using System.Net.Sockets;
using System.Text;

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

        public void SendMessage(String message)
        {
            var data = Encoding.ASCII.GetBytes(message + "\n");
            channel.GetStream().Write(data, 0, data.Length);
        }

        public Deck GetDeck()
        {
            return (deck);
        }
    }
}