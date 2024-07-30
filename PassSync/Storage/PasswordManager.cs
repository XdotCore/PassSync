using System;
using System.Collections.Generic;
using System.Linq;

namespace PassSync.Storage {
    /// <summary>
    /// Manages the storage and retrieval of passwords
    /// </summary>
    public static class PasswordManager {
        /// <summary>
        /// The list of current passwords
        /// </summary>
        private static Dictionary<Guid, Password> Passwords { get; set; } = [];

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
        public static IEnumerable<Password> GetAll() => Passwords.Values;

        /// <summary>
        /// Adds the given password
        /// </summary>
        /// <param name="password">The password to add</param>
        public static void Add(Password password) {
            Passwords.TryAdd(password.Id, password);
            Save();
        }

        /// <summary>
        /// Removes the given password
        /// </summary>
        /// <param name="password">The password to remove</param>
        public static void Remove(Password password) {
            Passwords.Remove(password.Id);
            Save();
        }
    }
}
