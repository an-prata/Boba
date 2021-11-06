using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;

namespace Boba.AvaloniaDesktop.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		private string _applicationTextBlock_Text;
		private string _usernameTextBlock_Text;
		private string _passwordTextBlock_Text;
		private int _passwordEntriesListBox_SelectedIndex;
		private ObservableCollection<string> _passwordEntriesListBox_Items;

		public string ApplicationTextBlock_Text { get => _applicationTextBlock_Text; set => this.RaiseAndSetIfChanged(ref _applicationTextBlock_Text, value); }
		public string UsernameTextBlock_Text { get => _usernameTextBlock_Text; set => this.RaiseAndSetIfChanged(ref _applicationTextBlock_Text, value); }
		public string PasswordTextBlock_Text { get => _passwordTextBlock_Text; set => this.RaiseAndSetIfChanged(ref _passwordTextBlock_Text, value); }
		public int PasswordEntriesListBox_SelectedIndex { get => _passwordEntriesListBox_SelectedIndex; set => this.RaiseAndSetIfChanged(ref _passwordEntriesListBox_SelectedIndex, value); }
		public ObservableCollection<string> PasswordEntriesListBox_Items { get => _passwordEntriesListBox_Items; set => this.RaiseAndSetIfChanged(ref _passwordEntriesListBox_Items, value); }

		public void AddEntryButton_Clicked()
		{
			PasswordEntriesListBox_Items.Add("new entry");
		}

		public void RemoveEntryButton_Clicked()
		{
			try { PasswordEntriesListBox_Items.RemoveAt(PasswordEntriesListBox_SelectedIndex); }
			catch (ArgumentOutOfRangeException) { return; }
		}

		public MainWindowViewModel()
		{
			_passwordEntriesListBox_Items = new ObservableCollection<string> 
			{
				"yeet",
				"yeet",
				"yayeet"
			};

			_applicationTextBlock_Text = "Google.com";
			_usernameTextBlock_Text = "evanrileyoverman@gmail.com";
			_passwordTextBlock_Text = "**********";
		}
	}
}