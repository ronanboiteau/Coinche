using System;

namespace Server
{
    public class Team
    {
        private readonly Player[] _players = new Player[2];
        private readonly String _name;

        public Team(Player playerOne, Player playerTwo, String name)
        {
            _players[0] = playerOne;
            _players[1] = playerTwo;
            _name = name;
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
    }
}