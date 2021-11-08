using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Boba.AvaloniaDesktop.Views
{
    public partial class MessageBox : Window
    {
        public MessageBox()
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