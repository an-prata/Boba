// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Boba.PasswordManager
{
    public class PasswordEntryComparer : Comparer<PasswordEntry>
    {
        /// <summary>
		/// Compares two PasswordEntries for whichever should appear before the other
		/// in an alphabeticaly ordered list.
		/// </summary>
        public override int Compare(PasswordEntry x, PasswordEntry y) => string.Compare(x.Application, y.Application, StringComparison.InvariantCulture);
    }
}