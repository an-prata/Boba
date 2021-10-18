using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Boba.PasswordManager
{
	public class EncryptedPasswordLibrary : PasswordLibrary
	{
		bool _disposed = false;

		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public RSACryptoServiceProvider CryptoServiceProvider { get; set; }
		public new List<EncryptedPasswordEntry> PasswordEntries { get; set; }

		public void NewEntry(string name, byte[] password) => PasswordEntries.Add(new EncryptedPasswordEntry(CryptoServiceProvider, name, password));

		public void NewEntry(EncryptedPasswordEntry encryptedPasswordEntry) => PasswordEntries.Add(encryptedPasswordEntry);

		[JsonConstructor]
		public EncryptedPasswordLibrary(List<EncryptedPasswordEntry> passwordEntries)
		{
			PasswordEntries = passwordEntries;
		}

		public EncryptedPasswordLibrary(RSACryptoServiceProvider cryptoServiceProvider)
		{
			CryptoServiceProvider = cryptoServiceProvider;
			PasswordEntries = new List<EncryptedPasswordEntry>();
		}

		protected virtual new void Dispose(bool disposing)
		{
			if (_disposed) return;
			if (disposing) PasswordEntries.Clear();
			_disposed = true;
		}

		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~EncryptedPasswordLibrary()
		{
			Dispose(disposing: false);
		}
	}
}
