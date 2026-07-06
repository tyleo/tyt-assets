#nullable enable

using System;

namespace Tyleo.Logging.Core
{
    internal sealed class Logger : ILogger
    {
        private readonly ILoggingDependencies _dependencies;

        public string AbsolutePath { get; }

        public bool IsEnabled { get; set; }

        public Logger(
            ILoggingDependencies dependencies,
            string absolutePath,
            bool isEnabled
        )
        {
            _dependencies = dependencies;
            AbsolutePath = absolutePath;
            IsEnabled = isEnabled;
        }

        public void Info(string log)
        {
            if (!IsEnabled) return;

            var logString = FormatLogString(AbsolutePath, log);
            _dependencies.LogInfo(logString);
        }

        public void InfoWithContext(string log, object ctx)
        {
            if (!IsEnabled) return;

            var logString = FormatLogString(AbsolutePath, log);
            _dependencies.LogInfoCtx(logString, ctx);
        }

        public void InfoAlways(string log)
        {
            var logString = FormatLogString(AbsolutePath, log);
            _dependencies.LogInfo(logString);
        }

        public void InfoWithContextAlways(string log, object ctx)
        {
            var logString = FormatLogString(AbsolutePath, log);
            _dependencies.LogInfoCtx(logString, ctx);
        }

        public void Error(string log)
        {
            var logString = FormatLogString(AbsolutePath, log);
            _dependencies.LogError(logString);
        }

        public void ErrorWithContext(string log, object ctx)
        {
            var logString = FormatLogString(AbsolutePath, log);
            _dependencies.LogErrorCtx(logString, ctx);
        }

        public void Exception(Exception ex)
        {
            _dependencies.LogException(ex);
        }

        public void ExceptionWithContext(Exception ex, object ctx)
        {
            _dependencies.LogExceptionCtx(ex, ctx);
        }

        private static string FormatLogString(string logName, string log) =>
            $"[{logName}] {log}";
    }
}