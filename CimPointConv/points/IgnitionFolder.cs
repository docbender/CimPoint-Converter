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
using System.Text.Json;

namespace CimPointConv
{
    internal class IgnitionFolder
    {
        public string name { get; } = "Cimplicity";
        public string tagType { get; } = "Folder";
        public IEnumerable<IgnitionTag> tags { get; }

        public IgnitionFolder(IEnumerable<IgnitionTag> tags)
        {
            this.tags = tags;
        }

        public static string ToJson(IEnumerable<IgnitionTag> tags)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };
            string jsonString = JsonSerializer.Serialize(new IgnitionFolder(tags), options);
            return jsonString;
        }
    }
}
