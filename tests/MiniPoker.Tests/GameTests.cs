using MiniPoker.Enum;
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
    }
}
