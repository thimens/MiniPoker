
# MiniPoker

A short version of the Poker game. One deck (52 cards), five cards per player (up to 10 players), no draws, just deal and check for winners.

This game uses just a subset of the regular poker hands:

* Flush (5 cards of the same suit)
* Three of a kind (3 cards of the same rank)
* One pair (2 cards of the same rank)
* High hand (no other combination)

Obs: 4 cards of the same rank are assumed to be Three of a kind, the fourth card is not used.

In case of ties, the cards of the hand (cards that form one of the hands above) are used in descendent order to choose the winner. 
If still a tie, the remaining cards are used, also in descending order, and if after that there were still tied players, they are the winners.

## Usage

The game starts creating a new `Game`. The `Deal` method returns the players and their cards (2 up to 10 players).  Finally, use the `GetWinners` method to return the winners.

The code:

```c#
// create new game
var game = new Game();

// deal the cards
var players = game.Deal("Jen", "Mike", "Bob", "Alice");

// get the winners
var winners = game.GetWinners(players);
```

## Game Simulator

You can use the [game simulator console app](/tests/MiniPoker.GameSimulator) to simulate up to 30 games at a time.

Obs: Due to a [know issue](https://github.com/dotnet/sdk/issues/9594), I had to reference the `dll` file of MiniPoker library instead of the project (.csproj). So you might need to fix the reference after cloning the project.
