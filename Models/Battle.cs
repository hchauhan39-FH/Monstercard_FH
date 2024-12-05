using System;
using System.Collections.Generic;

namespace MonsterCardGame.Models
{
    /// <summary>
    /// Represents a battle between two players, consisting of multiple rounds.
    /// </summary>
    public class Battle
    {
        /// <summary>Gets the first player participating in the battle.</summary>
        public User CompetitorOne { get; private set; }

        /// <summary>Gets the second player participating in the battle.</summary>
        public User CompetitorTwo { get; private set; }

        /// <summary>Gets the winner of the battle, if any. Null if the battle is a draw.</summary>
        public User? OverallWinner { get; private set; }

        /// <summary>Stores the rounds played during the battle.</summary>
        private List<Round> BattleRounds { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Battle"/> class.
        /// </summary>
        /// <param name="competitorOne">The first player in the battle.</param>
        /// <param name="competitorTwo">The second player in the battle.</param>
        public Battle(User competitorOne, User competitorTwo)
        {
            CompetitorOne = competitorOne;
            CompetitorTwo = competitorTwo;
            BattleRounds = new List<Round>();
        }

        /// <summary>
        /// Initiates and conducts the battle between the two players.
        /// </summary>
        public void Start()
        {
            Console.WriteLine($"The battle begins between {CompetitorOne.UserName} and {CompetitorTwo.UserName}!");

            int winsForCompetitorOne = 0;
            int winsForCompetitorTwo = 0;

            // Conducting three rounds of battle as an example.
            const int totalRounds = 3;
            for (int roundNumber = 1; roundNumber <= totalRounds; roundNumber++)
            {
                Console.WriteLine($"Initiating Round {roundNumber}...");
                Round currentRound = new Round(CompetitorOne, CompetitorTwo);
                currentRound.Play();
                BattleRounds.Add(currentRound);

                // Determine round winner and update win counts.
                if (currentRound.Winner == CompetitorOne)
                {
                    winsForCompetitorOne++;
                }
                else if (currentRound.Winner == CompetitorTwo)
                {
                    winsForCompetitorTwo++;
                }
            }

            // Deciding the overall winner based on round results.
            if (winsForCompetitorOne > winsForCompetitorTwo)
            {
                OverallWinner = CompetitorOne;
                Console.WriteLine($"The ultimate winner of the battle is: {CompetitorOne.UserName}!");
            }
            else if (winsForCompetitorTwo > winsForCompetitorOne)
            {
                OverallWinner = CompetitorTwo;
                Console.WriteLine($"The ultimate winner of the battle is: {CompetitorTwo.UserName}!");
            }
            else
            {
                OverallWinner = null;
                Console.WriteLine("The battle concluded in a draw!");
            }
        }
    }
}
