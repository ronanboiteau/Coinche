using System;

namespace Shared
{
    public class Card
    {
        private readonly string _name;
        private readonly Suit _suit;
        private readonly int _id;
        private int _value;

        public Card(string name, Suit suit, int value, int id)
        {
            _name = name;
            _suit = suit;
            _value = value;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public bool IsBeloteCard(Suit trump)
        {
            return _suit == trump && (_name.Equals("K") || _name.Equals("Q"));
        }

        public void MakeTrump()
        {
            switch (_name)
            {
                case "Q":
                    _value = 3;
                    break;
                case "K":
                    _value = 4;
                    break;
                case "10":
                    _value = 10;
                    break;
                case "A":
                    _value = 11;
                    break;
                case "9":
                    _value = 14;
                    break;
                case "J":
                    _value = 20;
                    break;
            }
        }

        public Suit GetSuit()
        {
            return _suit;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetValue()
        {
            return _value;
        }
        
        public void SetValue(int newValue)
        {
            _value = newValue;
        }
        
        public void     PrintCard() {
            Console.Write(_name + " " + _suit);
        }

    }
}
