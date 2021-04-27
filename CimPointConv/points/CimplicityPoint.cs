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
using System.Text.Json;
using System.Reflection;

namespace CimPointConv
{
    public enum Format : int
    {
        WHATEVER,
        CIM75,
        CIM82,
        CIM95
    };

    public class CimplicityPoint
    {
        public string PT_ID { get; set; }
        public string ACCESS { get; set; }
        public string ACCESS_FILTER { get; set; }
        public string ADDR { get; set; }
        public string ADDR_OFFSET { get; set; }
        public string ADDR_TYPE { get; set; }
        public string ALM_ENABLE { get; set; }
        public string ALM_CLASS { get; set; }
        public string ALM_CRITERIA { get; set; }
        public string ALM_DEADBAND { get; set; }
        public int ALM_DELAY { get; set; }
        public string ALM_HIGH_1 { get; set; }
        public string ALM_HIGH_2 { get; set; }
        public string ALM_HLP_FILE { get; set; }
        public string ALM_LOW_1 { get; set; }
        public string ALM_LOW_2 { get; set; }
        public string ALM_MSG { get; set; }
        public string ALM_ROUTE_OPER { get; set; }
        public string ALM_ROUTE_SYSMGR { get; set; }
        public string ALM_ROUTE_USER { get; set; }
        public int ALM_SEVERITY { get; set; }
        public int ALM_STR { get; set; }
        public string ALM_TYPE { get; set; }
        public int ALM_UPDATE_VALUE { get; set; }
        public string ANALOG_DEADBAND { get; set; }
        public int BFR_COUNT { get; set; }
        public int BFR_DUR { get; set; }
        public int BFR_EVENT_PERIOD { get; set; }
        public string BFR_EVENT_PT_ID { get; set; }
        public int BFR_EVENT_TYPE { get; set; }
        public int BFR_EVENT_UNITS { get; set; }
        public int BFR_GATE_COND { get; set; }
        public int BFR_SYNC_TIME { get; set; }
        public string CALC_TYPE { get; set; }
        public string CONV_LIM_HIGH { get; set; }
        public string CONV_LIM_LOW { get; set; }
        public string CONV_TYPE { get; set; }
        public string DELAY_LOAD { get; set; }
        public string DESC { get; set; }
        public string DEVIATION_PT { get; set; }
        public string DEVICE_ID { get; set; }
        public string DISP_LIM_HIGH { get; set; }
        public string DISP_LIM_LOW { get; set; }
        public string DISP_TYPE { get; set; }
        public string DISP_WIDTH { get; set; }
        public int ELEMENTS { get; set; }
        public string ENG_UNITS { get; set; }
        public string ENUM_ID { get; set; }
        public string EQUATION { get; set; }
        public int EXTRA { get; set; }
        public string FW_CONV_EQ { get; set; }
        public string GR_SCREEN { get; set; }
        public string INIT_VAL { get; set; }
        public string JUSTIFICATION { get; set; }
        public int LEVEL { get; set; }
        public string LOCAL { get; set; }
        public int LOG_DATA { get; set; }
        public string MAX_STACKED { get; set; }
        public string MEASUREMENT_UNIT_ID { get; set; }
        public int MISC_FLAGS { get; set; }
        public string POLL_AFTER_SET { get; set; }
        public string PRECISION { get; set; }
        public string PROC_ID { get; set; }
        public string PTMGMT_PROC_ID { get; set; }
        public int PT_ENABLED { get; set; }
        public string PT_ORIGIN { get; set; }
        public string PT_SET_INTERVAL { get; set; }
        public string PT_SET_TIME { get; set; }
        public string PT_TYPE { get; set; }
        public string RANGE_HIGH { get; set; }
        public string RANGE_LOW { get; set; }
        public string RAW_LIM_HIGH { get; set; }
        public string RAW_LIM_LOW { get; set; }
        public string RESET_COND { get; set; }
        public string RESET_PT { get; set; }
        public string RESOURCE_ID { get; set; }
        public string REV_CONV_EQ { get; set; }
        public string ROLLOVER_VAL { get; set; }
        public string SAFETY_PT { get; set; }
        public int SAMPLE_INTV { get; set; }
        public string SAMPLE_INTV_UNIT { get; set; }
        public string SCAN_RATE { get; set; }
        public string SETPOINT_HIGH { get; set; }
        public string SETPOINT_LOW { get; set; }
        public string TIME_OF_DAY { get; set; }
        public string TRIG_CK_PT { get; set; }
        public string TRIG_PT { get; set; }
        public string TRIG_REL { get; set; }
        public string TRIG_VAL { get; set; }
        public string UAFSET { get; set; }
        public string UPDATE_CRITERIA { get; set; }
        public string VARIANCE_VAL { get; set; }
        public int VARS { get; set; }


        public T CloneTo<T>()
        {
            string jsonString = JsonSerializer.Serialize(this);
            var result = JsonSerializer.Deserialize<T>(jsonString);
            if (this is CimplicityPoint75 && result is CimplicityPoint82)
            {
                var src75 = this as CimplicityPoint75;
                var dst82 = result as CimplicityPoint82;
                dst82.ACK_TIMEOUT_HIHI = src75.ACK_TIMEOUT;
                dst82.DELETE_REQ_HIHI = src75.DELETE_REQ;
                dst82.LOG_ACK_HIHI = src75.LOG_ACK;
                dst82.LOG_DEL_HIHI = src75.LOG_DEL;
                dst82.LOG_GEN_HIHI = src75.LOG_GEN;
                dst82.LOG_RESET_HIHI = src75.LOG_RESET;
                dst82.REP_TIMEOUT_HIHI = src75.REP_TIMEOUT;
                dst82.RESET_ALLOWED_HIHI = src75.RESET_ALLOWED;
                dst82.RESET_TIMEOUT_HIHI = src75.RESET_TIMEOUT;
            }
            else if (typeof(T) == typeof(CimplicityPoint75) && this is CimplicityPoint82)
            {
                var src82 = this as CimplicityPoint82;
                var dst75 = result as CimplicityPoint75;
                dst75.ACK_TIMEOUT = src82.ACK_TIMEOUT_HIHI;
                dst75.DELETE_REQ = src82.DELETE_REQ_HIHI;
                dst75.LOG_ACK = src82.LOG_ACK_HIHI;
                dst75.LOG_DEL = src82.LOG_DEL_HIHI;
                dst75.LOG_GEN = src82.LOG_GEN_HIHI;
                dst75.LOG_RESET = src82.LOG_RESET_HIHI;
                dst75.REP_TIMEOUT = src82.REP_TIMEOUT_HIHI;
                dst75.RESET_ALLOWED = src82.RESET_ALLOWED_HIHI;
                dst75.RESET_TIMEOUT = src82.RESET_TIMEOUT_HIHI;
            }
            return result;
        }

        public int GetPropertiesCount()
        {
            return GetType().GetProperties().Length;
        }

        public static int GetPropertiesCount<T>()
        {
            return typeof(T).GetProperties().Length;
        }

        public static IEnumerable<string> GetPropertiesName<T>()
        {
            return typeof(T).GetProperties().Select(x => x.Name);
        }

        internal void SetColumn(string name, string value)
        {
            var property = GetType().GetProperties().First(x => x.Name.Equals(name));
            if (property.PropertyType == typeof(int))
            {
                if (value.Length == 0)
                    property.SetValue(this, 0);
                else
                    property.SetValue(this, int.Parse(value));
            }
            else
            {
                property.SetValue(this, value);
            }
        }
    }
}
