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
	public class PasswordDialogBox : Dialog
	{
		private Label PasswordsMatchLabel { get; set; }
		private Button CloseButton { get; set; }
		private PasswordBox PasswordTextBox { get; set; }
		private PasswordBox ConfirmPasswordTextBox { get; set; }
		public byte[] Result { get; private set; }

		public PasswordDialogBox(string title)
		{
			XamlReader.Load(this);
			Title = title;
			CloseButton.Click += CloseButton_Clicked;
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
				if (PasswordTextBox.Text == "")
				{
					MessageBox.Show("Please enter a password.");
					return;
				}

				Close();
			}
			else MessageBox.Show("Passwords do not match");
		}

		protected void PasswordTextBox_TextChanged(object sender, EventArgs e) => Result = Encoding.UTF8.GetBytes(PasswordTextBox.Text);
	}
}
