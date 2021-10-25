﻿// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

<<<<<<< HEAD
using System.Text;
=======
using System;
using System.Text;
using System.Collections.Generic;
>>>>>>> 05fce4ed41aa37bc98ba0a29292f027d426ed8a7
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boba.PasswordManager;
using Boba.PasswordManager.FileHandling;

namespace Boba.PasswordManagerTests
{
    [TestClass]
    public class JsonHandlerUnitTests
    {
		private readonly byte[] TestBytes = Encoding.UTF8.GetBytes("hd82bdciw08723bisd83b");

		[TestMethod]
		public void SaveAndReadTest()
		{
			PasswordEntry passwordEntry = new PasswordEntry(TestBytes, "Google.com", "Jake@gmail.com");
			JsonHandler.SaveToFile("passwordentry.xml", passwordEntry);
			Assert.AreEqual(passwordEntry.Username, JsonHandler.ReadFromFile<PasswordEntry>("passwordentry.xml").Username);
		}
	}
}
