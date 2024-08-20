using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PassSync.Storage {
    /// <summary>
    /// Stores everything about a password
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
    public record Password(string Name, string Username, string Url, bool IsRandom, bool UpperCase, bool LowerCase, bool Numbers, bool Underscore, bool Special, string Exclude, int Length) {
        private const string UpperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
        private const string NumberChars = "0123456789";
        private const string UnderscoreChar = "_";
        private const string SpecialChars = """!"#$%&'()*+,-./:;<=>?@[\]^`{|}~""";

        // parameterless ctor for xml serialization
        private Password() : this("", null, null, false, false, false, false, false, false, null, 0) { }

        /// <summary>
        /// The unique identifier of the password
        /// </summary>
        public Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// The actual password
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Generates a new random password. This will do nothing if <see cref="IsRandom"/> is set to <see langword="false"/>
        /// </summary>
        /// <returns></returns>
        public void GenerateRandom() {
            // Don't do anything if the password is not random
            if (!IsRandom)
                return;

            // Add together and exclude builder
            StringBuilder builder = new();
            if (UpperCase)
                builder.Append(UpperCaseChars);
            if (LowerCase)
                builder.Append(LowerCaseChars);
            if (Numbers)
                builder.Append(NumberChars);
            if (Underscore)
                builder.Append(UnderscoreChar);
            if (Special)
                builder.Append(SpecialChars);
            if (!string.IsNullOrWhiteSpace(Exclude)) {
                foreach (char c in Exclude)
                    builder.Replace(c.ToString(), "");
            }
            string chars = builder.ToString();

            // Can't have a password made from no builder
            if (chars.Length < 1)
                return; // TODO: throw exception or make popup

            // First pass: generate password
            char[] password = new char[Length];
            for (int i = 0; i < Length; i++)
                password[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];

            // Second pass: include one char from each enabled category
            List<int> availableIndeces = Enumerable.Range(0, Length).ToList();
            if (UpperCase)
                IncludeCategory(password, UpperCaseChars, availableIndeces);
            if (LowerCase)
                IncludeCategory(password, LowerCaseChars, availableIndeces);
            if (Numbers)
                IncludeCategory(password, NumberChars, availableIndeces);
            if (Underscore)
                IncludeCategory(password, UnderscoreChar, availableIndeces);
            if (Special)
                IncludeCategory(password, SpecialChars, availableIndeces);

            // Apply password
            Text = new string(password);
        }

        /// <summary>
        /// Makes sure that at least one char from the category is included as long as the exclude does not forbid it
        /// </summary>
        /// <param name="password"> The password to include the char </param>
        /// <param name="category"> The builder of the category to include </param>
        /// <param name="availableIndeces"> A list of indeces that are able to be changed, e.g. not forced </param>
        private void IncludeCategory(char[] password, string category, List<int> availableIndeces) {
            // Apply exclude to category
            StringBuilder builder = new(category);
            if (!string.IsNullOrWhiteSpace(Exclude)) {
                foreach (char c in Exclude)
                    builder.Replace(c.ToString(), "");
            }
            category = builder.ToString();

            // Early return if there are no builder available after exclude
            if (category.Length < 1)
                return;

            // choose available index and remove it from the list
            int index = RandomNumberGenerator.GetInt32(availableIndeces.Count);
            availableIndeces.RemoveAt(index);

            password[index] = category[RandomNumberGenerator.GetInt32(category.Length)];
        }
    }
}
