using System;
using System.Collections.Generic;
using Xunit;

namespace MiniPoker.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void CreatePlayer_NameAndCards_ReturnNewPlayer()
        {
            // arrange
            var cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.Ace, CardSuit.Hearts),
                new Card(CardRank.Ace, CardSuit.Clubs),
                new Card(CardRank.Ace, CardSuit.Diamonds),
                new Card(CardRank.King, CardSuit.Spades)
            };
            var player = new Player("Player 1", cards);

            // assert
            Assert.Equal("Player 1", player.Name);
            Assert.Equal(cards.Count, player.Cards.Count);
        }

        [Fact]
        public void CreatePlayer_NoNameAndCards_ThrowException()
        {
            // arrange
            var cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.Ace, CardSuit.Hearts),
                new Card(CardRank.Ace, CardSuit.Clubs),
                new Card(CardRank.Ace, CardSuit.Diamonds),
                new Card(CardRank.King, CardSuit.Spades)
            };

            // assert
            var exception = Assert.Throws<ArgumentException>("name", () => new Player("", cards));
            Assert.Equal("Name can't be empty or null (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void CreatePlayer_NameAndNullCards_ThrowException()
        {
            // assert
            Assert.Throws<ArgumentNullException>("cards", () => new Player("Player", null));
        }
    }
}
