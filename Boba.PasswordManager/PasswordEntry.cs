using System;

namespace Boba.PasswordManager
{
    public class PasswordEntry
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public PasswordEntry() { }
        public PasswordEntry(string name) => Name = name;
        public PasswordEntry(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
