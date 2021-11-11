// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
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
		public void SetPassword(RSACryptoServiceProvider cryptoServiceProvider, byte[] password) => _password = cryptoServiceProvider.Encrypt(password, UseOAEPPadding);

		/// <summary>
		/// Creates a new empty EncryptedPasswordEntry instance.
		/// </summary>
		public EncryptedPasswordEntry()
		{
			Application = "";
			Username = "";
			_password = null;
		}

		/// <summary>
		/// Creates a new instance of EncryptedPasswordEntry from an existing PasswordEntry instance.
		/// </summary>
		/// <param name="cryptoServiceProvider"> An RSACryptoServiceProvider to encrypt the password with. </param>
		/// <param name="passwordEntry"> A PasswordEntry instance to get properties from. </param>
		public EncryptedPasswordEntry(RSACryptoServiceProvider cryptoServiceProvider, PasswordEntry passwordEntry)
		{
			Application = passwordEntry.Application;
			Username = passwordEntry.Username;
			_password = passwordEntry.Password == null ? null : cryptoServiceProvider.Encrypt(passwordEntry.Password, UseOAEPPadding);
		}

		/// <summary>
		/// Creates a new EncryptedPasswordEntry instance.
		/// </summary>
		/// <param name="cryptoServiceProvider"> An RSACryptoServiceProvider object to encrypt the password with. </param>
		/// <param name="application"> The name of the entry. </param>
		/// <param name="username"> Username for the application. </param>
		/// <param name="password"> The unecrypted byte[] password. </param>
		public EncryptedPasswordEntry(RSACryptoServiceProvider cryptoServiceProvider, byte[] password, string application = "", string username = "")
		{
			Application = application;
			Username = username;
			_password = password == null ? null : cryptoServiceProvider.Encrypt(password, UseOAEPPadding);
		}

		/// <summary>
		/// Creates a new EncryptedPasswordEntry instance and sets the properties to their respective parameters.
		/// </summary>
		/// <param name="application"> Application that the EncryptedPasswordEntry should apply to. </param>
		/// <param name="username"> Username for the application. </param>
		/// <param name="password"> An already encrypted byte[] password. </param>
		[JsonConstructor]
		public EncryptedPasswordEntry(byte[] password, string application, string username)
		{
			Application = application;
			Username = username;
			_password = password;
		}
	}
}
