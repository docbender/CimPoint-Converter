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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using static CimPointConv.ProcessorOptions;

namespace CimPointConv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Processor processor { get; } = new();
        private bool _working = false;
        private string _configPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CimPointConverter");
        private readonly string _configFile = "settings.json";
        private readonly IConfiguration _config;
        private readonly SolidColorBrush _statusbarcolor;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            _statusbarcolor = (SolidColorBrush)Resources["StatusBarColor"];

            if (!Directory.Exists(_configPath))
            {
                try
                {
                    Directory.CreateDirectory(_configPath);
                }
                catch (Exception ex)
                {
                    Resources["StatusBarColor"] = Brushes.Orange;
                    statusBarItem1.Content = ex.Message;
                    _configPath = Directory.GetCurrentDirectory();
                }
            }

            // get setting
            _config = new ConfigurationBuilder()
                .SetBasePath(_configPath)
                .AddJsonFile(_configFile, true, false)
                .Build();

            #region UI setting

            var file = _config.GetSection("InputFile").Value;
            if (!string.IsNullOrEmpty(file) && File.Exists(file))            
                tbFile.Text = file;            
            tbFilterPoint.Text = _config.GetSection("FilterPoint").Value;
            tbFilterAddress.Text = _config.GetSection("FilterAddress").Value;
            tbFilterDevice.Text = _config.GetSection("FilterDevice").Value;
            if (bool.TrueString == _config.GetSection("FilterUseRegex").Value)
                cbFilterRegex.IsChecked = true;
            tbRenameFromDevice.Text = _config.GetSection("RenameDeviceMask").Value;
            tbRenameToDevice.Text = _config.GetSection("RenameDeviceTo").Value;
            tbRenameFromAddress.Text = _config.GetSection("RenameAddressMask").Value;
            tbRenameToAddress.Text = _config.GetSection("RenameAddressTo").Value;
            tbRenameFromPoint.Text = _config.GetSection("RenamePointMask").Value;
            tbRenameToPoint.Text = _config.GetSection("RenamePointTo").Value;
            if (bool.TrueString == _config.GetSection("RenameUseRegex").Value)
                cbRenameRegex.IsChecked = true;
            if (bool.TrueString == _config.GetSection("ConvertToVirtual").Value)
                cbToVirtual.IsChecked = true;
            if (bool.TrueString == _config.GetSection("DisableAlarm").Value)
                cbAlarmDisable.IsChecked = true;

            SetProperty set;
            if (Enum.TryParse<SetProperty>(_config.GetSection("EnableEnterprise").Value, out set))
            {
                if (set == SetProperty.Enable)
                {
                    cbEnterprise.IsChecked = true;
                    rbE8eEnable.IsChecked = true;
                }
                else if (set == SetProperty.Disable)
                {
                    cbEnterprise.IsChecked = true;
                    rbE8eDisable.IsChecked = true;
                }
            }
            if (Enum.TryParse<SetProperty>(_config.GetSection("EnablePoint").Value, out set))
            {
                if (set == SetProperty.Enable)
                {
                    cbEnablePoint.IsChecked = true;
                    rbPointEnable.IsChecked = true;
                }
                else if (set == SetProperty.Disable)
                {
                    cbEnablePoint.IsChecked = true;
                    rbPointDisable.IsChecked = true;
                }
            }
            if (Enum.TryParse<SetProperty>(_config.GetSection("LogData").Value, out set))
            {
                if (set == SetProperty.Enable)
                {
                    cbLogPoint.IsChecked = true;
                    rbLogEnable.IsChecked = true;
                }
                else if (set == SetProperty.Disable)
                {
                    cbLogPoint.IsChecked = true;
                    rbLogDisable.IsChecked = true;
                }
            }
            if (Enum.TryParse<SetProperty>(_config.GetSection("PollAfterSet").Value, out set))
            {
                if (set == SetProperty.Enable)
                {
                    cbPoll.IsChecked = true;
                    rbPollSet.IsChecked = true;
                }
                else if (set == SetProperty.Disable)
                {
                    cbPoll.IsChecked = true;
                    rbPollReset.IsChecked = true;
                }
            }
            if (Enum.TryParse<SetProperty>(_config.GetSection("ReadOnly").Value, out set))
            {
                if (set == SetProperty.Enable)
                {
                    cbReadOnly.IsChecked = true;
                    rbRoEnable.IsChecked = true;
                }
                else if (set == SetProperty.Disable)
                {
                    cbReadOnly.IsChecked = true;
                    rbRoDisable.IsChecked = true;
                }
            }
            if (Enum.TryParse<InitializationMode>(_config.GetSection("InitVirtualMode").Value, out InitializationMode initialization))
            {
                if (initialization != InitializationMode.NotSet)
                {
                    cbInit.IsChecked = true;
                    cbxInit.SelectedIndex = (int)initialization;
                }
            }
            if (Enum.TryParse<Format>(_config.GetSection("SaveTo").Value, out Format format))
            {
                switch (format)
                {
                    case Format.CIM75:
                        IsCheckedCimplicity = true;
                        rbSevenFive.IsChecked = true;
                        break;
                    case Format.CIM82:
                        IsCheckedCimplicity = true;
                        rbEightTwo.IsChecked = true;
                        break;
                    case Format.CIM95:
                        IsCheckedCimplicity = true;
                        rbNineFive.IsChecked = true;
                        break;
                    case Format.CIM115:
                        IsCheckedCimplicity = true;
                        rbElevenFive.IsChecked = true;
                        break;
                    case Format.IGNITION:
                        IsCheckedIgnition = true;
                        break;
                }
            }
            switch (_config.GetSection("SaveOption").Value)
            {
                case "1":
                    rbOutNew.IsChecked = true;
                    break;
                case "2":
                    rbOutSource.IsChecked = true;
                    break;
                case "3":
                    rbOutClipboard.IsChecked = true;
                    break;
                case "4":
                    rbOutManual.IsChecked = true;
                    break;
            }
            #endregion
        }

        public bool Working
        {
            get
            {
                return _working;
            }
            set
            {
                if (value)
                    Cursor = Cursors.Wait;
                else
                    Cursor = Cursors.Arrow;

                _working = value;

                OnPropertyChanged("Working");
                OnPropertyChanged("Unlocked");
            }
        }

        public bool Unlocked
        {
            get
            {
                return !Working;
            }
        }

        public static string Version
        {
            get
            {
                return $"{Assembly.GetEntryAssembly().GetName().Version.ToString(3)}";
            }
        }

        private bool _IsCheckedCimplicity;
        private bool _IsCheckedIgnition;

        public bool IsCheckedCimplicity
        {
            get { return _IsCheckedCimplicity; }
            set
            {
                if (true == value)
                {
                    IsCheckedIgnition = false;
                    _IsCheckedCimplicity = value;
                }
                else
                {
                    _IsCheckedCimplicity = value;
                }
                OnPropertyChanged("IsCheckedCimplicity");
            }
        }

        public bool IsCheckedIgnition
        {
            get { return _IsCheckedIgnition; }
            set
            {
                if (true == value)
                {
                    IsCheckedCimplicity = false;
                    _IsCheckedIgnition = value;
                }
                else
                {
                    _IsCheckedIgnition = value;
                }
                OnPropertyChanged("IsCheckedIgnition");
            }
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (Working)
                return;
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            {
                e.Effects = DragDropEffects.Link;
                var files = (string[])(e.Data?.GetData(DataFormats.FileDrop));
                if (files == null || files.Length == 0)
                {
                    e.Effects = DragDropEffects.None;
                }
                else
                {
                    var filepath = files[0];
                    if (filepath.Length > 50)
                    {
                        if (filepath.IndexOf('\\', filepath.Length - 50) == -1)
                            filepath = $"...{filepath.Substring(filepath.Length - 50)}";
                        else
                            filepath = $"...{filepath.Substring(filepath.IndexOf('\\', filepath.Length - 50))}";
                    }

                    statusBarItem1.Content = $"Ready to catch {filepath}";
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Window_DragLeave(object sender, DragEventArgs e)
        {
            if (Working)
                return;
            statusBarItem1.Content = "";
        }

        private async void Window_Drop(object sender, DragEventArgs e)
        {
            if (Working)
                return;
            var files = (string[])(e.Data?.GetData(DataFormats.FileDrop));
            if (files == null || files.Length == 0)
                return;
            await Load(files[0]).ConfigureAwait(false);
        }

        private async void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new()
            {
                Title = "Open input file...",
                Filter = "Text files (*.txt,*.csv)|*.txt;*.csv|All files (*.*)|*.*",
                InitialDirectory = _config.GetSection("RecentDir").Value
            };

            if (!dlg.ShowDialog().Value)
                return;
            await Load(dlg.FileName);
            _config.GetSection("RecentDir").Value = System.IO.Path.GetDirectoryName(dlg.FileName);
            _config.GetSection("InputFile").Value = dlg.FileName;
        }

        private async Task Load(string file)
        {
            Working = true;

            tbFile.Text = file;
            tbFile.CaretIndex = file.Length;
            tbFile.ScrollToHorizontalOffset(double.MaxValue);
            Resources["StatusBarColor"] = _statusbarcolor;
            statusBarItem1.Content = "Loading file...";

            if (!await processor.Load(file))
            {
                Resources["StatusBarColor"] = Brushes.Orange;
                statusBarItem1.Content = processor.Exception.Message;
            }
            else
            {
                if (processor.Exception != null)
                {
                    Resources["StatusBarColor"] = Brushes.DarkOrange;
                    statusBarItem1.Content = processor.Exception.Message;
                }
                else
                {
                    statusBarItem1.Content = "Ready";
                }

                if (processor.Version == Format.CIM75)
                    rbSevenFive.IsChecked = true;
                else if (processor.Version == Format.CIM82)
                    rbEightTwo.IsChecked = true;
                else if (processor.Version == Format.CIM95)
                    rbNineFive.IsChecked = true;
                else if (processor.Version == Format.CIM115)
                    rbElevenFive.IsChecked = true;
            }
            Working = false;
        }

        private async void btnRun_Click(object sender, RoutedEventArgs e)
        {
            Working = true;

            Resources["StatusBarColor"] = _statusbarcolor;
            statusBarItem1.Content = "Processing...";

            ProcessorOptions options = new()
            {
                ConvertToVirtual = cbToVirtual.IsChecked.Value,
                DisableAlarm = cbAlarmDisable.IsChecked.Value,
                EnableEnterprise = !cbEnterprise.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbE8eEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable,
                EnablePoint = !cbEnablePoint.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbPointEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable,
                InitVirtualMode = !cbInit.IsChecked.Value ? ProcessorOptions.InitializationMode.NotSet
                    : (ProcessorOptions.InitializationMode)cbxInit.SelectedIndex,
                LogData = !cbLogPoint.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbLogEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable,
                PollAfterSet = !cbPoll.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbPollSet.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable,
                ReadOnly = !cbReadOnly.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbRoEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable,
                FilterPoint = tbFilterPoint.Text,
                FilterAddress = tbFilterAddress.Text,
                FilterDevice = tbFilterDevice.Text,
                FilterUseRegex = cbFilterRegex.IsChecked.Value,
                RenameDeviceMask = tbRenameFromDevice.Text,
                RenameDeviceTo = tbRenameToDevice.Text,
                RenameAddressMask = tbRenameFromAddress.Text,
                RenameAddressTo = tbRenameToAddress.Text,
                RenamePointMask = tbRenameFromPoint.Text,
                RenamePointTo = tbRenameToPoint.Text,
                RenameUseRegex = cbRenameRegex.IsChecked.Value,
            };

            if (!await processor.Process(options))
            {
                Resources["StatusBarColor"] = Brushes.Orange;
                statusBarItem1.Content = processor.Exception.Message;
                Working = false;
                return;
            }


            if (processor.Exception != null)
            {
                Resources["StatusBarColor"] = Brushes.DarkOrange;
                statusBarItem1.Content = processor.Exception.Message;
            }
            else
            {
                statusBarItem1.Content = "Done";
            }

            if (rbOutClipboard.IsChecked.Value)
            {
                var result = processor.GetResultAsText(GetTargetFormat(), out int err, out _);
                if (result == null)
                {
                    Resources["StatusBarColor"] = Brushes.Orange;
                    statusBarItem1.Content = processor.Exception.Message;
                }
                else
                {
                    Clipboard.SetText(result);
                    statusBarItem1.Content = "Result put in the clipboard.";
                    if (err > 0)
                        statusBarItem1.Content += $" There was {err} conversion errors.";
                }
            }
            else if (rbOutSource.IsChecked.Value)
            {
                await SaveResult(tbFile.Text, GetTargetFormat());
                statusBarItem1.Content = "Result saved to the file";
            }
            else if (rbOutNew.IsChecked.Value)
            {
                var filename = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(tbFile.Text),
                    $"{System.IO.Path.GetFileNameWithoutExtension(tbFile.Text)}_{DateTime.Now:yyMMdd-HHmmss}{System.IO.Path.GetExtension(tbFile.Text)}");

                await SaveResult(filename, GetTargetFormat());
                statusBarItem1.Content = "Result saved to the file";
            }

            Working = false;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await SaveResultAs();
        }

        private async Task SaveResult(string fileName, Format format)
        {
            if (!await processor.Save(fileName, format))
            {
                Resources["StatusBarColor"] = Brushes.Orange;
                statusBarItem1.Content = processor.Exception.Message;
            }
            else
            {
                Resources["StatusBarColor"] = _statusbarcolor;
                statusBarItem1.Content = "Done";
            }
        }

        private async Task SaveResultAs()
        {
            var format = GetTargetFormat();
            SaveFileDialog dlg = new()
            {
                Title = "Save as...",
                InitialDirectory = _config.GetSection("RecentDir").Value
            };

            if (format == Format.IGNITION)
                dlg.Filter = "JSON file (*.json)|*.json";
            else
                dlg.Filter = "Text files (*.txt,*.csv)|*.txt,*.csv|All files (*.*)|*.*";

            if (dlg.ShowDialog().Value)
            {
                Working = true;
                await SaveResult(dlg.FileName, format);
                Working = false;

                _config.GetSection("RecentDir").Value = System.IO.Path.GetDirectoryName(dlg.FileName);
            }
        }

        private Format GetTargetFormat()
        {
            if (cbIgnition.IsChecked.Value)
                return Format.IGNITION;
            if (cbFormat.IsChecked.Value)
                return rbSevenFive.IsChecked.Value ? Format.CIM75 :
                    (rbEightTwo.IsChecked.Value ? Format.CIM82 :
                    (rbNineFive.IsChecked.Value ? Format.CIM95 :
                    (rbElevenFive.IsChecked.Value ? Format.CIM115 :
                    Format.WHATEVER)));
            return Format.WHATEVER;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("cmd", $"/c start {e.Uri.AbsoluteUri}") { CreateNoWindow = true });
            else
                System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveSetting();
        }

        private void SaveSetting()
        {
            var path = System.IO.Path.Combine(_configPath, _configFile);
            string jsonFile = String.Empty;
            JsonObject jsonObj;

            try
            {
                if (File.Exists(path))
                {
                    jsonFile = File.ReadAllText(path);
                    jsonObj = (JsonObject)JsonObject.Parse(jsonFile);
                }
                else
                {
                    jsonObj = new JsonObject();
                }

                jsonObj["RecentDir"] = _config.GetSection("RecentDir").Value;
                jsonObj["InputFile"] = _config.GetSection("InputFile").Value;

                jsonObj["FilterPoint"] = tbFilterPoint.Text;
                jsonObj["FilterAddress"] = tbFilterAddress.Text;
                jsonObj["FilterDevice"] = tbFilterDevice.Text;
                jsonObj["FilterUseRegex"] = cbFilterRegex.IsChecked.Value;
                jsonObj["RenameDeviceMask"] = tbRenameFromDevice.Text;
                jsonObj["RenameDeviceTo"] = tbRenameToDevice.Text;
                jsonObj["RenameAddressMask"] = tbRenameFromAddress.Text;
                jsonObj["RenameAddressTo"] = tbRenameToAddress.Text;
                jsonObj["RenamePointMask"] = tbRenameFromPoint.Text;
                jsonObj["RenamePointTo"] = tbRenameToPoint.Text;
                jsonObj["RenameUseRegex"] = cbRenameRegex.IsChecked.Value;

                jsonObj["ConvertToVirtual"] = cbToVirtual.IsChecked.Value;
                jsonObj["DisableAlarm"] = cbAlarmDisable.IsChecked.Value;
                jsonObj["EnableEnterprise"] = (!cbEnterprise.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbE8eEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable).ToString();
                jsonObj["EnablePoint"] = (!cbEnablePoint.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbPointEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable).ToString();
                jsonObj["InitVirtualMode"] = (!cbInit.IsChecked.Value ? ProcessorOptions.InitializationMode.NotSet
                    : (ProcessorOptions.InitializationMode)cbxInit.SelectedIndex).ToString();
                jsonObj["LogData"] = (!cbLogPoint.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbLogEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable).ToString();
                jsonObj["PollAfterSet"] = (!cbPoll.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbPollSet.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable).ToString();
                jsonObj["ReadOnly"] = (!cbReadOnly.IsChecked.Value ? ProcessorOptions.SetProperty.NotSet
                    : rbRoEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable).ToString();

                jsonObj["SaveTo"] = GetTargetFormat().ToString();
                jsonObj["SaveOption"] = rbOutNew.IsChecked.Value ? 1 : (
                                        rbOutSource.IsChecked.Value ? 2 : (
                                        rbOutClipboard.IsChecked.Value ? 3 : (
                                        rbOutManual.IsChecked.Value ? 4 : 0)));

                File.WriteAllText(path,
                    jsonObj.ToJsonString(
                        new JsonSerializerOptions()
                        {
                            WriteIndented = true
                        })
                    );
            }
            finally
            {

            }

        }
    }
}
