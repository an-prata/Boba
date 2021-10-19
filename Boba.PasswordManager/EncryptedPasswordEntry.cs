using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Boba.PasswordManager
{
	public class EncryptedPasswordEntry : PasswordEntry
	{
		private const bool UseOAEPPadding = true;

		private byte[] _password;

		public new byte[] Password { get => _password; }

		public void SetPassword(byte[] password, RSACryptoServiceProvider cryptoServiceProvider) => _password = cryptoServiceProvider.Encrypt(password, UseOAEPPadding);

		public EncryptedPasswordEntry(string name) => Name = name;
		public EncryptedPasswordEntry(RSACryptoServiceProvider cryptoServiceProvider, string name, byte[] password)
		{
			Name = name;
			_password = cryptoServiceProvider.Encrypt(password, UseOAEPPadding);
		}
		public EncryptedPasswordEntry(RSACryptoServiceProvider cryptoServiceProvider, PasswordEntry passwordEntry)
		{
			Name = passwordEntry.Name;
			_password = cryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(passwordEntry.Password), UseOAEPPadding);
		}
		[JsonConstructor]
		public EncryptedPasswordEntry(string name, byte[] password)
		{
			Name = name;
			_password = password;
		}
	}
}
