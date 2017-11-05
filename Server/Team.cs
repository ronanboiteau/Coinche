using System;

namespace Server
{
    public class Team
    {
        private Player[] players = new Player[2];
        private String name;

        public Team(Player playerOne, Player playerTwo, String name)
        {
            players[0] = playerOne;
            players[1] = playerTwo;
            this.name = name;
        }

        public Player[] GetPlayers()
        {
            return (players);
        }
        
        public Player GetPlayer(int id)
        {
            if (id >= 2 || id < 0)
                return (null);
            return (players[id]);
        }

        public String GetName()
        {
            return (name);
        }
    }
}