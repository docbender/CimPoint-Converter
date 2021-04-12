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

namespace CimPointConv
{
    internal class ArgsOptions : ProcessorOptions
    {
        public string InputFile { get; set; }
        public string OutputFile { get; set; }

        public bool Overwrite { get; set; } = false;
        public bool PrintVersion { get; set; } = false;
        public bool PrintPointsCount { get; set; } = false;
        public bool PrintDevices { get; set; } = false;        
    }
}