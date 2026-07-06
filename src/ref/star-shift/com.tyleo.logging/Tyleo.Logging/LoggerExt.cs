#nullable enable

using System;
using System.Runtime.CompilerServices;

namespace Tyleo.Logging
{
    /// <summary>
    /// Extension methods for <see cref="ILogger"/>.
    /// </summary>
    public static class LoggerExt
    {
        /// <summary>
        /// Logs an error and an exception.
        /// </summary>
        /// <param name="self">The logger to write to.</param>
        /// <param name="error">The error to log.</param>
        /// <param name="ex">The exception to log.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ErrorAndException(
            this ILogger self,
            string message,
            Exception ex
        )
        {
            self.Error(message);
            self.Exception(ex);
        }

        /// <summary>
        /// Logs an error and an exception with context.
        /// </summary>
        /// <param name="self">The logger to write to.</param>
        /// <param name="error">The error to log.</param>
        /// <param name="ex">The exception to log.</param>
        /// <param name="context">
        /// The context object to log alongside the error.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ErrorAndExceptionWithContext(
            this ILogger self,
            string error,
            Exception ex,
            object context
        )
        {
            self.ErrorWithContext(error, context);
            self.ExceptionWithContext(ex, context);
        }

        /// <summary>
        /// <para>
        /// Returns the logger if <see cref="ILogger.IsEnabled"/> is true;
        /// otherwise, returns null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var enabledLogger = logger.IfEnabled()?.Info("This message is logged only if the logger is enabled.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <returns>The logger if enabled; otherwise, null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EnabledLogger? IfEnabled(this ILogger self) =>
            self.IsEnabled ? new EnabledLogger(self) : null;

        /// <summary>
        /// <para>
        /// Returns the logger if the condition is fails; otherwise, returns
        /// null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var assertLogger = logger.IfFalse(condition)?.Error("Assertion failed.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>
        /// The logger if the condition fails; otherwise, null.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AssertLogger? IfFalse(
            this ILogger self,
            bool condition
        )
        {
            if (condition) return null;
            return new AssertLogger(self);
        }

        /// <summary>
        /// <para>
        /// Returns the logger if the condition succeeds; otherwise, returns
        /// null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var assertLogger = logger.IfTrue(condition)?.Error("Assertion failed.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>
        /// The logger if the condition succeeds; otherwise, null.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AssertLogger? IfTrue(
            this ILogger self,
            bool condition
        )
        {
            if (!condition) return null;
            return new AssertLogger(self);
        }

        /// <summary>
        /// Logs the caller member name as an informational message if
        /// <see cref="ILogger.IsEnabled"/> is true.
        /// </summary>
        /// <param name="self">The logger to write to.</param>
        /// <param name="callerName">The caller member name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InfoCallerName(
            this ILogger self,
            [CallerMemberName] string callerName = default!
        ) => self.Info(callerName);

        /// <summary>
        /// Logs the caller member name as an informational message.
        /// </summary>
        /// <param name="self">The logger to write to.</param>
        /// <param name="callerName">The caller member name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InfoCallerNameAlways(
            this ILogger self,
            [CallerMemberName] string callerName = default!
        ) => self.InfoAlways(callerName);

        /// <summary>
        /// Logs an empty informational message if
        /// <see cref="ILogger.IsEnabled"/> is true.
        /// </summary>
        /// <param name="self">The logger to write to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InfoEmpty(
            this ILogger self
        ) => self.Info(string.Empty);

        /// <summary>
        /// Logs an empty informational message.
        /// </summary>
        /// <param name="self">The logger to write to.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InfoEmptyAlways(
            this ILogger self
        ) => self.InfoAlways(string.Empty);
#if DEBUG
        /// <summary>
        /// <para>
        /// Returns the logger in debug builds if
        /// <see cref="ILogger.IsEnabled"/> is true; otherwise, returns null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var enabledLogger = logger.IfEnabledAndDebug()?.Info("This message is logged only if the logger is and in a debug build.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <returns>
        /// The logger if in a debug build and enabled; otherwise, null.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EnabledLogger? IfEnabledAndDebug(this ILogger self) =>
            self.IsEnabled ? new EnabledLogger(self) : null;

        /// <summary>
        /// <para>
        /// Returns the logger in debug builds if the condition is fails;
        /// otherwise, returns null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var assertLogger = logger.IfFalseAndDebug(condition)?.Error("Assertion failed in debug mode.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>
        /// The logger if in a debug build and the condition fails; otherwise,
        /// null.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AssertLogger? IfFalseAndDebug(
            this ILogger self,
            bool condition
        )
        {
            if (condition) return null;
            return new AssertLogger(self);
        }

        /// <summary>
        /// <para>
        /// Returns the logger in debug builds if the condition succeeds;
        /// otherwise, returns null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var assertLogger = logger.IfTrueAndDebug(condition)?.Error("Assertion failed in debug mode.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>
        /// The logger if in a debug build and the condition succeeds;
        /// otherwise, null.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AssertLogger? IfTrueAndDebug(
            this ILogger self,
            bool condition
        )
        {
            if (!condition) return null;
            return new AssertLogger(self);
        }

        /// <summary>
        /// <para>
        /// Returns the logger in debug builds; otherwise, returns null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var debugLogger = logger.IfDebug()?.Info("This message is logged only in debug builds.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <returns>The logger if in debug mode; otherwise, null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILogger? IfDebug(this ILogger self) => self;
#else // DEBUG
        /// <summary>
        /// <para>
        /// Returns the logger in debug builds if
        /// <see cref="ILogger.IsEnabled"/> is true; otherwise, returns null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var enabledLogger = logger.IfEnabledAndDebug()?.Info("This message is logged only if the logger is and in a debug build.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <returns>
        /// The logger if in a debug build and enabled; otherwise, null.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EnabledLogger? IfEnabledAndDebug(this ILogger self) =>
            null;

        /// <summary>
        /// <para>
        /// Returns the logger in debug builds if the condition is fails;
        /// otherwise, returns null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var assertLogger = logger.IfFalseAndDebug(condition)?.Error("Assertion failed in debug mode.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>
        /// The logger if in a debug build and the condition fails; otherwise,
        /// null.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AssertLogger? IfFalseAndDebug(
            this ILogger self,
            bool condition
        ) => null;

        /// <summary>
        /// <para>
        /// Returns the logger in debug builds if the condition succeeds;
        /// otherwise, returns null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var assertLogger = logger.IfTrueAndDebug(condition)?.Error("Assertion failed in debug mode.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>
        /// The logger if in a debug build and the condition succeeds;
        /// otherwise, null.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AssertLogger? IfTrueAndDebug(
            this ILogger self,
            bool condition
        ) => null;

        /// <summary>
        /// <para>
        /// Returns the logger in debug builds; otherwise, returns null.
        /// </para>
        /// <para>
        /// Example Usage:
        /// <code>
        /// var debugLogger = logger.IfDebug()?.Info("This message is logged only in debug builds.");
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="self">The logger to check.</param>
        /// <returns>The logger if in debug mode; otherwise, null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ILogger? IfDebug(this ILogger self) => null;
#endif // DEBUG
    }
}