# Tyleo.Logging

Logging interfaces and helpers. This package defines
the contracts. `com.tyleo.logging.core` provides the implementation.

## Types

### `ILogger`

Core logging interface. Supports `Info`, `Error`, and `Exception`
methods, each with optional `object context` and `Always` variants
that bypass the `IsEnabled` check. `AbsolutePath` identifies the
logger in the hierarchy.

### `ILogSys`

Exposes `AllLoggers` for iterating every registered logger.

### `ILoggerFactoryNode`

Factory for building a hierarchical logger tree.
`CreateChildLogger` produces an `ILogger`;
`CreateChildFilterNode` produces an intermediate grouping node.

### `EnabledLogger` / `AssertLogger`

Lightweight `readonly struct` wrappers returned by the
null-conditional helpers below. They forward calls to the
underlying `ILogger` without an additional enabled check.

### `LoggerExt`

Extension methods on `ILogger`:

- `IfEnabled()` - returns `EnabledLogger?`, non-null only when
  the logger is enabled.
- `IfFalse(bool)` - returns `AssertLogger?`, non-null only when
  the condition is false.
- `IfEnabledAndDebug()`, `IfFalseAndDebug()`, `IfDebug()` -
  DEBUG-only variants that compile away in release builds.
- `InfoCallerName` / `InfoEmpty` / `ErrorAndException` helpers.

## Example

```csharp
// 1. Define loggers for your module
public sealed record MyLoggers(
    MyLoggers.NetworkLoggers Network,
    ILogger Initialization
)
{
    internal static MyLoggers FromLoggerNode(
        ILoggerFactoryNode node
    )
    {
        var myNode = node.CreateChildFilterNode("MyModule");

        return new(
            Network: NetworkLoggers.FromLoggerNode(myNode),
            Initialization: myNode.CreateChildLogger(
                nameof(Initialization)
            )
        );
    }

    public sealed record NetworkLoggers(
        ILogger Connection,
        ILogger Messages
    )
    {
        internal static NetworkLoggers FromLoggerNode(
            ILoggerFactoryNode node
        )
        {
            var networkNode = node.CreateChildFilterNode(
                nameof(Network)
            );

            return new(
                Connection: networkNode.CreateChildLogger(
                    nameof(Connection)
                ),
                Messages: networkNode.CreateChildLogger(
                    nameof(Messages)
                )
            );
        }
    }
}

// 2. Store and expose loggers
public static class MyModule
{
    private static MyLoggers? _loggers;

    public static MyLoggers Log => _loggers!;

    public static void InitializeLoggers(
        ILoggerFactoryNode node
    )
    {
        _loggers = MyLoggers.FromLoggerNode(node);
    }
}

// 3. Use loggers
MyModule.Log.Initialization.Info("Started.");

MyModule.Log.Network.Connection.IfFalse(isConnected)?.Error(
    "Expected a connection."
);
```
