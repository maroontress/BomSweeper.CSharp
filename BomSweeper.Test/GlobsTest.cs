namespace BomSweeper.Test;

using BomSweeper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class GlobsTest
{
    [TestMethod]
    public void NoWildcardSingleArgument()
    {
        var p = Globs.ToPattern(["foo"]);
        Assert.AreEqual("^(foo)$", p);
    }

    [TestMethod]
    public void NoWildCardMultipleArguments()
    {
        var p = Globs.ToPattern(["foo", "bar"]);
        Assert.AreEqual("^(foo|bar)$", p);
    }

    [TestMethod]
    public void Asterisk()
    {
        var p = Globs.ToPattern(["foo*"]);
        Assert.AreEqual("^(foo[^/]*)$", p);
    }

    [TestMethod]
    public void TwoAsterisks()
    {
        var p = Globs.ToPattern(["foo**"]);
        Assert.AreEqual("^(foo[^/]*)$", p);
    }

    [TestMethod]
    public void ThreeAsterisks()
    {
        var p = Globs.ToPattern(["foo***"]);
        Assert.AreEqual("^(foo[^/]*)$", p);
    }

    [TestMethod]
    public void DoubleAsteriskOnly()
    {
        var p = Globs.ToPattern(["**"]);
        Assert.AreEqual("^(.+)$", p);
    }

    [TestMethod]
    public void BeginsWithDoubleAsteriskSlash()
    {
        var p = Globs.ToPattern(["**/foo"]);
        Assert.AreEqual("^(([^/]+/)*foo)$", p);
    }

    [TestMethod]
    public void BeginsWithDoubleAsteriskSlashRepeating()
    {
        var p = Globs.ToPattern(["**/**/foo"]);
        Assert.AreEqual("^(([^/]+/)*foo)$", p);
    }

    [TestMethod]
    public void EndsWithDoubleAsteriskSlash()
    {
        var p = Globs.ToPattern(["foo/**"]);
        Assert.AreEqual("^(foo/.+)$", p);
    }

    [TestMethod]
    public void ContainsSlashDoubleAsteriskSlash()
    {
        var p = Globs.ToPattern(["foo/**/bar"]);
        Assert.AreEqual("^(foo/([^/]+/)*bar)$", p);
    }

    [TestMethod]
    public void ContainsSlashDoubleAsteriskSlashRepeating()
    {
        var p = Globs.ToPattern(["foo/**/**/bar"]);
        Assert.AreEqual("^(foo/([^/]+/)*bar)$", p);
    }

    [TestMethod]
    public void PatternTwoCharacter()
    {
        var p = Globs.ToPattern(["ok"]);
        Assert.AreEqual("^(ok)$", p);
    }

    [TestMethod]
    public void PatternEscapeCharacter()
    {
        var p = Globs.ToPattern(["a.b"]);
        Assert.AreEqual("^(a\\.b)$", p);
    }
}
