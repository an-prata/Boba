// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace Boba.Desktop
{	
	public class AddPasswordEntryDialog : Dialog
	{
		private Button CloseButton { get; set; }

		private TextBox ApplicationTextBox { get; set; }
		private TextBox UsernameTextBox { get; set; }
		private PasswordBox PasswordTextBox { get; set; }

		public string Application { get; private set; }
		public string Username { get; private set; }
		public byte[] Password { get; private set; }

		public AddPasswordEntryDialog()
		{
			XamlReader.Load(this);
			CloseButton.Click += HandleCloseButtonClicked;
			ApplicationTextBox.TextChanged += HandleApplicationTextChanged;
			UsernameTextBox.TextChanged += HandleUsernameTextChanged;
			PasswordTextBox.TextChanged += HandlePasswordTextChanged;
		}

		protected void HandleCloseButtonClicked(object sender, EventArgs e) => Close();

		protected void HandleApplicationTextChanged(object sender, EventArgs e) => Application = ApplicationTextBox.Text;

		protected void HandleUsernameTextChanged(object sender, EventArgs e) => Username = UsernameTextBox.Text;

		protected void HandlePasswordTextChanged(object sender, EventArgs e) => Password = Encoding.UTF8.GetBytes(PasswordTextBox.Text);
	}
}
