namespace BomSweeper.Test.PathFinder
{
    using System.Collections.Generic;
    using System.IO;
    using BomSweeper;

    public class TestDirectoryAct : DirectoryAct
    {
        private readonly List<DirectoryAct> dirList;
        private readonly List<FileAct> fileList;

        public TestDirectoryAct(
            string name,
            FileAttributes attributes = FileAttributes.Directory)
        {
            Name = name;
            Attributes = attributes;
            dirList = new List<DirectoryAct>();
            fileList = new List<FileAct>();
        }

        public string Name { get; }

        public FileAttributes Attributes { get; }

        public IEnumerable<DirectoryAct> GetDirectories()
            => dirList;

        public IEnumerable<FileAct> GetFiles()
            => fileList;

        public TestDirectoryAct AddDir(DirectoryAct dir)
        {
            dirList.Add(dir);
            return this;
        }

        public TestDirectoryAct AddFile(FileAct file)
        {
            fileList.Add(file);
            return this;
        }
    }
}
