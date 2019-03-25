namespace BomSweeper.Test
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class ElementsTest
    {
        [TestMethod]
        public void SeparateWithEmpty()
        {
            var actual = Elements.Separate(Array.Empty<string>(), "foo");
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void SeparateWithSingle()
        {
            var actual = Elements.Separate(new[] { "bar" }, "foo");
            Assert.AreEqual(actual.Count(), 1);
            Assert.AreEqual(actual.First(), "bar");
        }

        [TestMethod]
        public void SeparateWithDouble()
        {
            var actual = Elements.Separate(new[] { "bar", "baz" }, "foo");
            var expected = new[] { "bar", "foo", "baz" };
            Assert.IsTrue(Enumerable.SequenceEqual(actual, expected));
        }
    }
}
