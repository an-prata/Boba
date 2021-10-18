using System;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Boba.PasswordManager
{
	public class EncryptedPasswordEntry : PasswordEntry
	{
		private const bool USE_OAEP_PADDING = true;

		private byte[] _password;

		public new byte[] Password { get => _password; }

		public void SetPassword(byte[] password, RSACryptoServiceProvider cryptoServiceProvider) => _password = cryptoServiceProvider.Encrypt(password, USE_OAEP_PADDING);

		public EncryptedPasswordEntry(string name) => Name = name;

		[JsonConstructor]
		public EncryptedPasswordEntry(string name, byte[] password)
		{
			Name = name;
			_password = password;
		}

		public EncryptedPasswordEntry(RSACryptoServiceProvider cryptoServiceProvider, string name, byte[] password)
		{
			Name = name;
			_password = cryptoServiceProvider.Encrypt(password, USE_OAEP_PADDING);
		}
	}
}
