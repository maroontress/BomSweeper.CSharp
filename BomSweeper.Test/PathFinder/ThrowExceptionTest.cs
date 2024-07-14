namespace BomSweeper.Test.PathFinder;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using BomSweeper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class ThrowExceptionTest
{
    [TestMethod]
    public void GetFiles()
    {
        static void Perform(int depth)
        {
            var actual = PathFinder.GetFiles(".", depth);
            var array = actual.ToArray();
            Array.Sort(array);
            var expected = new[]
            {
                Path.Combine(".", "baz", "foo"),
                Path.Combine(".", "foo"),
            };
            Assert.IsTrue(Enumerable.SequenceEqual(expected, array));
        }

        Toolkit.TheInstance = new TestToolkit();
        Perform(3);
    }

    private sealed class TestToolkit : Toolkit
    {
        private readonly Dictionary<string, DirectoryAct> map;

        public TestToolkit()
        {
            /*
                ./foo
                ./bar/     <-- DirectryNotFound
                ./baz/foo
                ./baz/bar/ <-- Security
            */
            var bazBar = new ExceptionThrowingDirectoryAct(
                () => new SecurityException(), "bar");
            var bar = new ExceptionThrowingDirectoryAct(
                () => new DirectoryNotFoundException(), "bar");
            var baz = new TestDirectoryAct("baz")
                .AddFile(new TestFileAct("foo"))
                .AddDir(bazBar);
            var current = new TestDirectoryAct("base")
                .AddFile(new TestFileAct("foo"))
                .AddDir(bar)
                .AddDir(baz);

            map = new Dictionary<string, DirectoryAct>
            {
                ["."] = current,
                [Path.Combine(".", "bar")] = bar,
                [Path.Combine(".", "baz")] = baz,
                [Path.Combine(".", "baz", "bar")] = bazBar,
            };
        }

        public override DirectoryAct GetDirectoryAct(string path)
        {
            return map[path];
        }
    }
}
