using MiniPoker.Enum;
using Xunit;

namespace MiniPoker.Tests
{
    public class CardTests
    {
        [Fact]
        public void CreateCard_ReturnNewCard()
        {
            // act
            var card = new Card(CardRank.Five, CardSuit.Hearts);

            // assert
            Assert.Equal(CardRank.Five, card.Rank);
            Assert.Equal(CardSuit.Hearts, card.Suit);
        }

        [Fact]
        public void CompareCards_UsingEqualsSameSuitSameRank_ReturnTrue()
        {
            // act
            var card1 = new Card(CardRank.Three, CardSuit.Clubs);
            var card2 = new Card(CardRank.Three, CardSuit.Clubs);

            // assert
            Assert.True(card1.Equals(card2));
        }

        [Fact]
        public void CompareCards_UsingEqualsSameSuitDifferentRank_ReturnFalse()
        {
            // act
            var card1 = new Card(CardRank.Nine, CardSuit.Clubs);
            var card2 = new Card(CardRank.Three, CardSuit.Clubs);

            // assert
            Assert.False(card1.Equals(card2));
        }

        [Fact]
        public void CompareCards_UsingEqualsDifferentSuitSameRank_ReturnFalse()
        {
            // act
            var card1 = new Card(CardRank.Three, CardSuit.Diamonds);
            var card2 = new Card(CardRank.Three, CardSuit.Clubs);

            // assert
            Assert.False(card1.Equals(card2));
        }

        [Fact]
        public void CompareCards_UsingEqualOperatorSameSuitSameRank_ReturnTrue()
        {
            // act
            var card1 = new Card(CardRank.Three, CardSuit.Clubs);
            var card2 = new Card(CardRank.Three, CardSuit.Clubs);

            // assert
            Assert.True(card1 == card2);
        }

        [Fact]
        public void CompareCards_UsingEqualOperatorSameSuitDifferentRank_ReturnFalse()
        {
            // act
            var card1 = new Card(CardRank.Nine, CardSuit.Clubs);
            var card2 = new Card(CardRank.Three, CardSuit.Clubs);

            // assert
            Assert.False(card1 == card2);
        }

        [Fact]
        public void CompareCards_UsingEqualOperatorDifferentSuitSameRank_ReturnFalse()
        {
            // act
            var card1 = new Card(CardRank.Three, CardSuit.Diamonds);
            var card2 = new Card(CardRank.Three, CardSuit.Clubs);

            // assert
            Assert.False(card1 == card2);
        }

        [Fact]
        public void CompareCards_UsingNotEqualOperatorSameSuitSameRank_ReturnFalse()
        {
            // act
            var card1 = new Card(CardRank.Three, CardSuit.Clubs);
            var card2 = new Card(CardRank.Three, CardSuit.Clubs);

            // assert
            Assert.False(card1 != card2);
        }

        [Fact]
        public void CompareCards_UsingNotEqualOperatorSameSuitDifferentRank_ReturnTrue()
        {
            // act
            var card1 = new Card(CardRank.Nine, CardSuit.Clubs);
            var card2 = new Card(CardRank.Three, CardSuit.Clubs);

            // assert
            Assert.True(card1 != card2);
        }

        [Fact]
        public void CompareCards_UsingNotEqualOperatorDifferentSuitSameRank_ReturnTrue()
        {
            // act
            var card1 = new Card(CardRank.Three, CardSuit.Diamonds);
            var card2 = new Card(CardRank.Three, CardSuit.Clubs);

            // assert
            Assert.True(card1 != card2);
        }

        [Fact]
        public void CompareCardsHashCode_SameSuitSameRank_SameHashCode()
        {
            // act
            var card1 = new Card(CardRank.Three, CardSuit.Diamonds);
            var card2 = new Card(CardRank.Three, CardSuit.Diamonds);

            // assert
            Assert.True(card1.GetHashCode() == card2.GetHashCode());
        }
    }
}
