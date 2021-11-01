// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Eto.Forms;
using Eto.Serialization.Xaml;
using Boba.PasswordManager;

namespace Boba.Desktop
{	
	public class NewPasswordLibraryDialog : Dialog
	{	
		private TextBox NameTextBox { get; set; }
		private TextBox KeySizeTextBox { get; set; }
		public string Name { get; set; }
		public int KeySize { get; set; }
		public EncryptedPasswordLibrary Result { get; set; }

		public NewPasswordLibraryDialog()
		{
			XamlReader.Load(this);
			NameTextBox.TextChanged += NameTextBox_TextChanged;
			KeySizeTextBox.TextChanged += KeySizeTextBox_TextChanged;
		}

		private void CloseButton_Clicked(object sender, EventArgs e)
        {
			try 
			{
				using RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(Convert.ToInt32(KeySizeTextBox.Text));
				Result = new EncryptedPasswordLibrary(cryptoServiceProvider, Name, new List<EncryptedPasswordEntry>());
				Close();
			}
			catch { MessageBox.Show("Not a valid key size."); }
        }

		private void NameTextBox_TextChanged(object sender, EventArgs e) => Name = NameTextBox.Text;

		private void KeySizeTextBox_TextChanged(object sender, EventArgs e)
		{
			try { KeySize = Convert.ToInt32(KeySizeTextBox.Text); }
			catch { return; }
		}
	}
}
