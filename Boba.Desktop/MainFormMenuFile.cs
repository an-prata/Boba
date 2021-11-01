// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.IO;
using System.Collections.Generic;
using Eto.Forms;
using Boba.PasswordManager;
using Boba.PasswordManager.FileHandling;

namespace Boba.Desktop
{
	public partial class MainForm : Form
	{
		public ButtonMenuItem OpenButtonMenuItem { get; set; }
		public ButtonMenuItem NewButtonMenuItem { get; set; }
		public ButtonMenuItem SaveButtonMenuItem { get; set; }
		public ButtonMenuItem SaveAsButtonMenuItem { get; set; }
		public ButtonMenuItem RenameLibraryButtonMenuItem { get; set; }

		protected void OpenButtonMenuItem_Clicked(object sender, EventArgs e)
		{
			using OpenFileDialog openFileDialog = new OpenFileDialog() { Title = "Open Password Library", CurrentFilter = new FileFilter("Json", ".json") };
			DialogResult dialogResult = openFileDialog.ShowDialog(this);
			if (openFileDialog.FileName == "" || openFileDialog.FileName == null) return;
			CurrentPasswordLibrary = JsonHandler.ReadFromFile<EncryptedPasswordLibrary>(openFileDialog.FileName);
			FilePath = openFileDialog.FileName;
			ListBoxEntries = new SortedSet<string>();
			foreach (EncryptedPasswordEntry encryptedPasswordEntry in CurrentPasswordLibrary.PasswordEntries) ListBoxEntries.Add(encryptedPasswordEntry.Application);
			PasswordEntriesListBox.DataStore = ListBoxEntries;
			PasswordEntriesListBox.UpdateBindings();
			Title = CurrentPasswordLibrary.Name;
		}

		protected void NewButtonMenuItem_Clicked(object sender, EventArgs e)
		{
			using NewPasswordLibraryDialog newPasswordLibraryDialog = new NewPasswordLibraryDialog() { Title = "New Password Library" };
			newPasswordLibraryDialog.ShowModal();
			CurrentPasswordLibrary = newPasswordLibraryDialog.Result;
			PasswordEntriesListBox.DataStore = new SortedSet<string>();
			PasswordEntriesListBox.UpdateBindings();
			Title = CurrentPasswordLibrary.Name;
		}

		protected void SaveButtonMenuItem_Clicked(object sender, EventArgs e)
		{
			if (!File.Exists(FilePath)) SaveAsButtonMenuItem_Clicked(sender, e);
			else
            {
				JsonHandler.SaveToFile(FilePath, CurrentPasswordLibrary);
				PasswordEntriesListBox.DataStore = ListBoxEntries;
				PasswordEntriesListBox.UpdateBindings();
				Title = CurrentPasswordLibrary.Name;
			}
		}

		protected void SaveAsButtonMenuItem_Clicked(object sender, EventArgs e)
		{
			using SaveFileDialog saveFileDialog = new SaveFileDialog() { Title = "Save Password Library As", CurrentFilter = new FileFilter("Json", ".json") };
			DialogResult dialogResult = saveFileDialog.ShowDialog(this);
			if (saveFileDialog.FileName == "" || saveFileDialog.FileName == null) return;
			JsonHandler.SaveToFile(saveFileDialog.FileName, CurrentPasswordLibrary);
			FilePath = saveFileDialog.FileName;
			PasswordEntriesListBox.DataStore = ListBoxEntries;
			PasswordEntriesListBox.UpdateBindings();
			Title = CurrentPasswordLibrary.Name;
		}

		protected void RenameLibraryButtonMenuItem_Clicked(object sender, EventArgs e)
        {
			SingleStringDialogBox renameDialogBox = new SingleStringDialogBox("Rename Library");
			renameDialogBox.ShowModal();
			CurrentPasswordLibrary.Name = renameDialogBox.Result;
			Title = CurrentPasswordLibrary.Name;
		}
	}
}
