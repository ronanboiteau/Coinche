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
            var card1 = new Card("7", Suit.DIAMONDS, 0, id1);
            var card2 = new Card("8", Suit.DIAMONDS, 0, id2);
            Assert.Equal(true, player.GetDeck().AddCard(card1));
            Assert.Equal(true, player.GetDeck().AddCard(card2));
            Assert.Equal(card1, player.GetDeck().GetCardById(id1));
            Assert.Equal(card2, player.GetDeck().GetCardById(id2));
            player.EmptyDeck();
            Assert.Equal(null, player.GetDeck().GetCardById(id1));
            Assert.Equal(null, player.GetDeck().GetCardById(id2));
        }

        [Fact]
        public void TestCheckBeloteTrue()
        {
            var player = new Player(0, "PlayerTest", null);
            var card1 = new Card("Q", Suit.DIAMONDS, 0, 0);
            var card2 = new Card("K", Suit.DIAMONDS, 0, 1);
            player.GetDeck().AddCard(card1);
            player.GetDeck().AddCard(card2);
            player.CheckBelote(Suit.DIAMONDS);
            Assert.Equal(true, player.HasBelote());
            Assert.Equal(2, player.GetBeloteCards());
        }

        [Fact]
        public void TestCheckBeloteFalse()
        {
            var player = new Player(0, "PlayerTest", null);
            var card1 = new Card("Q", Suit.DIAMONDS, 0, 0);
            var card2 = new Card("J", Suit.DIAMONDS, 0, 1);
            player.GetDeck().AddCard(card1);
            player.GetDeck().AddCard(card2);
            player.CheckBelote(Suit.DIAMONDS);
            Assert.Equal(false, player.HasBelote());
            Assert.Equal(0, player.GetBeloteCards());
        }

        [Fact]
        public void TestHasSuit()
        {
            var player = new Player(0, "PlayerTest", null);
            var card1 = new Card("Q", Suit.DIAMONDS, 0, 0);
            var card2 = new Card("Q", Suit.HEARTS, 0, 1);
            player.GetDeck().AddCard(card1);
            player.GetDeck().AddCard(card2);
            Assert.Equal(true, player.HasSuit(Suit.DIAMONDS));
            Assert.Equal(true, player.HasSuit(Suit.HEARTS));
            Assert.Equal(false, player.HasSuit(Suit.CLUBS));
            Assert.Equal(false, player.HasSuit(Suit.SPADES));
        }

        [Fact]
        public void TestHasGreater()
        {
            var player = new Player(0, "PlayerTest", null);
            var card1 = new Card("7", Suit.DIAMONDS, 0, 0);
            var card2 = new Card("8", Suit.DIAMONDS, 0, 1);
            player.GetDeck().AddCard(card1);
            player.GetDeck().AddCard(card2);
            Assert.Equal(true, player.HasGreater(card1, false));
            Assert.Equal(false, player.HasGreater(card2, false));
        }

        [Fact]
        public void TestHasGreaterTrumpMin()
        {
            var player = new Player(0, "PlayerTest", null);
            var card1 = new Card("7", Suit.DIAMONDS, 0, 0);
            var card2 = new Card("8", Suit.DIAMONDS, 0, 1);
            player.GetDeck().AddCard(card1);
            player.GetDeck().AddCard(card2);
            Assert.Equal(true, player.HasGreater(card1, true));
            Assert.Equal(false, player.HasGreater(card2, true));
        }

        [Fact]
        public void TestHasGreaterTrump()
        {
            var player = new Player(0, "PlayerTest", null);
            var card1 = new Card("10", Suit.DIAMONDS, 10, 0);
            var card2 = new Card("9", Suit.DIAMONDS, 14, 1);
            player.GetDeck().AddCard(card1);
            player.GetDeck().AddCard(card2);
            Assert.Equal(true, player.HasGreater(card1, true));
            Assert.Equal(false, player.HasGreater(card2, true));
        }
    }
}
