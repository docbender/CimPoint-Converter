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

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CimPointConv
{
    public enum Format : int
    {
        WHATEVER,
        CIM75,
        CIM82,
        CIM95,
        CIM115,
        CIM120,
        IGNITION
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

        public CimplicityPoint Clone()
        {
            return (CimplicityPoint)this.MemberwiseClone();
        }

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

        public bool Equals(CimplicityPoint obj)
        {
            return PT_ID == obj.PT_ID &&
                ACCESS == obj.ACCESS &&
                ACCESS_FILTER == obj.ACCESS_FILTER &&
                ADDR == obj.ADDR &&
                ADDR_OFFSET == obj.ADDR_OFFSET &&
                ADDR_TYPE == obj.ADDR_TYPE &&
                ALM_ENABLE == obj.ALM_ENABLE &&
                ALM_CLASS == obj.ALM_CLASS &&
                ALM_CRITERIA == obj.ALM_CRITERIA &&
                ALM_DEADBAND == obj.ALM_DEADBAND &&
                ALM_DELAY == obj.ALM_DELAY &&
                ALM_HIGH_1 == obj.ALM_HIGH_1 &&
                ALM_HIGH_2 == obj.ALM_HIGH_2 &&
                ALM_HLP_FILE == obj.ALM_HLP_FILE &&
                ALM_LOW_1 == obj.ALM_LOW_1 &&
                ALM_LOW_2 == obj.ALM_LOW_2 &&
                ALM_MSG == obj.ALM_MSG &&
                ALM_ROUTE_OPER == obj.ALM_ROUTE_OPER &&
                ALM_ROUTE_SYSMGR == obj.ALM_ROUTE_SYSMGR &&
                ALM_ROUTE_USER == obj.ALM_ROUTE_USER &&
                ALM_SEVERITY == obj.ALM_SEVERITY &&
                ALM_STR == obj.ALM_STR &&
                ALM_TYPE == obj.ALM_TYPE &&
                ALM_UPDATE_VALUE == obj.ALM_UPDATE_VALUE &&
                ANALOG_DEADBAND == obj.ANALOG_DEADBAND &&
                BFR_COUNT == obj.BFR_COUNT &&
                BFR_DUR == obj.BFR_DUR &&
                BFR_EVENT_PERIOD == obj.BFR_EVENT_PERIOD &&
                BFR_EVENT_PT_ID == obj.BFR_EVENT_PT_ID &&
                BFR_EVENT_TYPE == obj.BFR_EVENT_TYPE &&
                BFR_EVENT_UNITS == obj.BFR_EVENT_UNITS &&
                BFR_GATE_COND == obj.BFR_GATE_COND &&
                BFR_SYNC_TIME == obj.BFR_SYNC_TIME &&
                CALC_TYPE == obj.CALC_TYPE &&
                CONV_LIM_HIGH == obj.CONV_LIM_HIGH &&
                CONV_LIM_LOW == obj.CONV_LIM_LOW &&
                CONV_TYPE == obj.CONV_TYPE &&
                DELAY_LOAD == obj.DELAY_LOAD &&
                DESC == obj.DESC &&
                DEVIATION_PT == obj.DEVIATION_PT &&
                DEVICE_ID == obj.DEVICE_ID &&
                DISP_LIM_HIGH == obj.DISP_LIM_HIGH &&
                DISP_LIM_LOW == obj.DISP_LIM_LOW &&
                DISP_TYPE == obj.DISP_TYPE &&
                DISP_WIDTH == obj.DISP_WIDTH &&
                ELEMENTS == obj.ELEMENTS &&
                ENG_UNITS == obj.ENG_UNITS &&
                ENUM_ID == obj.ENUM_ID &&
                EQUATION == obj.EQUATION &&
                EXTRA == obj.EXTRA &&
                FW_CONV_EQ == obj.FW_CONV_EQ &&
                GR_SCREEN == obj.GR_SCREEN &&
                INIT_VAL == obj.INIT_VAL &&
                JUSTIFICATION == obj.JUSTIFICATION &&
                LEVEL == obj.LEVEL &&
                LOCAL == obj.LOCAL &&
                LOG_DATA == obj.LOG_DATA &&
                MAX_STACKED == obj.MAX_STACKED &&
                MEASUREMENT_UNIT_ID == obj.MEASUREMENT_UNIT_ID &&
                MISC_FLAGS == obj.MISC_FLAGS &&
                POLL_AFTER_SET == obj.POLL_AFTER_SET &&
                PRECISION == obj.PRECISION &&
                PROC_ID == obj.PROC_ID &&
                PTMGMT_PROC_ID == obj.PTMGMT_PROC_ID &&
                PT_ENABLED == obj.PT_ENABLED &&
                PT_ORIGIN == obj.PT_ORIGIN &&
                PT_SET_INTERVAL == obj.PT_SET_INTERVAL &&
                PT_SET_TIME == obj.PT_SET_TIME &&
                PT_TYPE == obj.PT_TYPE &&
                RANGE_HIGH == obj.RANGE_HIGH &&
                RANGE_LOW == obj.RANGE_LOW &&
                RAW_LIM_HIGH == obj.RAW_LIM_HIGH &&
                RAW_LIM_LOW == obj.RAW_LIM_LOW &&
                RESET_COND == obj.RESET_COND &&
                RESET_PT == obj.RESET_PT &&
                RESOURCE_ID == obj.RESOURCE_ID &&
                REV_CONV_EQ == obj.REV_CONV_EQ &&
                ROLLOVER_VAL == obj.ROLLOVER_VAL &&
                SAFETY_PT == obj.SAFETY_PT &&
                SAMPLE_INTV == obj.SAMPLE_INTV &&
                SAMPLE_INTV_UNIT == obj.SAMPLE_INTV_UNIT &&
                SCAN_RATE == obj.SCAN_RATE &&
                SETPOINT_HIGH == obj.SETPOINT_HIGH &&
                SETPOINT_LOW == obj.SETPOINT_LOW &&
                TIME_OF_DAY == obj.TIME_OF_DAY &&
                TRIG_CK_PT == obj.TRIG_CK_PT &&
                TRIG_PT == obj.TRIG_PT &&
                TRIG_REL == obj.TRIG_REL &&
                TRIG_VAL == obj.TRIG_VAL &&
                UAFSET == obj.UAFSET &&
                UPDATE_CRITERIA == obj.UPDATE_CRITERIA &&
                VARIANCE_VAL == obj.VARIANCE_VAL &&
                VARS == obj.VARS;
        }
    }
}
