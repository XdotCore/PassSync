using System;

namespace PassSync.Storage {
    /// <summary>
    /// Stores everything about the password
    /// </summary>
    /// <param name="Id"> The id of the password for storage and manipulation </param>
    /// <param name="Name"> The name of the string for display </param>
    /// <param name="Text"> The actual password </param>
    /// <param name="Username"> The username attached to the password </param>
    /// <param name="Url"> The url of the website this password is for </param>
    internal record Password(Guid Id, string Name, string Text, string Username, string Url) {

    }
}
