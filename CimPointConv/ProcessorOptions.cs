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
    public class ProcessorOptions
    {
        public enum SetProperty
        {
            NotSet,
            Enable,
            Disable
        }

        public enum InitializationMode : int
        {
            None = 0x0,
            Init = 0x1,
            Saved = 0x2,
            SavedOrInit = 0x3,
            NotSet = 0xFF
        }

        public string FilterPoint { get; set; }
        public string FilterAddress { get; set; }
        public string FilterDevice { get; set; }
        public bool FilterUseRegex { get; set; }

        public string RenamePointMask { get; set; }
        public string RenamePointTo { get; set; }
        public string RenameAddressMask { get; set; }
        public string RenameAddressTo { get; set; }
        public string RenameDeviceMask { get; set; }
        public string RenameDeviceTo { get; set; }
        public bool RenameUseRegex { get; set; }

        public bool ConvertToVirtual { get; set; }
        public InitializationMode InitVirtualMode { get; set; }
        public SetProperty EnablePoint { get; set; }
        public bool DisableAlarm { get; set; }
        public SetProperty ReadOnly { get; set; }
        public SetProperty EnableEnterprise { get; set; }
        public SetProperty LogData { get; set; }
        public SetProperty PollAfterSet { get; set; }
    }
}
