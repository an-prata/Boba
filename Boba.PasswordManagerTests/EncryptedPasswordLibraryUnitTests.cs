using System.Text;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boba.PasswordManager;
using Boba.PasswordManager.FileHandling;

namespace Boba.PasswordManagerTests
{
	[TestClass]
	public class EncryptedPasswordLibraryUnitTests
	{
		[TestMethod]
		public void GeneralTest()
		{
			string name = "name";
			string password = "password";

			RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
			EncryptedPasswordLibrary encryptedPasswordLibrary = new EncryptedPasswordLibrary(cryptoServiceProvider, "library");
			Assert.IsNotNull(encryptedPasswordLibrary);

			encryptedPasswordLibrary.NewEntry(name, password);
			encryptedPasswordLibrary.NewEntry("Jake", Encoding.UTF8.GetBytes("JakesVeryStrongPassword"));
			encryptedPasswordLibrary.NewEntry(new EncryptedPasswordEntry(cryptoServiceProvider, "Panda6179", Encoding.UTF8.GetBytes("LiterallyAJoke")));
			encryptedPasswordLibrary.NewEntry(new PasswordEntry("Dave", "DavesInsecurePassword"));

			XmlHandler.SaveToFile("publickey.xml", encryptedPasswordLibrary.CryptoServiceProvider.ExportParameters(false));
			XmlHandler.SaveToFile("privatekey.xml", encryptedPasswordLibrary.CryptoServiceProvider.ExportParameters(true));
			JsonHandler.SaveToFile("library.json", encryptedPasswordLibrary);
			encryptedPasswordLibrary.Dispose();

			EncryptedPasswordLibrary newEncryptedPasswordLibrary = JsonHandler.ReadFromFile<EncryptedPasswordLibrary>("library.json");
			newEncryptedPasswordLibrary.CryptoServiceProvider = new RSACryptoServiceProvider();
			newEncryptedPasswordLibrary.CryptoServiceProvider.ImportParameters(XmlHandler.ReadFromFile("publickey.xml"));

			encryptedPasswordLibrary.NewEntry("Will", Encoding.UTF8.GetBytes("WillsEvenStrongerPassword"));
			encryptedPasswordLibrary.NewEntry(new EncryptedPasswordEntry(cryptoServiceProvider, "Raccoon6179", Encoding.UTF8.GetBytes("NotLiterallyAJoke")));
			newEncryptedPasswordLibrary.CryptoServiceProvider.ImportParameters(XmlHandler.ReadFromFile("privatekey.xml"));

			foreach (EncryptedPasswordEntry entry in newEncryptedPasswordLibrary.PasswordEntries)
			{
				Assert.AreNotEqual(entry.Password,
					newEncryptedPasswordLibrary.CryptoServiceProvider.Decrypt(entry.Password, true));
			}

			Assert.AreEqual(name, newEncryptedPasswordLibrary.PasswordEntries[0].Name);
			Assert.AreEqual(password, Encoding.UTF8.GetString(newEncryptedPasswordLibrary.CryptoServiceProvider.Decrypt(newEncryptedPasswordLibrary.PasswordEntries[0].Password, true)));
		}
	}
}
