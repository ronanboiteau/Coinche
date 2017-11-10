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

        public void Generate32CardsDeck()
        {
            AddCard(new Card("7", Suit.DIAMONDS, 0, 0));
            AddCard(new Card("8", Suit.DIAMONDS, 0, 1));
            AddCard(new Card("9", Suit.DIAMONDS, 0, 2));
            AddCard(new Card("J", Suit.DIAMONDS, 2, 3));
            AddCard(new Card("Q", Suit.DIAMONDS, 3, 4));
            AddCard(new Card("K", Suit.DIAMONDS, 4, 5));
            AddCard(new Card("10", Suit.DIAMONDS, 10, 6));
            AddCard(new Card("A", Suit.DIAMONDS, 11, 7));
            AddCard(new Card("7", Suit.CLUBS, 0, 8));
            AddCard(new Card("8", Suit.CLUBS, 0, 9));
            AddCard(new Card("9", Suit.CLUBS, 0, 10));
            AddCard(new Card("J", Suit.CLUBS, 2, 11));
            AddCard(new Card("Q", Suit.CLUBS, 3, 12));
            AddCard(new Card("K", Suit.CLUBS, 4, 13));
            AddCard(new Card("10", Suit.CLUBS, 10, 14));
            AddCard(new Card("A", Suit.CLUBS, 11, 15));
            AddCard(new Card("7", Suit.HEARTS, 0, 16));
            AddCard(new Card("8", Suit.HEARTS, 0, 17));
            AddCard(new Card("9", Suit.HEARTS, 0, 18));
            AddCard(new Card("J", Suit.HEARTS, 2, 19));
            AddCard(new Card("Q", Suit.HEARTS, 3, 20));
            AddCard(new Card("K", Suit.HEARTS, 4, 21));
            AddCard(new Card("10", Suit.HEARTS, 10, 22));
            AddCard(new Card("A", Suit.HEARTS, 11, 23));
            AddCard(new Card("7", Suit.SPADES, 0, 24));
            AddCard(new Card("8", Suit.SPADES, 0, 25));
            AddCard(new Card("9", Suit.SPADES, 0, 26));
            AddCard(new Card("J", Suit.SPADES, 2, 27));
            AddCard(new Card("Q", Suit.SPADES, 3, 28));
            AddCard(new Card("K", Suit.SPADES, 4, 29));
            AddCard(new Card("10", Suit.SPADES, 10, 30));
            AddCard(new Card("A", Suit.SPADES, 11, 31));
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
