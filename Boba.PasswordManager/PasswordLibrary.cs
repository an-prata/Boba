using System;
using System.Collections.Generic;

namespace Boba.PasswordManager
{
	public class PasswordLibrary : IDisposable
	{
		bool _disposed = false;

		public List<PasswordEntry> PasswordEntries { get; set; }

		public void NewEntry(string name, string password) => PasswordEntries.Add(new PasswordEntry(name, password));

		public void NewEntry(PasswordEntry passwordEntry) => PasswordEntries.Add(passwordEntry);

		public PasswordLibrary() { }

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
