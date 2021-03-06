// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boba.PasswordManager;

namespace Boba.PasswordManagerTests
{
	[TestClass]
	public class PasswordLibraryUnitTests
	{
		//private readonly string[] Applications = { "google.com", "docs.microsoft.com", "github.com", "stackoverflow.com" };
		//private readonly string[] Usernames = { "jake@gmail.com", "jake", "jakemiester1234", "jakesmith@outlook.com" };
		//private readonly string[] Passwords = { "jakesSuperStrongPassword", "Password12345", "TheBatmanEmoji", "#sYfg@sh&*gskks%$#17291$sg%" };

		private readonly string[,] Credentials = 
		{
			{ "google.com", "jake@gmail.com", "jakesSuperStrongPassword" },
			{ "docs.microsoft.com", "jake", "Password12345" },
			{ "github.com", "jakemiester1234", "TheBatmanEmoji"},
			{ "stackoverflow.com", "jakesmith@outlook.com", "#sYfg@sh&*gskks%$#17291$sg%"}
		};

		private readonly string[,] SortedCredentials = 
		{
			{ "docs.microsoft.com", "jake", "Password12345" },
			{ "github.com", "jakemiester1234", "TheBatmanEmoji"},
			{ "google.com", "jake@gmail.com", "jakesSuperStrongPassword" },
			{ "stackoverflow.com", "jakesmith@outlook.com", "#sYfg@sh&*gskks%$#17291$sg%"}
		};

		[TestMethod]
		public void SortingTest()
		{
			using PasswordLibrary passwordLibrary = new("test library", new List<PasswordEntry>());
			using PasswordLibrary preSortedPasswordLibrary = new("test library", new List<PasswordEntry>());

			for (int i = 0; i < Credentials.GetLength(1); i++) 
				passwordLibrary.NewEntry(Encoding.UTF8.GetBytes(Credentials[i, 2]), Credentials[i, 1], Credentials[i, 0]);

			for (int i = 0; i < Credentials.GetLength(1); i++) 
				preSortedPasswordLibrary.NewEntry(Encoding.UTF8.GetBytes(SortedCredentials[i, 2]), SortedCredentials[i, 1], SortedCredentials[i, 0]);

			for (int i = 0; i < passwordLibrary.PasswordEntries.Count; i++) 
				Assert.AreEqual(passwordLibrary.PasswordEntries[i].Application, preSortedPasswordLibrary.PasswordEntries[i].Application);
		}
	}
}
