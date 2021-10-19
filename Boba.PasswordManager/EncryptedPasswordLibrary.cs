using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Boba.PasswordManager
{
	public class EncryptedPasswordLibrary : PasswordLibrary
	{
		private const bool UseOAEPPadding = true;
		bool _disposed = false;

		[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		public RSACryptoServiceProvider CryptoServiceProvider { get; set; }
		public int KeySize { get => CryptoServiceProvider.KeySize; }
		public new List<EncryptedPasswordEntry> PasswordEntries { get; set; }

		public new void NewEntry(string name, string password) => PasswordEntries.Add(new EncryptedPasswordEntry(name, CryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(password), UseOAEPPadding)));
		public new void NewEntry(PasswordEntry passwordEntry) => PasswordEntries.Add(new EncryptedPasswordEntry(CryptoServiceProvider, passwordEntry));
		public void NewEntry(EncryptedPasswordEntry encryptedPasswordEntry) => PasswordEntries.Add(encryptedPasswordEntry);
		public void NewEntry(string name, byte[] password) => PasswordEntries.Add(new EncryptedPasswordEntry(CryptoServiceProvider, name, password));

		///<summary>
		/// For use only with Json serialization and deserialization, do not use
		/// without setting <c>EncryptedPasswordLibrary.CryptoServiceProvider</c>
		/// with an <c>RSACryptoServiceProvider</c> object immediately after.
		///</summary>
		[JsonConstructor]
		public EncryptedPasswordLibrary(List<EncryptedPasswordEntry> passwordEntries, string name)
		{
			Name = name;
			PasswordEntries = passwordEntries;
		}
		public EncryptedPasswordLibrary(string name) => Name = name;
		public EncryptedPasswordLibrary(RSACryptoServiceProvider cryptoServiceProvider, string name)
		{
			Name = name;
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
