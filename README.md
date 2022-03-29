# CimPoint Converter
The tool provides batch CIMPLICITY points transformations as well as conversion to Ignition tags. 

[![Version](https://img.shields.io/github/v/release/docbender/CimPoint-Converter?include_prereleases)](https://github.com/docbender/CimPoint-Converter/releases)
[![Download](https://img.shields.io/github/downloads/docbender/CimPoint-Converter/total.svg)](https://github.com/docbender/CimPoint-Converter/releases)
[![License](https://img.shields.io/github/license/docbender/CimPoint-Converter.svg)](LICENSE)
[![Issues](https://img.shields.io/github/issues/docbender/CimPoint-Converter)](https://github.com/docbender/CimPoint-Converter/issues)

## Features
- Rename points, addresses and devices on a filtered set of points
- Device to Virtual conversion
- Convert into different CIMPLICITY version
- Disable alarms
- Enable/Disable data log, read only, poll after set,...
- Convert CIMPTICITY points to the Ignition tags

![App preview](/images/screenshot.png)

## Usage
Important: Backup your CIMPLICITY project file before applying CimPoint Converter output.
1. Export points from CIMPLICITY project using *clie* tool
2. Import points into the CimPoint Converter (Open button, Drag&Drop)
3. Perform filtering, renaming, converting
4. Import the result into the target CIMPLICITY project using *clie* tool or import result into the Ignition SCADA

Instead of a GUI an application console can also be used. Run *cpc --help* for more info.

## System requirements
- .NET 6.0

## Supported CIMPLICITY versions
- CIMPLICITY 7.5
- CIMPLICITY 8.2
- CIMPLICITY 9.5
- CIMPLICITY 10
- CIMPLICITY 11
- CIMPLICITY 11.5

CIMPLICITY is a registered trademark of GE Intelligent Platforms, Inc.
