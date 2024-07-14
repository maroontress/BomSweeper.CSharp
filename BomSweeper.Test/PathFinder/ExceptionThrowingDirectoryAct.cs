namespace BomSweeper.Test.PathFinder;

using System;
using System.Collections.Generic;
using System.IO;
using BomSweeper;

/// <summary>
/// A <see cref="DirectoryAct"/> implementation. All the methods throw an
/// exception.
/// </summary>
/// <param name="NewException">
/// The function that returns a new exception. Calling any method of this class
/// throws the exception this function supplies.
/// </param>
/// <param name="Name">
/// The directory's name.
/// </param>
/// <param name="Attributes">
/// The directory's attribute.
/// </param>
public record class ExceptionThrowingDirectoryAct(
    Func<Exception> NewException,
    string Name,
    FileAttributes Attributes = FileAttributes.Directory) : DirectoryAct
{
    /// <inheritdoc/>
    public IEnumerable<DirectoryAct> GetDirectories()
    {
        throw NewException();
    }

    /// <inheritdoc/>
    public IEnumerable<FileAct> GetFiles()
    {
        throw NewException();
    }
}
