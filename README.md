# HttpLogDataExtractor

Overview

Reads log file containing Http request data and:
1. Gets the number of Unique IP addresses
2. Gets the Top 3 Urls
3. Gets the Top 3 Active IP Addresses

NOTE: Sample log file is present in the SampleInput folder

Source code:
Consists of the following libraries:
1. HttpLogTokeniser - For parsing the log file lines into relevant sections
2. HttpLogDataExtractor - For getting the relevant information related to IP Addresses and Urls
3. HttpDataExtractorUnitTests - For running unit tests
4. HttpLogDataExtractorDemoApp - Minimal console based app for displaying usage

Build:
Prerequisites:
1. Visual Studio 2022 Community
2. .Net Framework 4.7.2

To Build:
1. Open the .sln file and build

Running Unit Tests:
1. Please update the path to the input logfile in the Test Class, as per location of input

