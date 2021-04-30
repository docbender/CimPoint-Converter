# CimPoint Converter
The tool provides batch CIMPLICITY points transformations.

[![Version](https://img.shields.io/github/v/release/docbender/CimPoint-Converter?include_prereleases)](https://github.com/docbender/CimPoint-Converter/releases)
[![Download](https://img.shields.io/github/downloads/docbender/CimPoint-Converter/total.svg)](https://github.com/docbender/CimPoint-Converter/releases)
[![License](https://img.shields.io/github/license/docbender/CimPoint-Converter.svg)](LICENSE)
[![Issues](https://img.shields.io/github/issues/docbender/CimPoint-Converter)](https://github.com/docbender/CimPoint-Converter/issues)
[![Average time to resolve an issue](http://isitmaintained.com/badge/resolution/docbender/CimPoint-Converter.svg)](http://isitmaintained.com/project/docbender/CimPoint-Converter "Average time to resolve an issue")
[![Percentage of issues still open](http://isitmaintained.com/badge/open/docbender/CimPoint-Converter.svg)](http://isitmaintained.com/project/docbender/CimPoint-Converter "Percentage of issues still open")

## Features
- Renaming points and devices
- Device to Virtual conversion
- Convert into different CIMPLICITY version
- Disable alarms
- Enable/Disable data log, read only, poll after set,...

![App preview](/images/screenshot.png)

## Usage
Important: Backup your CIMPLICITY project file before applying CimPoint Converter output.
1. Export points from CIMPLICITY project using *clie* tool
2. Import points into the CimPoint Converter (Open button, Drag&Drop)
3. Perform filtering, renaming, converting
4. Import the result into the target CIMPLICITY project using *clie* tool

Instead of a GUI an application console can also be used. Run *cpc --help* for more info.

## System requirements
- .NET 5.0

## Supported CIMPLICITY versions
- CIMPLICITY 7.5
- CIMPLICITY 8.2
- CIMPLICITY 9.5
- CIMPLICITY 10
- CIMPLICITY 11

CIMPLICITY is a registered trademark of GE Intelligent Platforms, Inc.
