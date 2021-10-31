// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boba.PasswordManager;

namespace Boba.PasswordManagerTests
{
	[TestClass]
	public class EncryptedPasswordLibraryUnitTests
	{
		//private readonly string[] Applications = { "google.com", "docs.microsoft.com", "github.com", "stackoverflow.com" };
		//private readonly string[] Usernames = { "jake@gmail.com", "jake", "jakemiester1234", "jakesmith@outlook.com" };
		//private readonly string[] Passwords = { "jakesSuperStrongPassword", "Password12345", "TheBatmanEmoji", "#sYfg@sh&*gskks%$#17291$sg%" };

		private readonly string[,] Credentials = {
			{ "google.com", "jake@gmail.com", "jakesSuperStrongPassword" },
			{ "docs.microsoft.com", "jake", "Password12345" },
			{ "github.com", "jakemiester1234", "TheBatmanEmoji"},
			{ "stackoverflow.com", "jakesmith@outlook.com", "#sYfg@sh&*gskks%$#17291$sg%"}
		};

		private readonly string[,] SortedCredentials = {
			{ "docs.microsoft.com", "jake", "Password12345" },
			{ "github.com", "jakemiester1234", "TheBatmanEmoji"},
			{ "google.com", "jake@gmail.com", "jakesSuperStrongPassword" },
			{ "stackoverflow.com", "jakesmith@outlook.com", "#sYfg@sh&*gskks%$#17291$sg%"}
		};

		[TestMethod]
		public void NewEntryTest()
		{
			RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
			EncryptedPasswordLibrary encryptedPasswordLibrary = new EncryptedPasswordLibrary(cryptoServiceProvider, "test library", new List<EncryptedPasswordEntry>());

			for (int i = 0; i < Credentials.GetLength(1); i++)
			{
				PasswordEntry passwordEntry = new PasswordEntry()
				{
					Application = SortedCredentials[i, 0],
					Username = SortedCredentials[i, 1],
					Password = Encoding.UTF8.GetBytes(Credentials[i, 2])
				};

				encryptedPasswordLibrary.NewEntry(passwordEntry);
				Assert.AreNotEqual(passwordEntry, encryptedPasswordLibrary.PasswordEntries[i]);
				Assert.AreEqual(Encoding.UTF8.GetString(passwordEntry.Password), Encoding.UTF8.GetString(cryptoServiceProvider.Decrypt(encryptedPasswordLibrary.PasswordEntries[i].Password, true)));
			}
		}
	}
}
