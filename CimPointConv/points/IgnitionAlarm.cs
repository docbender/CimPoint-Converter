// This file is part of CimPoint-Converter.
//
// Copyright(C) 2022 Vita Tucek
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
    internal class IgnitionAlarm
    {
        public double setpointA { get; set; } //": 1.0,
        public string mode { get; set; } //": "Equality",
        public string name { get; set; } //": "Bool alarm",
        public string label { get; set; } //": "",
        public string priority { get; set; } //": "Critical"
    }
}
