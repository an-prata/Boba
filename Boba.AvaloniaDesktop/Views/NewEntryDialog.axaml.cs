// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Boba.AvaloniaDesktop.ViewModels;

namespace Boba.AvaloniaDesktop.Views
{
    public partial class NewEntryDialog : Window
    {
        public NewEntryDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}