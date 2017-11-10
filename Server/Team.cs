namespace Server
{
    public class Team
    {
        private Player[] _players = new Player[2];
        private string _name;
        private int _score;
        private int _contract;
        private int _nbOfTrick;
        private bool _hasBelote;
        private bool _isCapot = true;

        public Team(Player playerOne, Player playerTwo, string name)
        {
            _players[0] = playerOne;
            _players[1] = playerTwo;
            _name = name;
            _contract = -1;
            _score = 0;
        }
        
        public void AddATrick()
        {
            _nbOfTrick += 1;
        }
        
        public int GetNbOfTrick()
        {
            return _nbOfTrick;
        }
        
        public void SetContract(int contract)
        {
            _contract = contract;
        }

        public int GetContract()
        {
            return _contract;
        }
        
        public int GetScore()
        {
            return _score;
        }
        
        public void AddScore(int toAdd)
        {
            _score += toAdd;
        }
        
        public Player[] GetPlayers()
        {
            return _players;
        }
        
        public Player GetPlayer(int id)
        {
            if (id >= 2 || id < 0)
                return null;
            return _players[id];
        }

        public string GetName()
        {
            return _name;
        }

        public void SetScore(int score)
        {
            _score = score;
        }

        public bool IsCapot()
        {
            return _isCapot;
        }

        public void SetIsCapot(bool isCapot)
        {
            _isCapot = isCapot;
        }

        public void SetHasBelote(bool hasBelote)
        {
            _hasBelote = hasBelote;
        }

        public bool HasBelote()
        {
            return _hasBelote;
        }

        public bool HasWon(int otherTeamScore)
        {
            return _score >= _contract && _score > otherTeamScore;
        }
    }
}
