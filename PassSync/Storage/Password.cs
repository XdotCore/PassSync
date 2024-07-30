using System;

namespace PassSync.Storage {
    /// <summary>
    /// Stores everything about the password
    /// </summary>
    /// <param name="Name"> The name of the string for display </param>
    /// <param name="Username"> The username attached to the password </param>
    /// <param name="Url"> The url of the website this password is for </param>
    /// <param name="IsRandom"> Whether the password will be randomly generated </param>
    /// <param name="LowerCase"> Whether the password should include lower case letters </param>
    /// <param name="UpperCase"> Whether the password should include upper case letters </param>
    /// <param name="Numbers"> Whether the password should include numbers </param>
    /// <param name="Underscore"> Whether the password should contain underscores </param>
    /// <param name="Special"> Whether the password should contain special characters </param>
    /// <param name="Exclude"> Characters that should be excluded </param>
    /// <param name="Length"> The length of the randomly generated password </param>
    internal record Password(string Name, string Username, string Url, bool IsRandom, bool UpperCase, bool LowerCase, bool Numbers, bool Underscore, bool Special, string Exclude, int Length) {
        /// <summary>
        /// The unique identifier of the password
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// The actual password
        /// </summary>
        public string Text { get; set; }
    }
}
