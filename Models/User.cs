using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using Monstercard_FH.MTC_Game.Exceptions;
using Monstercard_FH.MTC_Game;

namespace Monstercard_FH.Models
{
    /// <summary>This class represents a user in the system.</summary>
    public sealed class User
    {
        private static readonly Dictionary<string, User> _Users = new();

        // Card names for random generation
        private static readonly List<string> CardNames = new()
        {
            "Goblins", "Dragons", "Wizzard", "Knights", "Orks", "Kraken", "FireElves", "Lion", "Harman", "Rocklee",
            "Tetsu", "Amaterasu", "Bankai", "Raijin", "Susanoo", "Chauhan"
        };

        #region Public Properties

        public string UserName { get; private set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public int Coins { get; set; } = 20;
        public List<Card> Stack { get; set; } = new();
        public List<Card> Deck { get; set; } = new();
        public string? SessionToken { get; set; } = null;

        #endregion

        #region Constructors

        private User() { }

        #endregion

        #region Public Methods

        /// <summary>Saves changes to the user object after authenticating with a session token.</summary>
        /// <param name="token">Token used to authenticate the user.</param>
        /// <exception cref="SecurityException">Thrown if the user tries to modify another user's data.</exception>
        /// <exception cref="AuthenticationException">Thrown if the token is invalid.</exception>
        public void Save(string token)
        {
            var auth = Token.Authenticate(token);
            if (!auth.Success)
            {
                throw new AuthenticationException("Not authenticated.");
            }

            if (auth.User!.UserName != UserName)
            {
                throw new SecurityException("Trying to change another user's data.");
            }

            // Save data logic here (e.g., database save)
        }

        /// <summary>Allows the user to purchase a package of 5 random cards, deducting 5 coins.</summary>
        public void AddPackage()
        {
            if (Coins < 5)
            {
                throw new InvalidOperationException("Not enough coins to buy a package. You need at least 5 coins.");
            }

            Coins -= 5;
            var randNames = new Random();

            for (int i = 0; i < 5; i++)
            {
                string cardName = CardNames[randNames.Next(CardNames.Count)];
                Stack.Add(CreateCard(cardName));
            }
        }

        /// <summary>Chooses the top 4 cards based on damage and element type for the user's deck.</summary>
        public void ChooseDeck()
        {
            Deck.Clear();
            Deck = Stack
                .OrderByDescending(card => card.Damage)
                .ThenBy(card => card.CardElementType == ElementType.Water ? 1 :
                    card.CardElementType == ElementType.Fire ? 2 : 3)
                .ThenByDescending(card => card.GetType().Name)
                .Take(4)
                .ToList();
        }

        #endregion

        #region Private Methods

        /// <summary>Creates a new card based on its name.</summary>
        /// <param name="cardName">The name of the card to create.</param>
        /// <returns>A new instance of a Card (either MonsterCard, SpellCard, or NormalCard).</returns>
        private static Card CreateCard(string cardName)
        {
            return cardName switch
            {
                "Dragons" or "FireElves" or "Kraken" or "Lion" => new MonsterCard(cardName),
                "Wizzard" or "Tetsu" or "Amaterasu" or "Bankai" => new SpellCard(cardName),
                _ => new NormalCard(cardName)
            };
        }

        #endregion

        #region Public Static Methods

        /// <summary>Creates a new user with the specified username, password, and optional details.</summary>
        /// <param name="userName">The desired username for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="fullName">The full name of the user (optional).</param>
        /// <param name="eMail">The email address of the user (optional).</param>
        /// <exception cref="UserException">Thrown if the username already exists.</exception>
        public static void Create(string userName, string password, string fullName = "", string eMail = "")
        {
            if (_Users.ContainsKey(userName))
            {
                throw new UserException("Username already exists.");
            }

            var user = new User
            {
                UserName = userName,
                FullName = fullName,
                EMail = eMail
            };

            _Users.Add(userName, user);
        }

        /// <summary>Performs a logon operation for a user and returns an authentication token.</summary>
        /// <param name="userName">The username for the login.</param>
        /// <param name="password">The password for the login.</param>
        /// <returns>A tuple containing the success status and an authentication token if successful.</returns>
        public static (bool Success, string Token) Logon(string userName, string password)
        {
            if (_Users.ContainsKey(userName))
            {
                string token = Token._CreateTokenFor(_Users[userName]);
                _Users[userName].SessionToken = token;
                return (true, token);
            }

            return (false, string.Empty);
        }

        public static bool Exists(string userName) => _Users.ContainsKey(userName);

        public static User? Get(string userName) => _Users.TryGetValue(userName, out var user) ? user : null;

        public static IEnumerable<User> GetAllUsers() => _Users.Values;

        #endregion
    }
}
