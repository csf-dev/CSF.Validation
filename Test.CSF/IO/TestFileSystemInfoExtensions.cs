using System;
using NUnit.Framework;
using CSF.IO;
using System.IO;

namespace Test.CSF.IO
{
  [TestFixture]
  public class TestFileSystemInfoExtensions
  {
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
    
    #endregion
  }
}

