// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using ReactiveUI;

namespace Boba.AvaloniaDesktop.ViewModels
{
	public class MessageBoxViewModel : ViewModelBase
	{
		private string _messageTextBlock_Text;
		
		public string MessageTextBlock_Text { get => _messageTextBlock_Text; set => this.RaiseAndSetIfChanged(ref _messageTextBlock_Text, value); }

		/// <summary>
		/// Invoked when the window should be closed.
		/// </summary>
		public event EventHandler? OnRequestClose;

		public void OkButton_Clicked()
		{
			try { OnRequestClose!(this, new EventArgs()); }
			catch (NullReferenceException) { throw; }
		} 

		/// <summary>
		/// Creates a new MessageBoxViewModel.
		/// </summary>
		/// <param name="message"> The message to be displayed. </param>
		public MessageBoxViewModel(string message)
		{
			_messageTextBlock_Text = message;
		}
	}
}