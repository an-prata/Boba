// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Eto.Forms;
using Eto.Serialization.Xaml;
using Boba.PasswordManager;

namespace Boba.Desktop
{
	public partial class MainForm : Form
	{
		private const string PasswordPlaceholder = "**********";

		public string FilePath { get; set; }
		private ListBox PasswordEntriesListBox { get; set; }
		public EncryptedPasswordLibrary CurrentPasswordLibrary { get; set; } 
		public SortedSet<string> ListBoxEntries { get => (SortedSet<string>)PasswordEntriesListBox.DataStore; set => PasswordEntriesListBox.DataStore = value; }

		private Label PasswordEntryApplicationLabel { get; set; }
		private Label PasswordEntryUsernameLabel { get; set; }
		private Label PasswordEntryPasswordLabel { get; set; }

		private Button CopyPasswordEntryApplicationButton { get; set; }
		private Button CopyPasswordEntryUsernameButton { get; set; }
		private Button CopyPasswordEntryPasswordButton { get; set; }

		private Button ChangePasswordEntryApplicationButton { get; set; }
		private Button ChangePasswordEntryUsernameButton { get; set; }
		private Button ChangePasswordEntryPasswordButton { get; set; }

		public MainForm()
		{
			XamlReader.Load(this);
			CurrentPasswordLibrary = new EncryptedPasswordLibrary(new RSACryptoServiceProvider(), "untitled", new List<EncryptedPasswordEntry>());
			ListBoxEntries = new SortedSet<string>();
			Title = CurrentPasswordLibrary.Name;

			PasswordEntriesListBox.SelectedIndexChanged += PasswordEntriesListBox_SelectedIndexChanged;
			PasswordEntriesListBox.SelectedValueChanged += PasswordEntriesListBox_SelectedIndexChanged;
		}

		protected void AddPasswordEntryButton_Clicked(object sender, EventArgs e)
		{
			using AddPasswordEntryDialog addPasswordEntryDialog = new AddPasswordEntryDialog();
			addPasswordEntryDialog.ShowModal();
			CurrentPasswordLibrary.NewEntry(addPasswordEntryDialog.Password, addPasswordEntryDialog.Application, addPasswordEntryDialog.Username);
			ListBoxEntries.Add(addPasswordEntryDialog.Application);
			PasswordEntriesListBox.DataStore = ListBoxEntries;
			PasswordEntriesListBox.UpdateBindings();
		}

		protected void RemovePasswordEntryButton_Clicked(object sender, EventArgs e)
		{
			if (PasswordEntriesListBox.SelectedIndex < 0) return;
			ListBoxEntries.Remove(PasswordEntriesListBox.SelectedKey);
			CurrentPasswordLibrary.PasswordEntries.RemoveAt(PasswordEntriesListBox.SelectedIndex);
			PasswordEntriesListBox.DataStore = ListBoxEntries;
			PasswordEntriesListBox.UpdateBindings();
		}

		protected void CopyPasswordEntryApplicationButton_Clicked(object sender, EventArgs e)
		{
			try { Clipboard.Instance.Text = PasswordEntryApplicationLabel.Text; }
			catch { return; } // Returns if no value is present.
		}

		protected void CopyPasswordEntryUsernameButton_Clicked(object sender, EventArgs e)
		{
			try { Clipboard.Instance.Text = PasswordEntryUsernameLabel.Text; }
			catch { return; } // Returns if no value is present.
		}

		protected void CopyPasswordEntryPasswordButton_Clicked(object sender, EventArgs e)
		{
			if (CurrentPasswordLibrary.CryptoServiceProvider == null)
            {
				MessageBox.Show("No keys given, please open keys to decrypt password.");
				return;
            }
			try { Clipboard.Instance.Text = Encoding.UTF8.GetString(
					CurrentPasswordLibrary.CryptoServiceProvider.Decrypt(CurrentPasswordLibrary.PasswordEntries[PasswordEntriesListBox.SelectedIndex].Password, true)); }
			catch { return; } // Returns if no value is present or no entry is selected.
		}

		protected void ChangePasswordEntryApplicationButton_Clicked(object sender, EventArgs e)
		{
			SingleStringDialogBox newApplicationDialog = new SingleStringDialogBox("Application");
			newApplicationDialog.ShowModal();
			CurrentPasswordLibrary.PasswordEntries[PasswordEntriesListBox.SelectedIndex].Application = newApplicationDialog.Result;
			PasswordEntryApplicationLabel.Text = newApplicationDialog.Result;
			newApplicationDialog.Dispose();
		}

		protected void ChangePasswordEntryUsernameButton_Clicked(object sender, EventArgs e)
		{
			SingleStringDialogBox newUsernameDialog = new SingleStringDialogBox("Username");
			newUsernameDialog.ShowModal();
			CurrentPasswordLibrary.PasswordEntries[PasswordEntriesListBox.SelectedIndex].Application = newUsernameDialog.Result;
			PasswordEntryApplicationLabel.Text = newUsernameDialog.Result;
			newUsernameDialog.Dispose();
		}

		protected void ChangePasswordEntryPasswordButton_Clicked(object sender, EventArgs e)
		{
			PasswordDialogBox newPasswordDialog = new PasswordDialogBox("Password");
			newPasswordDialog.ShowModal();
			CurrentPasswordLibrary.PasswordEntries[PasswordEntriesListBox.SelectedIndex].SetPassword(CurrentPasswordLibrary.CryptoServiceProvider, newPasswordDialog.Result);
			PasswordEntryApplicationLabel.Text = PasswordPlaceholder;
			newPasswordDialog.Dispose();
		}

		protected void PasswordEntriesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (PasswordEntriesListBox.SelectedIndex < 0)
			{
				PasswordEntryApplicationLabel.Text = "";
				PasswordEntryUsernameLabel.Text = "";
				PasswordEntryPasswordLabel.Text = "";

				CopyPasswordEntryApplicationButton.Enabled = false;
				ChangePasswordEntryApplicationButton.Enabled = false;

				CopyPasswordEntryUsernameButton.Enabled = false;
				ChangePasswordEntryUsernameButton.Enabled = false;

				CopyPasswordEntryPasswordButton.Enabled = false;
				ChangePasswordEntryPasswordButton.Enabled = false;

				return;
			}

			CopyPasswordEntryApplicationButton.Enabled = true;
			ChangePasswordEntryApplicationButton.Enabled = true;

			CopyPasswordEntryUsernameButton.Enabled = true;
			ChangePasswordEntryUsernameButton.Enabled = true;

			CopyPasswordEntryPasswordButton.Enabled = true;
			ChangePasswordEntryPasswordButton.Enabled = true;

			PasswordEntryApplicationLabel.Text = CurrentPasswordLibrary.PasswordEntries[PasswordEntriesListBox.SelectedIndex].Application;
			PasswordEntryUsernameLabel.Text = CurrentPasswordLibrary.PasswordEntries[PasswordEntriesListBox.SelectedIndex].Username;
			PasswordEntryPasswordLabel.Text = PasswordPlaceholder;
		}
	}
}
