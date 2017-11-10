using Server;
using Shared;
using Xunit;

namespace Test
{
    public class ServerPlayerTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(123, 2)]
        public void TestEmptyDeck(int id1, int id2)
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card("7", Suit.DIAMONDS, 0, id1),
                new Card("8", Suit.DIAMONDS, 0, id2)
            };
            foreach (var card in cards)
                Assert.Equal(true, player.GetDeck().AddCard(card));
            Assert.Equal(cards[0], player.GetDeck().GetCardById(id1));
            Assert.Equal(cards[1], player.GetDeck().GetCardById(id2));
            player.EmptyDeck();
            Assert.Equal(null, player.GetDeck().GetCardById(id1));
            Assert.Equal(null, player.GetDeck().GetCardById(id2));
        }

        [Fact]
        public void TestCheckBeloteTrue()
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card("Q", Suit.DIAMONDS, 0, 0),
                new Card("K", Suit.DIAMONDS, 0, 1)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            player.CheckBelote(Suit.DIAMONDS);
            Assert.Equal(true, player.HasBelote());
            Assert.Equal(2, player.GetBeloteCards());
        }

        [Fact]
        public void TestCheckBeloteFalse()
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card("Q", Suit.DIAMONDS, 0, 0),
                new Card("J", Suit.DIAMONDS, 0, 1)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            player.CheckBelote(Suit.DIAMONDS);
            Assert.Equal(false, player.HasBelote());
            Assert.Equal(0, player.GetBeloteCards());
        }

        [Fact]
        public void TestHasSuit()
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card("Q", Suit.DIAMONDS, 0, 0),
                new Card("Q", Suit.HEARTS, 0, 1)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            Assert.Equal(true, player.HasSuit(Suit.DIAMONDS));
            Assert.Equal(true, player.HasSuit(Suit.HEARTS));
            Assert.Equal(false, player.HasSuit(Suit.CLUBS));
            Assert.Equal(false, player.HasSuit(Suit.SPADES));
        }

        [Fact]
        public void TestHasGreater()
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card("7", Suit.DIAMONDS, 0, 0),
                new Card("8", Suit.DIAMONDS, 0, 1)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            Assert.Equal(true, player.HasGreater(cards[0], false));
            Assert.Equal(false, player.HasGreater(cards[1], false));
        }

        [Fact]
        public void TestHasGreaterTrumpMin()
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card("7", Suit.DIAMONDS, 0, 0),
                new Card("8", Suit.DIAMONDS, 0, 1)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            Assert.Equal(true, player.HasGreater(cards[0], true));
            Assert.Equal(false, player.HasGreater(cards[1], true));
        }

        [Fact]
        public void TestHasGreaterTrump()
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card("10", Suit.DIAMONDS, 10, 0),
                new Card("9", Suit.DIAMONDS, 14, 1)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            Assert.Equal(true, player.HasGreater(cards[0], true));
            Assert.Equal(false, player.HasGreater(cards[1], true));
        }

        [Theory]
        [InlineData("J", 200)]
        [InlineData("9", 150)]
        [InlineData("A", 100)]
        [InlineData("10", 100)]
        [InlineData("K", 100)]
        [InlineData("Q", 100)]
        [InlineData("8", 0)]
        [InlineData("7", 0)]
        public void TestHasAllFour(string cardName, int score)
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card(cardName, Suit.DIAMONDS, 0, 0),
                new Card(cardName, Suit.CLUBS, 0, 0),
                new Card(cardName, Suit.HEARTS, 0, 0),
                new Card(cardName, Suit.SPADES, 0, 0)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            Assert.Equal(score, player.HasAllFour());
        }

        [Fact]
        public void TestHasAllFourJacksAndKings()
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card("J", Suit.DIAMONDS, 0, 0),
                new Card("J", Suit.CLUBS, 0, 0),
                new Card("J", Suit.HEARTS, 0, 0),
                new Card("J", Suit.SPADES, 0, 0),
                new Card("K", Suit.DIAMONDS, 0, 0),
                new Card("K", Suit.CLUBS, 0, 0),
                new Card("K", Suit.HEARTS, 0, 0),
                new Card("K", Suit.SPADES, 0, 0)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            Assert.Equal(200 + 100, player.HasAllFour());
        }

        [Fact]
        public void TestHasAllFourFalse()
        {
            var player = new Player(0, "PlayerTest", null);
            Card[] cards =
            {
                new Card("J", Suit.DIAMONDS, 0, 0),
                new Card("Q", Suit.CLUBS, 0, 0),
                new Card("J", Suit.HEARTS, 0, 0),
                new Card("Q", Suit.SPADES, 0, 0),
                new Card("9", Suit.DIAMONDS, 0, 0),
                new Card("J", Suit.CLUBS, 0, 0),
                new Card("10", Suit.HEARTS, 0, 0),
                new Card("7", Suit.SPADES, 0, 0)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            Assert.Equal(0, player.HasAllFour());
        }
    }

    public class TeamTests
    {
        [Theory]
        [InlineData(80, 90, 72, true)]
        [InlineData(162, 162, 0, true)]
        [InlineData(80, 80, 82, false)]
        [InlineData(80, 80, 80, false)]
        [InlineData(162, 160, 2, false)]
        [InlineData(80, 100, 100, false)]
        public void TestHasWon(int contract, int score, int otherTeamScore, bool shouldWin)
        {
            var team = new Team(new Player(0, "Player1", null),
                                new Player(1, "Player2", null),
                                "TeamTest");
            team.SetContract(contract);
            team.SetScore(score);
            Assert.Equal(shouldWin, team.HasWon(otherTeamScore));
        }
    }

    public class TrickTests
    {
        [Fact]
        public void TestPutCardTrumpTrick()
        {
            var player = new Player(0, "PlayerTest", null);
            const Suit trump = Suit.DIAMONDS;
            Card[] cards =
            {
                new Card("J", Suit.DIAMONDS, 0, 0),
                new Card("9", Suit.DIAMONDS, 0, 1),
                new Card("Q", Suit.CLUBS, 0, 2),
                new Card("J", Suit.CLUBS, 0, 3),
                new Card("Q", Suit.CLUBS, 0, 4),
                new Card("K", Suit.HEARTS, 0, 5),
                new Card("7", Suit.HEARTS, 0, 6),
                new Card("10", Suit.SPADES, 0, 7)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);

            var trick = new Trick();
            var firstCard = new Card("10", Suit.DIAMONDS, 0, 8);
            trick.AddCard(firstCard);
            trick.SetLeadingCard(firstCard);
            Assert.Equal(false, player.PutCard(trick, player.GetDeck().GetCardById(5).GetId(), trump));
            Assert.NotEqual(null, player.GetDeck().GetCardById(5));
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(1).GetId(), trump));
            Assert.Equal(null, player.GetDeck().GetCardById(1));
         }

        [Fact]
        public void TestPutCardNonTrumpTrick()
        {
            var player = new Player(0, "PlayerTest", null);
            const Suit trump = Suit.DIAMONDS;
            Card[] cards =
            {
                new Card("J", Suit.DIAMONDS, 0, 0),
                new Card("9", Suit.DIAMONDS, 0, 1),
                new Card("Q", Suit.CLUBS, 0, 2),
                new Card("J", Suit.CLUBS, 0, 3),
                new Card("Q", Suit.CLUBS, 0, 4),
                new Card("K", Suit.HEARTS, 0, 5),
                new Card("7", Suit.HEARTS, 0, 6),
                new Card("10", Suit.SPADES, 0, 7)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);

            var trick = new Trick();
            var firstCard = new Card("8", Suit.HEARTS, 0, 8);
            trick.AddCard(firstCard);
            trick.SetLeadingCard(firstCard);
            Assert.Equal(false, player.PutCard(trick, player.GetDeck().GetCardById(3).GetId(), trump));
            Assert.Equal(false, player.PutCard(trick, player.GetDeck().GetCardById(0).GetId(), trump));
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(5).GetId(), trump));
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(6).GetId(), trump));
        }

        [Fact]
        public void TestPutCardCuttingNonTrumpTrick()
        {
            var player = new Player(0, "PlayerTest", null);
            const Suit trump = Suit.DIAMONDS;
            Card[] cards =
            {
                new Card("J", Suit.DIAMONDS, 0, 0),
                new Card("9", Suit.DIAMONDS, 0, 1),
                new Card("Q", Suit.CLUBS, 0, 2),
                new Card("J", Suit.CLUBS, 0, 3),
                new Card("Q", Suit.CLUBS, 0, 4),
                new Card("K", Suit.HEARTS, 0, 5),
                new Card("7", Suit.HEARTS, 0, 6),
                new Card("10", Suit.SPADES, 0, 7)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);

            var trick = new Trick();
            var firstCard = new Card("8", Suit.SPADES, 0, 8);
            trick.AddCard(firstCard);
            trick.SetLeadingCard(firstCard);
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(7).GetId(), trump));
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(1).GetId(), trump));
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(0).GetId(), trump));
        }

        [Fact]
        public void TestReset()
        {
            var trick = new Trick();
            trick.AddCard(new Card("J", Suit.DIAMONDS, 1, 0));
            trick.AddCard(new Card("Q", Suit.CLUBS, 10, 1));
            trick.AddCard(new Card("10", Suit.SPADES, 100, 2));
            trick.AddCard(new Card("7", Suit.DIAMONDS, 1000, 3));
            trick.AddValue(42);
            trick.ResetDeck();
            Assert.Equal(0, trick.GetDeck().Count);
            Assert.Equal(0, trick.GetValue());
        }

        [Fact]
        public void TestValue()
        {
            var player = new Player(0, "PlayerTest", null);
            const Suit trump = Suit.DIAMONDS;
            Card[] cards =
            {
                new Card("10", Suit.DIAMONDS, 1, 0),
                new Card("A", Suit.DIAMONDS, 10, 1),
                new Card("9", Suit.DIAMONDS, 100, 2),
                new Card("J", Suit.DIAMONDS, 1000, 3)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            var trick = new Trick();
            var firstCard = new Card("8", Suit.SPADES, 0, 8);
            trick.AddCard(firstCard);
            trick.SetLeadingCard(firstCard);
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(0).GetId(), trump));
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(1).GetId(), trump));
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(2).GetId(), trump));
            Assert.Equal(true, player.PutCard(trick, player.GetDeck().GetCardById(3).GetId(), trump));
            Assert.Equal(1111, trick.GetValue());
        }
    }
}
