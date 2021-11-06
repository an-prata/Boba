// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System.Text;
using System.Collections.Generic;

namespace Boba.PasswordManager
{
    public class PasswordLibraryComparer : Comparer<PasswordLibrary>
    {
        /// <summary>
		/// Compares two PasswordEntries for whichever should appear before the other
		/// in an alphabeticaly ordered list.
		/// </summary>
        public override int Compare(PasswordLibrary x, PasswordLibrary y)
        {
			if (x == null)
			{
				if (y == null) { return 0; }
				else { return -1; }
			}
			else
            {
				if (y == null) { return 1; }
				else
                {
					byte[] bytesX = Encoding.UTF8.GetBytes(x.Name);
					byte[] bytesY = Encoding.UTF8.GetBytes(y.Name);

					for (int i = 0; i < (x.Name.Length > y.Name.Length ? y.Name.Length : x.Name.Length); i++)
                    {
						if (bytesX[i] > bytesY[i]) { return 1; }
						if (bytesY[i] > bytesX[i]) { return -1; }
					}

					if (x.Name.Length > y.Name.Length) { return 1; }
					if (y.Name.Length > x.Name.Length) { return -1; }
					return 0;
                }
            }
		}
    }
}