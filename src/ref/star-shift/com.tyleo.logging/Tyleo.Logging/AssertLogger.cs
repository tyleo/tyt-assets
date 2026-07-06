#nullable enable

using System.Runtime.CompilerServices;

namespace Tyleo.Logging
{
    /// <summary>
    /// Assertion logger returned by
    /// <see cref="LoggerExt.IfFalse"/>.
    /// </summary>
    public readonly struct AssertLogger
    {
        private readonly ILogger _logger;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal AssertLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="ILogger.Error"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Error(string message) => _logger.Error(message);

        /// <inheritdoc cref="ILogger.ErrorWithContext"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void ErrorWithContext(string message, object context) =>
            _logger.ErrorWithContext(message, context);
    }
}