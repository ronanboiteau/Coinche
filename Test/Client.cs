using Client;
using Shared;
using Xunit;

namespace Test
{
    public class PlayerTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(4242)]
        public void TestPutCard(int id)
        {
            var player = new Player(null);
            var card = new Card("7", Suit.DIAMONDS, 0, id);
            Assert.Equal(true, player.GetDeck().AddCard(card));
            Assert.Equal(card, player.GetDeck().GetCardById(id));
            player.PutCard(card);
            Assert.Equal(null, player.GetDeck().GetCardById(id));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(123, 2)]
        public void TestEmptyDeck(int id1, int id2)
        {
            var player = new Player(null);
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
    }
}
