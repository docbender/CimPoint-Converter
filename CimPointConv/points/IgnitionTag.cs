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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CimPointConv
{
    internal class IgnitionTag
    {
        public string name { get; private set; }
        public string tagType { get; private set; }
        public string dataType { get; private set; }
        public string valueSource { get; private set; }
        public string sourceTagPath { get; private set; }
        public string opcServer { get; private set; }
        public string opcItemPath { get; private set; }
        public string documentation { get; private set; }
        public string tooltip { get; private set; }
        public string formatString { get; private set; }
        public bool enabled { get; private set; }
        public string engUnit { get; private set; }
        public double engLow { get; private set; }
        public double engHigh { get; private set; }
        public string engLimitMode { get; private set; }
        public string expression { get; private set; }
        public double rawLow { get; private set; }
        public double rawHigh { get; private set; }
        public string scaleMode { get; private set; }
        public double scaledLow { get; private set; }
        public double scaledHigh { get; private set; }
        public bool readOnly { get; private set; }
        public double deadband { get; private set; }
        public bool historyEnabled { get; private set; }
        public IgnitionAlarm[] alarms { get; private set; }

        static List<IgnitionAlarm> alarmList = new List<IgnitionAlarm>();
        private string _conversionError;
        public void SetConversionError(string text)
        {
            if (string.IsNullOrEmpty(_conversionError))
                _conversionError = $"{name}: {text}";
            else
                _conversionError += $"{Environment.NewLine}  {text}";
        }
        public string GetConversionError()
        {
            return _conversionError;
        }

        public bool HasConversionError()
        {
            return !string.IsNullOrEmpty(_conversionError);
        }

        internal static IgnitionTag? Create(CimplicityPoint point)
        {
            var tag = new IgnitionTag();

            tag.name = point.PT_ID;
            tag.tagType = "AtomicTag";
            tag.enabled = point.PT_ENABLED == 1;
            tag.readOnly = (point.ACCESS == "R");

            switch (point.PT_ORIGIN)
            {
                case "D":
                    tag.valueSource = "opc";
                    tag.opcServer = "Ignition OPC UA Server";
                    tag.opcItemPath = point.ADDR;
                    break;
                case "R":
                    tag.valueSource = "expr";
                    tag.expression = evalExpression(point.CALC_TYPE, point.EQUATION);
                    break;
                case "G":
                    tag.valueSource = "memory";
                    break;
                default:
                    tag.SetConversionError("Unsupported PT_ORIGIN");
                    return null;
            }

            switch (point.PT_TYPE)
            {
                case "BOOL":
                    tag.dataType = point.ELEMENTS > 1 ? "BooleanArray" : "Boolean";
                    break;
                case "SINT":
                case "USINT":
                    tag.dataType = point.ELEMENTS > 1 ? "Int1Array" : "Int1";
                    break;
                case "INT":
                case "UINT":
                    tag.dataType = point.ELEMENTS > 1 ? "Int2Array" : "Int2";
                    break;
                case "DINT":
                case "UDINT":
                    tag.dataType = point.ELEMENTS > 1 ? "Int4Array" : "Int4";
                    break;
                case "QINT":
                case "UQINT":
                    tag.dataType = point.ELEMENTS > 1 ? "Int8Array" : "Int8";
                    break;
                case "STRING":
                    tag.dataType = "String";
                    break;
                case "REAL":
                    tag.dataType = point.ELEMENTS > 1 ? "Float4Array" : "Float4";
                    break;
                default:
                    tag.SetConversionError($"Unsupported PT_TYPE={point.PT_TYPE}");
                    return null;
            }

            // numeric properties
            if (!string.IsNullOrEmpty(point.ANALOG_DEADBAND) && double.TryParse(point.ANALOG_DEADBAND, out double deadband))
                if (deadband != 0.0)
                    tag.deadband = deadband;

            // NO None, LC Linear conversion, CS  Custom conversion
            if (point.CONV_TYPE == "LC")
            {
                tag.scaleMode = "Linear";
                try
                {
                    tag.rawLow = double.Parse(point.RAW_LIM_LOW);
                    tag.rawHigh = double.Parse(point.RAW_LIM_HIGH);
                    tag.scaledLow = double.Parse(point.CONV_LIM_LOW);
                    tag.scaledHigh = double.Parse(point.CONV_LIM_HIGH);
                }
                catch (Exception ex)
                {
                    tag.SetConversionError($"ERROR - Linear conversion exception ({ex}). Edit tag manually.");
                }
            }
            else if (point.CONV_TYPE == "CS")
            {
                tag.SetConversionError($"Unsupported CONV_TYPE={point.CONV_TYPE} - Custom conversion is not supported by Ignition. Edit tag manually.");
            }

            if (!string.IsNullOrEmpty(point.ENG_UNITS))
                tag.engUnit = point.ENG_UNITS;
            if (!string.IsNullOrEmpty(point.RANGE_LOW) && double.TryParse(point.RANGE_LOW, out double engLow))
                tag.engLow = engLow;
            if (!string.IsNullOrEmpty(point.RANGE_HIGH) && double.TryParse(point.RANGE_HIGH, out double engHigh))
                tag.engHigh = engHigh;
            if (tag.engLow != 0D || tag.engHigh != 0D)
                tag.engLimitMode = "Clamp_Both";

            if (point.PT_TYPE == "REAL" &&
                (point.DISP_TYPE != "f" || !string.IsNullOrEmpty(point.DISP_WIDTH) || !string.IsNullOrEmpty(point.PRECISION)))
            {
                int precision = 2;
                if (point.PRECISION.Length > 1)
                    precision = int.Parse(point.PRECISION);
                if (point.DISP_WIDTH.Length > 1 && point.DISP_WIDTH.StartsWith('0'))
                    tag.formatString = $"{new string('0', int.Parse(point.DISP_WIDTH) - precision)}.{new string('#', precision)}";
                else if (point.DISP_WIDTH.Length > 1)
                    tag.formatString = $"{new string('#', int.Parse(point.DISP_WIDTH) - 1 - precision)}0.{new string('#', precision)}";

                if (point.DISP_TYPE != "f")
                    tag.formatString += "E0";
            }
            else
            {
                if (point.DISP_WIDTH.Length > 1 && point.DISP_WIDTH.StartsWith('0'))
                    tag.formatString = $"{new string('0', int.Parse(point.DISP_WIDTH))}";
                else if (point.DISP_WIDTH.Length > 1)
                    tag.formatString = $"{new string('#', int.Parse(point.DISP_WIDTH) - 1)}0";
            }
            // documentation
            if (!string.IsNullOrEmpty(point.DESC))
                tag.documentation = point.DESC;
            // historian
            if (point.LOG_DATA == 1)
                tag.historyEnabled = true;
            // alarms
            if (!string.IsNullOrEmpty(point.ALM_MSG))
            {
                alarmList.Clear();
                // rate
                if (point.ALM_CRITERIA == "ROC")
                {
                    tag.SetConversionError($"Rate of change is not supported by Ignition. Edit tag manually.");
                }
                else if (point.ALM_CRITERIA == "ONU")
                {
                    alarmList.Add(new IgnitionAlarm()
                    {
                        name = point.PT_ID,
                        mode = "AnyChange",
                        label = transAlmMsg(point.ALM_MSG),
                        priority = transAlmClass(point.ALM_CLASS, tag)
                    });
                }
                else if (point.ALM_CRITERIA == "DEV")
                {
                    if (!string.IsNullOrEmpty(point.ALM_HIGH_2))
                    {
                        alarmList.Add(new IgnitionAlarm()
                        {
                            name = point.PT_ID,
                            mode = "AboveValue",
                            label = transAlmMsg(point.ALM_MSG),
                            priority = transAlmClass(point.ALM_CLASS, tag),
                            setpointA = $"[.]{point.DEVIATION_PT} + {point.ALM_HIGH_2}",
                            anyChange = point.ALM_UPDATE_VALUE == 1
                        });
                    }
                    if (!string.IsNullOrEmpty(point.ALM_HIGH_1))
                    {
                        alarmList.Add(new IgnitionAlarm()
                        {
                            name = point.PT_ID,
                            mode = "AboveValue",
                            label = transAlmMsg(point.ALM_MSG),
                            priority = transAlmClass(point.ALM_CLASS, tag),
                            setpointA = $"[.]{point.DEVIATION_PT} + {point.ALM_HIGH_1}",
                            anyChange = point.ALM_UPDATE_VALUE == 1
                        });
                    }
                    if (!string.IsNullOrEmpty(point.ALM_LOW_2))
                    {
                        alarmList.Add(new IgnitionAlarm()
                        {
                            name = point.PT_ID,
                            mode = "BelowValue",
                            label = transAlmMsg(point.ALM_MSG),
                            priority = transAlmClass(point.ALM_CLASS, tag),
                            setpointA = $"[.]{point.DEVIATION_PT} + {point.ALM_LOW_2}",
                            anyChange = point.ALM_UPDATE_VALUE == 1
                        });
                    }
                    if (!string.IsNullOrEmpty(point.ALM_LOW_1))
                    {
                        alarmList.Add(new IgnitionAlarm()
                        {
                            name = point.PT_ID,
                            mode = "BelowValue",
                            label = transAlmMsg(point.ALM_MSG),
                            priority = transAlmClass(point.ALM_CLASS, tag),
                            setpointA = $"[.]{point.DEVIATION_PT} + {point.ALM_LOW_1}",
                            anyChange = point.ALM_UPDATE_VALUE == 1
                        });
                    }
                }
                else if (point.ALM_CRITERIA == "AEQ")
                {
                    if (!string.IsNullOrEmpty(point.ALM_HIGH_2))
                    {
                        alarmList.Add(new IgnitionAlarm()
                        {
                            name = point.PT_ID,
                            mode = "Equality",
                            label = transAlmMsg(point.ALM_MSG),
                            priority = transAlmClass(point.ALM_CLASS, tag),
                            setpointA = $"{point.ALM_HIGH_2}",
                            anyChange = point.ALM_UPDATE_VALUE == 1
                        });
                    }
                    if (!string.IsNullOrEmpty(point.ALM_HIGH_1))
                    {
                        alarmList.Add(new IgnitionAlarm()
                        {
                            name = point.PT_ID,
                            mode = "Equality",
                            label = transAlmMsg(point.ALM_MSG),
                            priority = transAlmClass(point.ALM_CLASS, tag),
                            setpointA = $"{point.ALM_HIGH_1}",
                            anyChange = point.ALM_UPDATE_VALUE == 1
                        });
                    }
                    if (!string.IsNullOrEmpty(point.ALM_LOW_2))
                    {
                        alarmList.Add(new IgnitionAlarm()
                        {
                            name = point.PT_ID,
                            mode = "Equality",
                            label = transAlmMsg(point.ALM_MSG),
                            priority = transAlmClass(point.ALM_CLASS, tag),
                            setpointA = $"{point.ALM_LOW_2}",
                            anyChange = point.ALM_UPDATE_VALUE == 1
                        });
                    }
                    if (!string.IsNullOrEmpty(point.ALM_LOW_1))
                    {
                        alarmList.Add(new IgnitionAlarm()
                        {
                            name = point.PT_ID,
                            mode = "Equality",
                            label = transAlmMsg(point.ALM_MSG),
                            priority = transAlmClass(point.ALM_CLASS, tag),
                            setpointA = $"{point.ALM_LOW_1}",
                            anyChange = point.ALM_UPDATE_VALUE == 1
                        });
                    }
                }
                else if (point.ALM_CRITERIA == "ABS")
                {
                    if (point.PT_TYPE == "BOOL")
                    {

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(point.ALM_HIGH_2))
                        {
                            alarmList.Add(new IgnitionAlarm()
                            {
                                name = point.PT_ID,
                                mode = "AboveValue",
                                label = transAlmMsg(point.ALM_MSG),
                                priority = transAlmClass(point.ALM_CLASS, tag),
                                setpointA = $"{point.ALM_HIGH_2}",
                                anyChange = point.ALM_UPDATE_VALUE == 1
                            });
                        }
                        if (!string.IsNullOrEmpty(point.ALM_HIGH_1))
                        {
                            alarmList.Add(new IgnitionAlarm()
                            {
                                name = point.PT_ID,
                                mode = "AboveValue",
                                label = transAlmMsg(point.ALM_MSG),
                                priority = transAlmClass(point.ALM_CLASS, tag),
                                setpointA = $"{point.ALM_HIGH_1}",
                                anyChange = point.ALM_UPDATE_VALUE == 1
                            });
                        }
                        if (!string.IsNullOrEmpty(point.ALM_LOW_2))
                        {
                            alarmList.Add(new IgnitionAlarm()
                            {
                                name = point.PT_ID,
                                mode = "BelowValue",
                                label = transAlmMsg(point.ALM_MSG),
                                priority = transAlmClass(point.ALM_CLASS, tag),
                                setpointA = $"{point.ALM_LOW_2}",
                                anyChange = point.ALM_UPDATE_VALUE == 1
                            });
                        }
                        if (!string.IsNullOrEmpty(point.ALM_LOW_1))
                        {
                            alarmList.Add(new IgnitionAlarm()
                            {
                                name = point.PT_ID,
                                mode = "BelowValue",
                                label = transAlmMsg(point.ALM_MSG),
                                priority = transAlmClass(point.ALM_CLASS, tag),
                                setpointA = $"{point.ALM_LOW_1}",
                                anyChange = point.ALM_UPDATE_VALUE == 1
                            });
                        }
                    }
                }
                else
                {
                    tag.SetConversionError($"Unsupported alarm criteria={point.ALM_CRITERIA}. Edit tag manually.");
                }

                if (alarmList.Any())
                {
                    if (!string.IsNullOrEmpty(point.ALM_DEADBAND) && double.TryParse(point.ANALOG_DEADBAND, out double almdeadband))
                        alarmList.ForEach((alarm) => alarm.deadband = almdeadband);

                    tag.alarms = alarmList.ToArray();
                }
            }

            return tag;
        }

        /// <summary>
        /// Transform alarm class
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private static string transAlmClass(string className, IgnitionTag tag)
        {
            switch (className)
            {
                case "HIGH":
                    return "High";
                case "MED":
                    return "Medium";
                case "LOW":
                    return "Low";
            }

            if (!(className == "Diagnostic" || className == "Low" || className == "Medium"
                    || className == "High" || className == "Critical"))
                tag.SetConversionError($"Alarm class {className} is not supported by Ignition. Edit tag manually.");

            return className;
        }

        /// <summary>
        /// Transform alarm message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string transAlmMsg(string message)
        {
            return message;
        }

        /// <summary>
        /// Evaluate expression
        /// </summary>
        /// <param name="calcType"></param>
        /// <param name="equ"></param>
        /// <returns></returns>
        private static string evalExpression(string calcType, string equ)
        {
            throw new NotImplementedException();

            // CALC_TYPE
            //   EQU Equation
            //   DAC Delta Accumulator
            //   VAC Value Accumulator
            //   AVG Average
            //   MAX Maxim
            //   MIN Minimumum
            //   T_C Timer/ Counter
            //   HST Histogram
            //   T_H Transition High Accumulator
            //   EWO Equation with Override
        }
    }
}
