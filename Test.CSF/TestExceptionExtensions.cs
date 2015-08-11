using System;
using NUnit.Framework;
using CSF;
using System.Reflection;
using System.Runtime.Serialization;
using CSF.Reflection;

namespace Test.CSF
{
  [TestFixture]
  public class TestExceptionExtensions
  {
    #region tests

    [Test]
    [ExpectedException(typeof(TargetInvocationException))]
    public void TestFixStackTraceUsingSerialization()
    {
      try
      {
        InvalidOperationException fixedException = null;
        try
        {
          throw new InvalidOperationException("Oh nose!");
        }
        catch(InvalidOperationException ex)
        {
          fixedException = ExceptionExtensions.FixStackTraceUsingSerialization(ex);;
        }

        if(fixedException != null)
        {
          throw new TargetInvocationException(fixedException);
        }
      }
      catch(TargetInvocationException ex)
      {
        Assert.IsInstanceOf<InvalidOperationException>(ex.InnerException, "Inner exception");
        throw;
      }
    }

    [Test]
    [ExpectedException(typeof(CannotFixStackTraceException))]
    public void TestFixStackTraceUsingSerializationCustomException()
    {
      try
      {
        CustomException fixedException = null;
        try
        {
          throw new CustomException();
        }
        catch(CustomException ex)
        {
          fixedException = ExceptionExtensions.FixStackTraceUsingSerialization(ex);;
        }

        if(fixedException != null)
        {
          throw new TargetInvocationException(fixedException);
        }
      }
      catch(TargetInvocationException ex)
      {
        Assert.IsInstanceOf<CustomException>(ex.InnerException, "Inner exception");
        throw;
      }
    }

    [Test]
    [ExpectedException(typeof(TargetInvocationException))]
    public void TestFixStackTrace()
    {
      try
      {
        InvalidOperationException fixedException = null;
        try
        {
          throw new InvalidOperationException("Oh nose!");
        }
        catch(InvalidOperationException ex)
        {
          fixedException = ex.FixStackTrace();
        }

        if(fixedException != null)
        {
          throw new TargetInvocationException(fixedException);
        }
      }
      catch(TargetInvocationException ex)
      {
        Assert.IsInstanceOf<InvalidOperationException>(ex.InnerException, "Inner exception");
        throw;
      }
    }

    [Test]
    [ExpectedException(typeof(TargetInvocationException))]
    public void TestFixStackTraceCustomExceptionWithConstructor()
    {
      try
      {
        CustomSerializableException fixedException = null;
        try
        {
          throw new CustomSerializableException();
        }
        catch(CustomSerializableException ex)
        {
          fixedException = ex.FixStackTrace();
        }

        if(fixedException != null)
        {
          throw new TargetInvocationException(fixedException);
        }
      }
      catch(TargetInvocationException ex)
      {
        Assert.IsInstanceOf<CustomSerializableException>(ex.InnerException, "Inner exception");
        throw;
      }
    }

    [Test]
    [ExpectedException(typeof(TargetInvocationException))]
    public void TestTryFixStackTrace()
    {
      bool success = false;

      try
      {
        CustomSerializableException fixedException = null;
        try
        {
          throw new CustomSerializableException();
        }
        catch(CustomSerializableException ex)
        {
          success = ex.TryFixStackTrace(out fixedException);
        }

        if(fixedException != null)
        {
          throw new TargetInvocationException(fixedException);
        }
      }
      catch(TargetInvocationException ex)
      {
        Assert.IsInstanceOf<CustomSerializableException>(ex.InnerException, "Inner exception");
        Assert.IsTrue(success, "Success of fix");
        throw;
      }
    }

    [Test]
    [ExpectedException(typeof(TargetInvocationException))]
    public void TestTryFixStackTraceFailure()
    {
      if(!Reflect.IsMono())
      {
        Assert.Ignore("This test is not valid when not running on the open source Mono framework.  When executing " +
                      "against the official .NET framework, the 'private framework method' implementation for fixing " +
                      "stack traces will work and thus 'TryFixStackTrace' will not fail as intended.");
      }

      bool success = false;

      try
      {
        CustomException fixedException = null;
        try
        {
          throw new CustomException();
        }
        catch(CustomException ex)
        {
          success = ex.TryFixStackTrace(out fixedException);
        }

        if(fixedException != null)
        {
          throw new TargetInvocationException(fixedException);
        }
      }
      catch(TargetInvocationException ex)
      {
        Assert.IsInstanceOf<CustomException>(ex.InnerException, "Inner exception");
        Assert.IsFalse(success, "Success of fix");
        throw;
      }
    }

    #endregion

    #region contained custom exception

    private class CustomException : Exception {}

    private class CustomSerializableException : Exception
    {
      public CustomSerializableException() {}

      public CustomSerializableException(SerializationInfo ser, StreamingContext con) : base(ser, con) {}
    }

    #endregion
  }
}

