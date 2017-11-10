using System;
using System.Collections.Generic;

namespace Client
{
    public class Deck
    {
        private List<Card> cards = new List<Card>();
        private int maxCards;

        public Deck(int maxCards)
        {
            this.maxCards = maxCards;
        }

        public bool AddCard(Card newCard)
        {
            if (cards.Count >= maxCards)
                return (false);
            cards.Add(newCard);
            return (true);
        }

        public bool RemoveCard(Card toRemove)
        {
            return (cards.Remove(toRemove));
        }

        public Card GetCardById(int id)
        {
            for (int idx = 0; idx < cards.Count; idx += 1)
            {
                if (cards[idx].GetId() == id)
                    return (cards[idx]);
            }
            return (null);
        }

        public int Size()
        {
            return (cards.Count);
        }

        public List<Card> GetDeck()
        {
            return (cards);
        }

        public void PrintDeck()
        {
            Console.Write(" ");
            for (int idx = 0 ; idx < cards.Count ; idx += 1) {
                if (idx != 0)
                    Console.Write(" | ");
                Console.Write("(" + cards[idx].GetId() + ") ");
                cards[idx].PrintCard();
            }
            Console.Write("\n");
        }
    }
}
