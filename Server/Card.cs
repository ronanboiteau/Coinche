using System;

namespace Server
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
            return (name);
        }

        public bool IsBeloteCard(Suit trump)
        {
            return suit == trump && (name.Equals("K") || name.Equals("Q"));
        }

        public void MakeTrump()
        {
            if (name == "Q")
                value = 3;
            else if (name == "K")
                value = 4;
            else if (name == "10")
                value = 10;
            else if (name == "A")
                value = 11;
            else if (name == "9")
                value = 14;
            else if (name == "J")
                value = 20;
        }

        public Suit GetSuit()
        {
            return (suit);
        }

        public int GetId()
        {
            return (id);
        }

        public int GetValue()
        {
            return (value);
        }
        
        public void SetValue(int newValue)
        {
            value = newValue;
        }
        
        public void     PrintCard() {
            Console.Write(name + " " + suit);
        }

    }
}
