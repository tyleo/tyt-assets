#nullable enable

using System;
using System.Collections.Generic;

namespace Tyleo.Logging.Core
{
    internal sealed class LoggerFactoryNode : ILoggerFactoryNode
    {
        private readonly ILoggingDependencies _dependencies;

        private readonly ICollection<string> _filterNodeAbsolutePaths;
        private readonly IDictionary<string, Logger> _loggersByAbsolutePath;

        private readonly string _absolutePath;

        private LoggerFactoryNode(
            ILoggingDependencies dependencies,
            ICollection<string> filterNodeAbsolutePaths,
            IDictionary<string, Logger> loggersByAbsolutePath,
            string absolutePath
        )
        {
            _dependencies = dependencies;
            _filterNodeAbsolutePaths = filterNodeAbsolutePaths;
            _loggersByAbsolutePath = loggersByAbsolutePath;
            _absolutePath = absolutePath;
        }

        public static LoggerFactoryNode CreateRootNode(
            ILoggingDependencies dependencies,
            IDictionary<string, Logger> loggers
        )
        {
            var filterNodePaths = new HashSet<string>() { string.Empty };
            return new LoggerFactoryNode(
                dependencies,
                filterNodePaths,
                loggers,
                string.Empty
            );
        }

        public ILogger CreateChildLogger(string loggerName)
        {
            if (loggerName == string.Empty)
            {
                throw new ArgumentException(
                    "Logger name cannot be empty.",
                    nameof(loggerName)
                );
            }

            if (loggerName.Contains("/"))
            {
                throw new ArgumentException(
                    "Logger name cannot contain '/'.",
                    nameof(loggerName)
                );
            }

            var loggerAbsolutePath = $"{_absolutePath}{loggerName}";
            if (_loggersByAbsolutePath.ContainsKey(loggerAbsolutePath))
            {
                throw new InvalidOperationException(
                    $"Logger at path '{loggerAbsolutePath}' already exists."
                );
            }

            var filterNodePath = $"{loggerAbsolutePath}/";
            if (_filterNodeAbsolutePaths.Contains(filterNodePath))
            {
                throw new InvalidOperationException(
                    $"Logger at path '{filterNodePath}' exists as a filter node."
                );
            }

            var logger = new Logger(
                _dependencies,
                loggerAbsolutePath,
                isEnabled: false
            );
            _loggersByAbsolutePath.Add(loggerAbsolutePath, logger);

            return logger;
        }

        public ILoggerFactoryNode CreateChildFilterNode(string filterNodeName)
        {
            if (filterNodeName == string.Empty)
            {
                throw new ArgumentException(
                    "Filter node name cannot be empty.",
                    nameof(filterNodeName)
                );
            }

            if (filterNodeName.Contains("/"))
            {
                throw new ArgumentException(
                    "Filter node name cannot contain '/'.",
                    nameof(filterNodeName)
                );
            }

            var filterNodePath = $"{_absolutePath}{filterNodeName}/";
            if (_filterNodeAbsolutePaths.Contains(filterNodePath))
            {
                throw new InvalidOperationException(
                    $"Filter node at path '{filterNodePath}' already exists."
                );
            }

            var loggerPath = filterNodePath[..^1];
            if (_loggersByAbsolutePath.ContainsKey(loggerPath))
            {
                throw new InvalidOperationException(
                    $"Filter node at path '{filterNodePath}' exists as a logger."
                );
            }

            var factoryNode = new LoggerFactoryNode(
                _dependencies,
                _filterNodeAbsolutePaths,
                _loggersByAbsolutePath,
                filterNodePath
            );
            _filterNodeAbsolutePaths.Add(filterNodePath);

            return factoryNode;
        }
    }
}