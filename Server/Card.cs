using System;

namespace Server
{
    public class Card
    {
        private String name;
        private Suit suit;
        private int id;

        public Card(String name, Suit suit, int id)
        {
            this.name = name;
            this.suit = suit;
            this.id = id;
        }

        public String GetName()
        {
            return (name);
        }

        public Suit GetSuit()
        {
            return (suit);
        }

        public int GetId()
        {
            return (id);
        }
    }
}