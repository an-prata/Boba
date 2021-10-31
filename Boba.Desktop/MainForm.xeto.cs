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
	public class MainForm : Form
	{
		private ListBox passwordEntriesListBox;

		public EncryptedPasswordLibrary CurrentPasswordLibrary { get; set; } 
		public SortedSet<string> ListBoxEntries { get => (SortedSet<string>)passwordEntriesListBox.DataStore; set => passwordEntriesListBox.DataStore = value; }

		public MainForm()
		{
			XamlReader.Load(this);
			CurrentPasswordLibrary = new EncryptedPasswordLibrary(new RSACryptoServiceProvider(), "name", new List<EncryptedPasswordEntry>());
			ListBoxEntries = new SortedSet<string>();
		}

		protected void HandleAddPasswordEntryButton(object sender, EventArgs e)
		{
			using AddPasswordEntryDialog addPasswordEntryDialog = new AddPasswordEntryDialog();
			addPasswordEntryDialog.ShowModal();
			CurrentPasswordLibrary.NewEntry(addPasswordEntryDialog.Password, addPasswordEntryDialog.Application, addPasswordEntryDialog.Username);
			ListBoxEntries.Add(addPasswordEntryDialog.Application);
			passwordEntriesListBox.DataStore = ListBoxEntries;
			passwordEntriesListBox.UpdateBindings();
		}

		protected void HandleRemovePasswordEntryButton(object sender, EventArgs e)
		{
			if (passwordEntriesListBox.SelectedIndex < 0) return;
			ListBoxEntries.Remove(passwordEntriesListBox.SelectedKey);
			CurrentPasswordLibrary.PasswordEntries.RemoveAt(passwordEntriesListBox.SelectedIndex);
			passwordEntriesListBox.DataStore = ListBoxEntries;
			passwordEntriesListBox.UpdateBindings();
		}
	}
}
