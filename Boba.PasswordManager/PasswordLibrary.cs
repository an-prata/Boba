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
		private bool _disposed = false;
		private List<PasswordEntry> _passwordEntries;

		/// <summary>
		/// A List object containing all the entries in the Library.
		/// </summary>
		public List<PasswordEntry> PasswordEntries { get => _passwordEntries; set => _passwordEntries = Sort(value); }
		public string Name { get; set; }

		/// <summary>
		/// Creates and adds a new PasswordEntry object to PasswordEntries.
		/// </summary>
		/// <param name="name"> Name for the new PasswordEntry. </param>
		/// <param name="password"> Password for the new PasswordEntry.  </param>
		public void NewEntry(byte[] password, string username = "", string application = "")
		{
			PasswordEntries.Add(new PasswordEntry(password, username, application));
			PasswordEntries.Sort(ComparePasswordEntryAlphabeticaly);
		}

		/// <summary>
		/// Adds a PasswordEntry object to PasswordEntries.
		/// </summary>
		/// <param name="passwordEntry"> The PasswordEntry object to add. </param>
		public void NewEntry(PasswordEntry passwordEntry)
		{
			PasswordEntries.Add(passwordEntry);
			PasswordEntries.Sort(ComparePasswordEntryAlphabeticaly);
		}

		/// <summary>
		/// Compares two PasswordEntries for whichever should appear before the other
		/// in an alphabeticaly ordered list.
		/// </summary>
		protected int ComparePasswordEntryAlphabeticaly(PasswordEntry x, PasswordEntry y)
        {
			if (x == null)
			{
				if (y == null) { return 0; }
				else { return -1; }
			}
			else
            {
				if (y == null) { return 1; }
				else
                {
					byte[] bytesX = Encoding.UTF8.GetBytes(x.Application);
					byte[] bytesY = Encoding.UTF8.GetBytes(y.Application);

					for (int i = 0; i < (x.Application.Length > y.Application.Length ? y.Application.Length : x.Application.Length); i++)
                    {
						if (bytesX[i] > bytesY[i]) { return 1; }
						if (bytesY[i] > bytesX[i]) { return -1; }
					}

					if (x.Application.Length > y.Application.Length) { return 1; }
					if (y.Application.Length > x.Application.Length) { return -1; }
					return 0;
                }
            }
		}

		protected List<PasswordEntry> Sort(List<PasswordEntry> list)
        {
			list.Sort(ComparePasswordEntryAlphabeticaly);
			return list;
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
