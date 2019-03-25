namespace BomSweeper.Test.PathFinder
{
    using System.IO;
    using BomSweeper;

    public sealed class TestFileAct : FileAct
    {
        public TestFileAct(string name, FileAttributes attributes = default)
        {
            Name = name;
            Attributes = attributes;
        }

        public string Name { get; }

        public FileAttributes Attributes { get; }
    }
}
