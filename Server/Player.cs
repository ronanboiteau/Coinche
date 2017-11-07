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
        private bool _trumpChooser;

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

        public void SetTrumpChooser(bool trumpChooser)
        {
            _trumpChooser = trumpChooser;
        }

//        public bool PlayerIsBuddy(Player player)
//        {
//            return id == 0 && player.GetId() == 2 || id == 1 && player.GetId() == 3 ||
//                   id == 2 && player.GetId() == 0 || id == 3 && player.GetId() == 1;
//        }

        public bool HasSuit(Suit suit)
        {
            foreach (var card in deck.GetDeck())
            {
                if (card.GetSuit() == suit)
                    return true;
            }
            return false;
        }

        public bool PutCard(Trick trick, int cardId, Suit trump)
        {
            var found = false;
            Card cardToPlay = null;
            foreach (var card in deck.GetDeck())
            {
                if (card.GetId() == cardId)
                {
                    found = true;
                    cardToPlay = card;
                }
            }
            if (!found)
                return (false);
            var isLegal = trick.PlayIsLegal(cardToPlay, this, trump);
            if (isLegal)
            {
                if (trick.CardIsLeading(cardToPlay, trump))
                {
                    trick.SetLeadingPlayer(this);
                    trick.SetLeadingCard(cardToPlay);
                }
                trick.AddCard(cardToPlay);
                deck.RemoveCard(cardToPlay);
            }
            return (isLegal);
        }

        public bool IsTrumpChooser()
        {
            return (_trumpChooser);
        }
        
        public void SendDeck()
        {
            var msgDeck = "DECK ";
            for (var idx = 0 ; idx < deck.Size() ; idx += 1) {
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