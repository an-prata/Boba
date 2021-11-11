// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text;
using ReactiveUI;
using Boba.AvaloniaDesktop.Views;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls.ApplicationLifetimes;

namespace Boba.AvaloniaDesktop.ViewModels
{
    public class NewEntryDialogViewModel : ViewModelBase
	{
		/// <summary>
		/// Used in the NewEntryDialogViewModel constructor to disable PasswordTextBox 
		/// and ConfirmPasswordTextBox.
		/// </summary>
		public const bool PasswordsDisabled = false;

		/// <summary>
		/// Used in the NewEntryDialogViewModel constructor to enable PasswordTextBox 
		/// and ConfirmPasswordTextBox.
		/// </summary>
		public const bool PasswordsEnabled = true;

		private bool _passwordTextBox_IsEnabled;
		private bool _confirmPasswordTextBox_IsEnabled;
		private string _applicationTextBox_Text;
		private string _usernameTextBox_Text;
		private string _passwordTextBox_Text;
		private string _confirmPasswordTextBox_Text;
		private string _passwordsMatchTextBlock_Text;
		private SolidColorBrush _passwordsMatchTextBlock_Foreground;

		public bool PasswordTextBox_IsEnabled 
		{ 
			get => _passwordTextBox_IsEnabled; 
			set => this.RaiseAndSetIfChanged(ref _passwordTextBox_IsEnabled, value); 
		}

		public bool ConfirmPasswordTextBox_IsEnabled 
		{ 
			get => _confirmPasswordTextBox_IsEnabled; 
			set => this.RaiseAndSetIfChanged(ref _confirmPasswordTextBox_IsEnabled, value); 
		}

		public string ApplicationTextBox_Text 
		{ 
			get => _applicationTextBox_Text; 
			set => this.RaiseAndSetIfChanged(ref _applicationTextBox_Text, value); 
		}

		public string UsernameTextBox_Text 
		{ 
			get => _usernameTextBox_Text; 
			set => this.RaiseAndSetIfChanged(ref _usernameTextBox_Text, value); 
		}

		public string PasswordsMatchTextBlock_Text 
		{ 
			get => _passwordsMatchTextBlock_Text; 
			set => this.RaiseAndSetIfChanged(ref _passwordsMatchTextBlock_Text, value); 
		}

		public string PasswordTextBox_Text 
		{ 
			get => _passwordTextBox_Text; 
			set => this.RaiseAndSetIfChanged(ref _passwordTextBox_Text, value); 
		}

		public string ConfirmPasswordTextBox_Text 
		{ 
			get => _confirmPasswordTextBox_Text; 
			set => this.RaiseAndSetIfChanged(ref _confirmPasswordTextBox_Text, value); 
		}

		public SolidColorBrush PasswordMatchTextBlock_Foreground 
		{ 
			get => _passwordsMatchTextBlock_Foreground; 
			set => this.RaiseAndSetIfChanged(ref _passwordsMatchTextBlock_Foreground, value); 
		}

		public string? ApplicationResult { get; set; }
		public string? UsernameResult { get; set; }
		public byte[]? PasswordResult { get; set; }

		public bool Canceled { get; set; } = true;

		public event EventHandler<NewEntryDialogOnRequestCloseEventArgs>? OnRequestClose;

		public void CancelButton_Clicked()
		{
			if (OnRequestClose == null) throw new NullReferenceException();
			OnRequestClose(this, new NewEntryDialogOnRequestCloseEventArgs() { NewEntryDialogViewModel = this } );
		}
		
		public void EnterButton_Clicked()
		{
			if (_passwordTextBox_Text != _confirmPasswordTextBox_Text)
			{
				if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
				{
					_passwordsMatchTextBlock_Text = PasswordsDontMatchMessage;
					var viewModel = new MessageBoxViewModel(PasswordsDontMatchMessage);
					var messageBox = new MessageBox() { DataContext = viewModel };
					viewModel.OnRequestClose += (sender, e) => messageBox.Close();
					messageBox.ShowDialog(desktop.MainWindow);
					return;
				}
			}

			Canceled = false;
			ApplicationResult = _applicationTextBox_Text == "" ? null : _applicationTextBox_Text;
			UsernameResult = _usernameTextBox_Text == "" ? null : _usernameTextBox_Text;
			PasswordResult = _passwordTextBox_Text == "" ? null : Encoding.UTF8.GetBytes(_passwordTextBox_Text);

			if (OnRequestClose == null) throw new NullReferenceException("No event handler assigned to OnRequestClose.");
			OnRequestClose(this, new NewEntryDialogOnRequestCloseEventArgs() { NewEntryDialogViewModel = this } );
		}

		/// <summary>
		/// Creates a new NewEntryDialogViewModel.
		/// </summary>
		/// <param name="passwordBoxesEnabled"> Enables/Disables PasswordTextBox and ConfirmPasswordTextBox</param>
		public NewEntryDialogViewModel(bool passwordBoxesEnabled)
		{
			PasswordTextBox_IsEnabled = passwordBoxesEnabled;
			ConfirmPasswordTextBox_IsEnabled = passwordBoxesEnabled;

			_applicationTextBox_Text = "";
			_usernameTextBox_Text = "";
			_passwordTextBox_Text = "";
			_confirmPasswordTextBox_Text = "";
			_passwordsMatchTextBlock_Text = "";
			_passwordsMatchTextBlock_Foreground = new SolidColorBrush(new Color(255, 200, 80, 80));
		}
	}
}