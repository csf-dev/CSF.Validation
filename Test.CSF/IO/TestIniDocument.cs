//
// TestIniDocument.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using NUnit.Framework;
using CSF.IniDocuments;

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

