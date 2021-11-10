// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using Avalonia;
using Boba.PasswordManager;
using Boba.AvaloniaDesktop.Views;
using Boba.AvaloniaDesktop.Models;

namespace Boba.AvaloniaDesktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
	{
		private string _title;
		private string _applicationTextBlock_Text;
		private string _usernameTextBlock_Text;
		private string _passwordTextBlock_Text;
		private bool _editEntryButton_IsEnabled;
		private bool _copyApplicationButton_IsEnabled;
		private bool _copyUsernameButton_IsEnabled;
		private bool _copyPasswordButton_IsEnabled;
		private int _passwordEntriesListBox_SelectedIndex;
		private ObservableCollection<string> _passwordEntriesListBox_Items;
		private readonly EncryptedPasswordLibraryModel model;
		private NewEntryDialog? newEntryDialog;

		public string Title
		{
			get => _title;
			set => this.RaiseAndSetIfChanged(ref _title, value);
		}
		public string ApplicationTextBlock_Text 
		{ 
			get => _applicationTextBlock_Text; 
			set => this.RaiseAndSetIfChanged(ref _applicationTextBlock_Text, value); 
		}

		public string UsernameTextBlock_Text 
		{ 
			get => _usernameTextBlock_Text; 
			set => this.RaiseAndSetIfChanged(ref _usernameTextBlock_Text, value); 
		}

		public string PasswordTextBlock_Text 
		{ 
			get => _passwordTextBlock_Text; 
			set => this.RaiseAndSetIfChanged(ref _passwordTextBlock_Text, value);
		}

		public bool EditEntryButton_IsEnabled
		{
			get => _editEntryButton_IsEnabled;
			set => this.RaiseAndSetIfChanged(ref _editEntryButton_IsEnabled, value);
		}

		public bool CopyApplicationButton_IsEnabled
		{
			get => _editEntryButton_IsEnabled;
			set => this.RaiseAndSetIfChanged(ref _copyApplicationButton_IsEnabled, value);
		}

		public bool CopyUsernameButton_IsEnabled
		{
			get => _editEntryButton_IsEnabled;
			set => this.RaiseAndSetIfChanged(ref _copyUsernameButton_IsEnabled, value);
		}

		public bool CopyPasswordButton_IsEnabled
		{
			get => _editEntryButton_IsEnabled;
			set => this.RaiseAndSetIfChanged(ref _copyPasswordButton_IsEnabled, value);
		}

		public int PasswordEntriesListBox_SelectedIndex 
		{ 
			get => _passwordEntriesListBox_SelectedIndex; 
			set 
			{
				this.RaiseAndSetIfChanged(ref _passwordEntriesListBox_SelectedIndex, value);
				EventHandler eventHandler = SelectedEntryChanged;
				eventHandler.Invoke(this, new EventArgs());
			} 
		}

		public ObservableCollection<string> PasswordEntriesListBox_Items 
		{ 
			get => _passwordEntriesListBox_Items; 
			set => this.RaiseAndSetIfChanged(ref _passwordEntriesListBox_Items, value); 
		}

		private event EventHandler SelectedEntryChanged;

		public void AddEntryButton_Clicked()
		{
			NewEntryDialogViewModel newEntryDialogViewModel = new(NewEntryDialogViewModel.PasswordsEnabled);
			newEntryDialog = new NewEntryDialog { DataContext = newEntryDialogViewModel };
			newEntryDialogViewModel.OnRequestClose += NewEntryDialog_OnRequestClose;
			newEntryDialog.Show();
		}

		public void RemoveEntryButton_Clicked()
		{
			try { _passwordEntriesListBox_Items.RemoveAt(PasswordEntriesListBox_SelectedIndex); }
			catch (ArgumentOutOfRangeException) { return; }
		}

		public void CopyApplicationButton_Clicked() => Application.Current.Clipboard.SetTextAsync(_applicationTextBlock_Text);

		public void CopyUsernameButton_Clicked() => Application.Current.Clipboard.SetTextAsync(_usernameTextBlock_Text);

		public void CopyPasswordButton_Clicked()
		{
			if (model.EncryptedPasswordLibraries[0].CryptoServiceProvider == null) 
			{
				var viewModel = new MessageBoxViewModel(NoPrivateKeyMessage);
				var messageBox = new MessageBox() { DataContext = viewModel, Width = 480, Height = 140 };
				viewModel.OnRequestClose += (sender, e) => messageBox.Close();
				messageBox.Show();
				return;
			}

			byte[] unencryptedPassword = model.EncryptedPasswordLibraries[0].GetPassword(PasswordEntriesListBox_SelectedIndex);
			Application.Current.Clipboard.SetTextAsync(Encoding.UTF8.GetString(unencryptedPassword));
		} 

		public void EditEntryButton_Clicked()
		{
			NewEntryDialogViewModel newEntryDialogViewModel;

			if (model.EncryptedPasswordLibraries[0].CryptoServiceProvider == null)
			{
				newEntryDialogViewModel = new(NewEntryDialogViewModel.PasswordsDisabled)
				{
					ApplicationTextBox_Text = _applicationTextBlock_Text,
					UsernameTextBox_Text = _usernameTextBlock_Text,
					PasswordTextBox_Text = PasswordPlaceholder,
					ConfirmPasswordTextBox_Text = PasswordPlaceholder
				};
			}
			else
			{
				string? unencryptedPassword = Encoding.UTF8.GetString(model.EncryptedPasswordLibraries[0].GetPassword(PasswordEntriesListBox_SelectedIndex));
				newEntryDialogViewModel = new(NewEntryDialogViewModel.PasswordsEnabled)
				{
					ApplicationTextBox_Text = _applicationTextBlock_Text,
					UsernameTextBox_Text = _usernameTextBlock_Text,
					PasswordTextBox_Text = unencryptedPassword,
					ConfirmPasswordTextBox_Text = unencryptedPassword
				};
			}

			newEntryDialog = new NewEntryDialog { DataContext = newEntryDialogViewModel };
			newEntryDialogViewModel.OnRequestClose += EditEntryDialog_OnRequestClose;
			newEntryDialog.Show();
		}

		protected void NewEntryDialog_OnRequestClose(object? sender, NewEntryDialogOnRequestCloseEventArgs e)
		{
			if (e.NewEntryDialogViewModel == null || newEntryDialog == null) throw new NullReferenceException();

			newEntryDialog.Close();

			if (e.NewEntryDialogViewModel.Canceled) return;
			
			model.EncryptedPasswordLibraries[0].NewEntry(e.NewEntryDialogViewModel.PasswordResult, e.NewEntryDialogViewModel.ApplicationResult, e.NewEntryDialogViewModel.UsernameResult);
			PasswordEntriesListBox_Items.Clear();

			model.EncryptedPasswordLibraries[0].PasswordEntries.ForEach(delegate(EncryptedPasswordEntry encryptedPasswordEntry)
			{
				PasswordEntriesListBox_Items.Add(encryptedPasswordEntry.Application);
			});
		}

		protected void EditEntryDialog_OnRequestClose(object? sender, NewEntryDialogOnRequestCloseEventArgs e)
		{
			if (e.NewEntryDialogViewModel == null || newEntryDialog == null) throw new NullReferenceException();

			newEntryDialog.Close();

			if (e.NewEntryDialogViewModel.Canceled) return;

			model.EncryptedPasswordLibraries[0].PasswordEntries[_passwordEntriesListBox_SelectedIndex].Application = e.NewEntryDialogViewModel.ApplicationResult;
			model.EncryptedPasswordLibraries[0].PasswordEntries[_passwordEntriesListBox_SelectedIndex].Username = e.NewEntryDialogViewModel.UsernameResult;

			if (!e.NoPrivateKey)
			{
				model.EncryptedPasswordLibraries[0].PasswordEntries[_passwordEntriesListBox_SelectedIndex].SetPassword(
					model.EncryptedPasswordLibraries[0].CryptoServiceProvider, e.NewEntryDialogViewModel.PasswordResult);
			} 

			PasswordEntriesListBox_Items[_passwordEntriesListBox_SelectedIndex] = e.NewEntryDialogViewModel.ApplicationResult;
			EventHandler eventHandler = SelectedEntryChanged;
			eventHandler.Invoke(this, new EventArgs());
		}

		protected void OnSelectedEntryChanged(object? sender, EventArgs e)
		{
			if (PasswordEntriesListBox_SelectedIndex == -1)
			{
				ApplicationTextBlock_Text = "";
				UsernameTextBlock_Text = "";
				PasswordTextBlock_Text = "";
				EditEntryButton_IsEnabled = false;
				CopyApplicationButton_IsEnabled = false;
				CopyUsernameButton_IsEnabled = false;
				CopyPasswordButton_IsEnabled = false;
				return;
			}

			ApplicationTextBlock_Text = model.EncryptedPasswordLibraries[0].PasswordEntries[PasswordEntriesListBox_SelectedIndex].Application;
			UsernameTextBlock_Text = model.EncryptedPasswordLibraries[0].PasswordEntries[PasswordEntriesListBox_SelectedIndex].Username;
			PasswordTextBlock_Text = PasswordPlaceholder;
			EditEntryButton_IsEnabled = true;
			CopyApplicationButton_IsEnabled = true;
			CopyUsernameButton_IsEnabled = true;
			CopyPasswordButton_IsEnabled = true;
		}

		public MainWindowViewModel()
		{
			_passwordEntriesListBox_Items = new ObservableCollection<string>();
			SelectedEntryChanged += OnSelectedEntryChanged;
			model = new EncryptedPasswordLibraryModel(2048, "name");
			FilePaths = new List<string?>();
			_title = "Untitled";

			_applicationTextBlock_Text = "";
			_usernameTextBlock_Text = "";
			_passwordTextBlock_Text = "";
			_editEntryButton_IsEnabled = false;
		}
	}
}