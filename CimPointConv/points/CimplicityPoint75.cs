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
    class CimplicityPoint75 : CimplicityPoint
    {
        public string ACK_TIMEOUT { get; set; }
        public string DELETE_REQ { get; set; }
        public string LOG_ACK { get; set; }
        public string LOG_DEL { get; set; }
        public string LOG_GEN { get; set; }
        public string LOG_RESET { get; set; }        
        public string REP_TIMEOUT { get; set; }
        public string RESET_ALLOWED { get; set; }
        public string RESET_TIMEOUT { get; set; }        
    }
}
