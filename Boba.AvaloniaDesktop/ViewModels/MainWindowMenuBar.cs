// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Boba.AvaloniaDesktop.Views;
using Boba.PasswordManager;
using Boba.PasswordManager.FileHandling;

namespace Boba.AvaloniaDesktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
	{
		private readonly FileDialogFilter XmlFileFilter = new() { Extensions = { "xml" }, Name = "XML" };
		private readonly FileDialogFilter JsonFileFilter = new() { Extensions = { "json" }, Name = "JSON" };
		public List<string?> FilePaths { get; set; }

		public async void OpenMenuItem_Clicked()
		{
			if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				string[]? files = await new OpenFileDialog()
				{
					Title = "OpenLibrary ...",
					AllowMultiple = false,
					Filters = new List<FileDialogFilter> { JsonFileFilter }
				}.ShowAsync(desktop.MainWindow);
				if (files == null) return; 

				FilePaths.Clear();
				FilePaths.Add(files[0]);

				try { model.EncryptedPasswordLibraries[0] = JsonHandler.ReadFromFile<EncryptedPasswordLibrary>(files[0]); }
				catch (JsonException)
				{
					var viewModel = new MessageBoxViewModel(JsonExceptionMessage);
					var messageBox = new MessageBox() { DataContext = viewModel, Width = 480, Height = 140 };
					viewModel.OnRequestClose += (sender, e) => messageBox.Close();
					messageBox.Show();
					return;
				}

				Title = model.EncryptedPasswordLibraries[0].Name;
				PasswordEntriesListBox_Items.Clear();

				model.EncryptedPasswordLibraries[0].PasswordEntries.ForEach(delegate(EncryptedPasswordEntry encryptedPasswordEntry)
				{
					PasswordEntriesListBox_Items.Add(encryptedPasswordEntry.Application);
				});
			}
		}

		public async void NewMenuItem_Clicked()
		{
			if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				var viewModel = new SingleStringDialogViewModel() { IntegerMode = true };
				var keySizeDialog = new SingleStringDialog() { Title = "Enter Key Size", DataContext = viewModel };
				viewModel.OnRequestClose += (sender, e) => keySizeDialog.Close();
				await keySizeDialog.ShowDialog(desktop.MainWindow);

				foreach (KeySizes keySizes in model.EncryptedPasswordLibraries[0].CryptoServiceProvider.LegalKeySizes)
				{
					for (int i = keySizes.MinSize; i <= keySizes.MaxSize; i += keySizes.SkipSize)
					{
						if (i == viewModel.IntegerResult)
						{
							model.EncryptedPasswordLibraries[0] = new EncryptedPasswordLibrary(new RSACryptoServiceProvider(viewModel.IntegerResult), 
																				DefaultLibraryName, 
																				new List<EncryptedPasswordEntry>());													   
							FilePaths.Clear();
							Title = model.EncryptedPasswordLibraries[0].Name;
							PasswordEntriesListBox_Items.Clear();

							model.EncryptedPasswordLibraries[0].PasswordEntries.ForEach(delegate(EncryptedPasswordEntry encryptedPasswordEntry)
							{
								PasswordEntriesListBox_Items.Add(encryptedPasswordEntry.Application);
							});

							return;
						}
					}
				}

				var messageBoxViewModel = new MessageBoxViewModel(NotValidKeySize);
				var messageBox = new MessageBox() { DataContext = messageBoxViewModel };
				messageBoxViewModel.OnRequestClose += (sender, e) => messageBox.Close();
				await messageBox.ShowDialog(desktop.MainWindow);
			}
		}

		public void SaveMenuItem_Clicked()
		{
			if (FilePaths.Count == 0 || FilePaths[0] == null)
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
				string? fileName = await new SaveFileDialog()
				{
					Title = "Save Library As ...",
					Filters = new List<FileDialogFilter> { JsonFileFilter }
				}.ShowAsync(desktop.MainWindow);
				if (fileName == null) return;
				FilePaths.Clear();
				JsonHandler.SaveToFile(fileName, model.EncryptedPasswordLibraries[0]);
			}
		}

		public async void ExportPublicKeyMenuItem_Clicked()
		{
			if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				string? fileName = await new SaveFileDialog()
				{
					Title = "Export Public Key ...",
					Filters = new List<FileDialogFilter> { XmlFileFilter }
				}.ShowAsync(desktop.MainWindow);
				if (fileName == null) return;
				XmlHandler.SaveToFile(fileName, model.EncryptedPasswordLibraries[0].CryptoServiceProvider.ExportParameters(false));
			}
		}

		public async void ExportPrivateKeyMenuItem_Clicked()
		{
			if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				string? fileName = await new SaveFileDialog()
				{
					Title = "Export Private Key ...",
					Filters = new List<FileDialogFilter> { XmlFileFilter }
				}.ShowAsync(desktop.MainWindow);
				if (fileName == null) return; 
				XmlHandler.SaveToFile(fileName, model.EncryptedPasswordLibraries[0].CryptoServiceProvider.ExportParameters(true));
			}
		}

		public async void ImportPrivateKeyMenuItem_Clicked()
		{
			if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				string[]? files = await new OpenFileDialog()
				{  
					Title = "Import Private Key ...",
					AllowMultiple = false,
					Filters = new List<FileDialogFilter> { XmlFileFilter }
				}.ShowAsync(desktop.MainWindow);
				if (files == null) return;

                RSACryptoServiceProvider cryptoServiceProvider = new();

                try { cryptoServiceProvider.ImportParameters(XmlHandler.ReadFromFile<RSAParameters>(files[0])); } 
				catch (InvalidOperationException) 
				{
					var viewModel = new MessageBoxViewModel(XmlExceptionMessage);
					var messageBox = new MessageBox() { DataContext = viewModel, Width = 480, Height = 140 };
					viewModel.OnRequestClose += (sender, e) => messageBox.Close();
					messageBox.Show();
					return;
				}

                model.EncryptedPasswordLibraries[0].CryptoServiceProvider = cryptoServiceProvider;
            }
		}

		public void RenameMenuItem_Clicked()
		{
			if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) 
			{
				var viewModel = new SingleStringDialogViewModel();
				var nameDialog = new SingleStringDialog() { Title = "Rename Library", DataContext = viewModel };
				nameDialog.ShowDialog(desktop.MainWindow);

				viewModel.OnRequestClose += (sender, e) =>
				{
					nameDialog.Close();
					model.EncryptedPasswordLibraries[0].Name = viewModel.Result;
					Title = model.EncryptedPasswordLibraries[0].Name;
				};
			}
		}
	}
}