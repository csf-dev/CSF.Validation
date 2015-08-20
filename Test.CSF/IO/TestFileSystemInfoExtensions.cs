//
// TestFileSystemInfoExtensions.cs
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
using CSF.IO;
using System.IO;

namespace Test.CSF.IO
{
  [TestFixture]
  public class TestFileSystemInfoExtensions
  {
    #region fields

    private DirectoryInfo _testRoot;

    #endregion

    #region setup

    [TestFixtureSetUp]
    public void FixtureSetup()
    {
      var config = global::CSF.Configuration.ConfigurationHelper.GetSection<TestConfiguration>();
      if(config == null)
      {
        string message = String.Format("Serious test error; `{0}' must not be null.", typeof(TestConfiguration).Name);
        throw new InvalidOperationException(message);
      }

      var mainRoot = config.GetFilesystemTestingRootPath();

      _testRoot = new DirectoryInfo(Path.Combine(mainRoot.FullName, this.GetType().Name));
      if(!_testRoot.Exists)
      {
        _testRoot.Create();
      }
    }

    [SetUp]
    public void Setup()
    {
      _testRoot.Refresh();

      foreach(var file in _testRoot.EnumerateFiles())
      {
        File.Delete(file.FullName);
      }
      foreach(var dir in _testRoot.EnumerateDirectories())
      {
        Directory.Delete(dir.FullName, true);
      }

      _testRoot.Refresh();
    }

    #endregion

    #region tests
    
    [Test]
    public void TestIsChildOf()
    {
      FileInfo file = new FileInfo(@"C:\SomeDirectory\SomeFile.txt");
      
      Assert.IsTrue(file.IsChildOf(new DirectoryInfo(@"C:\SomeDirectory")), "Is child of C:\\SomeDirectory");
      Assert.IsFalse(file.IsChildOf(new DirectoryInfo(@"C:\OtherDirectory")), "Is not child of C:\\OtherDirectory");
    }
    
    [Test]
    public void TestGetRelative()
    {
      FileInfo file = new FileInfo(@"C:\SomeDirectory\SomeFile.txt");
      
      Assert.AreEqual(@"SomeDirectory\SomeFile.txt",
                      file.GetRelative(new DirectoryInfo(@"C:\")),
                      "Correct relative path");
    }
    
    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void TestGetRelativeNotRooted()
    {
      FileInfo file = new FileInfo(@"C:\SomeDirectory\SomeFile.txt");
      file.GetRelative(new DirectoryInfo(@"D:\"));
      Assert.Fail("Test should not reach this point");
    }
    
    [Test]
    public void TestGetParent()
    {
      DirectoryInfo currentDir = new DirectoryInfo(System.Environment.CurrentDirectory);
      FileInfo file = new FileInfo(currentDir.FullName + Path.DirectorySeparatorChar + "testFile.txt");
      
      Assert.AreEqual(currentDir,
                      file.GetParent(),
                      "File");
      Assert.AreEqual(currentDir.Parent,
                      currentDir.GetParent(),
                      "Directory");
      Assert.IsNull(currentDir.Root.GetParent(), "Filesystem root");
    }

    [Test]
    [TestCase("foo/bar/baz", '/')]
    [TestCase("foo", '/')]
    public void TestCreateRecursive(string relativePath, char separator)
    {
      // Arrange
      var pathParts = relativePath.Split(separator);
      /* Dismantle and reassemble the path, because we want to be OS independent;
       * not everyone uses forward-slashes to separate paths.
       */
      var reassembledRelativePath = Path.Combine(pathParts);
      var desiredPath = new DirectoryInfo(Path.Combine(_testRoot.FullName, reassembledRelativePath));

      // Act
      desiredPath.CreateRecursive();

      // Assert
      Assert.IsTrue(desiredPath.Exists, "Path has been created");
    }
    
    #endregion
  }
}

