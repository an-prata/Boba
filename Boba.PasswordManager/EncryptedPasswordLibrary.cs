// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
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
		public void NewEntry(EncryptedPasswordEntry encryptedPasswordEntry)
		{
			PasswordEntries.Add(encryptedPasswordEntry);
			PasswordEntries.Sort(ComparePasswordEntryAlphabeticaly);
		}

		/// <summary>
		/// Creates a new EncryptedPasswordEntry object from an existing PasswordEntry object and adds it to PasswordEntries.
		/// </summary>
		/// <param name="passwordEntry"> The PasswordEntry to encrypt and add. </param>
		public new void NewEntry(PasswordEntry passwordEntry)
		{	
			PasswordEntries.Add(new EncryptedPasswordEntry(CryptoServiceProvider, passwordEntry));
			PasswordEntries.Sort(ComparePasswordEntryAlphabeticaly);
		}

		/// <summary>
		/// Creates a new EncryptedPasswordEntry object.
		/// </summary>
		/// <param name="password"> Unecrypted byte[] password for the new EncryptedPasswordEntry. </param>
		/// <param name="application"> Name of the application the new EncryptedPasswordEntry should apply to. </param>
		/// <param name="username"> Username for the application. </param>
		public new void NewEntry(byte[] password, string application = "", string username = "")
		{
			PasswordEntries.Add(new EncryptedPasswordEntry(CryptoServiceProvider, password, application, username));
			PasswordEntries.Sort(ComparePasswordEntryAlphabeticaly);
		}

		/// <summary>
		/// Creates a new EncryptedPasswordLibrary instance with empty properties.
		/// </summary>
		public EncryptedPasswordLibrary() { }

		/// <summary>
		/// Creates an instance of EncryptedPasswordLibrary.
		/// </summary>
		/// <param name="cryptoServiceProvider"> An RSACryptoServiceProvider to encrypt with. </param>
		/// <param name="name"> Name for the new Encrypted PasswordLibrary. </param>
		/// <param name="passwordEntries"> A list of EncryptedPasswordEntries. </param>
		public EncryptedPasswordLibrary(RSACryptoServiceProvider cryptoServiceProvider, string name, List<EncryptedPasswordEntry> passwordEntries)
		{
			CryptoServiceProvider = cryptoServiceProvider;
			Name = name;
			PasswordEntries = passwordEntries;
		}

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
