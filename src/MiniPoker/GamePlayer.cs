using MiniPoker.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniPoker
{
    internal class GamePlayer
    {
        public GamePlayer(Player player)
        {
            PlayerInfo = player ?? throw new ArgumentNullException(nameof(player));
        }

        public Player PlayerInfo { get; }

        public Hand Hand { get; private set; }

        public IReadOnlyCollection<Card> HandCards { get; private set; }

        public IReadOnlyCollection<Card> TieBreakCards { get; private set; }

        public int HandCardsPoints { get; private set; }

        public int TieBreakCardsPoints { get; private set; }

        public bool ValidateCards() =>
            PlayerInfo.Cards.Count == 5 && !PlayerInfo.Cards.Any(c => c == null);

        public void CalculateHand()
        {
            // sort cards
            var sortedCards = PlayerInfo.Cards.OrderByDescending(c => c.Rank).ToList();

            // check for flush
            if (sortedCards.Select(c => c.Suit).Distinct().Count() == 1)
            {
                Hand = Hand.Flush;
                HandCards = new List<Card>(sortedCards).AsReadOnly();
                TieBreakCards = new List<Card>().AsReadOnly();
                HandCardsPoints = GetCardsPoints(HandCards);
            }
            else
            {
                // group cards and order by count
                var repeatedCards = sortedCards.GroupBy(c => c.Rank).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).First();
                var cardsCount = repeatedCards.Count();

                // check for pairs and three of a kind
                if (cardsCount > 1)
                {
                    Hand = cardsCount == 2 ? Hand.Pair : Hand.ThreeOfAKind;

                    // check for four of a kind 
                    // take only the first three cards and discard the other one
                    HandCards = new List<Card>(cardsCount == 4 ? repeatedCards.Take(3) : repeatedCards).AsReadOnly();
                    HandCardsPoints = GetCardsPoints(HandCards);

                    sortedCards.RemoveAll(c => HandCards.Contains(c));
                    TieBreakCards = new List<Card>(sortedCards).AsReadOnly();
                    TieBreakCardsPoints = GetCardsPoints(TieBreakCards);
                }
                else
                {
                    // high cards
                    Hand = Hand.HighCard;
                    HandCards = new List<Card>(sortedCards).AsReadOnly();
                    TieBreakCards = new List<Card>().AsReadOnly();
                    HandCardsPoints = GetCardsPoints(HandCards);
                }
            }
        }

        private int GetCardsPoints(IEnumerable<Card> cards)
        {
            var multiplier = (int)Math.Pow(10, 2 * (cards.Count() - 1));
            int points = 0;

            foreach (var card in cards)
            {
                points += (int)card.Rank * multiplier;
                multiplier /= 100;
            }

            return points;
        }
    }
}
