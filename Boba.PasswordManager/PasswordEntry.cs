// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;

namespace Boba.PasswordManager
{
	public class PasswordEntry
	{
		private byte[] _password;
		public string Application { get; set; }
		public string Username { get; set; }
		public byte[] Password { get => _password; set => _password = value; }

		/// <summary>
		/// Creates a new empty PasswordEntry instance.
		/// </summary>
		public PasswordEntry()
		{
			Application = "";
			Username = "";
			Password = Array.Empty<byte>();
		}

		/// <summary>
		/// Creates a new PasswordEntry instance.
		/// </summary>
		/// <param name="application"> The application in which the PasswordEntry should apply. </param>
		/// <param name="username"> Username for the Application. </param>
		/// <param name="password"> The byte[] password for the PasswordEntry. </param>
		[JsonConstructor]
		public PasswordEntry(byte[] password, string application = "", string username = "")
		{
			Application = application;
			Username = username;
			Password = password;
		}
	}
}
