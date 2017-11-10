using Client;
using Shared;
using Xunit;

namespace Test
{
    public class PlayerTests
    {
        [Fact]
        public void TestPutCard()
        {
            var player = new Player(null);
            var card = new Card("7", Suit.DIAMONDS, 0, 0);
            Assert.Equal(true, player.GetDeck().AddCard(card));
            Assert.Equal(card, player.GetDeck().GetCardById(0));
            player.PutCard(card);
            Assert.Equal(null, player.GetDeck().GetCardById(0));
        }

        [Fact]
        public void TestEmptyDeck()
        {
            var player = new Player(null);
            var card1 = new Card("7", Suit.DIAMONDS, 0, 0);
            var card2 = new Card("8", Suit.DIAMONDS, 0, 1);
            Assert.Equal(true, player.GetDeck().AddCard(card1));
            Assert.Equal(true, player.GetDeck().AddCard(card2));
            Assert.Equal(card1, player.GetDeck().GetCardById(0));
            Assert.Equal(card2, player.GetDeck().GetCardById(1));
            player.EmptyDeck();
            Assert.Equal(null, player.GetDeck().GetCardById(0));
            Assert.Equal(null, player.GetDeck().GetCardById(1));
        }
    }
}
