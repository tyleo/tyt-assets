# Tyleo.Logging.Core

Concrete implementation of the interfaces defined in
`com.tyleo.logging`.

## Types

### `LogSys`

Entry point. Call `LogSys.Create(ILoggingDependencies)` to get a
`LogSys` (implements `ILogSys`) and a root `ILoggerFactoryNode`.
Use the factory node to build out the logger tree, then pass
individual `ILogger` instances to the systems that need them.

### `Logger` *(internal)*

Implements `ILogger`. Prefixes every message with its absolute
path: `[path/to/logger] message`. Honors `IsEnabled` for
conditional methods.

### `LoggerFactoryNode` *(internal)*

Implements `ILoggerFactoryNode`. Manages a tree of loggers and
filter nodes keyed by `/`-delimited paths. Validates that names
are non-empty, contain no `/`, and are unique at each level.

### `ILoggingDependencies`

Dependency-injection seam for the actual log output. Implement
this interface to route messages to Unity's console, a file, or
any other backend.

## Integration

```
com.tyleo.logging          (interfaces)
        ^
        |
com.tyleo.logging.core     (implementation)
        |
        v
ILoggingDependencies        (your backend)
```

Consumer code depends only on `com.tyleo.logging`. The core
package is wired up once at startup via `LogSys.Create`, and the
`ILoggingDependencies` implementation bridges to the actual
logging backend.
