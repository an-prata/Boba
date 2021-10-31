// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;

namespace Boba.Desktop
{	
	public class SingleStringDialogBox : Dialog
	{	
		private TextBox StringTextBox { get; set; }
		private Button EnterButton { get; set; }
		public string Result { get; set; }

		public SingleStringDialogBox(string name)
		{
			XamlReader.Load(this);
			Title = name;
            StringTextBox.TextChanged += StringTextBox_TextChanged;
		}

		private void EnterButton_Clicked(object sender, EventArgs e)
        {
			Close();
        }

        private void StringTextBox_TextChanged(object sender, EventArgs e)
        {
			if (StringTextBox.Text == "" || StringTextBox.Text == null) EnterButton.Enabled = false;
			else EnterButton.Enabled = true;
			Result = StringTextBox.Text;
        }
    }
}
