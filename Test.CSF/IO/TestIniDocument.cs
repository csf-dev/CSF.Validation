using System;
using NUnit.Framework;
using CSF.IO;

namespace Test.CSF.IO
{
  [TestFixture]
  public class TestIniDocument
  {
    [Test]
    public void TestWrite()
    {
      IniDocument document = new IniDocument();
      
      document["foo"] = "bar";
      document["bork"] = "baz";
      
      document.Sections.Add("test", new IniSection());
      document.Sections["test"]["sample"] = "someValue";
      
      document.Sections.Add("another test", new IniSection());
      document.Sections["another test"]["value"] = "another value";
      
      string output = document.Write();
      Assert.AreEqual(@"foo = bar
bork = baz
[test]
sample = someValue
[another test]
value = another value", output, "Correct output");
    }
    
    [Test]
    public void TestRead()
    {
      IIniDocument document = IniDocument.Read(@"foo = bar
bork = baz
[test]
sample = someValue
[another test]
value = another value");
      
      Assert.AreEqual(2, document.Sections.Count, "Correct count of sections");
      Assert.AreEqual(2, document.Count, "Correct count of values");
      Assert.AreEqual("baz", document["bork"], "Correct value for 'bork'");
      Assert.AreEqual("someValue", document.Sections["test"]["sample"], "Correct value for 'test' » 'sample'");
    }
    
    [Test]
    public void TestReadCommentedFile()
    {
      IIniDocument document = IniDocument.Read(@"foo = bar

; This is a comment
# This is a different comment

bork = ""This is a quoted value""
[test]
sample = someValue
[another test]
value = another value");
      
      Assert.AreEqual(2, document.Sections.Count, "Correct count of sections");
      Assert.AreEqual(2, document.Count, "Correct count of values");
      Assert.AreEqual("\"This is a quoted value\"", document["bork"], "Correct value for 'bork'");
      Assert.AreEqual("someValue", document.Sections["test"]["sample"], "Correct value for 'test' » 'sample'");
    }
    
    [Test]
    [ExpectedException(typeof(FormatException))]
    public void TestReadInvalidFile()
    {
      IniDocument.Read(@"foo = bar

; This is a comment
# This is a different comment

this is an invalid line

bork = ""This is a quoted value""
[test]
sample = someValue
[another test]
value = another value");
      
      Assert.Fail("Test should not reach this point");
    }
  }
}

