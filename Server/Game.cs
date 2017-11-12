using System;
using System.Collections.Generic;
using Shared;

namespace Server
{
    public class Game
    {        
        private Team[] _teams = new Team[2];
        private readonly Deck _modelDeck = new Deck(32);
        private Deck _deck;
        private List<Player> _allPlayers;
        private Suit _trump;
        private Trick _trick = new Trick();
        private int _multiplier = 1;
        
        public Game(List<Player> allPlayers, Team teamOne, Team teamTwo)
        {
            _teams[0] = teamOne;
            _teams[1] = teamTwo;
            _allPlayers = allPlayers;
        }

        private void SetTrump(Suit trump)
        {
            _trump = trump;
            foreach (var card in _modelDeck.GetDeck())
            {
                if (card.GetSuit() == _trump)
                    card.MakeTrump();
            }
        }

        private void ResetDeck()
        {
            _deck = new Deck(32);
            for (var idx = 0; idx < _modelDeck.Size(); idx += 1)
                _deck.AddCard(_modelDeck.GetDeck()[idx]);
            _teams[0].GetPlayer(0).EmptyDeck();
            _teams[0].GetPlayer(1).EmptyDeck();
            _teams[1].GetPlayer(0).EmptyDeck();
            _teams[1].GetPlayer(1).EmptyDeck();
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
                for (var i = 0; i < 3; i += 1)
                {
                    int randCard;
                    if (_deck.Size() == 8 || _deck.Size() == 6 || _deck.Size() == 4 || _deck.Size() == 2)
                        i = 1;
                    randCard = _deck.Size() > 1 ? rand.Next(1, _deck.Size()) : 0;
                    _teams[idxTeam].GetPlayer(idxPlayer).GetDeck().AddCard(_deck.GetDeck()[randCard]);
                    _deck.RemoveCard(_deck.GetDeck()[randCard]);
                }
                idxPlayer += 1;
            }
        }

        private void Broadcast(string message)
        {
            if (message.Equals("DECK"))
            {
                _teams[0].GetPlayer(0).SendDeck();
                _teams[0].GetPlayer(1).SendDeck();
                _teams[1].GetPlayer(0).SendDeck();
                _teams[1].GetPlayer(1).SendDeck();
                return;
            }
            _teams[0].GetPlayer(0).SendMessage(message);
            _teams[0].GetPlayer(1).SendMessage(message);
            _teams[1].GetPlayer(0).SendMessage(message);
            _teams[1].GetPlayer(1).SendMessage(message);
        }

        private void PreBidding() {
            _teams[0].GetPlayer(0).SendMessage("MSG You are " + _teams[0].GetPlayer(0).GetName() + ". Together with your mate "
                                              + _teams[0].GetPlayer(1).GetName() + ", you make " + _teams[0].GetName() + ".");
            _teams[0].GetPlayer(1).SendMessage("MSG You are " + _teams[0].GetPlayer(1).GetName() + ". Together with your mate "
                                              + _teams[0].GetPlayer(0).GetName() + ", you make " + _teams[0].GetName() + ".");
            _teams[1].GetPlayer(0).SendMessage("MSG You are " + _teams[1].GetPlayer(0).GetName() + ". Together with your mate "
                                              + _teams[1].GetPlayer(1).GetName() + ", you make " + _teams[1].GetName() + ".");
            _teams[1].GetPlayer(1).SendMessage("MSG You are " + _teams[1].GetPlayer(1).GetName() + ". Together with your mate "
                                              + _teams[1].GetPlayer(0).GetName() + ", you make " + _teams[1].GetName() + ".");
        }

        private void AnnounceScores()
        {
            Broadcast("MSG " + "=========== NEW ROUND ===========");
            Broadcast("MSG " + _teams[0].GetName() + " (" + _teams[0].GetPlayer(0).GetName() + ", "
                      + _teams[0].GetPlayer(1).GetName() + ") has " + _teams[0].GetScore()
                      + (_teams[0].GetContract() == -1 ? "" : "/" + _teams[0].GetContract()) + " points || "
                      + _teams[1].GetName() + " (" + _teams[1].GetPlayer(0).GetName() + ", "
                      + _teams[1].GetPlayer(1).GetName() + ") has " + _teams[1].GetScore()
                      + (_teams[1].GetContract() == -1 ? "" : "/" + _teams[1].GetContract()) + " points. "
                      + "Trump: " + _trump);
        }

        private int GetTrumpChooser()
        {
            foreach (var player in _allPlayers)
            {
                if (player.IsTrumpChooser())
                    return (player.GetId());
            }
            return (-1);
        }

        private void ComputeScore()
        {
            var leadingPlayerId = _trick.GetLeadingPlayer().GetId();
            foreach (var team in _teams)
            {
                foreach (var player in team.GetPlayers())
                {
                    if (player.GetId() == leadingPlayerId)
                    {
                        team.AddScore(_trick.GetValue());
                        team.SetIsCapot(false);
                    }
                }
            }
            if (_allPlayers[leadingPlayerId].GetDeck().Size() != 0)
                return;
            _teams[leadingPlayerId % 2].AddScore(10);
            Broadcast("MSG " + _teams[leadingPlayerId % 2].GetName() + " won the last trick and got " + 10 + " bonus points!");
            
        }

        private void ComputeWinners()
        {
            Broadcast("MSG Calculating score...");
            var takers = _teams[0].GetContract() == -1 ? _teams[1] : _teams[0];
            var defense = _teams[0].GetContract() == -1 ? _teams[0] : _teams[1];
            if (takers.HasWon(defense.GetScore()))
            {
                if (_multiplier == 1)
                {
                    takers.SetScore(takers.GetScore() + (takers.GetContract() == 160 && defense.IsCapot() ? 250 : takers.GetContract()));
                    takers.SetScore(takers.GetScore() * _multiplier);
                }
                else
                {
                    takers.SetScore(takers.GetScore() + defense.GetScore() + (takers.GetContract() == 160 && defense.IsCapot() ? 250 : takers.GetContract()) - 2);
                    takers.SetScore(takers.GetScore() * _multiplier);
                    defense.SetScore(0);
                }
                Broadcast("MSG " + takers.GetName() + " (takers) has " + takers.GetScore() + " points and "
                        + defense.GetName() + " has " + defense.GetScore() + ".");
                Broadcast("MSG Takers completed their contract! Congratulations " + takers.GetName() + "!");
            }
            else
            {
                defense.SetScore(defense.GetScore() + takers.GetScore() + (takers.GetContract() == 160 && defense.IsCapot() ? 250 : takers.GetContract()) - 2);
                defense.SetScore(defense.GetScore() * _multiplier);
                takers.SetScore(0);
                if (takers.HasBelote())
                    takers.SetScore(takers.GetScore() + 20);
                Broadcast("MSG " + takers.GetName() + " has " + takers.GetScore() + " points and "
                          + defense.GetName() + " has " + defense.GetScore() + ".");
                Broadcast("MSG Defense took down the contract! Congratulations " + defense.GetName() + "!");
            }
        }

        private void CheckDeclarations(Player player)
        {
            var toAdd = 0;
            if (player.GetDeck().Size() != 8)
                return;
            player.CheckBelote(_trump);
            if ((toAdd += player.HasAllFour()) != 0)
                Broadcast("MSG " + player.GetName() + " has an all-four!");
            _teams[player.GetId() % 2].AddScore(toAdd);
            if (toAdd != 0)
                Broadcast("MSG " + _teams[player.GetId() % 2].GetName() + " has now "
                          + _teams[player.GetId() % 2].GetScore() + " points.");
        }
        
        private void HandleBelote(Player player, Card card)
        {
            if (player.HasBelote() && card.IsBeloteCard(_trump))
            {
                if (player.GetBeloteCards() == 2)
                {
                    player.SetBeloteCards((short)(player.GetBeloteCards() - 1));
                    Broadcast("MSG [" + player.GetName() + "] BELOTE!");
                }
                else if (player.GetBeloteCards() == 1)
                {
                    player.SetBeloteCards((short)(player.GetBeloteCards() - 1));
                    Broadcast("MSG [" + player.GetName() + "] RE-BELOTE!");
                    _teams[player.GetId() % 2].AddScore(20);
                    _teams[player.GetId() % 2].SetHasBelote(true);
                }
            }
        }
        
        private void StartPlaying()
        {
            var player = _allPlayers[GetTrumpChooser()];
            while (_allPlayers[0].GetDeck().Size() != 0)
            {
                AnnounceScores();
                var isFirstTry = true;
                while (_trick.Size() != 4)
                {
                    if (isFirstTry)
                    {
                        player.SendDeck();
                        Broadcast("MSG " + player.GetName() + "'s turn...");
                        CheckDeclarations(player);
                    }
                    player.SendMessage("PLAY");
                    isFirstTry = false;
                    var msg = player.GetNextMessage().Split();
                    if (msg.Length == 2 && msg[0].Equals("PLAY") &&
                        int.TryParse(msg[1], out var cardId))
                    {
                        if (player.PutCard(_trick, cardId, _trump))
                        {
                            Broadcast("MSG " + player.GetName() + " put a "
                                      + _modelDeck.GetDeck()[cardId].GetName() + " of " +
                                      _modelDeck.GetDeck()[cardId].GetSuit()
                                      + ". " + _trick.GetLeadingPlayer().GetName() + " is leading this turn.");
                            HandleBelote(player, _modelDeck.GetDeck()[cardId]);
                            player.SendMessage("PLAY OK");
                            _teams[player.GetId() % 2].AddATrick();
                            isFirstTry = true;
                            player = _allPlayers[(player.GetId() == 3 ? 0 : player.GetId() + 1)];
                        }
                        else
                            player.SendMessage("PLAY KO");
                    }
                    else
                        player.SendMessage("PLAY KO");
                }
                player = _allPlayers[_trick.GetLeadingPlayer().GetId()];
                ComputeScore();
                _trick.ResetDeck();
            }
            ComputeWinners();
            Broadcast("END");
        }

        private bool RecursiveBidding(int idPlayer, int maxIterations)
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
                    if (msg.Equals("BID N"))
                        break;
                    if (msg.Equals("BID COINCHE"))
                    {
                        if (_multiplier >= 2 || (_teams[0].GetContract() < 0 && _teams[1].GetContract() < 0)
                            || _teams[player.GetId() % 2].GetContract() != -1)
                        {
                            player.SendMessage("BID KO");
                            continue;
                        }
                        Broadcast("MSG " + player.GetName() + " coinched the contract!");
                        _multiplier = 2;
                        iterations = 0;
                        break;
                    }
                    if (msg.Equals("BID SURCOINCHE"))
                    {
                        if (_multiplier != 2 || _teams[player.GetId() % 2].GetContract() == -1)
                        {
                            player.SendMessage("BID KO");
                            return false;
                        }
                        Broadcast("MSG " + player.GetName() + " surcoinched the contract!");
                        _multiplier = 4;
                        break;
                    }
                    if (_multiplier == 2)
                    {
                        player.SendMessage("BID KO");
                        continue;
                    }
                    var msgTab = msg.Split();
                    if (!msgTab[0].Equals("BID") || !msgTab[1].Equals("Y") || msgTab.Length != 4)
                    {
                        player.SendMessage("BID KO");
                        continue;
                    }
                    if (!int.TryParse(msgTab[2], out var contract))
                    {
                        player.SendMessage("BID KO");
                        continue;
                    }
                    var suit = msgTab[3];
                    if (contract <= _teams[0].GetContract() || contract <= _teams[1].GetContract()
                        || contract < 80 || contract > 160 || contract % 10 != 0)
                    {
                        player.SendMessage("BID KO");
                        continue;
                    }
                    if (!(suit.ToUpper().Equals("HEARTS") ||
                          suit.ToUpper().Equals("DIAMONDS") ||
                          suit.ToUpper().Equals("CLUBS") ||
                          suit.ToUpper().Equals("SPADES")))
                    {
                        player.SendMessage("BID KO");
                        continue;
                    }
                    SetTrump((Suit)Enum.Parse(typeof(Suit), suit.ToUpper()));
                    _teams[(idPlayer == 0 || idPlayer == 2) ? 0 : 1].SetContract(contract);
                    _teams[(idPlayer == 0 || idPlayer == 2) ? 1 : 0].SetContract(-1);
                    Broadcast("MSG " + player.GetName() + " from " + _teams[(idPlayer == 0 || idPlayer == 2) ? 0 : 1].GetName()
                              + " bid " + contract + " on " + suit.ToUpper());
                    if (!RecursiveBidding(idPlayer + 1, 3))
                        _allPlayers[idPlayer].SetTrumpChooser(true);
                    return true;
                }
                ++idPlayer;
                ++iterations;
            }
            return false;
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
            foreach (var team in _teams)
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
            _modelDeck.Generate32CardsDeck();
            DrawCards();
            PreBidding();
            StartBidding();
            StartPlaying();
        }
    }
}
