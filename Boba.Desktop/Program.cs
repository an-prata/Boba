// Boba Password Manager (https://github.com/an-prata/Boba)
// Copyright (c) 2021 Evan Overman (https://github.com/an-prata)
// Licensed under the MIT License.

using System;
using Eto.Forms;

namespace Boba.Desktop
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            new Application(Eto.Platform.Detect).Run(new MainForm());
        }
    }
}
