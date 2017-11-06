﻿using System;

namespace Server
{
    public class Game
    {        
        private Team[] teams = new Team[2];
        private Deck modelDeck = new Deck(32);
        private Deck _deck;
        
        public Game(Team teamOne, Team teamTwo)
        {
            teams[0] = teamOne;
            teams[1] = teamTwo;
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

//        private Boolean         recursiveBidding(int idPlayer, int max_iterations) {
//            String              msg;
//            int iterations = 0;
//            while (iterations < max_iterations) {
//                if (idPlayer >= 4)
//                    idPlayer = idPlayer % 4;
//                ServerPlayer player = this.players.get(idPlayer);
//                player.sendMessage("BID");
//                broadcastToOthers(player, "MSG " + player.getName() + " is making a decision...");
//                while (true) {
//                    while (player.peekMsg() == null)
//                        Thread.sleep(500);
//                    System.out.println("[SG] Received client answer!");
//                    msg = player.popMsg();
//                    System.out.println(msg);
//                    if (msg.equals("BID N")) {
//                        break;
//                    }
//                    if (!msg.startsWith("BID Y ")) {
//                        player.sendMessage("BID KO");
//                        continue;
//                    }
//                    Scanner     scanner = new Scanner(msg);
//                    scanner.next();
//                    scanner.next();
//                    if (!scanner.hasNextInt()) {
//                        player.sendMessage("BID KO");
//                        continue;
//                    }
//                    int         contract = scanner.nextInt();
//                    if (!scanner.hasNext()) {
//                        player.sendMessage("BID KO");
//                        continue;
//                    }
//                    String      suit = scanner.next();
//                    System.out.println(this.teams.get(0).getContract() + " - " + this.teams.get(0).getContract());
//                    if (contract <= this.teams.get(0).getContract()
//                            || contract <= this.teams.get(1).getContract()
//                            || contract < 80
//                            || contract % 10 != 0) {
//                        player.sendMessage("BID KO");
//                        continue;
//                    }
//                    if (!(suit.equalsIgnoreCase("HEARTS")
//                            || suit.equalsIgnoreCase("DIAMONDS")
//                            || suit.equalsIgnoreCase("CLUBS")
//                            || suit.equalsIgnoreCase("SPADES"))) {
//                        player.sendMessage("BID KO");
//                        continue;
//                    }
//                    this.trump = Suit.valueOf(suit.toUpperCase());
//                    this.teams.get((idPlayer == 0 || idPlayer == 2) ? 0 : 1).setContract(contract);
//                    this.teams.get((idPlayer == 0 || idPlayer == 2) ? 1 : 0).setContract(-1);
//                    broadcast("MSG " + player.getName() + " from " + this.teams.get((idPlayer == 0 || idPlayer == 2) ? 0 : 1).getName()
//                    + " bid " + contract + " on " + suit.toUpperCase());
//                    if (!recursiveBidding(idPlayer + 1, 3))
//                        this.players.get(idPlayer).setTrumpChooser(true);
//                    return (true);
//                }
//                ++idPlayer;
//                ++iterations;
//            }
//            return (false);
//        }

//        private void StartBidding()
//        {
//            var rand = new Random();
//            int idPlayer;
//            var playerName = "Player1";
//            var teamName = "Team2";
//            var contract = 0;
//            idPlayer = rand.Next(0, 4);
//            var contractTaken = false;
//            while (!contractTaken)
//            {
//                DrawCards();
//                BroadcastDecks();
//                if (RecursiveBidding(idPlayer, 4))
//                    contractTaken = true;
//                if (!contractTaken)
//                    Broadcast("BID RESET");
//            }
//            Broadcast("BID STOP");
//            for (Team team : teams)
//            {
//                for (Player player : team.getPlayers())
//                {
//                    if (player.isTrumpChooser())
//                    {
//                        playerName = player.getName();
//                        teamName = team.getName();
//                        contract = team.getContract();
//                    }
//                }
//            }
//            broadcast("MSG " + playerName + " from " + teamName + " made the final bid! The chosen trump is " + this.trump + ". "
//                    + teamName + "'s contract is " + contract + ".");
//        }
        
        public void StartGame()
        {
            Console.Write("Starting game...\n");
            CreateModelDeck();
            DrawCards();
            PreBidding();
            Broadcast("DECK");
            Broadcast("END");
//            StartBidding();
        }
    }
}