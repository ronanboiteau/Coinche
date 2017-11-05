namespace Server
{
    public class Game
    {        
        Team[] teams = new Team[2];
        
        public Game(Team teamOne, Team teamTwo)
        {
            teams[0] = teamOne;
            teams[1] = teamTwo;
        }

        public void StartGame()
        {
            
        }
    }
}