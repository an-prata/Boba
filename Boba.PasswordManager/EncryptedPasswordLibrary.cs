using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Boba.PasswordManager
{
	public class EncryptedPasswordLibrary : PasswordLibrary
	{
		private const bool UseOAEPPadding = true;
		bool _disposed = false;

		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public RSACryptoServiceProvider CryptoServiceProvider { get; set; }
		public new List<EncryptedPasswordEntry> PasswordEntries { get; set; }

		/// <summary>
		/// Adds an EncryptedPasswordEntry object to PasswordEntries.
		/// </summary>
		/// <param name="encryptedPasswordEntry"> The EncryptedPasswordEntry to add. </param>
		public void NewEntry(EncryptedPasswordEntry encryptedPasswordEntry) => PasswordEntries.Add(encryptedPasswordEntry);

		/// <summary>
		/// Creates a new EncryptedPasswordEntry object from an existing PasswordEntry object and adds it to PasswordEntries.
		/// </summary>
		/// <param name="passwordEntry"> The PasswordEntry to encrypt and add. </param>
		public new void NewEntry(PasswordEntry passwordEntry) => PasswordEntries.Add(new EncryptedPasswordEntry(CryptoServiceProvider, passwordEntry));
		
		/// <summary>
		/// Creates a new EncryptedPasswordEntry object.
		/// </summary>
		/// <param name="name"> Name of the new EncryptedPasswordEntry. </param>
		/// <param name="password"> Unecrypted byte[] password for the new EncryptedPasswordEntry. </param>
		public new void NewEntry(string name, byte[] password) => PasswordEntries.Add(new EncryptedPasswordEntry(CryptoServiceProvider, name, password));

		/// <summary>
		/// Creates a new EncreyptedPaswordLibrary from a List<EncryptedPasswordEntry> object and a name.
		/// </summary>
		/// <param name="passwordEntries"> List of EncryptedPasswordEntries. </param>
		/// <param name="name"> Name for new EncryptedPasswordLibrary. </param>
		[JsonConstructor]
		public EncryptedPasswordLibrary(List<EncryptedPasswordEntry> passwordEntries, string name)
		{
			Name = name;
			PasswordEntries = passwordEntries;
		}

		/// <summary>
		/// Creates an instance of EncryptedPasswordLibrary with an empty PasswordEntries.
		/// </summary>
		/// <param name="name"> Name for new EncryptedPasswordLibrary. </param>
		public EncryptedPasswordLibrary(string name) => Name = name;

		/// <summary>
		/// Creates an instance of EncryptedPasswordLibrary.
		/// </summary>
		/// <param name="cryptoServiceProvider"> An RSACryptoServiceProvider to encrypt with. </param>
		/// <param name="name"> Name for the new Encrypted PasswordLibrary. </param>
		public EncryptedPasswordLibrary(RSACryptoServiceProvider cryptoServiceProvider, string name)
		{
			Name = name;
			CryptoServiceProvider = cryptoServiceProvider;
			PasswordEntries = new List<EncryptedPasswordEntry>();
		}

		protected virtual new void Dispose(bool disposing)
		{
			if (_disposed) return;
			if (disposing) PasswordEntries.Clear();
			_disposed = true;
		}

		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~EncryptedPasswordLibrary()
		{
			Dispose(disposing: false);
		}
	}
}
