#nullable enable

using System;
using System.Text;

namespace Tyleo.Logging
{
    /// <summary>
    /// Extensions for <see cref="ILogSys"/>.
    /// </summary>
    public static class LogSysExt
    {
        /// <summary>
        /// Gets a string representation of all enabled loggers in the log
        /// system, with each logger's absolute path separated by the specified
        /// delimiter.
        /// </summary>
        /// <param name="self">The log system.</param>
        /// <param name="delimiter">The delimiter to use.</param>
        /// <returns>
        /// A string representation of all enabled loggers, or null if none are
        /// enabled.
        /// </returns>
        public static string GetEnabledLoggersString(
            this ILogSys self,
            string delimiter = "\n"
        )
        {
            var enabledLoggers = self.GetEnabledLoggers();
            if (enabledLoggers.Length == 0) return string.Empty;
            Array.Sort(
                enabledLoggers,
                (lhs, rhs) => string.Compare(
                    lhs.AbsolutePath,
                    rhs.AbsolutePath,
                    StringComparison.InvariantCulture
                )
            );

            var sb = new StringBuilder(enabledLoggers[0].AbsolutePath);
            for (var i = 1; i < enabledLoggers.Length; i++)
            {
                sb.Append(delimiter).Append(enabledLoggers[i].AbsolutePath);
            }
            return sb.ToString();
        }
    }
}
