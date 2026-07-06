#nullable enable

using System.Collections.Generic;

namespace Tyleo.Logging.Core
{
    public sealed class LogSys : ILogSys
    {
        private readonly Dictionary<string, Logger> _loggers;

        public int LoggerCount => _loggers.Count;

        public IEnumerable<ILogger> AllLoggers => _loggers.Values;

        private LogSys(Dictionary<string, Logger> logs)
        {
            _loggers = logs;
        }

        /// <summary>
        /// Initializes the module.
        /// </summary>
        /// <param name="dependencies">
        /// The dependencies for this module.
        /// </param>
        /// <returns>The root filter node for logging.</returns>
        public static (LogSys LogSys, ILoggerFactoryNode RootFactoryNode) Create(ILoggingDependencies dependencies)
        {
            var loggers = new Dictionary<string, Logger>();

            var logSystem = new LogSys(loggers);

            var rootFactoryNode = LoggerFactoryNode.CreateRootNode(
                dependencies,
                loggers
            );

            return (logSystem, rootFactoryNode);
        }

        public void DisableAllLoggers()
        {
            foreach (var logger in _loggers.Values)
            {
                logger.IsEnabled = false;
            }
        }

        public ILogger[] GetEnabledLoggers()
        {
            var enabledLoggerCount = 0;
            foreach (var logger in _loggers.Values)
            {
                if (!logger.IsEnabled) continue;
                enabledLoggerCount++;
            }

            var enabledLoggers = new ILogger[enabledLoggerCount];
            var enabledLoggerIndex = 0;
            foreach (var logger in _loggers.Values)
            {
                if (!logger.IsEnabled) continue;
                enabledLoggers[enabledLoggerIndex] = logger;
                enabledLoggerIndex++;
            }

            return enabledLoggers;
        }
    }
}