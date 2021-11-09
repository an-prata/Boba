// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using Eto.Forms;
using Boba.PasswordManager;
using Boba.PasswordManager.FileHandling;

namespace Boba.Desktop
{
    public partial class MainForm : Form
    {
        public ButtonMenuItem SaveKeysButtonMenuItem { get; set; }
        public ButtonMenuItem OpenKeysButtonMenuItem { get; set; }

        protected void SaveKeysButtonMenuItem_Clicked(object sender, EventArgs e)
        {
            using SaveFileDialog saveKeysDialog = new();

            saveKeysDialog.Title = "Save Public Key";
            saveKeysDialog.ShowDialog(this);
            if (saveKeysDialog.FileName == "" || saveKeysDialog.FileName == null) return;
            XmlHandler.SaveToFile(saveKeysDialog.FileName, CurrentPasswordLibrary.CryptoServiceProvider.ExportParameters(false));

            saveKeysDialog.Title = "Save Private Key";
            saveKeysDialog.ShowDialog(this);
            if (saveKeysDialog.FileName == "" || saveKeysDialog.FileName == null) return;
            XmlHandler.SaveToFile(saveKeysDialog.FileName, CurrentPasswordLibrary.CryptoServiceProvider.ExportParameters(true));
        }

        protected void OpenKeysButtonMenuItem_Clicked(object sender, EventArgs e)
        {
            OpenFileDialog openKeysDialog = new();
            openKeysDialog.ShowDialog(this);
            if (openKeysDialog.FileName == "" || openKeysDialog.FileName == null) return;
            CurrentPasswordLibrary.CryptoServiceProvider = new RSACryptoServiceProvider();
            CurrentPasswordLibrary.CryptoServiceProvider.ImportParameters(XmlHandler.ReadFromFile<RSAParameters>(openKeysDialog.FileName));
        }
    }
}
