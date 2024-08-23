using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PassSync.Storage {
    /// <summary>
    /// Manages the storage and retrieval of passwords
    /// </summary>
    public static class PasswordManager {
        private const string PasswordIdsString = "PasswordIds";

        /// <summary>
        /// The list of current passwords
        /// </summary>
        private static Dictionary<Guid, Password> Passwords { get; set; } = [];

        /// <summary>
        /// Makes sure that loading is done before doing any other task
        /// </summary>
        private static Task LoadTask { get; } = Load();
        /// <summary>
        /// Makes sure that saving is done before doing another save or modifying task
        /// </summary>
        private static SemaphoreSlim SaveSem { get; } = new(1, 1);

        /// <summary>
        /// Loads the passwords from file
        /// </summary>
        private async static Task Load() {
            string passwordIdsStr = await SecureStorage.GetAsync(PasswordIdsString);
            if (string.IsNullOrWhiteSpace(passwordIdsStr))
                return;

            string[] idStrs = passwordIdsStr.Split(',');

            foreach (string idStr in idStrs) {
                string xml = await SecureStorage.GetAsync(idStr);
                if (xml == null)
                    continue;

                try {
                    Password password = Password.FromXml(xml);
                    Passwords[password.Id] = password;
                } catch (Exception e) {
                    await Application.Current.MainPage.DisplayAlert("Error loading password.\nClose and make a GitHub issue.", $"{e}\n\n{xml}", "Okay");
                    continue;
                }
            }
        }

        /// <summary>
        /// Saves the passwords to file
        /// </summary>
        private async static Task Save() {
            // SetAsync string can't be null or "" (empty) on windows, source: https://stackoverflow.com/a/77897392
            string passwordIdsStr = string.Join(',', Passwords.Keys);
            if (string.IsNullOrEmpty(passwordIdsStr))
                passwordIdsStr = " ";
            await SecureStorage.SetAsync(PasswordIdsString, passwordIdsStr);

            foreach ((Guid id, Password password) in Passwords)
                await SecureStorage.SetAsync(id.ToString(), password.ToXml());
        }

        /// <summary>
        /// Gets all the passwords as a readonly enumerable
        /// </summary>
        public static async Task<IEnumerable<Password>> GetAll() {
            await LoadTask;
            return Passwords.Values;
        }

        /// <summary>
        /// Removes all existing passwords in place of the given ones
        /// </summary>
        /// <param name="passwords"> The new passwords </param>
        public static async Task ReplaceAll(IEnumerable<Password> passwords) {
            await LoadTask;
            await SaveSem.WaitAsync();

            Passwords.Clear();
            foreach (Password password in passwords)
                Passwords[password.Id] = password;
            await Save();

            SaveSem.Release();
        }

        /// <summary>
        /// Adds the given password
        /// </summary>
        /// <param name="password">The password to add</param>
        public static async Task Add(Password password) {
            await LoadTask;
            await SaveSem.WaitAsync();

            Passwords[password.Id] = password;
            await Save();

            SaveSem.Release();
        }

        /// <summary>
        /// Removes the given password
        /// </summary>
        /// <param name="password">The password to remove</param>
        public static async Task Remove(Password password) {
            await LoadTask;
            await SaveSem.WaitAsync();

            Passwords.Remove(password.Id);
            await Save();

            SaveSem.Release();
        }
    }
}
