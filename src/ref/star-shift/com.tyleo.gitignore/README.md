# com.tyleo.gitignore

.gitignore-style regex matching for file paths.

## Overview

This package provides glob-style pattern matching modeled after `.gitignore`
rules. Patterns like `**/HelloWorld.txt` or `/**/Temp/*` are compiled into
regular expressions and matched against file paths.

## Types

### `GitIgnoreRegex`

A signed pattern. The sign controls whether a match includes or excludes.
Patterns prefixed with `!` negate the match (exclude), otherwise they
include.

```csharp
var regex = GitIgnoreRegex.FromSpan("**/Logs/");
```

### `UnsignedGitIgnoreRegex`

The underlying compiled regex without a sign. Handles the glob-to-regex
translation and exposes `IsMatch(string)`.

### `GitIgnoreRegexKind`

Indicates whether a pattern matches only directories (`Directory`) or
files and directories (`FileOrDirectory`). A trailing `/` in the pattern
produces `Directory`.

### `GitIgnoreExt`

Extension methods for matching arrays or spans of regexes against paths:

- `IsDirectoryMatch` - aggregates signed directory matches across a set
  of patterns.
- `IsFileMatch` - aggregates signed file matches, skipping directory-only
  patterns.
- `IsPathMatch` - aggregates path matches.

## Pattern syntax

| Pattern      | Meaning                                   |
| ------------ | ----------------------------------------- |
| `*`          | Matches any file-name characters (no `/`) |
| `**/foo`     | Matches `foo` in any directory            |
| `foo/**`     | Matches everything inside `foo`           |
| `foo/**/bar` | Zero or more directories between          |
| `!pattern`   | Negates the match                         |
| `pattern/`   | Matches directories only                  |
| `/pattern`   | Anchors the match to the root             |

Patterns follow `.gitignore` semantics: later patterns in a list override
earlier ones, and both inclusion and exclusion are supported.
