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
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
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
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
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

