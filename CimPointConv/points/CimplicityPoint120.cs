// This file is part of CimPoint-Converter.
//
// Copyright(C) 2023 Vita Tucek
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
    class CimplicityPoint120 : CimplicityPoint115
    {
        public string ALM_ATTR_01 { get; set; }
        public string ALM_ATTR_02 { get; set; }
        public string ALM_ATTR_03 { get; set; }
        public string ALM_ATTR_04 { get; set; }
        public string ALM_ATTR_05 { get; set; }
        public string ALM_ATTR_06 { get; set; }
        public string ALM_ATTR_07 { get; set; }
        public string ALM_ATTR_08 { get; set; }
        public string ALM_ATTR_09 { get; set; }
        public string ALM_ATTR_10 { get; set; }
        public string ALM_ATTR_DESC_01 { get; set; }
        public string ALM_ATTR_DESC_02 { get; set; }
        public string ALM_ATTR_DESC_03 { get; set; }
        public string ALM_ATTR_DESC_04 { get; set; }
        public string ALM_ATTR_DESC_05 { get; set; }
        public string ALM_ATTR_DESC_06 { get; set; }
        public string ALM_ATTR_DESC_07 { get; set; }
        public string ALM_ATTR_DESC_08 { get; set; }
        public string ALM_ATTR_DESC_09 { get; set; }
        public string ALM_ATTR_DESC_10 { get; set; }

        public CimplicityPoint120 Clone()
        {
            return (CimplicityPoint120)this.MemberwiseClone();
        }
    }
}
