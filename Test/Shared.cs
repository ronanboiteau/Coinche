using Shared;
using Xunit;

namespace Test
{
    public class DeckTests
    {
        [Fact]
        public void TestAddCards()
        {
            var deck = new Deck(2);
            Assert.Equal(true, deck.AddCard(new Card("7", Suit.DIAMONDS, 0, 0)));
            Assert.Equal(true, deck.AddCard(new Card("8", Suit.DIAMONDS, 0, 1)));
            Assert.Equal(false, deck.AddCard(new Card("9", Suit.DIAMONDS, 0, 1)));
        }

        [Fact]
        public void TestRemoveCard()
        {
            var deck = new Deck(2);
            Assert.Equal(true, deck.AddCard(new Card("7", Suit.DIAMONDS, 0, 0)));
            Assert.Equal(true, deck.AddCard(new Card("8", Suit.DIAMONDS, 0, 1)));
            Assert.Equal(true, deck.RemoveCard(deck.GetCardById(0)));
            Assert.Equal(null, deck.GetCardById(0));
            Assert.NotEqual(null, deck.GetCardById(1));
        }
    }
}
