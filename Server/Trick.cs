﻿using System.Runtime.CompilerServices;

namespace Server
{
    public class Trick : Deck
    {
        private int _value;
        private Player _leadingPlayer;
        private Card _leadingCard;
        
        public Trick() : base(4)
        {
        }

        public void SetLeadingPlayer(Player player)
        {
            _leadingPlayer = player;
        }

        public bool PlayIsLegal(Card card, Player player, Suit trump)
        {
            // First play
            if (cards.Count == 0)
                return true;
            var suit = cards[0].GetSuit();
            // The card belongs to the requested suit
            if (card.GetSuit() == suit && suit != trump)
            {
                if (suit != trump)
                    return true;
                return card.GetValue() > _leadingCard.GetValue() ||
                       card.GetName().Equals("8") && _leadingCard.GetName().Equals("7");
            }
            // The card doesn't belong to the requested suit
            if (card.GetSuit() != suit)
            {
                if (player.HasSuit(suit))
                    return false;
                if (card.GetSuit() != trump && player.HasSuit(trump))
                    return false;
                if (card.GetSuit() == trump && _leadingCard.GetSuit() != trump)
                    return true;
                if (card.GetSuit() == trump && _leadingCard.GetSuit() == trump)
                {
                    if (card.GetValue() > _leadingCard.GetValue() ||
                        (card.GetName().Equals("8") && _leadingCard.GetName().Equals("7")))
                        return true;
                }
            }
            return true;
        }

        public bool CardIsLeading(Card card, Suit trump)
        {
            // First play
            if (cards.Count == 0)
                return true;
            // The card belongs to the requested suit 
            if (cards[0].GetSuit() == card.GetSuit())
            {
                if (card.GetSuit() != trump && card.GetId() > _leadingCard.GetId())
                    return true;
                if (card.GetSuit() == trump && (card.GetValue() > _leadingCard.GetValue()
                                                || card.GetName().Equals("8") && _leadingCard.GetName().Equals("7")))
                    return true;
            }
            // The player cut
            else if (card.GetSuit() == trump) {
                if (_leadingCard.GetSuit() != trump
                    || card.GetName().Equals("8") && _leadingCard.GetName().Equals("7")
                    || card.GetValue() > _leadingCard.GetValue())
                    return true;
            }
            // The card isn't of the requested suit & isn't a trump
            return (false);
        }

        public Player GetLeadingPlayer()
        {
            return _leadingPlayer;
        }

        public void AddValue(int toAdd)
        {
            _value += toAdd;
        }

        public int GetValue()
        {
            return (_value);
        }

        public void SetLeadingCard(Card card)
        {
            _leadingCard = card;
        }

        public Card GetLeadingCard()
        {
            return _leadingCard;
        }

        public void ResetDeck()
        {
            for (int i = 0 ; i < cards.Count ; i += 1)
                cards.RemoveAt(0);
        }
    }
}