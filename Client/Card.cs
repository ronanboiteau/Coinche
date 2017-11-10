using System;

namespace Client
{
    public class Card
    {
        private string name;
        private Suit suit;
        private int id;
        private int value;

        public Card(string name, Suit suit, int value, int id)
        {
            this.name = name;
            this.suit = suit;
            this.value = value;
            this.id = id;
        }

        public string GetName()
        {
            return name;
        }

        public Suit GetSuit()
        {
            return suit;
        }

        public int GetId()
        {
            return id;
        }

        public int GetValue()
        {
            return value;
        }
        
        public void     PrintCard() {
            Console.Write(name + " " + suit);
        }

    }
}
