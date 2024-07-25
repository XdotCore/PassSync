using System;
using System.Collections.Generic;

namespace PassSync.Storage {
    /// <summary>
    /// Manages the storage and retrieval of passwords
    /// </summary>
    internal class PasswordManager {
        /// <summary>
        /// The list of current passwords
        /// </summary>
        private static List<Password> Passwords { get; set; } = [];

        /// <summary>
        /// The static constructor to load the passwords from file on startup
        /// </summary>
        static PasswordManager() {
            // TODO: retrieve the passwords from file
        }

        /// <summary>
        /// Saves the passwords to file
        /// </summary>
        private static void Save() {
            // TODO: save the passwords to file
        }

        /// <summary>
        /// Gets all the passwords as a readonly enumerable
        /// </summary>
        public static IEnumerable<Password> GetAll() => Passwords.AsReadOnly();

        /// <summary>
        /// Adds a new password
        /// </summary>
        /// <param name="name"> The name of the string for display </param>
        /// <param name="text"> The actual password </param>
        /// <param name="username"> The username attached to the password </param>
        /// <param name="url"> The url of the website this password is for </param>
        /// <param name="site"> The name of the website this password is for </param>
        public static void Add(string name, string text, string username, string url, string site) {
            Passwords.Add(new(Guid.NewGuid(), name, text, username, url, site));
            Save();
        }

        /// <summary>
        /// Removes the given password based on its id
        /// </summary>
        /// <param name="password">The password to remove</param>
        public static void Remove(Password password) {
            Passwords.RemoveAll(p => p.Id == password.Id);
            Save();
        }
    }
}
