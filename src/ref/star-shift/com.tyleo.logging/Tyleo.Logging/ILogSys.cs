#nullable enable

using System.Collections.Generic;

namespace Tyleo.Logging
{
    /// <summary>
    /// Represents the logging system.
    /// </summary>
    public interface ILogSys
    {
        /// <summary>
        /// Gets all of the loggers.
        /// </summary>
        IEnumerable<ILogger> AllLoggers { get; }

        /// <summary>
        /// Disables all loggers in the log system.
        /// </summary>
        void DisableAllLoggers()
        {
            foreach (var logger in AllLoggers) logger.IsEnabled = false;
        }

        /// <summary>
        /// Gets an array of all enabled loggers in the log system.
        /// </summary>
        /// <returns>
        /// An array of all enabled loggers in the log system.
        /// </returns>
        ILogger[] GetEnabledLoggers()
        {
            var enabledLoggerCount = 0;
            foreach (var logger in AllLoggers)
            {
                if (!logger.IsEnabled) continue;
                enabledLoggerCount++;
            }

            var enabledLoggers = new ILogger[enabledLoggerCount];
            var enabledLoggerIndex = 0;
            foreach (var logger in AllLoggers)
            {
                if (!logger.IsEnabled) continue;
                enabledLoggers[enabledLoggerIndex] = logger;
                enabledLoggerIndex++;
            }

            return enabledLoggers;
        }
    }
}