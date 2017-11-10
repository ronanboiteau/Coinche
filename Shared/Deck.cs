using System;
using System.Collections.Generic;

namespace Shared
{
    public class Deck
    {
        protected readonly List<Card> _cards = new List<Card>();
        private readonly int _maxCards;

        public Deck(int maxCards)
        {
            this._maxCards = maxCards;
        }

        public bool AddCard(Card newCard)
        {
            if (_cards.Count >= _maxCards)
                return false;
            _cards.Add(newCard);
            return true;
        }

        public bool RemoveCard(Card toRemove)
        {
            return _cards.Remove(toRemove);
        }

        public Card GetCardById(int id)
        {
            for (var idx = 0; idx < _cards.Count; idx += 1)
            {
                if (_cards[idx].GetId() == id)
                    return _cards[idx];
            }
            return null;
        }

        public int Size()
        {
            return _cards.Count;
        }

        public List<Card> GetDeck()
        {
            return _cards;
        }

        public void PrintDeck()
        {
            Console.Write(" ");
            for (int idx = 0 ; idx < _cards.Count ; idx += 1) {
                if (idx != 0)
                    Console.Write(" | ");
                Console.Write("(" + _cards[idx].GetId() + ") ");
                _cards[idx].PrintCard();
            }
            Console.Write("\n");
        }
    }
}
