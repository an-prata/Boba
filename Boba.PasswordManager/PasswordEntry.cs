using System;

namespace Boba.PasswordManager
{
    public class PasswordEntry
    {
        public string Name { get; set; }
        public byte[] Password { get; set; }

        /// <summary>
        /// Creates an empty PasswordEntry instance with empty name and password.
        /// </summary>
        public PasswordEntry() 
        {
            Name = "";
            Password = new byte[0];
        }

        /// <summary>
        /// Creates a new PasswordEntry instance with an empty password.
        /// </summary>
        /// <param name="name"> The name for the PasswordEntry. </param>
        public PasswordEntry(string name)
        {
            Name = name;
            Password = new byte[0];
        }

        /// <summary>
        /// Creates a new PasswordEntry instance.
        /// </summary>
        /// <param name="name"> The name for the PasswordEntry. </param>
        /// <param name="password"> The byte[] password for the PasswordEntry </param>
        public PasswordEntry(string name, byte[] password)
        {
            Name = name;
            Password = password;
        }
    }
}
