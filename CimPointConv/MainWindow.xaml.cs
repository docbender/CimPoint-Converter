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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
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

namespace CimPointConv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Processor processor { get; } = new();
        private bool _working = false;


        private readonly SolidColorBrush _statusbarcolor;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            _statusbarcolor = (SolidColorBrush)Resources["StatusBarColor"];
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

        public string Version
        {
            get
            {
                return $"{Assembly.GetEntryAssembly().GetName().Version.ToString(3)} alpha";
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
            OpenFileDialog dlg = new();
            dlg.Title = "Open input file...";
            dlg.Filter = "Text files (*.txt,*.csv)|*.txt;*.csv|All files (*.*)|*.*";

            if (!dlg.ShowDialog().Value)
                return;
            await Load(dlg.FileName);
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

                if (processor.Version == CimFormat.CIM75)
                    rbSevenFive.IsChecked = true;
                else if (processor.Version == CimFormat.CIM82)
                    rbEightTwo.IsChecked = true;
                else if (processor.Version == CimFormat.CIM95)
                    rbNineFive.IsChecked = true;
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
                EnableEnterprise = !cbEnterprise.IsChecked.Value ? ProcessorOptions.SetProperty.Disable
                    : rbE8eEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable,
                EnablePoint = !cbEnablePoint.IsChecked.Value ? ProcessorOptions.SetProperty.Disable
                    : rbPointEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable,
                InitVirtualMode = !cbInit.IsChecked.Value ? ProcessorOptions.InitializationMode.NotSet
                    : (ProcessorOptions.InitializationMode)cbxInit.SelectedIndex,
                LogData = !cbLogPoint.IsChecked.Value ? ProcessorOptions.SetProperty.Disable
                    : rbLogEnable.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable,
                PollAfterSet = !cbPoll.IsChecked.Value ? ProcessorOptions.SetProperty.Disable
                    : rbPollSet.IsChecked.Value ? ProcessorOptions.SetProperty.Enable : ProcessorOptions.SetProperty.Disable,
                ReadOnly = !cbReadOnly.IsChecked.Value ? ProcessorOptions.SetProperty.Disable
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
                //TODO
                throw new NotImplementedException();
            }
            else if (rbOutSource.IsChecked.Value)
            {
                //TODO
                throw new NotImplementedException();
            }
            else if (rbOutSource.IsChecked.Value)
            {
                SaveResult();
            }

            Working = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveResult();
        }

        private void SaveResult()
        {
            SaveFileDialog dlg = new();
            dlg.Title = "Save as...";
            dlg.Filter = "All files (*.*)|*.*";

            if (dlg.ShowDialog().Value)
            {
                var format = cbFormat.IsChecked.Value ? (rbSevenFive.IsChecked.Value ? CimFormat.CIM75 :
                                                        (rbEightTwo.IsChecked.Value ? CimFormat.CIM82 :
                                                        (rbNineFive.IsChecked.Value ? CimFormat.CIM95 :
                                                        CimFormat.WHATEVER))) : CimFormat.WHATEVER;

                if (!processor.Save(dlg.FileName, format))
                {
                    Resources["StatusBarColor"] = Brushes.Orange;
                    statusBarItem1.Content = processor.Exception.Message;
                }
            }
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
    }
}
