using System;
using System.Collections.Generic;

namespace Boba.PasswordManager
{
	public class PasswordLibrary : IDisposable
	{
		bool _disposed = false;

		/// <summary>
		/// A List object containing all the entries in the Library.
		/// </summary>
		public List<PasswordEntry> PasswordEntries { get; set; }
		public string Name { get; set; }

		/// <summary>
		/// Creates and adds a new PasswordEntry object to PasswordEntries.
		/// </summary>
		/// <param name="name"> Name for the new PasswordEntry. </param>
		/// <param name="password"> Password for the new PasswordEntry.  </param>
		public void NewEntry(byte[] password, string username = "", string application = "") => PasswordEntries.Add(new PasswordEntry(password, username, application));

		/// <summary>
		/// Adds a PasswordEntry object to PasswordEntries.
		/// </summary>
		/// <param name="passwordEntry"> The PasswordEntry object to add. </param>
		public void NewEntry(PasswordEntry passwordEntry) => PasswordEntries.Add(passwordEntry);

		/// <summary>
		/// Creates a new PasswordLibrary instance with empty properties.
		/// </summary>
		public PasswordLibrary() { }

		/// <summary>
		/// Creates a new PasswordLibrary without any PasswordEntry objects.
		/// </summary>
		/// <param name="name"> Name for the PasswordLibrary. </param>
		public PasswordLibrary(string name, List<PasswordEntry> passwordEntries)
		{
			Name = name;
			PasswordEntries = passwordEntries;
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			if (disposing) PasswordEntries.Clear();
			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~PasswordLibrary()
		{
			Dispose(disposing: false);
		}
	}
}
