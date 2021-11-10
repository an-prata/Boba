// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using Boba.AvaloniaDesktop.Views;

namespace Boba.AvaloniaDesktop.ViewModels
{
	public class SingleStringDialogViewModel : ViewModelBase
	{
        private string _singleStringTextBox_Text;

        public string SingleStringTextBox_Text 
        { 
            get => _singleStringTextBox_Text; 
            set => this.RaiseAndSetIfChanged(ref _singleStringTextBox_Text, value); 
        
        }

        public string Result { get; set; }

		/// <summary>
		/// Invoked when the window should be closed.
		/// </summary>
		public event EventHandler? OnRequestClose;

		public void OkButton_Clicked()
		{
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (_singleStringTextBox_Text == "")
                {
                    var viewModel = new MessageBoxViewModel(NoNameGivenMessage);
                    var messageBox = new MessageBox() { DataContext = viewModel };
                    viewModel.OnRequestClose += (sender, e) => messageBox.Close();
                    messageBox.ShowDialog(desktop.MainWindow);
                    return;
                }
            }
            else throw new PlatformNotSupportedException(PlatformNotSupportedMessage);
            
            Result = _singleStringTextBox_Text;

			try { OnRequestClose!(this, new EventArgs()); }
			catch (NullReferenceException) { throw; }
		} 

		/// <summary>
		/// Creates a new SingleStringDialogViewModel.
		/// </summary>
		public SingleStringDialogViewModel()
		{
            _singleStringTextBox_Text = "";
            Result = "";
		}
	}
}