#nullable enable

using System.Runtime.CompilerServices;

namespace Tyleo.Logging
{
    /// <summary>
    /// Enabled logger returned by <see cref="LoggerExt.IfEnabled"/>.
    /// </summary>
    public readonly struct EnabledLogger
    {
        private readonly ILogger _logger;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal EnabledLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="ILogger.InfoAlways"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Info(string message) =>
            _logger.InfoAlways(message);

        /// <inheritdoc cref="LoggerExt.InfoCallerNameAlways"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void InfoCallerName(
            [CallerMemberName] string callerName = default!
        ) => _logger.InfoAlways(callerName);

        /// <inheritdoc cref="LoggerExt.InfoEmptyAlways"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void InfoEmpty() =>
            _logger.InfoEmptyAlways();

        /// <inheritdoc cref="ILogger.InfoWithContextAlways"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void InfoWithContext(string message, object context) =>
            _logger.InfoWithContextAlways(message, context);
    }
}