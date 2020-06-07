using MiniPoker.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPoker
{
    public sealed class Card
    {
        public Card(CardRank rank, CardSuit suit)
        {
            Suit = suit;
            Rank = rank;
        }

        public CardRank Rank { get; }

        public CardSuit Suit { get; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Card);
        }

        public bool Equals(Card card)
        {
            // If parameter is null, return false.
            if (Object.ReferenceEquals(card, null))
                return false;

            // Common success case.
            if (Object.ReferenceEquals(this, card))
                return true;

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != card.GetType())
                return false;

            // Return true if the properties match.
            return (Rank == card.Rank) && (Suit == card.Suit);
        }

        public override int GetHashCode()
        {
            return Rank.GetHashCode() * 0x00010000 + Suit.GetHashCode();
        }

        public static bool operator ==(Card leftCard, Card rightCard)
        {
            // Check for null on left side.
            if (leftCard is null)
            {
                if (rightCard is null)
                    return true;

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return leftCard.Equals(rightCard);
        }

        public static bool operator !=(Card leftCard, Card rightCard)
        {
            return !(leftCard == rightCard);
        }
    }
}
