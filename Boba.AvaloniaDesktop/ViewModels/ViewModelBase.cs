﻿// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using ReactiveUI;

namespace Boba.AvaloniaDesktop.ViewModels
{
	public class ViewModelBase : ReactiveObject
	{
		protected const string PasswordPlaceholder = "••••••••••••";
		protected const string PasswordsDontMatchMessage = "Passwords Dont Match! >:(";
		protected const string NoPrivateKeyMessage = "No key to decrypt password, please import a private key. \n(Keys > Import Private Key).";
	}
}
