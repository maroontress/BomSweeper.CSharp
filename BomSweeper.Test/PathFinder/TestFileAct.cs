namespace BomSweeper.Test.PathFinder;

using System.IO;
using BomSweeper;

/// <summary>
/// A <see cref="FileAct"/> implementation for unit test.
/// </summary>
/// <param name="Name">
/// The file's name.
/// </param>
/// <param name="Attributes">
/// The file's attribute.
/// </param>
public record class TestFileAct(
        string Name, FileAttributes Attributes = default)
    : FileAct
{
}
