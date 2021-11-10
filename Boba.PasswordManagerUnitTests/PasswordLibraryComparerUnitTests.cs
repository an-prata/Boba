// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boba.PasswordManager;

namespace Boba.PasswordManagerUnitTests
{
	[TestClass]
	public class PasswordLibraryComparerUnitTests
	{
		//private readonly string[] Applications = { "google.com", "docs.microsoft.com", "github.com", "stackoverflow.com" };
		//private readonly string[] Usernames = { "jake@gmail.com", "jake", "jakemiester1234", "jakesmith@outlook.com" };
		//private readonly string[] Passwords = { "jakesSuperStrongPassword", "Password12345", "TheBatmanEmoji", "#sYfg@sh&*gskks%$#17291$sg%" };

		private readonly List<PasswordLibrary> SortedPasswordLibraries = new()
        {
			new PasswordLibrary("A library", new List<PasswordEntry>()),
            new PasswordLibrary("bussines passwords", new List<PasswordEntry>()),
            new PasswordLibrary("google suite", new List<PasswordEntry>()),
            new PasswordLibrary("personal", new List<PasswordEntry>())
		};

		[TestMethod]
		public void PasswordEntryComparisonCheck()
		{
			Comparer<PasswordLibrary> passwordLibraryComparer = new PasswordLibraryComparer();
			Assert.AreEqual(passwordLibraryComparer.Compare(SortedPasswordLibraries[0], SortedPasswordLibraries[1]), -1);
			Assert.AreEqual(passwordLibraryComparer.Compare(SortedPasswordLibraries[1], SortedPasswordLibraries[0]), 1);
			Assert.AreEqual(passwordLibraryComparer.Compare(SortedPasswordLibraries[2], SortedPasswordLibraries[2]), 0);
		}

        [TestMethod]
		public void EncryptedPasswordEntryComparisonCheck()
		{
			Comparer<PasswordLibrary> passwordLibraryComparer = new PasswordLibraryComparer();
            
            List<EncryptedPasswordLibrary> encryptedPasswordLibraries = new()
            {
                new EncryptedPasswordLibrary(new RSACryptoServiceProvider(), "A library", new List<EncryptedPasswordEntry>()),
                new EncryptedPasswordLibrary(new RSACryptoServiceProvider(), "bussines passwords", new List<EncryptedPasswordEntry>()),
                new EncryptedPasswordLibrary(new RSACryptoServiceProvider(), "google suite", new List<EncryptedPasswordEntry>()),
                new EncryptedPasswordLibrary(new RSACryptoServiceProvider(), "personal", new List<EncryptedPasswordEntry>())
            };

			Assert.AreEqual(passwordLibraryComparer.Compare(encryptedPasswordLibraries[0], encryptedPasswordLibraries[1]), -1);
			Assert.AreEqual(passwordLibraryComparer.Compare(encryptedPasswordLibraries[1], encryptedPasswordLibraries[0]), 1);
			Assert.AreEqual(passwordLibraryComparer.Compare(encryptedPasswordLibraries[2], encryptedPasswordLibraries[2]), 0);
		}
	}
}