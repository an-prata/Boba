// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boba.PasswordManager;

namespace Boba.PasswordManagerUnitTests
{
	[TestClass]
	public class PasswordEntryComparerUnitTests
	{
		//private readonly string[] Applications = { "google.com", "docs.microsoft.com", "github.com", "stackoverflow.com" };
		//private readonly string[] Usernames = { "jake@gmail.com", "jake", "jakemiester1234", "jakesmith@outlook.com" };
		//private readonly string[] Passwords = { "jakesSuperStrongPassword", "Password12345", "TheBatmanEmoji", "#sYfg@sh&*gskks%$#17291$sg%" };

		private static readonly string[,] SortedCredentials = 
		{
			{ "docs.microsoft.com", "jake", "Password12345" },
			{ "github.com", "jakemiester1234", "TheBatmanEmoji"},
			{ "google.com", "jake@gmail.com", "jakesSuperStrongPassword" },
			{ "stackoverflow.com", "jakesmith@outlook.com", "#sYfg@sh&*gskks%$#17291$sg%"}
		};

		private readonly List<PasswordEntry> SortedPasswordEntries = new()
        {
			new PasswordEntry(Encoding.UTF8.GetBytes(SortedCredentials[0, 2]), SortedCredentials[0, 0], SortedCredentials[0, 1]),
			new PasswordEntry(Encoding.UTF8.GetBytes(SortedCredentials[1, 2]), SortedCredentials[1, 0], SortedCredentials[1, 1]),
			new PasswordEntry(Encoding.UTF8.GetBytes(SortedCredentials[2, 2]), SortedCredentials[2, 0], SortedCredentials[2, 1]),
			new PasswordEntry(Encoding.UTF8.GetBytes(SortedCredentials[3, 2]), SortedCredentials[3, 0], SortedCredentials[3, 1]),
		};

		[TestMethod]
		public void PasswordEntryComparisonCheck()
		{
			Comparer<PasswordEntry> passwordEntryComparer = new PasswordEntryComparer();
			Assert.AreEqual(passwordEntryComparer.Compare(SortedPasswordEntries[0], SortedPasswordEntries[1]), -1);
			Assert.AreEqual(passwordEntryComparer.Compare(SortedPasswordEntries[1], SortedPasswordEntries[0]), 1);
			Assert.AreEqual(passwordEntryComparer.Compare(SortedPasswordEntries[2], SortedPasswordEntries[2]), 0);
		}

		[TestMethod]
		public void EncryptedPasswordEntryComparisonCheck()
		{
			Comparer<PasswordEntry> passwordEntryComparer = new PasswordEntryComparer();
			EncryptedPasswordLibrary encryptedPasswordLibrary = new(new RSACryptoServiceProvider(), "test library", new List<EncryptedPasswordEntry>());
			foreach(PasswordEntry passwordEntry in SortedPasswordEntries) encryptedPasswordLibrary.NewEntry(passwordEntry);
			Assert.AreEqual(passwordEntryComparer.Compare(encryptedPasswordLibrary.PasswordEntries[0], encryptedPasswordLibrary.PasswordEntries[1]), -1);
			Assert.AreEqual(passwordEntryComparer.Compare(encryptedPasswordLibrary.PasswordEntries[1], encryptedPasswordLibrary.PasswordEntries[0]), 1);
			Assert.AreEqual(passwordEntryComparer.Compare(encryptedPasswordLibrary.PasswordEntries[2], encryptedPasswordLibrary.PasswordEntries[2]), 0);
			encryptedPasswordLibrary.Dispose();
		}
	}
}