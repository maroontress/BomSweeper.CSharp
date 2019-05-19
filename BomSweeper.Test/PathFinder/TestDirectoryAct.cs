namespace BomSweeper.Test.PathFinder
{
    using System.Collections.Generic;
    using System.IO;
    using BomSweeper;

    /// <summary>
    /// A <see cref="DirectoryAct"/> implementation for unit test.
    /// </summary>
    public sealed class TestDirectoryAct : DirectoryAct
    {
        private readonly List<DirectoryAct> dirList;
        private readonly List<FileAct> fileList;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDirectoryAct"/>
        /// class.
        /// </summary>
        /// <param name="name">
        /// The directory's name.
        /// </param>
        /// <param name="attributes">
        /// The directory's attribute.
        /// </param>
        public TestDirectoryAct(
            string name,
            FileAttributes attributes = FileAttributes.Directory)
        {
            Name = name;
            Attributes = attributes;
            dirList = new List<DirectoryAct>();
            fileList = new List<FileAct>();
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public FileAttributes Attributes { get; }

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
}
