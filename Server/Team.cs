using System;

namespace Server
{
    public class Team
    {
        private Player[] _players = new Player[2];
        private String _name;
        private int _score;
        private int _contract;

        public Team(Player playerOne, Player playerTwo, String name)
        {
            _players[0] = playerOne;
            _players[1] = playerTwo;
            _name = name;
            _contract = -1;
        }

        public void SetContract(int contract)
        {
            _contract = contract;
        }

        public int GetContract()
        {
            return (_contract);
        }
        
        public int GetScore()
        {
            return (_score);
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
                return (null);
            return _players[id];
        }

        public String GetName()
        {
            return _name;
        }

        public Boolean HasWon(int otherTeamScore)
        {
            if (_score >= _contract && _score > otherTeamScore) {
                _score += _contract;
                return (true);
            }
            _score = 0;
            return (false);
        }
    }
}