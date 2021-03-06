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
		private List<EncryptedPasswordEntry> _encryptedPasswordEntries;

		/// <summary>
		/// The RSACryptoServiceProvider used for encrypting and decrypting passwords.
		/// </summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public RSACryptoServiceProvider CryptoServiceProvider { get; set; }

		/// <summary>
		/// A List object containing all the entries in the Library.
		/// </summary>
		public new List<EncryptedPasswordEntry> PasswordEntries { get => _encryptedPasswordEntries; set => _encryptedPasswordEntries = value; }

		/// <summary>
		/// Gets a decrypted byte[] of the given EncryptedPasswordEntry by index.
		/// </summary>
		/// <param name="entryIndex">Index of the EncryptedPasswordEntry</param>
		public byte[] GetPassword(int entryIndex) 
		{
			if (_encryptedPasswordEntries[entryIndex].Password == null) return null;
			return CryptoServiceProvider.Decrypt(_encryptedPasswordEntries[entryIndex].Password, true);
		} 

		/// <summary>
		/// Adds an EncryptedPasswordEntry object to PasswordEntries.
		/// </summary>
		/// <param name="encryptedPasswordEntry"> The EncryptedPasswordEntry to add. </param>
		public void NewEntry(EncryptedPasswordEntry encryptedPasswordEntry)
		{
			PasswordEntries.Add(encryptedPasswordEntry);
			PasswordEntries.Sort(new PasswordEntryComparer());
		}

		/// <summary>
		/// Creates a new EncryptedPasswordEntry object from an existing PasswordEntry object and adds it to PasswordEntries.
		/// </summary>
		/// <param name="passwordEntry"> The PasswordEntry to encrypt and add. </param>
		public new void NewEntry(PasswordEntry passwordEntry)
		{	
			PasswordEntries.Add(new EncryptedPasswordEntry(CryptoServiceProvider, passwordEntry));
			PasswordEntries.Sort(new PasswordEntryComparer());
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
			PasswordEntries.Sort(new PasswordEntryComparer());
		}

		/// <summary>
		/// Sorts the given list of EncryptedPasswordEntries.
		/// </summary>
		/// <param name="list">The list to be sorted.</param>
		protected static List<EncryptedPasswordEntry> Sort(List<EncryptedPasswordEntry> list)
		{
			list.Sort(new PasswordEntryComparer());
			return list;
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
	}
}
