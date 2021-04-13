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

namespace CimPointConv
{
    internal static class ArgsParser
    {
        internal static ArgsOptions Parse(string[] args)
        {
            var options = new ArgsOptions();

            for (int i = 0; i < args.Length; i++)
            {
                if (i == 0)
                {
                    options.InputFile = args[0];
                    if (args.Length == 1)
                    {
                        options.PrintVersion = true;
                        options.PrintPointsCount = true;
                    }
                }
                else
                {
                    if (args[i] == "-v")
                    {
                        options.PrintVersion = true;
                    }
                    else if (args[i] == "-c")
                    {
                        options.PrintPointsCount = true;
                    }
                    else if (args[i] == "-d")
                    {
                        options.PrintDevices = true;
                    }
                    else if (args[i] == "-o")
                    {
                        if (args.Length == i + 1 || args[i + 1].StartsWith("-"))
                            throw new ArgumentException("After switch -o output file name have to be specified.");
                        options.OutputFile = args[++i];
                    }
                    else if (args[i] == "--overwrite")
                    {
                        options.Overwrite = true;
                    }
                    else if (args[i] == "--format")
                    {
                        options.Overwrite = true;
                        if (args.Length == i + 1 || args[i + 1].StartsWith("-"))
                            throw new ArgumentException("After switch --format format have to be specified.");
                        try
                        {
                            options.OutputFormat = (Format)Enum.Parse(typeof(Format), args[++i]);
                        }
                        catch
                        {
                            throw new ArgumentException($"Unsupported output format {args[i]}.");
                        }
                    }
                    else if (args[i] == "-fp")
                    {
                        if (args.Length == i + 1 || args[i + 1].StartsWith("-"))
                            throw new ArgumentException("After switch -fp filter have to be specified.");
                        options.FilterPoint = args[++i];
                    }
                    else if (args[i] == "-fa")
                    {
                        if (args.Length == i + 1 || args[i + 1].StartsWith("-"))
                            throw new ArgumentException("After switch -fa filter have to be specified.");
                        options.FilterAddress = args[++i];
                    }
                    else if (args[i] == "-fd")
                    {
                        if (args.Length == i + 1 || args[i + 1].StartsWith("-"))
                            throw new ArgumentException("After switch -fd filter have to be specified.");
                        options.FilterDevice = args[++i];
                    }
                    else if (args[i] == "-fr")
                    {
                        options.FilterUseRegex = true;
                    }
                    else if (args[i] == "-rp")
                    {
                        if (args.Length == i + 2 || args[i + 1].StartsWith("-") || args[i + 2].StartsWith("-"))
                            throw new ArgumentException("After switch -rp rename FROM and TO have to be specified.");
                        options.RenamePointMask = args[++i];
                        options.RenamePointTo = args[++i];
                    }
                    else if (args[i] == "-ra")
                    {
                        if (args.Length == i + 2 || args[i + 1].StartsWith("-") || args[i + 2].StartsWith("-"))
                            throw new ArgumentException("After switch -ra rename FROM and TO have to be specified.");
                        options.RenameAddressMask = args[++i];
                        options.RenameAddressTo = args[++i];
                    }
                    else if (args[i] == "-rd")
                    {
                        if (args.Length == i + 2 || args[i + 1].StartsWith("-") || args[i + 2].StartsWith("-"))
                            throw new ArgumentException("After switch -rd rename FROM and TO have to be specified.");
                        options.RenameDeviceMask = args[++i];
                        options.RenameDeviceTo = args[++i];
                    }
                    else if (args[i] == "-cv")
                    {
                        options.ConvertToVirtual = true;
                    }
                    else if (args[i] == "-iv")
                    {
                        if (args.Length == i + 1 || args[i + 1].StartsWith("-"))
                            throw new ArgumentException("After switch -iv initialization mode have to be specified.");

                        switch (args[++i])
                        {
                            case "NI":
                                options.InitVirtualMode = ProcessorOptions.InitializationMode.None;
                                break;
                            case "I":
                                options.InitVirtualMode = ProcessorOptions.InitializationMode.Init;
                                break;
                            case "SA":
                                options.InitVirtualMode = ProcessorOptions.InitializationMode.Saved;
                                break;
                            case "SI":
                                options.InitVirtualMode = ProcessorOptions.InitializationMode.SavedOrInit;
                                break;
                            default:
                                throw new ArgumentException($"Invalid initialization mode {args[i]}.");
                        }
                    }
                    else if (args[i] == "-pe")
                    {
                        options.EnablePoint = ProcessorOptions.SetProperty.Enable;
                    }
                    else if (args[i] == "-pd")
                    {
                        options.EnablePoint = ProcessorOptions.SetProperty.Disable;
                    }
                    else if (args[i] == "-ad")
                    {
                        options.DisableAlarm = true;
                    }
                    else if (args[i] == "-ro")
                    {
                        options.ReadOnly = ProcessorOptions.SetProperty.Enable;
                    }
                    else if (args[i] == "-rw")
                    {
                        options.ReadOnly = ProcessorOptions.SetProperty.Disable;
                    }
                    else if (args[i] == "-ee")
                    {
                        options.EnableEnterprise = ProcessorOptions.SetProperty.Enable;
                    }
                    else if (args[i] == "-ed")
                    {
                        options.EnableEnterprise = ProcessorOptions.SetProperty.Disable;
                    }
                    else if (args[i] == "-le")
                    {
                        options.LogData = ProcessorOptions.SetProperty.Enable;
                    }
                    else if (args[i] == "-ld")
                    {
                        options.LogData = ProcessorOptions.SetProperty.Disable;
                    }
                    else if (args[i] == "-pe")
                    {
                        options.PollAfterSet = ProcessorOptions.SetProperty.Enable;
                    }
                    else if (args[i] == "-pd")
                    {
                        options.PollAfterSet = ProcessorOptions.SetProperty.Disable;
                    }
                    else
                    {
                        throw new ArgumentException($"Unknown argument {args[i]}.");
                    }
                }
            }
            return options;
        }
    }
}
