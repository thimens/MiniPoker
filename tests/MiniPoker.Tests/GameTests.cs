using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MiniPoker.Tests
{
    public class GameTests
    {
        [Fact]
        public void GetWinners_InvalidNullCards_ThrowsException()
        {
            // arrange
            var cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.Ace, CardSuit.Hearts),
                new Card(CardRank.Ace, CardSuit.Clubs),
                new Card(CardRank.Two, CardSuit.Diamonds),
                new Card(CardRank.King, CardSuit.Spades)
            };
            var player1 = new Player("Player 1", cards);

            cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Diamonds),
                new Card(CardRank.Ten, CardSuit.Hearts),
                new Card(CardRank.Nine, CardSuit.Clubs),
                new Card(CardRank.Queen, CardSuit.Diamonds),
                null
            };
            var player2 = new Player("Player 2", cards);
            var players = new List<Player> { player1, player2 };
            var game = new Game();

            // assert
            var exception = Assert.Throws<ArgumentException>(() => game.GetWinners(players));
            Assert.Equal("Invalid cards for player 'Player 2'. Players need 5 cards and cards can't be null", exception.Message);
        }

        [Fact]
        public void GetWinners_InvalidDuplicatedCards_ThrowsException()
        {
            // arrange
            var cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.Ace, CardSuit.Hearts),
                new Card(CardRank.Ace, CardSuit.Clubs),
                new Card(CardRank.Two, CardSuit.Diamonds),
                new Card(CardRank.King, CardSuit.Spades)
            };
            var player1 = new Player("Player 1", cards);

            cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Diamonds),
                new Card(CardRank.Ten, CardSuit.Hearts),
                new Card(CardRank.Nine, CardSuit.Clubs),
                new Card(CardRank.Queen, CardSuit.Diamonds),
                new Card(CardRank.Ace, CardSuit.Spades),
            };
            var player2 = new Player("Player 2", cards);
            var players = new List<Player> { player1, player2 };
            var game = new Game();

            // assert
            var exception = Assert.Throws<ArgumentException>(() => game.GetWinners(players));
            Assert.Equal("Invalid cards. Duplicated cards are not allowed", exception.Message);
        }

        [Fact]
        public void GetWinners_InvalidDuplicatedNames_ThrowsException()
        {
            // arrange
            var cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.Ace, CardSuit.Hearts),
                new Card(CardRank.Ace, CardSuit.Clubs),
                new Card(CardRank.Two, CardSuit.Diamonds),
                new Card(CardRank.King, CardSuit.Spades)
            };
            var player1 = new Player("Player 1", cards);

            cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Diamonds),
                new Card(CardRank.Ten, CardSuit.Hearts),
                new Card(CardRank.Nine, CardSuit.Clubs),
                new Card(CardRank.Queen, CardSuit.Diamonds),
                new Card(CardRank.Three, CardSuit.Spades),
            };
            var player2 = new Player("Player 1", cards);
            var players = new List<Player> { player1, player2 };
            var game = new Game();

            // assert
            var exception = Assert.Throws<ArgumentException>(() => game.GetWinners(players));
            Assert.Equal("Two or more players have the same name 'Player 1'", exception.Message);
        }

        [Fact]
        public void GetWinners_InvalidNullPlayer_ThrowsException()
        {
            // arrange
            var cards = new List<Card> {
                new Card(CardRank.Ace, CardSuit.Spades),
                new Card(CardRank.Ace, CardSuit.Hearts),
                new Card(CardRank.Ace, CardSuit.Clubs),
                new Card(CardRank.Two, CardSuit.Diamonds),
                new Card(CardRank.King, CardSuit.Spades)
            };
            var player1 = new Player("Player 1", cards);
            var players = new List<Player> { player1, null };
            var game = new Game();

            // assert
            Assert.Throws<ArgumentNullException>("player", () => game.GetWinners(players));
        }

        [Fact]
        public void DealCards_FivePlayers_ReturnPlayers()
        {
            // arrange
            var names = new string[] { "Player1", "Player2", "Player3", "Player4", "Player5" };
            var game = new Game();

            // act
            var players = game.Deal(names);

            // assert
            Assert.Equal(5, players.Count);
            Assert.Equal(25, players.SelectMany(p => p.Cards).Count());
        }

        [Fact]
        public void DealCards_NoPlayers_ThrowsException()
        {
            // arrange
            var names = new string[] { };
            var game = new Game();

            // assert
            var exception = Assert.Throws<ArgumentException>("playersNames", () => game.Deal(names));
            Assert.Equal("There must be between 2 and 10 players (Parameter 'playersNames')", exception.Message);
        }

        [Fact]
        public void DealCards_DuplicatedNames_ThrowsException()
        {
            // arrange
            var names = new string[] { "Player1", "Player2", "Player2" };
            var game = new Game();

            // assert
            var exception = Assert.Throws<ArgumentException>(() => game.Deal(names));
            Assert.Equal("Two or more players have the same name 'Player2'", exception.Message);
        }

        [Fact]
        public void DealCards_EmptyName_ThrowsException()
        {
            // arrange
            var names = new string[] { "Player1", "Player2", string.Empty };
            var game = new Game();

            // assert
            var exception = Assert.Throws<ArgumentException>(() => game.Deal(names));
            Assert.Equal("Name can't be empty or null", exception.Message);
        }

        [Theory]
        [MemberData(nameof(GetWinnersData))]
        public void GetWinners(Player player1, Player player2, Player player3, string winnersName)
        {
            // arrange
            var players = new List<Player> { player1, player2, player3 };
            var game = new Game();

            // act
            var winners = game.GetWinners(players);
            var names = string.Join(",", winners.Select(w => w.Name));

            // assert
            Assert.Equal(winnersName, names);
        }

        public static IEnumerable<object[]> GetWinnersData =>
            new List<object[]>
            {
                new object[] { 
                    new Player("Player1", GetCards(Hand.Flush, 1)),
                    new Player("Player2", GetCards(Hand.Flush, 2)),
                    new Player("Player3", GetCards(Hand.ThreeOfAKind, 1)),
                    "Player1,Player2"
                },
                new object[] {
                    new Player("Player1", GetCards(Hand.Flush, 1)),
                    new Player("Player2", GetCards(Hand.Flush, 3)),
                    new Player("Player3", GetCards(Hand.Pair, 2)),
                    "Player1"
                },
                new object[] {
                    new Player("Player2", GetCards(Hand.Flush, 1)),
                    new Player("Player1", GetCards(Hand.Flush, 4)),
                    new Player("Player3", GetCards(Hand.HighCard, 4)),
                    "Player2"
                },
                new object[] {
                    new Player("Player3", GetCards(Hand.ThreeOfAKind, 1)),
                    new Player("Player2", GetCards(Hand.ThreeOfAKind, 2)),
                    new Player("Player1", GetCards(Hand.Pair, 4)),
                    "Player3"
                },
                new object[] {
                    new Player("Player1", GetCards(Hand.ThreeOfAKind, 1)),
                    new Player("Player2", GetCards(Hand.ThreeOfAKind, 3)),
                    new Player("Player3", GetCards(Hand.HighCard, 2)),
                    "Player1"
                },
                new object[] {
                    new Player("Player3", GetCards(Hand.Pair, 1)),
                    new Player("Player2", GetCards(Hand.Pair, 2)),
                    new Player("Player1", GetCards(Hand.HighCard, 4)),
                    "Player3,Player2"
                },
                new object[] {
                    new Player("Player1", GetCards(Hand.Pair, 1)),
                    new Player("Player2", GetCards(Hand.Pair, 3)),
                    new Player("Player3", GetCards(Hand.HighCard, 2)),
                    "Player1"
                },
                new object[] {
                    new Player("Player2", GetCards(Hand.Pair, 1)),
                    new Player("Player3", GetCards(Hand.Pair, 4)),
                    new Player("Player1", GetCards(Hand.HighCard, 1)),
                    "Player2"
                },
                new object[] {
                    new Player("Player1", GetCards(Hand.HighCard, 1)),
                    new Player("Player3", GetCards(Hand.HighCard, 2)),
                    new Player("Player2", GetCards(Hand.HighCard, 3)),
                    "Player1,Player3"
                },
                new object[] {
                    new Player("Player1", GetCards(Hand.HighCard, 1)),
                    new Player("Player2", GetCards(Hand.HighCard, 3)),
                    new Player("Player3", GetCards(Hand.HighCard, 4)),
                    "Player1"
                }
            };

        private static IEnumerable<Card> GetCards(Hand hand, int option)
        {
            return hand switch
            {
                Hand.Flush when option == 1 => new[] {
                                                    new Card(CardRank.Six, CardSuit.Spades),
                                                    new Card(CardRank.Jack, CardSuit.Spades),
                                                    new Card(CardRank.Five, CardSuit.Spades),
                                                    new Card(CardRank.Two, CardSuit.Spades),
                                                    new Card(CardRank.King, CardSuit.Spades)
                                                },
                Hand.Flush when option == 2 => new[] {
                                                    new Card(CardRank.Six, CardSuit.Diamonds),
                                                    new Card(CardRank.Jack, CardSuit.Diamonds),
                                                    new Card(CardRank.Five, CardSuit.Diamonds),
                                                    new Card(CardRank.Two, CardSuit.Diamonds),
                                                    new Card(CardRank.King, CardSuit.Diamonds)
                                                },
                Hand.Flush when option == 3 => new[] {
                                                    new Card(CardRank.Six, CardSuit.Hearts),
                                                    new Card(CardRank.Ten, CardSuit.Hearts),
                                                    new Card(CardRank.Five, CardSuit.Hearts),
                                                    new Card(CardRank.Two, CardSuit.Hearts),
                                                    new Card(CardRank.King, CardSuit.Hearts)
                                                },
                Hand.Flush when option == 4 => new[] {
                                                    new Card(CardRank.Eight, CardSuit.Clubs),
                                                    new Card(CardRank.Three, CardSuit.Clubs),
                                                    new Card(CardRank.Six, CardSuit.Clubs),
                                                    new Card(CardRank.Two, CardSuit.Clubs),
                                                    new Card(CardRank.Seven, CardSuit.Clubs)
                                                },
                Hand.ThreeOfAKind when option == 1 => new[] {
                                                            new Card(CardRank.Ace, CardSuit.Spades),
                                                            new Card(CardRank.Ace, CardSuit.Clubs),
                                                            new Card(CardRank.Ace, CardSuit.Hearts),
                                                            new Card(CardRank.Ace, CardSuit.Diamonds),
                                                            new Card(CardRank.Ten, CardSuit.Hearts)
                                                        },
                Hand.ThreeOfAKind when option == 2 =>   new[] {
                                                            new Card(CardRank.Jack, CardSuit.Spades),
                                                            new Card(CardRank.Jack, CardSuit.Clubs),
                                                            new Card(CardRank.Jack, CardSuit.Hearts),
                                                            new Card(CardRank.Two, CardSuit.Spades),
                                                            new Card(CardRank.King, CardSuit.Clubs)
                                                        },
                Hand.ThreeOfAKind when option == 3 =>   new[] {
                                                            new Card(CardRank.Nine, CardSuit.Spades),
                                                            new Card(CardRank.Nine, CardSuit.Clubs),
                                                            new Card(CardRank.Nine, CardSuit.Hearts),
                                                            new Card(CardRank.Nine, CardSuit.Diamonds),
                                                            new Card(CardRank.Six, CardSuit.Diamonds)
                                                        },
                Hand.Pair when option == 1 =>   new[] {
                                                    new Card(CardRank.Four, CardSuit.Spades),
                                                    new Card(CardRank.Four, CardSuit.Clubs),
                                                    new Card(CardRank.Queen, CardSuit.Hearts),
                                                    new Card(CardRank.Ten, CardSuit.Diamonds),
                                                    new Card(CardRank.Ten, CardSuit.Hearts)
                                                },
                Hand.Pair when option == 2 =>   new[] {
                                                    new Card(CardRank.Four, CardSuit.Hearts),
                                                    new Card(CardRank.Four, CardSuit.Diamonds),
                                                    new Card(CardRank.Queen, CardSuit.Clubs),
                                                    new Card(CardRank.Ten, CardSuit.Clubs),
                                                    new Card(CardRank.Ten, CardSuit.Spades)
                                                },
                Hand.Pair when option == 3 =>   new[] {
                                                    new Card(CardRank.Four, CardSuit.Hearts),
                                                    new Card(CardRank.Four, CardSuit.Diamonds),
                                                    new Card(CardRank.Three, CardSuit.Clubs),
                                                    new Card(CardRank.Ten, CardSuit.Clubs),
                                                    new Card(CardRank.Ten, CardSuit.Spades)
                                                },
                Hand.Pair when option == 4 => new[] {
                                                    new Card(CardRank.Four, CardSuit.Hearts),
                                                    new Card(CardRank.Four, CardSuit.Diamonds),
                                                    new Card(CardRank.Queen, CardSuit.Clubs),
                                                    new Card(CardRank.King, CardSuit.Diamonds),
                                                    new Card(CardRank.Eight, CardSuit.Spades)
                                                },
                Hand.HighCard when option == 1 =>   new[] {
                                                        new Card(CardRank.King, CardSuit.Spades),
                                                        new Card(CardRank.Three, CardSuit.Clubs),
                                                        new Card(CardRank.Queen, CardSuit.Diamonds),
                                                        new Card(CardRank.Six, CardSuit.Diamonds),
                                                        new Card(CardRank.Jack, CardSuit.Hearts)
                                                    },
                Hand.HighCard when option == 2 => new[] {
                                                        new Card(CardRank.King, CardSuit.Diamonds),
                                                        new Card(CardRank.Three, CardSuit.Hearts),
                                                        new Card(CardRank.Queen, CardSuit.Spades),
                                                        new Card(CardRank.Six, CardSuit.Hearts),
                                                        new Card(CardRank.Jack, CardSuit.Clubs)
                                                    },
                Hand.HighCard when option == 3 => new[] {
                                                        new Card(CardRank.King, CardSuit.Hearts),
                                                        new Card(CardRank.Two, CardSuit.Hearts),
                                                        new Card(CardRank.Queen, CardSuit.Clubs),
                                                        new Card(CardRank.Six, CardSuit.Spades),
                                                        new Card(CardRank.Jack, CardSuit.Diamonds)
                                                    },
                Hand.HighCard when option == 4 => new[] {
                                                        new Card(CardRank.Five, CardSuit.Diamonds),
                                                        new Card(CardRank.Two, CardSuit.Diamonds),
                                                        new Card(CardRank.Eight, CardSuit.Spades),
                                                        new Card(CardRank.Three, CardSuit.Hearts),
                                                        new Card(CardRank.Nine, CardSuit.Clubs)
                                                    },
            };
        }
    }
}
