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
            message += "\n";
            var asen = new ASCIIEncoding();
            var toSend = asen.GetBytes(message);
            channel.GetStream().Write(toSend, 0, toSend.Length);
        }

        public Deck GetDeck()
        {
            return (deck);
        }
    }
}