// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Boba.PasswordManager;
using Boba.PasswordManager.FileHandling;

namespace Boba.AvaloniaDesktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
	{
		public List<string?> FilePaths { get; set; }

		public async void OpenMenuItem_Clicked()
		{
			if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				string[]? files = await new OpenFileDialog().ShowAsync(desktop.MainWindow);
				if (files == null) { return; }

				FilePaths[0] = files[0];
				model.EncryptedPasswordLibraries[0] = JsonHandler.ReadFromFile<EncryptedPasswordLibrary>(files[0]);
				PasswordEntriesListBox_Items.Clear();

				model.EncryptedPasswordLibraries[0].PasswordEntries.ForEach(delegate(EncryptedPasswordEntry encryptedPasswordEntry)
				{
					PasswordEntriesListBox_Items.Add(encryptedPasswordEntry.Application);
				});
			}
		}

		public void NewMenuItem_Clicked()
		{
			model.EncryptedPasswordLibraries[0] = new EncryptedPasswordLibrary(new RSACryptoServiceProvider(), "untitled", new List<EncryptedPasswordEntry>());
			FilePaths[0] = null;
			PasswordEntriesListBox_Items.Clear();

			model.EncryptedPasswordLibraries[0].PasswordEntries.ForEach(delegate(EncryptedPasswordEntry encryptedPasswordEntry)
			{
				PasswordEntriesListBox_Items.Add(encryptedPasswordEntry.Application);
			});
		}

		public void SaveMenuItem_Clicked()
		{
			if (FilePaths[0] == null)
			{
				SaveAsMenuItem_Clicked();
				return;
			}

			JsonHandler.SaveToFile(FilePaths[0], model.EncryptedPasswordLibraries[0]);
		}

		public async void SaveAsMenuItem_Clicked()
		{
			if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				string? fileName = await new SaveFileDialog().ShowAsync(desktop.MainWindow);
				if (fileName == null) { return; }
				FilePaths[0] = fileName;
				JsonHandler.SaveToFile(fileName, model.EncryptedPasswordLibraries[0]);
			}
		}
	}
}