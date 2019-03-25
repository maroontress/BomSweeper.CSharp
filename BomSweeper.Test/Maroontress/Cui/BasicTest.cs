#pragma warning disable SA1118

namespace Maroontress.Cui.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Maroontress.Cui;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class BasicTest
    {
        [TestMethod]
        public void NoOptionNoArg()
        {
            var schema = Options.NewSchema();
            var settings = schema.Parse(Array.Empty<string>());
            Assert.AreEqual(0, settings.Arguments.Count());
            Assert.AreEqual(0, settings.Options.Count());
            Assert.AreSame(schema, settings.Schema);
        }

        [TestMethod]
        public void OneOptionOneArg()
        {
            var schema = Options.NewSchema()
                .Add("help", 'h', "Show help message");

            void Check(params string[] args)
            {
                var settings = schema.Parse(args);
                var arguments = settings.Arguments.ToArray();
                var options = settings.Options.ToArray();
                Assert.AreEqual(1, arguments.Length);
                Assert.AreEqual("foo", arguments[0]);
                Assert.AreEqual(1, options.Length);
                var o = options[0];
                Assert.AreEqual("help", o.Name);
                Assert.IsTrue(o.ShortName.HasValue);
                Assert.AreEqual('h', o.ShortName.GetValueOrDefault());
                Assert.AreEqual("Show help message", o.Description);
                Assert.AreSame(schema, o.Schema);
                Assert.AreSame(schema, settings.Schema);
            }

            Check("--help", "foo");
            Check("-h", "foo");
            Check("--help", "-", "foo");
            Check("-h", "-", "foo");

            var helpLines = schema.GetHelpMessage().ToArray();
            Assert.AreEqual(1, helpLines.Length);
            var help = helpLines[0];
            Assert.AreEqual(
                "-h, --help  Show help message",
                help);
        }

        [TestMethod]
        public void OneArgOptionOneArg()
        {
            var list = new List<ValueOption>();
            var schema = Options.NewSchema()
                .Add(
                    "file",
                    'f',
                    o => list.Add(o),
                    "FILE",
                    "Specify input file");

            void CheckValue(ValueOption v)
            {
                Assert.AreEqual("bar", v.Value);
                var a = v.Values;
                Assert.AreEqual(1, a.Count());
                Assert.AreEqual("bar", a.First());
            }

            void CheckArg(ValueOption v)
            {
                Assert.AreEqual("file", v.Name);
                Assert.IsTrue(v.ShortName.HasValue);
                Assert.AreEqual('f', v.ShortName.GetValueOrDefault());
                Assert.AreEqual("Specify input file", v.Description);
                Assert.AreEqual("FILE", v.ArgumentName);
            }

            void Check(params string[] args)
            {
                list.Clear();
                var settings = schema.Parse(args);
                var arguments = settings.Arguments.ToArray();
                var options = settings.Options.ToArray();
                Assert.AreEqual(1, arguments.Length);
                Assert.AreEqual("foo", arguments[0]);
                Assert.AreEqual(1, options.Length);
                var o = options[0];
                if (o is ValueOption v)
                {
                    CheckArg(v);
                    CheckValue(v);
                }
                else
                {
                    Assert.Fail();
                }

                Assert.AreSame(schema, o.Schema);
                Assert.AreSame(schema, settings.Schema);

                Assert.AreEqual(1, list.Count());
                if (list.First() is ValueOption e)
                {
                    CheckArg(e);
                    CheckValue(e);
                }
                else
                {
                    Assert.Fail();
                }
            }

            Check("--file=bar", "foo");
            Check("-f", "bar", "foo");
            Check("--file=bar", "-", "foo");
            Check("-f", "bar", "-", "foo");

            var helpLines = schema.GetHelpMessage().ToArray();
            Assert.AreEqual(1, helpLines.Length);
            var help = helpLines[0];
            Assert.AreEqual(
                "-f FILE, --file=FILE    Specify input file",
                help);
        }

        [TestMethod]
        public void HelpMessageOfLongNameOption()
        {
            var list = new List<ValueOption>();
            var schema = Options.NewSchema()
                .Add(
                    "very-very-long-name-option",
                    'v',
                    v => list.Add(v),
                    "ARGUMENT",
                    "Specify an argument");

            var helpLines = schema.GetHelpMessage().ToArray();
            var expected = new[]
            {
                "-v ARGUMENT, --very-very-long-name-option=ARGUMENT",
                "                                Specify an argument",
            };
            Assert.IsTrue(expected.SequenceEqual(helpLines));
        }

        [TestMethod]
        public void HelpMessageHasMutipleLines()
        {
            var list = new List<ValueOption>();
            var schema = Options.NewSchema()
                .Add(
                    "file",
                    'f',
                    v => list.Add(v),
                    "FILE",
                    "Specify input file\n"
                    + "Example: -f foo.txt, --file=foo.txt");

            var helpLines = schema.GetHelpMessage().ToArray();
            var expected = new[]
            {
                "-f FILE, --file=FILE    Specify input file",
                "                        Example: -f foo.txt, --file=foo.txt",
            };
            Assert.IsTrue(expected.SequenceEqual(helpLines));
        }

        [TestMethod]
        public void HelpMessageSorting()
        {
            var list = new List<ValueOption>();
            var schema = Options.NewSchema()
                .Add(
                    "file",
                    'f',
                    v => list.Add(v),
                    "FILE",
                    "Specify input file")
                .Add(
                    "dir",
                    'd',
                    v => list.Add(v),
                    "DIR",
                    "Change the current directory");

            var helpLines = schema.GetHelpMessage().ToArray();
            var expected = new[]
            {
                "-d DIR, --dir=DIR       Change the current directory",
                "-f FILE, --file=FILE    Specify input file",
            };
            Assert.IsTrue(expected.SequenceEqual(helpLines));
        }

        [TestMethod]
        public void HelpMessageWithNoShortNameOption()
        {
            var schema = Options.NewSchema()
                .Add("verbose", null, "Be verbose")
                .Add("debug", 'd', "Be debug mode");

            var helpLines = schema.GetHelpMessage().ToArray();
            var expected = new[]
            {
            /*   |---|---|---|---| */
                "-d, --debug     Be debug mode",
                "    --verbose   Be verbose",
            };
            Assert.IsTrue(expected.SequenceEqual(helpLines));
        }

        [TestMethod]
        public void ConcatenatingShortOptions()
        {
            var schema = Options.NewSchema()
                .Add(
                    "verbose",
                    'v',
                    "Be verbose")
                .Add(
                    "help",
                    'h',
                    "Show help message and exit");
            var settings = schema.Parse(new[] { "-vh" });
            Assert.AreSame(schema, settings.Schema);

            var arguments = settings.Arguments;
            Assert.IsFalse(arguments.Any());

            var options = settings.Options.ToArray();
            Assert.AreEqual(2, options.Length);

            void Check(Option o, string name)
            {
                if (o is ValueOption)
                {
                    Assert.Fail();
                }
                Assert.AreEqual(name, o.Name);
                Assert.AreSame(schema, o.Schema);
            }

            Check(options[0], "verbose");
            Check(options[1], "help");
        }

        [TestMethod]
        public void MultipleArguments()
        {
            var list = new List<ValueOption>();
            var schema = Options.NewSchema()
                .Add(
                    "file",
                    'f',
                    v => list.Add(v),
                    "FILE",
                    "Specify input file");

            var settings = schema.Parse(new[]
            {
                "--file=foo.txt",
                "--file=bar.txt",
            });
            Assert.AreSame(schema, settings.Schema);

            var arguments = settings.Arguments;
            Assert.IsFalse(arguments.Any());

            var options = settings.Options.ToArray();
            Assert.AreEqual(2, options.Length);

            void CheckFirst(Option o)
            {
                if (!(o is ValueOption v))
                {
                    Assert.Fail();
                    return;
                }
                Assert.AreEqual("file", v.Name);
                Assert.AreSame(schema, v.Schema);
                Assert.AreEqual("foo.txt", v.Value);
                var expectedValues = new[]
                {
                    "foo.txt",
                };
                Assert.IsTrue(expectedValues.SequenceEqual(v.Values));
            }

            void CheckSecond(Option o)
            {
                if (!(o is ValueOption v))
                {
                    Assert.Fail();
                    return;
                }
                Assert.AreEqual("file", v.Name);
                Assert.AreSame(schema, v.Schema);
                Assert.AreEqual("bar.txt", v.Value);
                var expectedValues = new[]
                {
                    "foo.txt",
                    "bar.txt",
                };
                Assert.IsTrue(expectedValues.SequenceEqual(v.Values));
            }

            CheckFirst(options[0]);
            CheckSecond(options[1]);
            Assert.IsTrue(list.SequenceEqual(options));
        }
    }
}
