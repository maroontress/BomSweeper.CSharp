# BomSweeper

BomSweeper is a command line tool that finds the files starting with a UTF-8
Byte Order Mark (BOM) in the directory tree and removing a BOM from those files.

## Requirements

- [.NET Core 2.2 Runtime (Runtime 2.2.4)][dotnet-core-runtime]

## Synopsis

> `dotnet BomSweeper.dll` [`-C` _DIR_] [`-D` _N_] [`-Rhv`] [`-`] _PATTERN_...

## Description

The _PATTERN_ arguments represent the glob patterns that match
the paths of the files to find.

The path separator in the pattern must be a slash ('`/`') character
regardless of the platform. The directory names `.` and `..` in the pattern
are not interpreted specially (that is, `.` and `..` do not mean the current
and parent directory, respectively). So, for example, the pattern
`foo/../bar/baz.cs` does not match `bar/baz.cs`.

Note that the pattern matching is performed with the relative paths to the
current directory, so if the pattern starts with a slash, it does not match
any file.

The pattern can contain an asterisk ('`*`') character as a wildcard, which
matches any character other than a slash zero or more times. It can also
contain a double asterisk ('`**`'), which represents as follows:

- if the pattern equals `**`, it matches all files in the current directory
  and in its subdirectories.

- if the pattern ends with `/**` (a slash followed by a double asterisk), the
  subpattern `/**` matches all files in the directory and subdirectories.

- if the pattern starts with `**/` (a double asterisk followed by a slash),
  the subpattern `**/` matches the current directory and its subdirectories.
  For example, `**/foo` matches `foo`, `bar/foo` and `bar/baz/foo`.

- if the pattern contains `/**/`, the subpattern `/**/` matches a slash,
  the directories and subdirectories. For example, `foo/**/bar` matches
  `foo/bar`, `foo/baz/bar` and `foo/baz/qux/bar`.

Options are as follows:

| Option | Description |
|:---|:---|
| `-C` _DIR_, `--directory=`_DIR_ | Change to directory. (Default: `.`) |
| `-D` _N_, `--max-depth=`_N_ | The maximum number of directory levels to search. (Default: `16`) |
| `-R`, `--remove` | Remove a BOM |
| `-h`, `--help` | Show help message and exit |
| `-v`, `--verbose` | Be verbose |

## Exit status

BomSweeper exits 0 if no files starting with a UTF-8 BOM are found,
and &gt;0 if one or more files are found or if an error occurs.

## Example

```bash
dotnet BomSweeper.dll '**/*.cs'
```

Find `.cs` files in the current directory and subdirectories.

```bash
dotnet BomSweeper.dll -R '**/*.cs'
```

Find `.cs` files in the current directory and subdirectories,
and remove a UTF-8 BOM from the files if any.

## How to build

### Requirements to build

- Visual Studio 2019 Version 16.0
  or [.NET Core 2.2 SDK (SDK 2.2.203)][dotnet-core-sdk]

### How to get started

```bash
git clone URL
cd BomSweeper.CSharp
dotnet restore
dotnet build
```

### How to get test coverage report with Coverlet

```bash
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover \
        --no-build BomSweeper.Test
dotnet ANYWHERE/reportgenerator.dll \
        --reports:BomSweeper.Test/coverage.opencover.xml \
        --targetdir:Coverlet-html
```

[dotnet-core-sdk]:
  https://dotnet.microsoft.com/download/dotnet-core/2.2
[dotnet-core-runtime]:
  https://dotnet.microsoft.com/download/dotnet-core/2.2
