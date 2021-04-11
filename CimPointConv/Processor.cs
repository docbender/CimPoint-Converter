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
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Reflection;

namespace CimPointConv
{
    public class Processor : INotifyPropertyChanged
    {
        /// <summary> Line splited values </summary>
        private readonly List<string> _values = new List<string>(160);
        /// <summary> Text version representation </summary>
        private string _versionText = "-";
        /// <summary> Process exception </summary>
        public Exception Exception { get; private set; }
        /// <summary> Source points </summary>
        public List<CimplicityPoint> Points { get; } = new List<CimplicityPoint>(1000);
        /// <summary> Processed points </summary>
        public IEnumerable<CimplicityPoint> PointsProcesed;

        /// <summary>
        /// Point format version
        /// </summary>
        public CimFormat Version { get; private set; }

        /// <summary>
        /// Text version representation
        /// </summary>
        public string VersionText
        {
            get
            {
                return _versionText;
            }
            private set
            {
                _versionText = value;

                if (_versionText.Equals("7.5"))
                    Version = CimFormat.CIM75;
                else if (_versionText.Equals("8.2"))
                    Version = CimFormat.CIM82;
                else if (VersionText.Equals("9.5") || VersionText.StartsWith("10.") || VersionText.StartsWith("11."))
                    Version = CimFormat.CIM95;
                else
                    Version = CimFormat.WHATEVER;

                OnPropertyChanged("Version");
                OnPropertyChanged("VersionText");
            }
        }

        /// <summary>
        /// Source points count
        /// </summary>
        public int PointsCount { get => Points.Count; }

        /// <summary>
        /// Filtered points count
        /// </summary>
        public int PointsProcessedCount { get => PointsProcesed?.Count() ?? 0; }

        /// <summary>
        /// Clear data
        /// </summary>
        public void Clear()
        {
            VersionText = string.Empty;
            Exception = null;
            Points.Clear();
            PointsProcesed = null;
        }

        /// <summary>
        /// Load points from file
        /// </summary>
        /// <param name="file">Path to  file</param>
        /// <returns></returns>
        public async Task<bool> Load(string file)
        {
#if DEBUG
            Console.WriteLine($"Opening file {file}...");
#endif
            Clear();
            return await Task.Run(() =>
            {
                try
                {
                    //var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    bool hasHeader = false;
                    string[] _columnNames = null;

                    foreach (var line in File.ReadLines(file, Encoding.ASCII))
                    {
                        //comment
                        if (line.StartsWith("##  File created by"))
                        {
                            var match = Regex.Match(line, @"Ver\. (\d+\.\d)");
                            if (match.Success)
                                VersionText = match.Groups[1].Value;
                        }
                        else if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }
                        else
                        {
                            if (!hasHeader)
                            {
                                if (!line.StartsWith("PT_ID,"))
                                    throw new Exception("Header not found. Header should start with PT_ID column first.");

                                hasHeader = true;
                                _columnNames = line.Split(',');

                                if (!string.IsNullOrEmpty(VersionText))
                                {
                                    if (!(Version == CimFormat.CIM75 && _columnNames.Length == CimplicityPoint.GetPropertiesCount<CimplicityPoint75>()
                                        || Version == CimFormat.CIM82 && _columnNames.Length == CimplicityPoint.GetPropertiesCount<CimplicityPoint82>()
                                        || Version == CimFormat.CIM95 && _columnNames.Length == CimplicityPoint.GetPropertiesCount<CimplicityPoint95>()))
                                    {
                                        Exception = new Exception("Columns did not match with expected version from file header");
                                        VersionText = FindVersion(_columnNames);
                                    }
                                }
                                else
                                {
                                    VersionText = FindVersion(_columnNames);
                                }

                                if (string.IsNullOrEmpty(VersionText))
                                    throw new Exception("Unsupported CIMPLICITY file format");

                                continue;
                            }

                            CimplicityPoint point = null;
                            if (Version == CimFormat.CIM75)
                                point = new CimplicityPoint75();
                            else if (Version == CimFormat.CIM82)
                                point = new CimplicityPoint82();
                            else if (Version == CimFormat.CIM95)
                                point = new CimplicityPoint95();
                            else
                                throw new Exception("Unsupported CIMPLICITY version");

                            var values = SplitLine(line);
                            for (int j = 0; j < values.Count(); j++)
                            {
                                if (j >= _columnNames.Length)
                                    break;

                                point.SetColumn(_columnNames[j], values.ElementAt(j));

                            }

                            Points.Add(point);
                            if (Points.Count % 100 == 0)
                                OnPropertyChanged("PointsCount");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    OnPropertyChanged("PointsCount");
                    return false;
                }

                OnPropertyChanged("PointsCount");
                return true;
            });
        }

        /// <summary>
        /// Transform wildcard string into regular expression
        /// </summary>
        /// <param name="value">Wildcard string</param>
        /// <returns></returns>
        private static string WildCardToRegular(string value)
        {
            return "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }

        /// <summary>
        /// Process loaded points
        /// </summary>
        /// <param name="options">Process parameters</param>
        /// <returns></returns>
        public async Task<bool> Process(ProcessorOptions options)
        {
            return await Task.Run(() =>
            {
                FilterPoints(options);

                Regex pointrgx, addressrgx, devicergx;

                bool pointRename = !string.IsNullOrEmpty(options.RenamePointMask);
                bool addressRename = !string.IsNullOrEmpty(options.RenameAddressMask);
                bool deviceRename = !string.IsNullOrEmpty(options.RenameDeviceMask);

                if (pointRename && options.RenameUseRegex)
                    pointrgx = new Regex(options.RenamePointMask);
                else
                    pointrgx = null;
                if (addressRename && options.RenameUseRegex)
                    addressrgx = new Regex(options.RenameAddressMask);
                else
                    addressrgx = null;
                if (deviceRename && options.RenameUseRegex)
                    devicergx = new Regex(options.RenameDeviceMask);
                else
                    devicergx = null;

                foreach (var p in PointsProcesed)
                {
                    if (pointRename)
                    {
                        if (options.RenameUseRegex)
                            p.PT_ID = pointrgx.Replace(p.PT_ID, options.RenamePointTo);
                        else
                            p.PT_ID = p.PT_ID.Replace(options.RenamePointMask, options.RenamePointTo);
                    }
                    if (addressRename)
                    {
                        if (options.RenameUseRegex)
                            p.ADDR = addressrgx.Replace(p.ADDR, options.RenameAddressTo);
                        else
                            p.ADDR = p.ADDR.Replace(options.RenameAddressMask, options.RenameAddressTo);
                    }
                    if (deviceRename)
                    {
                        if (options.RenameUseRegex)
                            p.DEVICE_ID = devicergx.Replace(p.DEVICE_ID, options.RenameDeviceTo);
                        else
                            p.DEVICE_ID = p.DEVICE_ID.Replace(options.RenameDeviceMask, options.RenameDeviceTo);
                    }

                    if (options.ConvertToVirtual && p.PT_ORIGIN == "D")
                    {
                        p.ADDR = "";
                        p.ADDR_OFFSET = "";
                        p.ADDR_TYPE = "";
                        p.CALC_TYPE = "EQU";
                        p.CONV_TYPE = "";
                        p.DELAY_LOAD = "";
                        p.DEVICE_ID = "";
                        p.LOCAL = "0";
                        p.POLL_AFTER_SET = "";
                        p.PTMGMT_PROC_ID = "MASTER_PTM0_RP";
                        p.PT_ORIGIN = "G";
                        p.RESET_COND = "UN";
                        p.SCAN_RATE = "";
                        p.TRIG_REL = "";
                        p.UPDATE_CRITERIA = "";
                        p.VARIANCE_VAL = "0";
                    }
                    if (options.InitVirtualMode != ProcessorOptions.InitializationMode.NotSet)
                    {
                        if (p.RESET_COND.Length > 0)
                        {
                            if (options.InitVirtualMode == ProcessorOptions.InitializationMode.Init)
                            {
                                p.RESET_COND = "IN";
                                p.INIT_VAL = "0";
                            }
                            else if (options.InitVirtualMode == ProcessorOptions.InitializationMode.None)
                            {
                                p.RESET_COND = "UN";
                            }
                            else if (options.InitVirtualMode == ProcessorOptions.InitializationMode.Saved)
                            {
                                p.RESET_COND = "SA";
                            }
                            else if (options.InitVirtualMode == ProcessorOptions.InitializationMode.SavedOrInit)
                            {
                                p.RESET_COND = "SI";
                                p.INIT_VAL = "0";
                            }
                        }
                    }
                    if (options.EnablePoint != ProcessorOptions.SetProperty.NotSet)
                        p.PT_ENABLED = options.EnablePoint == ProcessorOptions.SetProperty.Enable ? 1 : 0;
                    if (options.DisableAlarm && p.ALM_ENABLE == "1")
                        p.ALM_ENABLE = "0";
                    if (options.ReadOnly != ProcessorOptions.SetProperty.NotSet)
                        p.ACCESS = options.ReadOnly == ProcessorOptions.SetProperty.Enable ? "R" : "W";
                    if (options.EnableEnterprise != ProcessorOptions.SetProperty.NotSet)
                        p.ACCESS_FILTER = options.EnableEnterprise == ProcessorOptions.SetProperty.Enable ? "E" : "";
                    if (options.LogData != ProcessorOptions.SetProperty.NotSet)
                        p.LOG_DATA = options.LogData == ProcessorOptions.SetProperty.Enable ? 1 : 0;
                    if (options.PollAfterSet != ProcessorOptions.SetProperty.NotSet && p.PT_ORIGIN == "D")
                        p.POLL_AFTER_SET = options.PollAfterSet == ProcessorOptions.SetProperty.Enable ? "1" : "0";
                }

                return true;
            });
        }

        /// <summary>
        /// Filter source points
        /// </summary>
        /// <param name="options">Parameters</param>
        private void FilterPoints(ProcessorOptions options)
        {
            Regex pointrgx = null;
            Regex addressrgx = null;
            Regex devicergx = null;

            if (string.IsNullOrWhiteSpace(options.FilterPoint) && string.IsNullOrWhiteSpace(options.FilterAddress) && string.IsNullOrWhiteSpace(options.FilterDevice))
                PointsProcesed = Points;
            else
            {
                string pointPattern = options.FilterPoint;
                string addressPattern = options.FilterAddress;
                string devicePattern = options.FilterDevice;

                if (!options.FilterUseRegex)
                {
                    if (!string.IsNullOrWhiteSpace(pointPattern))
                        pointPattern = WildCardToRegular(pointPattern);
                    if (!string.IsNullOrWhiteSpace(addressPattern))
                        addressPattern = WildCardToRegular(addressPattern);
                    if (!string.IsNullOrWhiteSpace(devicePattern))
                        devicePattern = WildCardToRegular(devicePattern);
                }

                if (!string.IsNullOrWhiteSpace(pointPattern))
                    pointrgx = new Regex(pointPattern);
                if (!string.IsNullOrWhiteSpace(addressPattern))
                    addressrgx = new Regex(addressPattern);
                if (!string.IsNullOrWhiteSpace(devicePattern))
                    devicergx = new Regex(devicePattern);

                if (pointrgx != null && devicergx != null && addressrgx != null)
                    PointsProcesed = Points.Where(x => pointrgx.IsMatch(x.PT_ID) && addressrgx.IsMatch(x.ADDR) && devicergx.IsMatch(x.DEVICE_ID)).ToArray();
                else if (pointrgx != null && addressrgx != null)
                    PointsProcesed = Points.Where(x => pointrgx.IsMatch(x.PT_ID) && addressrgx.IsMatch(x.ADDR)).ToArray();
                else if (pointrgx != null && devicergx != null)
                    PointsProcesed = Points.Where(x => pointrgx.IsMatch(x.PT_ID) && devicergx.IsMatch(x.DEVICE_ID)).ToArray();
                else if (devicergx != null && addressrgx != null)
                    PointsProcesed = Points.Where(x => addressrgx.IsMatch(x.ADDR) && devicergx.IsMatch(x.DEVICE_ID)).ToArray();
                else if (pointrgx != null)
                    PointsProcesed = Points.Where(x => pointrgx.IsMatch(x.PT_ID)).ToArray();
                else if (addressrgx != null)
                    PointsProcesed = Points.Where(x => addressrgx.IsMatch(x.ADDR)).ToArray();
                else
                    PointsProcesed = Points.Where(x => devicergx.IsMatch(x.DEVICE_ID)).ToArray();
            }

            OnPropertyChanged("PointsProcessedCount");
        }

        /// <summary>
        /// Save processed points into file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public bool Save(string file, CimFormat format)
        {
            if (PointsProcesed == null)
            {
                Exception = new Exception("Run point processing first");
                return false;
            }

            if (!PointsProcesed.Any())
            {
                Exception = new Exception("No point in process result");
                return false;
            }

            if (format == CimFormat.WHATEVER)
                format = Version;

            PropertyInfo[] properties;

            switch (format)
            {
                case CimFormat.CIM75:
                    properties = typeof(CimplicityPoint75).GetProperties().Where(x => !x.Name.Equals("PT_ID")).OrderBy(x => x.Name).ToArray();
                    break;
                case CimFormat.CIM82:
                    properties = typeof(CimplicityPoint82).GetProperties().Where(x => !x.Name.Equals("PT_ID")).OrderBy(x => x.Name).ToArray();
                    break;
                case CimFormat.CIM95:
                    properties = typeof(CimplicityPoint95).GetProperties().Where(x => !x.Name.Equals("PT_ID")).OrderBy(x => x.Name).ToArray();
                    break;
                default:
                    Exception = new Exception("Unsupported CIMPLICITY output format");
                    return false;
            }

            var lines = new List<string>();
            lines.Add(string.Concat("PT_ID,", string.Join(",", properties.Select(p => p.Name))));

            foreach (var pt in PointsProcesed)
            {
                lines.Add(string.Concat(pt.PT_ID, ",", string.Join(",", properties.Select(p => (p.GetValue(pt, null) ?? "").ToString()))));
            }

            try
            {
                File.WriteAllLines(file, lines, Encoding.ASCII);
                return true;
            }
            catch (Exception ex)
            {
                Exception = ex;
                return false;
            }
        }

        /// <summary>
        /// Split line with comma separated values. Function respect quoted strings (description with comma)
        /// </summary>
        /// <param name="line">Input line</param>
        /// <returns></returns>
        private IEnumerable<string> SplitLine(string line)
        {
            _values.Clear();
            int pos = 0;
            bool inQuota = false;
            string text;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i].Equals('"'))
                {
                    if (!inQuota)
                    {
                        inQuota = true;
                        pos = i;
                        continue;
                    }

                    //yield return line.Substring(pos, i - pos);
                    _values.Add(line.Substring(pos, i - pos + 1));
                    inQuota = false;
                    while (++i < line.Length && !line[i].Equals(',')) ;
                    pos = i + 1;
                }
                else if (line[i].Equals(','))
                {
                    if (inQuota)
                        continue;
                    text = line.Substring(pos, i - pos);
                    pos = i + 1;
                    //yield return text;
                    _values.Add(text);
                }
            }
            //yield return line.Substring(pos);
            _values.Add(line.Substring(pos));

            return _values;
        }

        /// <summary>
        /// Try to find CIMPLCICITY version format comparing column names
        /// </summary>
        /// <param name="columnNames">Source file column names</param>
        /// <returns></returns>
        private string FindVersion(string[] columnNames)
        {
            if (columnNames.Length == CimplicityPoint.GetPropertiesCount<CimplicityPoint75>())
            {
                if (CimplicityPoint.GetPropertiesName<CimplicityPoint75>().All(columnNames.Contains))
                    return "7.5";
            }
            else if (columnNames.Length == CimplicityPoint.GetPropertiesCount<CimplicityPoint82>())
            {
                if (CimplicityPoint.GetPropertiesName<CimplicityPoint82>().All(columnNames.Contains))
                    return "8.2";
            }
            else if (columnNames.Length == CimplicityPoint.GetPropertiesCount<CimplicityPoint95>())
            {
                if (CimplicityPoint.GetPropertiesName<CimplicityPoint95>().All(columnNames.Contains))
                    return "9.5";
            }

            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }
    }
}
