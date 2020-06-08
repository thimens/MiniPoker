using System;
using System.Linq;
using System.Text;

namespace MiniPoker.GameSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to MiniPoker Simulator!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            var totalGames = 0;
            while (totalGames < 1 || totalGames > 30)
            {
                Console.WriteLine("How many games would you like to simulate? (1-30):");
                var line = Console.ReadLine();
                if (int.TryParse(line, out int result))
                    totalGames = result;
            }

            var game = new Game();
            for (int round = 0; round < totalGames; round++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(new String('-', 15));
                Console.WriteLine();
                Console.WriteLine("Staring new game..");
                Console.WriteLine("Dealing cards..");

                var players = game.Deal("Jen", "Mike", "Bob", "Alice");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Players:");
                Console.ForegroundColor = ConsoleColor.White;                

                foreach (var player in players)
                    Console.WriteLine($"{player.Name} \t {string.Join(" ", player.Cards.Select(c => c.ToString().PadLeft(3)))}");

                var winners = game.GetWinners(players);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Winner:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(string.Join(" & ", winners.Select(w => w.Name)));
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Hit any key to exit");
            Console.ReadKey();
        }
    }
}
