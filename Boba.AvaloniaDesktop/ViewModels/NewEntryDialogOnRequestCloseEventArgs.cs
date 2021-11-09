// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using Boba.AvaloniaDesktop.Views;

namespace Boba.AvaloniaDesktop.ViewModels
{
    public class NewEntryDialogOnRequestCloseEventArgs : EventArgs
    {
        public NewEntryDialogViewModel? NewEntryDialogViewModel { get; set; }
        public bool NoPrivateKey { get; set; }
    }
}