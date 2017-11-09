using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks.Dataflow;
using NUnit.Framework;

namespace Server
{
    public class Player
    {
        private int id;
        private string name;
        private TcpClient channel;
        private Deck deck = new Deck(8);
        private bool _trumpChooser;
        private short _beloteCards;
        
        public Player(int id, string name, TcpClient channel)
        {
            this.id = id;
            this.name = name;
            this.channel = channel;
        }
        
        public bool HasGreater(Card cardMin)
        {
            foreach (var card in GetDeck().GetDeck())
            {
                if (card.GetSuit() == cardMin.GetSuit()
                    && card.GetValue() > cardMin.GetValue())
                    return true;
            }
            return false;
        }

        public int HasAllFour()
        {
            var score = 0;
            int[] nb = {0, 0, 0, 0, 0, 0};
            foreach (var card in deck.GetDeck())
            {
                if (card.GetName().Equals("J"))
                    nb[(int) CardName.Jack] += 1;
                else if (card.GetName().Equals("9"))
                    nb[(int) CardName.Nine] += 1;
                else if (card.GetName().Equals("A"))
                    nb[(int) CardName.Ace] += 1;
                else if (card.GetName().Equals("10"))
                    nb[(int) CardName.Ten] += 1;
                else if (card.GetName().Equals("K"))
                    nb[(int) CardName.King] += 1;
                else if (card.GetName().Equals("Q"))
                    nb[(int) CardName.Queen] += 1;
            }
            if (nb[(int) CardName.Jack] == 4)
                score += 200;
            else if (nb[(int) CardName.Nine] == 4)
                score += 150;
            for (var idx = 2; idx <= (int) CardName.Queen ; idx += 1)
            {
                if (nb[idx] == 4)
                    score += 100;
            }
            return score;
        }
        
        public void SendMessage(string message)
        {
            var data = Encoding.ASCII.GetBytes(message + "\n");
            channel.GetStream().Write(data, 0, data.Length);
        }

        public string GetName()
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
        
        public string GetNextMessage()
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
            return buffer.Substring(0, buffer.IndexOf('\n'));
        }

        public void SetTrumpChooser(bool trumpChooser)
        {
            _trumpChooser = trumpChooser;
        }

        public void CheckBelote(Suit trump)
        {
            var check = 0;
            foreach (var card in deck.GetDeck())
            {
                if (card.GetSuit() == trump && card.GetName().Equals("K"))
                    check += 1;
                else if (card.GetSuit() == trump && card.GetName().Equals("Q"))
                    check += 1;
            }
            if (check == 2)
                _beloteCards = 2;
        }

        public short GetBeloteCards()
        {
            return _beloteCards;
        }

        public void SetBeloteCards(short cards)
        {
            _beloteCards = cards; 
        }

        public bool HasBelote()
        {
            return _beloteCards > 0;
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
                trick.AddValue(cardToPlay.GetValue());
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
