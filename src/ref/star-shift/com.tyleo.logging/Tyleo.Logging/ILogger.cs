#nullable enable

using System;

namespace Tyleo
{
    /// <summary>
    /// Interface for logging functionality.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets the absolute path of this logger.
        /// </summary>
        string AbsolutePath { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this logger is enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="error">The error to log.</param>
        void Error(string error);

        /// <summary>
        /// Logs an error with context.
        /// </summary>
        /// <param name="error">The error to log.</param>
        /// <param name="context">
        /// The context object to log alongside the error.
        /// </param>
        void ErrorWithContext(string error, object context);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        void Exception(Exception ex);

        /// <summary>
        /// Logs an exception with context.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <param name="context">
        /// The context object to log alongside the exception.
        /// </param>
        void ExceptionWithContext(Exception ex, object context);

        /// <summary>
        /// Logs a message if the logger is enabled.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Info(string message);

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void InfoAlways(string message);

        /// <summary>
        /// Logs a message with context if the logger is enabled.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="context">
        /// The context object to log alongside the message.
        /// </param>
        void InfoWithContext(string message, object context);

        /// <summary>
        /// Logs a message with context.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="context">
        /// The context object to log alongside the message.
        /// </param>
        void InfoWithContextAlways(string message, object context);
    }
}
