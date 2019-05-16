namespace BomSweeper.Test.PathFinder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BomSweeper;

    /// <summary>
    /// A <see cref="DirectoryAct"/> implementation. All the methods throw
    /// an exception.
    /// </summary>
    public sealed class ExceptionThrowingDirectoryAct : DirectoryAct
    {
        private readonly Func<Exception> newException;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ExceptionThrowingDirectoryAct"/> class.
        /// </summary>
        /// <param name="newException">
        /// The function that returns a new exception. Calling any method of
        /// this class throws the exception this function supplies.
        /// </param>
        /// <param name="name">
        /// The directory's name.
        /// </param>
        /// <param name="attributes">
        /// The directory's attribute.
        /// </param>
        public ExceptionThrowingDirectoryAct(
            Func<Exception> newException,
            string name,
            FileAttributes attributes = FileAttributes.Directory)
        {
            this.newException = newException;
            Name = name;
            Attributes = attributes;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public FileAttributes Attributes { get; }

        /// <inheritdoc/>
        public IEnumerable<DirectoryAct> GetDirectories()
        {
            throw newException();
        }

        /// <inheritdoc/>
        public IEnumerable<FileAct> GetFiles()
        {
            throw newException();
        }
    }
}
