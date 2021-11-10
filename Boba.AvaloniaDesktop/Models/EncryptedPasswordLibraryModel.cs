// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using Boba.PasswordManager;
using Boba.PasswordManager.FileHandling;

namespace Boba.AvaloniaDesktop.Models
{
	public class EncryptedPasswordLibraryModel
	{
		private readonly PasswordLibraryComparer libraryComparer = new();
		private List<EncryptedPasswordLibrary> _encryptedPasswordLibraries;

		/// <summary>
		/// Stores the Model's EncryptedPasswordLibrary objects, avoid directly adding to the List,
		/// try to do it through the class instead, so that this way it will remain sorted.
		/// </summary>
		public List<EncryptedPasswordLibrary> EncryptedPasswordLibraries 
		{ 
			get => _encryptedPasswordLibraries; 
			set 
			{
				_encryptedPasswordLibraries = value; 
				_encryptedPasswordLibraries.Sort(libraryComparer);
			} 
		}

		public void AddLibrary(EncryptedPasswordLibrary library)
		{
			EncryptedPasswordLibraries.Add(library);
			EncryptedPasswordLibraries.Sort(libraryComparer);
		}

		public EncryptedPasswordLibraryModel(int keySize, string libraryName)
		{
			_encryptedPasswordLibraries = new List<EncryptedPasswordLibrary>() 
			{
				new EncryptedPasswordLibrary(new RSACryptoServiceProvider(keySize), libraryName, new List<EncryptedPasswordEntry>())
			};
		}
	}
}