// This file is part of CimPoint-Converter.
//
// Copyright(C) 2021 Vita Tucek
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace CimPointConv
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AllocConsole();
        [DllImport("kernel32")]
        private static extern bool FreeConsole();

#if DEBUG
        bool console;
#endif

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                List<string> lowercaseArgs = e.Args.ToList().ConvertAll(x => x.ToLower());
                if (AllocConsole())
                {
                    Contr.Run(e.Args);
#if DEBUG
                    Console.WriteLine("Press Enter to exit");
                    Console.ReadLine();
#endif
                    FreeConsole();
                }
                Shutdown();
            }
            else
            {
#if DEBUG
                console = AllocConsole();
#endif
                base.OnStartup(e);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
#if DEBUG
            if (console)
                FreeConsole();
#endif
            base.OnExit(e);
        }
    }
}
