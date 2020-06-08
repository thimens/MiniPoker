using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniPoker
{
    public sealed class Game
    {
        public IReadOnlyCollection<Player> Deal(params string[] playersNames)
        {
            // validate number of players
            ValidateNumberOfPlayers(playersNames?.Length);

            // validate players names
            ValidateNames(playersNames);

            var players = new Dictionary<string, List<Card>>();

            // create new players as a dictonary
            foreach (var name in playersNames)
                players.Add(name, new List<Card>());

            var deck = ShuffleDeck();

            // distribute cards in using round robin logic
            for(int round = 0; round < 5; round++)
                foreach (var player in players)
                    player.Value.Add(deck.Dequeue());

            return players.Select(p => new Player(p.Key, p.Value.OrderByDescending(c => c.Rank))).ToList().AsReadOnly();
        }

        public IEnumerable<Player> GetWinners(IEnumerable<Player> players)
        {
            // validate number of players
            ValidateNumberOfPlayers(players?.Count());

            var gamePlayers = players.Select(p => new GamePlayer(p)).ToList();

            // validate players
            ValidatePlayers(gamePlayers);

            // calculate players score
            foreach (var player in gamePlayers)
                player.CalculateHand();

            // players on game
            var playersOnGame = gamePlayers.GroupBy(p => p.Hand).OrderByDescending(g => g.Key).First();

            var winners = Untie(playersOnGame, playersOnGame.Key);

            return winners.Select(w => w.PlayerInfo);
        }

        private Queue<Card> ShuffleDeck()
        {
            var deck = new List<Card>();
            var shuffleDeck = new Queue<Card>();

            // create deck
            foreach (var suit in Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>())
                foreach (var rank in Enum.GetValues(typeof(CardRank)).Cast<CardRank>())
                    deck.Add(new Card(rank, suit));

            var rand = new Random();

            // remove random card from deck and add it to shuffled deck
            while (deck.Any())
            {
                var index = rand.Next(0, deck.Count - 1);
                shuffleDeck.Enqueue(deck[index]);
                deck.RemoveAt(index);
            }

            return shuffleDeck;
        }

        private void ValidatePlayers(IEnumerable<GamePlayer> players)
        {
            // validate player cards
            foreach (var player in players)
                if (!player.ValidateCards())
                    throw new ArgumentException($"Invalid cards for player '{player.PlayerInfo.Name}'. Players need 5 cards and cards can't be null");

            // check for duplicated cards
            var cards = new HashSet<Card>();
            foreach (var card in players.SelectMany(p => p.PlayerInfo.Cards))
                if (!cards.Add(card))
                    throw new ArgumentException($"Invalid cards. Duplicated cards are not allowed");

            // check for duplicated names
            ValidateNames(players.Select(p => p.PlayerInfo.Name));                
        }

        private void ValidateNames(IEnumerable<string> names)
        {
            var newNames = new HashSet<string>();
            foreach (var name in names)
                if (!newNames.Add(name))
                    throw new ArgumentException($"Two or more players have the same name '{name}'");
                else if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException($"Name can't be empty or null");
        }

        private void ValidateNumberOfPlayers(int? playersCount)
        {
            if (playersCount == null || playersCount < 2 || playersCount > 10)
                throw new ArgumentException("There must be between 2 and 10 players");
        }

        private IEnumerable<GamePlayer> Untie(IEnumerable<GamePlayer> players, Hand hand)
        {
            // check if only one winner
            if (players.Count() == 1)
                return players;

            // tie-break logic
            // 1. group by hand cards points, order descending and get first group
            // 2. if group have one player return group, else repeat first step using tie-break cards points for the previous selected players and get first group
            // obs. flush and high hand only need first step, as them don't have tie-break cards

            var winners = players.GroupBy(p => p.HandCardsPoints).OrderByDescending(p => p.Key).First();

            return (hand == Hand.Flush || hand == Hand.HighCard || winners.Count() == 1) ?
                    winners :
                    winners.GroupBy(p => p.TieBreakCardsPoints).OrderByDescending(p => p.Key).First();
        }                
    }
}
