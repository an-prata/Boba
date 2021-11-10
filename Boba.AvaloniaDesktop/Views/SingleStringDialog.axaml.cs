using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Boba.AvaloniaDesktop.Views
{
    public partial class SingleStringDialog : Window
    {
        public SingleStringDialog()
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