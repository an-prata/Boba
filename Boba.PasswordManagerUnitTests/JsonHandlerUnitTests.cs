// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System.Text;
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
			PasswordEntry passwordEntry = new(TestBytes, "Google.com", "Jake@gmail.com");
			JsonHandler.SaveToFile("passwordentry.xml", passwordEntry);
			Assert.AreEqual(passwordEntry.Username, JsonHandler.ReadFromFile<PasswordEntry>("passwordentry.xml").Username);
		}
	}
}
