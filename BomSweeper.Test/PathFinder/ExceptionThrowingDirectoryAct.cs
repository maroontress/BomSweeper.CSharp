namespace BomSweeper.Test.PathFinder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BomSweeper;

    public class ExceptionThrowingDirectoryAct : DirectoryAct
    {
        private readonly Action throwingAction;

        public ExceptionThrowingDirectoryAct(
            Action throwingAction,
            string name,
            FileAttributes attributes = FileAttributes.Directory)
        {
            this.throwingAction = throwingAction;
            Name = name;
            Attributes = attributes;
        }

        public string Name { get; }

        public FileAttributes Attributes { get; }

        public IEnumerable<DirectoryAct> GetDirectories()
        {
            throwingAction();
            throw new ApplicationException();
        }

        public IEnumerable<FileAct> GetFiles()
        {
            throwingAction();
            throw new ApplicationException();
        }
    }
}
