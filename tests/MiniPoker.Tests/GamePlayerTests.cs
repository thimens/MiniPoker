using MiniPoker.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MiniPoker.Tests
{
    public class GamePlayerTests
    {
        [Fact]
        public void CreateGamePlayer_PlayerNotNull_ReturnsNewPlayer()
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
            var gamePlayer = new GamePlayer(player);

            // assert
            Assert.NotNull(gamePlayer);
            Assert.Equal(Hand.None, gamePlayer.Hand);
        }

        [Fact]
        public void CreateGamePlayer_NullPlayer_ThrowsException()
        {
            // assert
            Assert.Throws<ArgumentNullException>("player", () => new GamePlayer(null));
        }

        [Fact]
        public void ValidateCards_FiveCards_ReturnsTrue()
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
            var gamePlayer = new GamePlayer(player);

            // assert
            Assert.True(gamePlayer.ValidateCards());
        }

        [Fact]
        public void ValidateCards_FourCards_ReturnsFalse()
        {
            // arrange
            var cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.Ace, CardSuit.Hearts),
                new Card(CardRank.Ace, CardSuit.Clubs),
                new Card(CardRank.Ace, CardSuit.Diamonds)
            };
            var player = new GamePlayer(new Player("Player 1", cards));

            // assert
            Assert.False(player.ValidateCards());
        }

        [Fact]
        public void ValidateCards_FiveCardsOneNull_ReturnsFalse()
        {
            // arrange
            var cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.Ace, CardSuit.Hearts),
                new Card(CardRank.Ace, CardSuit.Clubs),
                new Card(CardRank.Ace, CardSuit.Diamonds),
                null
            };
            var player = new GamePlayer(new Player("Player 1", cards));

            // assert
            Assert.False(player.ValidateCards());
        }

        [Theory]
        [MemberData(nameof(CalculateHandData))]
        public void CalculateHand(IEnumerable<Card> cards, int hand, int handCardsCount, int handCardsPoints, int tieBreakCardsCount, int tieBreakCardsPoints)
        {
            // arrange            
            var player = new GamePlayer(new Player("Player 1", cards));

            // act
            player.CalculateHand();

            // assert
            Assert.Equal(hand, (int)player.Hand);
            Assert.Equal(handCardsCount, player.HandCards.Count);
            Assert.Equal(handCardsPoints, player.HandCardsPoints);
            Assert.Equal(tieBreakCardsCount, player.TieBreakCards.Count);
            Assert.Equal(tieBreakCardsPoints, player.TieBreakCardsPoints);
        }


        public static IEnumerable<object[]> CalculateHandData =>
            new List<object[]>
            {
                new object[] {
                    new List<Card> {
                        new Card(CardRank.Ace, CardSuit.Spades),
                        new Card(CardRank.Ten, CardSuit.Hearts),
                        new Card(CardRank.Eight, CardSuit.Clubs),
                        new Card(CardRank.Five, CardSuit.Diamonds),
                        new Card(CardRank.King, CardSuit.Diamonds)
                    },
                    (int)Hand.HighCard, 5, 1413100805, 0, 0
                },

                new object[] {
                    new List<Card> {
                        new Card(CardRank.Ace, CardSuit.Spades),
                        new Card(CardRank.Ten, CardSuit.Hearts),
                        new Card(CardRank.Eight, CardSuit.Clubs),
                        new Card(CardRank.Eight, CardSuit.Diamonds),
                        new Card(CardRank.King, CardSuit.Diamonds)
                    },
                    (int)Hand.Pair, 2, 808, 3, 141310
                },

                new object[] {
                    new List<Card> {
                        new Card(CardRank.Five, CardSuit.Spades),
                        new Card(CardRank.Five, CardSuit.Hearts),
                        new Card(CardRank.Five, CardSuit.Clubs),
                        new Card(CardRank.Five, CardSuit.Diamonds),
                        new Card(CardRank.Three, CardSuit.Diamonds)
                    },
                    (int)Hand.ThreeOfAKind, 3, 50505, 2, 503
                },

                new object[] {
                    new List<Card> {
                        new Card(CardRank.Ace, CardSuit.Spades),
                        new Card(CardRank.Ace, CardSuit.Hearts),
                        new Card(CardRank.Three, CardSuit.Clubs),
                        new Card(CardRank.Two, CardSuit.Diamonds),
                        new Card(CardRank.Ace, CardSuit.Clubs)
                    },
                    (int)Hand.ThreeOfAKind, 3, 141414, 2, 302
                },

                new object[] {
                    new List<Card> {
                        new Card(CardRank.Ace, CardSuit.Spades),
                        new Card(CardRank.Five, CardSuit.Spades),
                        new Card(CardRank.Queen, CardSuit.Spades),
                        new Card(CardRank.Two, CardSuit.Spades),
                        new Card(CardRank.Three, CardSuit.Spades)
                    },
                    (int)Hand.Flush, 5, 1412050302, 0, 0
                },


            };


    }
}
