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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimPointConv
{
    class CimplicityPoint82 : CimplicityPoint
    {
        public string ACK_TIMEOUT_HI { get; set; }
        public string ACK_TIMEOUT_HIHI { get; set; }
        public string ACK_TIMEOUT_LO { get; set; }
        public string ACK_TIMEOUT_LOLO { get; set; }
        public string ALARM_CHANGEAPPROVAL { get; set; }
        public int ALARM_DELAY_HI { get; set; }
        public int ALARM_DELAY_HIHI { get; set; }
        public int ALARM_DELAY_LO { get; set; }
        public int ALARM_DELAY_LOLO { get; set; }
        public string ALARM_DELAY_UNIT_HI { get; set; }
        public string ALARM_DELAY_UNIT_HIHI { get; set; }
        public string ALARM_DELAY_UNIT_LO { get; set; }
        public string ALARM_DELAY_UNIT_LOLO { get; set; }
        public string ALARM_PUBLISH { get; set; }
        public string CHANGEAPPROVAL { get; set; }
        public string DELETE_REQ_HI { get; set; }
        public string DELETE_REQ_HIHI { get; set; }
        public string DELETE_REQ_LO { get; set; }
        public string DELETE_REQ_LOLO { get; set; }
        public string LOG_ACK_HI { get; set; }
        public string LOG_ACK_HIHI { get; set; }
        public string LOG_ACK_LO { get; set; }
        public string LOG_ACK_LOLO { get; set; }
        public string LOG_DEL_HI { get; set; }
        public string LOG_DEL_HIHI { get; set; }
        public string LOG_DEL_LO { get; set; }
        public string LOG_DEL_LOLO { get; set; }
        public string LOG_GEN_HI { get; set; }
        public string LOG_GEN_HIHI { get; set; }
        public string LOG_GEN_LO { get; set; }
        public string LOG_GEN_LOLO { get; set; }
        public string LOG_RESET_HI { get; set; }
        public string LOG_RESET_HIHI { get; set; }
        public string LOG_RESET_LO { get; set; }
        public string LOG_RESET_LOLO { get; set; }
        public string REP_TIMEOUT_HI { get; set; }
        public string REP_TIMEOUT_HIHI { get; set; }
        public string REP_TIMEOUT_LO { get; set; }
        public string REP_TIMEOUT_LOLO { get; set; }
        public string RESET_ALLOWED_HI { get; set; }
        public string RESET_ALLOWED_HIHI { get; set; }
        public string RESET_ALLOWED_LO { get; set; }
        public string RESET_ALLOWED_LOLO { get; set; }
        public string RESET_TIMEOUT_HI { get; set; }
        public string RESET_TIMEOUT_HIHI { get; set; }
        public string RESET_TIMEOUT_LO { get; set; }
        public string RESET_TIMEOUT_LOLO { get; set; }        
    }
}
