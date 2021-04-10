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
    class CimplicityPoint95 : CimplicityPoint82
    {
        public int ALARM_OFF_DELAY_HI { get; set; }
        public int ALARM_OFF_DELAY_HIHI { get; set; }
        public int ALARM_OFF_DELAY_LO { get; set; }
        public int ALARM_OFF_DELAY_LOLO { get; set; }
        public string ALARM_OFF_DELAY_UNIT_HI { get; set; }
        public string ALARM_OFF_DELAY_UNIT_HIHI { get; set; }
        public string ALARM_OFF_DELAY_UNIT_LO { get; set; }
        public string ALARM_OFF_DELAY_UNIT_LOLO { get; set; }
        public string ALM_CRITERIA_EX { get; set; }
        public string ALM_OFF_DELAY { get; set; }
        public int LOG_DATA_HISTORIAN { get; set; }
       

        public CimplicityPoint95 Clone()
        {
            return (CimplicityPoint95)this.MemberwiseClone();
        }
    }
}
