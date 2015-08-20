//
// TestAssemblyExtensions.cs
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
using System.Reflection;
using CSF.Reflection;
using NUnit.Framework;

namespace Test.CSF.Reflection
{
  [TestFixture]
  public class TestAssemblyExtensions
  {
    [Test]
    public void TestGetManifestResourceText()
    {
      string resourceText = Assembly.GetExecutingAssembly().GetManifestResourceText("TestResource.txt");
      Assert.AreEqual("This is a test resource file", resourceText, "Correct resource text");
    }

    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestGetManifestResourceTextInvalid()
    {
      try
      {
      Assembly.GetExecutingAssembly().GetManifestResourceText("Nonexistent.txt");
      }
      catch(InvalidOperationException ex)
      {
        Assert.IsTrue(ex.Message.StartsWith("Manifest resource 'Nonexistent.txt' was not found in the assembly 'Test.CSF, Version="));
        throw;
      }
      Assert.Fail("Test should not reach this point");
    }

    [Test]
    public void TestGetManifestResourceTextType()
    {
      string resourceText = Assembly.GetExecutingAssembly().GetManifestResourceText(typeof(TestAssemblyExtensions),
                                                                                    "TestResourceType.txt");
      Assert.AreEqual("This is a test resource file, stored by namespace", resourceText, "Correct resource text");
    }

    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestGetManifestResourceTextTypeInvalid()
    {
      try
      {
      Assembly.GetExecutingAssembly().GetManifestResourceText(typeof(TestAssemblyExtensions), "Nonexistent.txt");
      }
      catch(InvalidOperationException ex)
      {
        Assert.IsTrue(ex.Message.StartsWith("Manifest resource 'Test.CSF.Reflection.Nonexistent.txt' was not found in the assembly 'Test.CSF, Version="));
        throw;
      }
      Assert.Fail("Test should not reach this point");
    }

    [Test]
    public void TestGetAttribute()
    {
      Assembly assembly = Assembly.GetAssembly(typeof(AssemblyExtensions));
      AssemblyProductAttribute attrib = assembly.GetAttribute<AssemblyProductAttribute>();

      Assert.IsNotNull(attrib);
      Assert.AreEqual("CSF Software Utilities", attrib.Product);
    }

    [Test]
    public void TestHasAttribute()
    {
      Assembly assembly = Assembly.GetAssembly(typeof(AssemblyExtensions));
      Assert.IsTrue(assembly.HasAttribute<AssemblyProductAttribute>());
    }
  }
}

