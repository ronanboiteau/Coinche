using System;
using System.Collections.Generic;

namespace Server
{
    public class Game
    {        
        private Team[] teams = new Team[2];
        private Deck modelDeck = new Deck(32);
        private Deck _deck;
        private List<Player> _allPlayers;
        private Suit _trump;
        private Trick trick = new Trick();
        
        public Game(List<Player> allPlayers, Team teamOne, Team teamTwo)
        {
            teams[0] = teamOne;
            teams[1] = teamTwo;
            _allPlayers = allPlayers;
        }

        private void CreateModelDeck()
        {
            modelDeck.AddCard(new Card("7", Suit.DIAMONDS, 0, 0));
            modelDeck.AddCard(new Card("8", Suit.DIAMONDS, 0, 1));
            modelDeck.AddCard(new Card("9", Suit.DIAMONDS, 0, 2));
            modelDeck.AddCard(new Card("10", Suit.DIAMONDS, 10, 3));
            modelDeck.AddCard(new Card("J", Suit.DIAMONDS, 2, 4));
            modelDeck.AddCard(new Card("Q", Suit.DIAMONDS, 3, 5));
            modelDeck.AddCard(new Card("K", Suit.DIAMONDS, 4, 6));
            modelDeck.AddCard(new Card("A", Suit.DIAMONDS, 11, 7));
            modelDeck.AddCard(new Card("7", Suit.CLUBS, 0, 8));
            modelDeck.AddCard(new Card("8", Suit.CLUBS, 0, 9));
            modelDeck.AddCard(new Card("9", Suit.CLUBS, 0, 10));
            modelDeck.AddCard(new Card("10", Suit.CLUBS, 10, 11));
            modelDeck.AddCard(new Card("J", Suit.CLUBS, 2, 12));
            modelDeck.AddCard(new Card("Q", Suit.CLUBS, 3, 13));
            modelDeck.AddCard(new Card("K", Suit.CLUBS, 4, 14));
            modelDeck.AddCard(new Card("A", Suit.CLUBS, 11, 15));
            modelDeck.AddCard(new Card("7", Suit.HEARTS, 0, 16));
            modelDeck.AddCard(new Card("8", Suit.HEARTS, 0, 17));
            modelDeck.AddCard(new Card("9", Suit.HEARTS, 0, 18));
            modelDeck.AddCard(new Card("10", Suit.HEARTS, 10, 19));
            modelDeck.AddCard(new Card("J", Suit.HEARTS, 2, 20));
            modelDeck.AddCard(new Card("Q", Suit.HEARTS, 3, 21));
            modelDeck.AddCard(new Card("K", Suit.HEARTS, 4, 22));
            modelDeck.AddCard(new Card("A", Suit.HEARTS, 11, 23));
            modelDeck.AddCard(new Card("7", Suit.SPADES, 0, 24));
            modelDeck.AddCard(new Card("8", Suit.SPADES, 0, 25));
            modelDeck.AddCard(new Card("9", Suit.SPADES, 0, 26));
            modelDeck.AddCard(new Card("10", Suit.SPADES, 10, 27));
            modelDeck.AddCard(new Card("J", Suit.SPADES, 2, 28));
            modelDeck.AddCard(new Card("Q", Suit.SPADES, 3, 29));
            modelDeck.AddCard(new Card("K", Suit.SPADES, 4, 30));
            modelDeck.AddCard(new Card("A", Suit.SPADES, 11, 31));
        }

        private void ResetDeck()
        {
            _deck = new Deck(32);
            for (var idx = 0; idx < modelDeck.Size(); idx += 1)
                _deck.AddCard(modelDeck.GetDeck()[idx]);
        }
        
        private void DrawCards()
        {
            var rand = new Random();
            ResetDeck();
            int idxPlayer;
            int idxTeam;
            idxPlayer = rand.Next(1, 2);
            idxTeam = rand.Next(1, 2);
            while (_deck.Size() != 0)
            {
                if (idxPlayer >= 2) {
                    idxPlayer = 0;
                    idxTeam += 1;
                }
                if (idxTeam >= 2)
                    idxTeam = 0;
                Console.Write("========== CARDS REMAINING : " + _deck.Size() + " =========\n");
                for (var i = 0; i < 3; i += 1)
                {
                    int randCard;
                    if (_deck.Size() == 8 || _deck.Size() == 6 || _deck.Size() == 4 || _deck.Size() == 2)
                        i = 1;
                    randCard = _deck.Size() > 1 ? rand.Next(1, _deck.Size()) : 0;
                    Console.Write("Giving card - ");
                    _deck.GetDeck()[randCard].PrintCard();
                    Console.Write(" - to " + teams[idxTeam].GetPlayer(idxPlayer).GetName() +
                                       " of " + teams[idxTeam].GetName() + "\n");
                    teams[idxTeam].GetPlayer(idxPlayer).GetDeck().AddCard(_deck.GetDeck()[randCard]);
                    _deck.RemoveCard(_deck.GetDeck()[randCard]);
                }
                idxPlayer += 1;
            }
        }

        public void Broadcast(String message)
        {
            if (message.Equals("DECK"))
            {
                teams[0].GetPlayer(0).SendDeck();
                teams[0].GetPlayer(1).SendDeck();
                teams[1].GetPlayer(0).SendDeck();
                teams[1].GetPlayer(1).SendDeck();
                return;
            }
            teams[0].GetPlayer(0).SendMessage(message);
            teams[0].GetPlayer(1).SendMessage(message);
            teams[1].GetPlayer(0).SendMessage(message);
            teams[1].GetPlayer(1).SendMessage(message);
        }

        private void PreBidding() {
            teams[0].GetPlayer(0).SendMessage("MSG You are " + teams[0].GetPlayer(0).GetName() + ". Together with your mate "
                                              + teams[0].GetPlayer(1).GetName() + ", you make " + teams[0].GetName() + ".");
            teams[0].GetPlayer(1).SendMessage("MSG You are " + teams[0].GetPlayer(1).GetName() + ". Together with your mate "
                                              + teams[0].GetPlayer(0).GetName() + ", you make " + teams[0].GetName() + ".");
            teams[1].GetPlayer(0).SendMessage("MSG You are " + teams[1].GetPlayer(0).GetName() + ". Together with your mate "
                                              + teams[1].GetPlayer(1).GetName() + ", you make " + teams[1].GetName() + ".");
            teams[1].GetPlayer(1).SendMessage("MSG You are " + teams[1].GetPlayer(1).GetName() + ". Together with your mate "
                                              + teams[1].GetPlayer(0).GetName() + ", you make " + teams[1].GetName() + ".");
        }

        private void AnnounceScores()
        {
            Broadcast("MSG " + teams[0].GetName() + " (" + teams[0].GetPlayer(0).GetName() + ", "
                      + teams[0].GetPlayer(1).GetName() + ") has " + teams[0].GetScore()
                      + (teams[0].GetContract() == -1 ? "" : "/" + teams[0].GetContract()) + " points || "
                      + teams[1].GetName() + " (" + teams[1].GetPlayer(0).GetName() + ", "
                      + teams[1].GetPlayer(1).GetName() + ") has " + teams[1].GetScore()
                      + (teams[1].GetContract() == -1 ? "" : "/" + teams[1].GetContract()) + " points. "
                      + "Trump: " + _trump);
        }

        private int GetTrumpChooser()
        {
            foreach (var team in teams)
            {
                foreach (var player in team.GetPlayers())
                {
                    if (player.IsTrumpChooser())
                        return (player.GetId());
                }
            }
            return (-1);
        }

        public void CalculateScore()
        {
            foreach (var team in teams)
            {
                foreach (var player in team.GetPlayers())
                {
                    if (player.GetId() == trick.GetLeadingPlayer().GetId())
                        team.AddScore(trick.GetValue());
                }
            }
        }

        public void AnnounceWinner()
        {
            if (teams[0].HasWon(teams[1].GetScore()))
            {
                Broadcast("MSG " + teams[0].GetName() + " has " + teams[0].GetScore() + " points and "
                        + teams[1].GetName() + " has " + teams[1].GetScore() + ".");
                Broadcast(teams[0].GetName() + "won! Congratulations!!");
            }
            else
            {
                Broadcast("MSG " + teams[0].GetName() + " has " + teams[0].GetScore() + " points and "
                          + teams[1].GetName() + " has " + teams[1].GetScore() + ".");
                Broadcast(teams[1].GetName() + "won! Congratulations!!");
            }
        }
        
        public void StartPlaying()
        {
            var playerId = GetTrumpChooser();
            while (_allPlayers[0].GetDeck().Size() != 0)
            {
                Broadcast("MSG " + teams[0].GetName() + " (" + teams[0].GetPlayer(0).GetName() + ", "
                          + teams[0].GetPlayer(1).GetName() + ") has " + teams[0].GetScore()
                          + (teams[0].GetContract() == -1 ? "" : "/" + teams[0].GetContract()) + " points || "
                          + teams[1].GetName() + " (" + teams[1].GetPlayer(0).GetName() + ", "
                          + teams[1].GetPlayer(1).GetName() + ") has " + teams[1].GetScore()
                          + (teams[1].GetContract() == -1 ? "" : "/" + teams[1].GetContract()) + " points. "
                          + "Trump: " + _trump);
                AnnounceScores();
                while (trick.Size() != 4)
                {
                    _allPlayers[playerId].SendDeck();
                    _allPlayers[playerId].SendMessage("PLAY");
                    Broadcast("MSG " + _allPlayers[playerId].GetName() + "'s turn...");
                    var msg = _allPlayers[playerId].GetNextMessage().Split();
                    if (msg.Length != 2 && msg[0].Equals("PLAY") &&
                        Int32.TryParse(msg[1], out var cardId))
                    {
                        if (_allPlayers[playerId].PutCard(trick, cardId, _trump))
                        {
                            Broadcast("MSG " + _allPlayers[playerId].GetName() + " put a "
                                      + modelDeck.GetDeck()[cardId].GetName() + " of " +
                                      modelDeck.GetDeck()[cardId].GetSuit()
                                      + ". " + trick.GetLeadingPlayer().GetName() + " is leading this turn.");
                            _allPlayers[playerId].SendMessage("PLAY OK");
                            playerId += 1;
                            playerId = (playerId >= 4 ? 0 : playerId);
                        }
                        else
                            _allPlayers[playerId].SendMessage("PLAY KO");
                    }
                    else
                        _allPlayers[playerId].SendMessage("PLAY KO");
                    playerId = trick.GetLeadingPlayer().GetId();
                    CalculateScore();
                    trick.ResetDeck();
                }
                AnnounceWinner();
                Broadcast("END");
            }
        }

        private Boolean RecursiveBidding(int idPlayer, int maxIterations)
        {
            var iterations = 0;
            while (iterations < maxIterations)
            {
                if (idPlayer >= 4)
                    idPlayer = idPlayer % 4;
                var player = _allPlayers[idPlayer];
                Broadcast("MSG " + player.GetName() + " is making a decision...");
                player.SendMessage("BID");
                while (true)
                {
                    var msg = player.GetNextMessage();
                    Console.Write("[SG] Received client answer: " + msg + "\n");
                    if (msg.Equals("BID N"))
                        break;
                    var msgTab = msg.Split();
                    if (!msgTab[0].Equals("BID") || !msgTab[1].Equals("Y") || msgTab.Length != 4)
                    {
                        player.SendMessage("BID KO");
                        continue;
                    }
                    if (!Int32.TryParse(msgTab[2], out var contract))
                    {
                        player.SendMessage("BID KO");
                        continue;
                    }
                    var suit = msgTab[3];
                    if (contract <= teams[0].GetContract()
                            || contract <= teams[1].GetContract()
                            || contract < 80
                            || contract % 10 != 0)
                    {
                        player.SendMessage("BID KO");
                        continue;
                    }
                    if (!(suit.ToUpper().Equals("HEARTS")
                            || suit.ToUpper().Equals("DIAMONDS")
                            || suit.ToUpper().Equals("CLUBS")
                            || suit.ToUpper().Equals("SPADES")))
                    {
                        player.SendMessage("BID KO");
                        continue;
                    }
                    _trump = (Suit)Enum.Parse(typeof(Suit), suit.ToUpper());
                    teams[(idPlayer == 0 || idPlayer == 2) ? 0 : 1].SetContract(contract);
                    teams[(idPlayer == 0 || idPlayer == 2) ? 1 : 0].SetContract(-1);
                    Broadcast("MSG " + player.GetName() + " from " + teams[(idPlayer == 0 || idPlayer == 2) ? 0 : 1].GetName()
                    + " bid " + contract + " on " + suit.ToUpper());
                    if (!RecursiveBidding(idPlayer + 1, 3))
                        _allPlayers[idPlayer].SetTrumpChooser(true);
                    return (true);
                }
                ++idPlayer;
                ++iterations;
            }
            return (false);
        }

        private void StartBidding()
        {
            var rand = new Random();
            int idPlayer;
            var playerName = "Player1";
            var teamName = "Team2";
            var contract = 0;
            idPlayer = rand.Next(0, 4);
            var contractTaken = false;
            while (!contractTaken)
            {
                DrawCards();
                Broadcast("DECK");
                if (RecursiveBidding(idPlayer, 4))
                    contractTaken = true;
                if (!contractTaken)
                    Broadcast("BID RESET");
            }
            Broadcast("BID STOP");
            foreach (var team in teams)
            {
                foreach (var player in team.GetPlayers())
                {
                    if (player.IsTrumpChooser())
                    {
                        playerName = player.GetName();
                        teamName = team.GetName();
                        contract = team.GetContract();
                    }
                }
            }
            Broadcast("MSG " + playerName + " from " + teamName + " made the final bid! The chosen trump is " + _trump + ". "
                      + teamName + "'s contract is " + contract + ".");
        }
        
        public void StartGame()
        {
            Console.Write("Starting game...\n");
            CreateModelDeck();
            DrawCards();
            PreBidding();
            Broadcast("DECK");
            StartBidding();
//            StartPlaying();
            Broadcast("END");
        }
    }
}