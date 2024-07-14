namespace BomSweeper.Test.PathFinder;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BomSweeper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class BasicTest
{
    [TestMethod]
    public void GetFilesWithNegativeMaxDepth()
    {
        Assert.ThrowsException<ArgumentException>(
            () => PathFinder.GetFiles(".", -1));
    }

    [TestMethod]
    public void GetFilesWithZeroMaxDepth()
    {
        var actual = PathFinder.GetFiles(".", 0);
        Assert.AreEqual(0, actual.Count());
    }

    [TestMethod]
    public void GetFilesWithOneMaxDepth()
    {
        Toolkit.TheInstance = new TestToolkit();
        var actual = PathFinder.GetFiles(".", 1);
        var array = actual.ToArray();
        Assert.AreEqual(1, array.Length);
        Assert.AreEqual(Path.Combine(".", "foo"), array[0]);
    }

    [TestMethod]
    public void GetFilesWithTwoMaxDepth()
    {
        Toolkit.TheInstance = new TestToolkit();
        var actual = PathFinder.GetFiles(".", 2);
        var array = actual.ToArray();
        Array.Sort(array);
        var expected = new[]
        {
            Path.Combine(".", "bar", "foo"),
            Path.Combine(".", "baz", "foo"),
            Path.Combine(".", "foo"),
        };
        Assert.IsTrue(Enumerable.SequenceEqual(expected, array));
    }

    [TestMethod]
    public void GetFilesWithThreeOrMoreMaxDepth()
    {
        Toolkit.TheInstance = new TestToolkit();

        static void Perform(int depth)
        {
            var actual = PathFinder.GetFiles(".", depth);
            var array = actual.ToArray();
            Array.Sort(array);
            var expected = new[]
            {
                Path.Combine(".", "bar", "foo"),
                Path.Combine(".", "baz", "bar", "foo"),
                Path.Combine(".", "baz", "foo"),
                Path.Combine(".", "foo"),
            };
            Assert.IsTrue(Enumerable.SequenceEqual(expected, array));
        }
        Perform(3);
        Perform(4);
    }

    private sealed class TestToolkit : Toolkit
    {
        private readonly Dictionary<string, DirectoryAct> map;

        public TestToolkit()
        {
            /*
                ./foo
                ./bar/foo
                ./baz/foo
                ./baz/bar/foo
            */

            var bazBar = new TestDirectoryAct("bar")
                .AddFile(new TestFileAct("foo"));
            var bar = new TestDirectoryAct("bar")
                .AddFile(new TestFileAct("foo"));
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
