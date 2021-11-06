// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text;
using System.Collections.Generic;

namespace Boba.PasswordManager
{
	public class PasswordLibrary : IDisposable
	{
		bool _disposed = false;
		private List<PasswordEntry> _passwordEntries;

		/// <summary>
		/// A List object containing all the entries in the Library.
		/// </summary>
		public List<PasswordEntry> PasswordEntries 
		{
            get { return _passwordEntries; }
            set
            {
				_passwordEntries = value;
			}
		}

		public string Name { get; set; }

		/// <summary>
		/// Creates and adds a new PasswordEntry object to PasswordEntries.
		/// </summary>
		/// <param name="name"> Name for the new PasswordEntry. </param>
		/// <param name="password"> Password for the new PasswordEntry.  </param>
		public void NewEntry(byte[] password, string username = "", string application = "")
		{
			PasswordEntries.Add(new PasswordEntry(password, username, application));
			PasswordEntries.Sort(new PasswordEntryComparer());
		}

		/// <summary>
		/// Adds a PasswordEntry object to PasswordEntries.
		/// </summary>
		/// <param name="passwordEntry"> The PasswordEntry object to add. </param>
		public void NewEntry(PasswordEntry passwordEntry)
		{
			PasswordEntries.Add(passwordEntry);
			PasswordEntries.Sort(new PasswordEntryComparer());
		}

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
			try { if (disposing) PasswordEntries.Clear(); }
			catch (NullReferenceException) { }
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
