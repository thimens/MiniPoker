using MiniPoker.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MiniPoker
{
    public sealed class Game
    {
        public IEnumerable<Player> GetWinners(IEnumerable<Player> players)
        {
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
            var names = new HashSet<string>();
            foreach (var name in players.Select(p => p.PlayerInfo.Name))
                if (!names.Add(name))
                    throw new ArgumentException($"Two or more players have the same name '{name}'");

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
