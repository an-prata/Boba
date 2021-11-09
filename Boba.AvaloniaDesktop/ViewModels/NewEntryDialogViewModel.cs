// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text;
using ReactiveUI;
using Avalonia.Media;
using Boba.AvaloniaDesktop.Views;


namespace Boba.AvaloniaDesktop.ViewModels
{
    public class NewEntryDialogViewModel : ViewModelBase
	{
		/// <summary>
		/// Caused in the NewEntryDialogConstructor to disable PasswordTextBox 
		/// and ConfirmPasswordTextBox and to set the NoPrivateKey property
		/// of the NewEntryDialogOnRequestCloseEventArgs event.
		/// </summary>
		public const bool PasswordsDisabled = true;

		/// <summary>
		/// Caused in the NewEntryDialogConstructor to enable PasswordTextBox 
		/// and ConfirmPasswordTextBox and to set the NoPrivateKey property
		/// of the NewEntryDialogOnRequestCloseEventArgs event.
		/// </summary>
		public const bool PasswordsEnabled = false;

		private bool _passwordTextBox_IsEnabled;
		private bool _confirmPasswordTextBox_IsEnabled;
		private string _applicationTextBox_Text;
		private string _usernameTextBox_Text;
		private string _passwordTextBox_Text;
		private string _confirmPasswordTextBox_Text;
		private string _passwordsMatchTextBlock_Text;
		private SolidColorBrush _passwordsMatchTextBlock_Foreground;

		public bool EditMode { get; set; }
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

		public string ApplicationResult { get; set; }
		public string UsernameResult { get; set; }
		public byte[] PasswordResult { get; set; }

		public bool Canceled { get; set; } = true;

		public event EventHandler<NewEntryDialogOnRequestCloseEventArgs>? OnRequestClose;

		public void CancelButton_Clicked()
		{
			if (OnRequestClose == null) throw new NullReferenceException();
			OnRequestClose(this, new NewEntryDialogOnRequestCloseEventArgs() { NewEntryDialogViewModel = this, NoPrivateKey = EditMode } );
		}
		
		public void EnterButton_Clicked()
		{
			if (_passwordTextBox_Text != _confirmPasswordTextBox_Text)
			{
				_passwordsMatchTextBlock_Text = PasswordsDontMatchMessage;
				var viewModel = new MessageBoxViewModel(PasswordsDontMatchMessage);
				var messageBox = new MessageBox() { DataContext = viewModel };
				viewModel.OnRequestClose += (sender, e) => messageBox.Close();
				messageBox.Show();
				return;
			}

			ApplicationResult = _applicationTextBox_Text;
			UsernameResult = _usernameTextBox_Text;
			PasswordResult = Encoding.UTF8.GetBytes(_passwordTextBox_Text);

			Canceled = false;
			if (OnRequestClose == null) throw new NullReferenceException();
			OnRequestClose(this, new NewEntryDialogOnRequestCloseEventArgs() { NewEntryDialogViewModel = this, NoPrivateKey = EditMode } );
		}

		/// <summary>
		/// Creates a new NewEntryDialogViewModel.
		/// </summary>
		/// <param name="editMode"> 
		/// 	<para>
		/// 		The edit mode of the ViewModel, true means that password boxes are disabled and
		/// 		the NewEntryDialogOnRequestCloseEventArgs.NoPrivateKey will be set equal to this value.
		/// 	</para> 
		/// 	<para>
		/// 		You can use the constants PasswordsDisabled and PasswordEnabled to more easily
		/// 		assign this parameter.
		/// 	</para>
		/// </param>
		public NewEntryDialogViewModel(bool editMode)
		{
			_passwordTextBox_IsEnabled = !editMode;
			_confirmPasswordTextBox_IsEnabled = !editMode;
			_applicationTextBox_Text = "";
			_usernameTextBox_Text = "";
			_passwordTextBox_Text = "";
			_confirmPasswordTextBox_Text = "";
			_passwordsMatchTextBlock_Text = "";
			ApplicationResult = "";
			UsernameResult = "";
			PasswordResult = Array.Empty<byte>();
			_passwordsMatchTextBlock_Foreground = new SolidColorBrush(new Color(255, 200, 80, 80));
			EditMode = editMode;
		}
	}
}