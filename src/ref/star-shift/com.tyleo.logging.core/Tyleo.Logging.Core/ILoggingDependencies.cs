#nullable enable

using System;

namespace Tyleo.Logging.Core
{
    /// <summary>
    /// Dependencies for the logging module.
    /// </summary>
    public interface ILoggingDependencies
    {
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs a message with context.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="ctx">
        /// The context object to log alongside the message.
        /// </param>
        void LogInfoCtx(string message, object ctx);

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="error">The error to log.</param>
        void LogError(string error);

        /// <summary>
        /// Logs an error with context.
        /// </summary>
        /// <param name="error">The error to log.</param>
        /// <param name="ctx">
        /// The context object to log alongside the error.
        /// </param>
        void LogErrorCtx(string error, object ctx);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        void LogException(Exception ex);

        /// <summary>
        /// Logs an exception with context.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="ctx">
        /// The context object to log alongside the exception.
        /// </param>
        void LogExceptionCtx(Exception ex, object ctx);
    }
}