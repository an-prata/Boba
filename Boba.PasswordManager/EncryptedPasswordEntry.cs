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

		/// <summary>
		/// Sets the Password property of the EncryptedPasswordEntry.
		/// </summary>
		/// <param name="password"> An unecrypted byte[] password. </param>
		/// <param name="cryptoServiceProvider"> An RSACryptoServiceProvider object to encrypt the password with. </param>
		public void SetPassword(byte[] password, RSACryptoServiceProvider cryptoServiceProvider) => _password = cryptoServiceProvider.Encrypt(password, UseOAEPPadding);

		/// <summary>
		/// Creates a new EncryptedPasswordEntry instance with empty properties.
		/// </summary>
		public EncryptedPasswordEntry()
		{
			Name = "";
			_password = new byte[0];
		}

		/// <summary>
		/// Creates a new EncryptedPasswordEntry instance with an empty password.
		/// </summary>
		/// <param name="name"> The name of the new entry. </param>
		public EncryptedPasswordEntry(string name)
		{
			Name = name;
			_password = new byte[0];
		}

		/// <summary>
		/// Creates a new EncryptedPasswordEntry instance.
		/// </summary>
		/// <param name="cryptoServiceProvider"> An RSACryptoServiceProvider object to encrypt the password with. </param>
		/// <param name="name"> The name of the entry. </param>
		/// <param name="password"> The unecrypted byte[] password. </param>
		public EncryptedPasswordEntry(RSACryptoServiceProvider cryptoServiceProvider, string name, byte[] password)
		{
			Name = name;
			_password = cryptoServiceProvider.Encrypt(password, UseOAEPPadding);
		}

		/// <summary>
		/// Creates a new EncryptedPasswordEntry instance using a PasswordEntry object.
		/// </summary>
		/// <param name="cryptoServiceProvider"> An RSACryptoServiceProvider object to encrypt the password with. </param>
		/// <param name="passwordEntry"> The PasswordEntry object to get the Name and Password from. </param>
		public EncryptedPasswordEntry(RSACryptoServiceProvider cryptoServiceProvider, PasswordEntry passwordEntry)
		{
			Name = passwordEntry.Name;
			_password = cryptoServiceProvider.Encrypt(passwordEntry.Password, UseOAEPPadding);
		}

		/// <summary>
		/// Creates a new EncryptedPasswordEntry instance and sets the properties to their respective parameters.
		/// </summary>
		/// <param name="name"> The name of the entry. </param>
		/// <param name="password"> An already encrypted byte[] password. </param>
		[JsonConstructor]
		public EncryptedPasswordEntry(string name, byte[] password)
		{
			Name = name;
			_password = password;
		}
	}
}
