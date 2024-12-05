using System;
using System.Linq;

namespace Monstercard_FH.Models
{
    /// <summary>
    /// Represents a single round of a battle.
    /// </summary>
    public class Round
    {
        public User Player1 { get; }
        public User Player2 { get; }
        public User? Winner { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Round"/> class.
        /// </summary>
        /// <param name="player1">The first player.</param>
        /// <param name="player2">The second player.</param>
        public Round(User player1, User player2)
        {
            Player1 = player1 ?? throw new ArgumentNullException(nameof(player1), "Player1 cannot be null.");
            Player2 = player2 ?? throw new ArgumentNullException(nameof(player2), "Player2 cannot be null.");
        }

        /// <summary>
        /// Plays the round and determines the winner based on the total damage of their decks.
        /// </summary>
        public void Play()
        {
            int player1TotalDamage = CalculateTotalDamage(Player1);
            int player2TotalDamage = CalculateTotalDamage(Player2);

            DisplayRoundSummary(player1TotalDamage, player2TotalDamage);

            Winner = DetermineWinner(player1TotalDamage, player2TotalDamage);

            if (Winner != null)
            {
                Console.WriteLine($"Round winner: {Winner.UserName}");
            }
            else
            {
                Console.WriteLine("The round ended in a draw.");
            }
        }

        private static int CalculateTotalDamage(User player)
        {
            if (player.Deck == null || !player.Deck.Any())
            {
                throw new InvalidOperationException($"{player.UserName} has an empty or null deck.");
            }

            return player.Deck.Sum(card => card.Damage);
        }

        private static void DisplayRoundSummary(int player1TotalDamage, int player2TotalDamage)
        {
            Console.WriteLine($"Player 1 total damage: {player1TotalDamage}");
            Console.WriteLine($"Player 2 total damage: {player2TotalDamage}");
        }

        private static User? DetermineWinner(int player1TotalDamage, int player2TotalDamage)
        {
            return player1TotalDamage > player2TotalDamage ? Player1 :
                   player2TotalDamage > player1TotalDamage ? Player2 :
                   null; // Draw
        }
    }
}
