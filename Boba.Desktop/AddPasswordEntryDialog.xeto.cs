// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Text;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace Boba.Desktop
{	
	public class AddPasswordEntryDialog : Dialog
	{
		private Label PasswordsMatchLabel { get; set; }
		private Button CloseButton { get; set; }
		private TextBox ApplicationTextBox { get; set; }
		private TextBox UsernameTextBox { get; set; }
		private PasswordBox PasswordTextBox { get; set; }
		private PasswordBox ConfirmPasswordTextBox { get; set; }

		public string Application { get; private set; }
		public string Username { get; private set; }
		public byte[] Password { get; private set; }

		public AddPasswordEntryDialog()
		{
			XamlReader.Load(this);
			CloseButton.Click += CloseButton_Clicked;
			ApplicationTextBox.TextChanged += ApplicationTextBox_TextChanged;
			UsernameTextBox.TextChanged += UsernameTextBox_TextChanged;
			PasswordTextBox.TextChanged += PasswordTextBox_TextChanged;

			PasswordTextBox.TextChanged += PasswordTextBoxes_TextChanged;
			ConfirmPasswordTextBox.TextChanged += PasswordTextBoxes_TextChanged;
		}

		protected void PasswordTextBoxes_TextChanged(object sender, EventArgs e)
		{
			if (PasswordTextBox.Text == ConfirmPasswordTextBox.Text) 
			{
				PasswordsMatchLabel.TextColor = Color.FromArgb(99, 180, 86);
				PasswordsMatchLabel.Text = "Passwords Match!";
				return;
			}

			PasswordsMatchLabel.TextColor = Color.FromArgb(103, 47, 47);
			PasswordsMatchLabel.Text = "Passwords Dont Match";
		}

		protected void CloseButton_Clicked(object sender, EventArgs e)
		{
			if (PasswordTextBox.Text == ConfirmPasswordTextBox.Text)
			{
				if (ApplicationTextBox.Text == "" || ApplicationTextBox.Text == null) ApplicationTextBox.Text = "No Application";
				if (UsernameTextBox.Text == "" || UsernameTextBox.Text == null) UsernameTextBox.Text = "No Username";
				if (PasswordTextBox.Text == "")
				{
					MessageBox.Show("Please enter a password.");
					return;
				}

				Close();
			}
			else MessageBox.Show("Passwords do not match");
		}

		protected void ApplicationTextBox_TextChanged(object sender, EventArgs e) => Application = ApplicationTextBox.Text;

		protected void UsernameTextBox_TextChanged(object sender, EventArgs e) => Username = UsernameTextBox.Text;

		protected void PasswordTextBox_TextChanged(object sender, EventArgs e) => Password = Encoding.UTF8.GetBytes(PasswordTextBox.Text);
	}
}
