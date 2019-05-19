namespace BomSweeper.Test.PathFinder
{
    using System.IO;
    using BomSweeper;

    /// <summary>
    /// A <see cref="FileAct"/> implementation for unit test.
    /// </summary>
    public sealed class TestFileAct : FileAct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFileAct"/> class.
        /// </summary>
        /// <param name="name">
        /// The file's name.
        /// </param>
        /// <param name="attributes">
        /// The file's attribute.
        /// </param>
        public TestFileAct(string name, FileAttributes attributes = default)
        {
            Name = name;
            Attributes = attributes;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public FileAttributes Attributes { get; }
    }
}
