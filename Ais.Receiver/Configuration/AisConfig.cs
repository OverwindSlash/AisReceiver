// <copyright file="AisConfig.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

// Configuration binding types are typically better off as null-oblivious, because the contents
// of config files are outside the compiler's control.

namespace Ais.Receiver.Configuration
{
    public class AisConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public LoggerVerbosity LoggerVerbosity { get; set; }
        public TimeSpan StatisticsPeriodicity { get; set; }
        public int RetryAttempts { get; set; }
        public TimeSpan RetryPeriodicity { get; set; }

        public bool TestMode { get; set; }
        public string TestFile { get; set; }
        public TimeSpan TestFileLineInterval { get; set; }
    }
}