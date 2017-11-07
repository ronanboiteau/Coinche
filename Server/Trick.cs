namespace Server
{
    public class Trick : Deck
    {
        private int _value;
        private Player _leadingPlayer;
        private int _leadingCardId;
        
        public Trick() : base(4)
        {
        }

        public void SetLeadingPlayer(Player player)
        {
            _leadingPlayer = player;
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

        public void SetLeadingCardId(int id)
        {
            _leadingCardId = id;
        }

        public int GetLeadingCardId()
        {
            return _leadingCardId;
        }

        public void ResetDeck()
        {
            for (int i = 0 ; i < cards.Count ; i += 1)
                cards.RemoveAt(0);
        }
    }
}