namespace BomSweeper.Test.PathFinder;

using System.Collections.Generic;
using System.IO;
using BomSweeper;

/// <summary>
/// A <see cref="DirectoryAct"/> implementation for unit test.
/// </summary>
/// <param name="Name">
/// The directory's name.
/// </param>
/// <param name="Attributes">
/// The directory's attribute.
/// </param>
public record class TestDirectoryAct(
    string Name,
    FileAttributes Attributes = FileAttributes.Directory) : DirectoryAct
{
    private readonly List<DirectoryAct> dirList = [];
    private readonly List<FileAct> fileList = [];

    /// <inheritdoc/>
    public IEnumerable<DirectoryAct> GetDirectories()
        => dirList;

    /// <inheritdoc/>
    public IEnumerable<FileAct> GetFiles()
        => fileList;

    /// <summary>
    /// Add a child directory to this directory.
    /// </summary>
    /// <param name="dir">
    /// The directory to be added to this.
    /// </param>
    /// <returns>
    /// This object.
    /// </returns>
    public TestDirectoryAct AddDir(DirectoryAct dir)
    {
        dirList.Add(dir);
        return this;
    }

    /// <summary>
    /// Add a file to this directory.
    /// </summary>
    /// <param name="file">
    /// The file to be added to this.
    /// </param>
    /// <returns>
    /// This object.
    /// </returns>
    public TestDirectoryAct AddFile(FileAct file)
    {
        fileList.Add(file);
        return this;
    }
}
