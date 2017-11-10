using Client;
using Shared;
using Xunit;

namespace Test
{
    public class ClientPlayerTests
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
            var cards = new Card[2]
            {
                new Card("7", Suit.DIAMONDS, 0, id1),
                new Card("8", Suit.DIAMONDS, 0, id2)
            };
            foreach (var card in cards)
                player.GetDeck().AddCard(card);
            Assert.Equal(true, player.GetDeck().AddCard(cards[0]));
            Assert.Equal(true, player.GetDeck().AddCard(cards[1]));
            Assert.Equal(cards[0], player.GetDeck().GetCardById(id1));
            Assert.Equal(cards[1], player.GetDeck().GetCardById(id2));
            player.EmptyDeck();
            Assert.Equal(null, player.GetDeck().GetCardById(id1));
            Assert.Equal(null, player.GetDeck().GetCardById(id2));
        }
    }
}
