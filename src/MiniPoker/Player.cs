using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniPoker
{
    public sealed class Player
    {
        public Player(string name, IEnumerable<Card> cards)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name.Trim() : throw new ArgumentException("Name can't be empty or null", nameof(name));
            // create defensive copy of cards            
            Cards = new List<Card>(cards ?? throw new ArgumentNullException(nameof(cards))).AsReadOnly();
        }

        public string Name { get; }

        // need to remove readonly if draw cards enabled in the future
        public IReadOnlyCollection<Card> Cards { get; }
    }
}
