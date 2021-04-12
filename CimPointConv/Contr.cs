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
using System.Linq;
using System.Reflection;

namespace CimPointConv
{
    static class Contr
    {
        public static void Run(string[] args)
        {
            Console.WriteLine($"CimPoint Converter v.{Assembly.GetEntryAssembly().GetName().Version.ToString(3)}");
            Console.WriteLine($"CimPoint Converter converts CIMPLICITY points");
            Console.WriteLine();
            if (args.Contains("--help") || args.Contains("-h") || args.Contains("/?") || args.Contains("-?"))
            {
                PrintHelp();
                return;
            }

            ArgsOptions options;
            try
            {
                options = ArgsParser.Parse(args);
            }
            catch (ArgumentException ex)
            {
                Console.Write(ex.Message);
                Console.WriteLine(" See --help for usage info.");
                return;
            }
            catch (Exception ex)
            {
                Console.Write("Error parsing arguments. ");
                Console.WriteLine(ex.Message);
                return;
            }

            if(!System.IO.File.Exists(options.InputFile))
            {
                Console.WriteLine($"File {options.InputFile} does not exist");
                return;
            }

            if (!string.IsNullOrEmpty(options.OutputFile) && !System.IO.File.Exists(options.OutputFile))
            {
                Console.WriteLine($"File {options.OutputFile} does not exist");
                return;
            }

            Processor p = new Processor();
            Console.WriteLine($"Loading data from file {options.InputFile}...");
            if (!p.Load(options.InputFile).Result)
            {
                Console.Write("  ");
                Console.WriteLine(p.Exception.Message);
                return;
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage: cpc [FILE] [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("  --help           Display this help");
            Console.WriteLine("  -v               Print file format version");
            Console.WriteLine("  -c               Print points count");
            Console.WriteLine("  -d               Print all devices");
            Console.WriteLine("  -o FILE          Write result into file");
            Console.WriteLine("  --overwrite      Overwrite existing file");
            Console.WriteLine("  -fp FILTER       Filter by points");
            Console.WriteLine("  -fa FILTER       Filter by address");
            Console.WriteLine("  -fd FILTER       Filter by device name");
            Console.WriteLine("  -fr              Filter using regular expressions");
            Console.WriteLine("  -rp FROM TO      Rename points");
            Console.WriteLine("  -ra FROM TO      Rename address");
            Console.WriteLine("  -rd FROM TO      Rename device");
            Console.WriteLine("  -rr              Rename using regular expressions");
            Console.WriteLine("  -cv              Convert device points into virtual points");
            Console.WriteLine("  -iv MODE         Initialize virtual points. Supported modes:");
            Console.WriteLine("                     NI - not initialized, I - initialized,");
            Console.WriteLine("                     SA - saved, SI - saved and initialized");
            Console.WriteLine("  -pe              Enable points");
            Console.WriteLine("  -pd              Disable points");
            Console.WriteLine("  -ad              Disable alarms");
            Console.WriteLine("  -ro              Set points read only");
            Console.WriteLine("  -rw              Set points read write");
            Console.WriteLine("  -ee              Enable enterprise points");
            Console.WriteLine("  -ed              Disable enterprise points");
            Console.WriteLine("  -le              Enable points log data");
            Console.WriteLine("  -ld              Disable points log data");
            Console.WriteLine("  -pe              Enable poll after set");
            Console.WriteLine("  -pd              Disable poll after set");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  cpc points.txt -v    # Print file format version");
            Console.WriteLine("  cpc points.txt -o out.txt -cv    # Convert to virtual points and save into out.txt file");
            Console.WriteLine("  cpc points.txt -o out.txt -fp SYS%.EN -ad     # Filter point by name and disable alarms");
            Console.WriteLine();
        }
    }
}
