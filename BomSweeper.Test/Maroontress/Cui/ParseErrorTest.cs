namespace Maroontress.Cui.Test
{
    using System.Collections.Generic;
    using Maroontress.Cui;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class ParseErrorTest
    {
        [TestMethod]
        public void DuplicateLongNameOption()
        {
            var list = new List<Option>();
            var schema = Options.NewSchema()
                .Add(
                    "help",
                    'h',
                    o => list.Add(o),
                    "Show help message and exit");

            Assert.ThrowsException<InvalidOptionSchemaException>(
                () => schema.Add(
                    "help",
                    'H',
                    o => list.Add(o),
                    "Show help message and exit"));
        }

        [TestMethod]
        public void DuplicateShortNameOption()
        {
            var list = new List<Option>();
            var schema = Options.NewSchema()
                .Add(
                    "show-help",
                    'h',
                    o => list.Add(o),
                    "Show help message and exit");

            Assert.ThrowsException<InvalidOptionSchemaException>(
                () => schema.Add(
                    "help",
                    'h',
                    o => list.Add(o),
                    "Show help message and exit"));
        }

        [TestMethod]
        public void ShortNameIsHyphen()
        {
            Assert.ThrowsException<InvalidOptionSchemaException>(
                () => Options.NewSchema()
                .Add(
                    "enable-hyphen",
                    '-',
                    "Enable hyphen"));
        }

        [TestMethod]
        public void NoArgumentWithLongNameOption()
        {
            var list = new List<ValueOption>();
            var schema = Options.NewSchema()
                .Add(
                    "file",
                    'f',
                    v => list.Add(v),
                    "FILE",
                    "Specify input file");

            var args = new[]
            {
                "--file",
            };
            Assert.ThrowsException<OptionParsingException>(
                () => _ = schema.Parse(args));
        }

        [TestMethod]
        public void UnableToGetArgumentWithLongNameOption()
        {
            var list = new List<Option>();
            var schema = Options.NewSchema()
                .Add(
                    "help",
                    'h',
                    o => list.Add(o),
                    "Show help message and exit");

            var args = new[]
            {
                "--help=yes",
            };
            Assert.ThrowsException<OptionParsingException>(
                () => _ = schema.Parse(args));
        }

        [TestMethod]
        public void UnknownOptionWithShortName()
        {
            var schema = Options.NewSchema();

            var args = new[]
            {
                "-h",
            };
            Assert.ThrowsException<OptionParsingException>(
                () => _ = schema.Parse(args));
        }

        [TestMethod]
        public void UnknownOptionWithNoArgShortName()
        {
            var schema = Options.NewSchema()
                .Add(
                    "help",
                    'h',
                    "Show help message and exit");

            var args = new[]
            {
                "-vh",
            };
            Assert.ThrowsException<OptionParsingException>(
                () => _ = schema.Parse(args));
        }

        [TestMethod]
        public void UnknownOptionWithLongName()
        {
            var schema = Options.NewSchema();

            var args = new[]
            {
                "--help",
            };
            Assert.ThrowsException<OptionParsingException>(
                () => _ = schema.Parse(args));
        }

        [TestMethod]
        public void ShortNameOptionRequiringArgumentWithoutArgument()
        {
            var list = new List<ValueOption>();
            var schema = Options.NewSchema()
                .Add(
                    "file",
                    'f',
                    v => list.Add(v),
                    "FILE",
                    "Specify input file");

            var args = new[]
            {
                "-f",
            };
            Assert.ThrowsException<OptionParsingException>(
                () => _ = schema.Parse(args));
        }

        [TestMethod]
        public void ConcatenatedShortNameOptionRequiringArgument()
        {
            var list = new List<ValueOption>();
            var schema = Options.NewSchema()
                .Add(
                    "help",
                    'h',
                    "Show help message and exit")
                .Add(
                    "file",
                    'f',
                    v => list.Add(v),
                    "FILE",
                    "Specify input file");

            var args = new[]
            {
                "-fh",
            };
            Assert.ThrowsException<OptionParsingException>(
                () => _ = schema.Parse(args));
        }
    }
}
