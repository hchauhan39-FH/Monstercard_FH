using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security;
using MTC_Game.Exceptions;

namespace MTC_Game
{
    public sealed class User
    {
        private static readonly Dictionary<string, User> RegisteredUsers = new();
        private User() { }

        public string Username { get; private set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? SessionToken { get; set; }

        private static readonly List<string> CardNames = new()
        {
            "Goblins", "Dragons", "Wizard", "Knights", "Orcs", "Kraken", "Chauhan", "FireElves", "Lion", "Rocklee",
            "Tetsu", "Amaterasu", "Bankai", "Raijin", "HarmanKing", "Susanoo"
        };

        public void Save(string token)
        {
            var authResult = Token.Authenticate(token);
            if (!authResult.Success) throw new AuthenticationException("Not authenticated.");
            if (authResult.User!.Username != Username) throw new SecurityException("Trying to change another user's data.");
            // save logic
        }

        public static void Create(string username, string password, string fullName = "", string email = "")
        {
            if (RegisteredUsers.ContainsKey(username)) throw new UserException("Username already exists.");

            var newUser = new User
            {
                Username = username,
                FullName = fullName,
                Email = email
            };
            RegisteredUsers.Add(username, newUser);
        }

        public static (bool Success, string Token) Logon(string username, string password)
        {
            if (!RegisteredUsers.TryGetValue(username, out var user)) return (false, string.Empty);

            var token = Token._CreateTokenFor(user);
            user.SessionToken = token;
            return (true, token);
        }

        public static bool Exists(string username) => RegisteredUsers.ContainsKey(username);

        public static User? Get(string username) => RegisteredUsers.TryGetValue(username, out var user) ? user : null;

        public static IEnumerable<User> GetAllUsers() => RegisteredUsers.Values;
    }
}
