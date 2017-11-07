using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Player
    {
        private int id;
        private String name;
        private TcpClient channel;
        private Deck deck = new Deck(8);
        private Boolean _trumpChooser;

        public Player(int id, String name, TcpClient channel)
        {
            this.id = id;
            this.name = name;
            this.channel = channel;
        }

        public void SendMessage(String message)
        {
            var data = Encoding.ASCII.GetBytes(message + "\n");
            channel.GetStream().Write(data, 0, data.Length);
        }

        public String GetName()
        {
            return name;
        }

        public int GetId()
        {
            return id;
        }

        public Deck GetDeck()
        {
            return (deck);
        }
        
        public String GetNextMessage()
        {
            var buffer = "";
            while (buffer.IndexOf('\n') < 0)
            {
                var stream = channel.GetStream();
                var buff = new byte[1];
                var readStr = stream.Read(buff, 0, 1);
                for (var i = 0; i < readStr; i++)
                    buffer += Convert.ToChar(buff[i]);
            }
            var received = buffer.Substring(0, buffer.IndexOf('\n'));
            buffer = buffer.Substring(buffer.IndexOf('\n') + 1, buffer.Length - (buffer.IndexOf('\n') + 1));
            return received;
        }

        public void SetTrumpChooser(Boolean trumpChooser)
        {
            _trumpChooser = trumpChooser;
        }

        public Boolean PutCard(Trick trick, int cardId, Suit suit)
        {
            // TODO : update leadingPlayer & leadingCardId of the trick
            foreach (var card in deck.GetDeck())
            {
                if (card.GetId() == cardId)
                {
                    trick.AddCard(card);
                    deck.RemoveCard(card);
                    return (true);
                }
            }
            return (false);
        }

        public Boolean IsTrumpChooser()
        {
            return (_trumpChooser);
        }
        
        public void SendDeck()
        {
            String msgDeck = "DECK ";
            for (int idx = 0 ; idx < deck.Size() ; idx += 1) {
                msgDeck += deck.GetDeck()[idx].GetId();
                if (idx < deck.Size() - 1)
                    msgDeck += " ";
            }
            SendMessage(msgDeck);
        }

        public void EmptyDeck()
        {
            var count = deck.GetDeck().Count;
            while (count != 0) {
                deck.GetDeck().RemoveAt(0);
                count -= 1;
            }
        }
    }
}