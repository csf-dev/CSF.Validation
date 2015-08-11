//
//  ExceptionExtensions.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2013 Craig Fowler
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace CSF
{
  /// <summary>
  /// Extension methods relating to the <c>System.Exception</c> type and subclasses.
  /// </summary>
  public static class ExceptionExtensions
  {
    #region constants

    private const string 
      InternalPreserveMethod  = "InternalPreserveStackTrace",
      PrepForRemotingMethod   = "PrepForRemoting";

    #endregion

    #region static fields

    private static readonly Action<Exception> InternalPreserveStackTrace;
    private static readonly Action<Exception> PrepForRemoting;

    #endregion

    #region extension methods

    /// <summary>
    /// Makes a number of attempts to fix an exception's stack trace.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method uses a priority-ordered set of attempts to fix the stack trace upon the given exception.  If any
    /// method fails and returns a null result then the next is attempted.
    /// </para>
    /// <list type="number">
    /// <item>An attempt is made using <see cref="M:FixStackTraceUsingInternalPreserve{TException}"/></item>
    /// <item>An attempt is made using <see cref="M:FixStackTraceUsingPrepForRemoting{TException}"/></item>
    /// <item>An attempt is made using <see cref="M:FixStackTraceUsingSerialization{TException}"/></item>
    /// </list>
    /// <para>
    /// If all of these methods fail to return a result then an exception of type
    /// <see cref="CannotFixStackTraceException"/> is raised.  In this scenario it is likely that the framework
    /// does not support either of the 'unofficial' mechanisms tried (perhaps we are using Mono and not the official
    /// .NET framework).  The two methods that these mechanisms use are unsupported internal functionality of the
    /// framework and could vanish at any time.
    /// </para>
    /// <para>
    /// The last-attempted mechanism should work on just about any exception, as long as it posesses a deserialization
    /// constructor.  That is, a constructor of the signature
    /// <c>(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)</c>.  If an
    /// exception possesses such a constructor then this mechanism will work in order to preserve the exception's stack
    /// trace.
    /// </para>
    /// </remarks>
    /// <returns>
    /// An exception of the same type as was specified as a parameter.  This exception has its stack trace 'fixed' and
    /// thus preserved.
    /// </returns>
    /// <param name='ex'>
    /// The exception for which to preserve the stack trace.
    /// </param>
    /// <typeparam name='TException'>
    /// The type of exception.
    /// </typeparam>
    /// <exception cref="CannotFixStackTraceException">
    /// If none of the mechanisms attempted are able to preserve the stack trace, this exception is raised.
    /// </exception>
    public static TException FixStackTrace<TException>(this TException ex)
      where TException : Exception
    {
      TException output;

      if(ex == null)
      {
        throw new ArgumentNullException("ex");
      }

      output = FixStackTraceUsingInternalPreserve(ex);

      if(output == null)
      {
        output = FixStackTraceUsingPrepForRemoting(ex);
      }

      if(output == null)
      {
        output = FixStackTraceUsingSerialization(ex);
      }

      if(output == null)
      {
        throw new CannotFixStackTraceException(ex);
      }

      return output;
    }

    /// <summary>
    /// Makes a number of attempts to fix an exception's stack trace, this method does not throw an exception if all
    /// attempts fail.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In almost all scenarios this method should not be used, instead favouring the method
    /// <c>FixStackTrace&lt;TException&gt;</c>.  This method exists as a last-ditch way of making a best-efforts attempt
    /// to fix an exception's stack trace when we cannot be sure that it will be possible to do so.  For example, where
    /// the developer does not control the exception type to be 'fixed' (such as if it is from a third party library) or
    /// where it is impossible for some reason to add a deserialisation constructor to the exception type.
    /// </para>
    /// <para>
    /// This method uses a priority-ordered set of attempts to fix the stack trace upon the given exception.  If any
    /// method fails and returns a null result then the next is attempted.
    /// </para>
    /// <list type="number">
    /// <item>An attempt is made using <see cref="M:FixStackTraceUsingInternalPreserve{TException}"/></item>
    /// <item>An attempt is made using <see cref="M:FixStackTraceUsingPrepForRemoting{TException}"/></item>
    /// <item>An attempt is made using <see cref="M:FixStackTraceUsingSerialization{TException}"/></item>
    /// </list>
    /// <para>
    /// If all of these methods fail to return a result then this method will return false and the
    /// <paramref name="fixedException"/> parameter wille expose the original, unmodified exception.  In this scenario
    /// it is likely that the framework does not support either of the 'unofficial' mechanisms tried (perhaps we are
    /// using Mono and not the official .NET framework).  The two methods that these mechanisms use are unsupported
    /// internal functionality of the framework and could vanish at any time.
    /// </para>
    /// <para>
    /// The last-attempted mechanism should work on just about any exception, as long as it posesses a deserialization
    /// constructor.  That is, a constructor of the signature
    /// <c>(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)</c>.  If an
    /// exception possesses such a constructor then this mechanism will work in order to preserve the exception's stack
    /// trace.
    /// </para>
    /// </remarks>
    /// <returns>
    /// <c>true</c> if this method successfully fixed the exception's stack trace; <c>false</c> otherwise.
    /// </returns>
    /// <param name='ex'>
    /// The exception for which to preserve the stack trace.
    /// </param>
    /// <param name='fixedException'>
    /// If this method returns <c>true</c>, indicating success, then this parameter exposes the fixed exception
    /// instance.  If this method returns <c>false</c> then this parameter exposes the original, unmodified exception
    /// instance.
    /// </param>
    /// <typeparam name='TException'>
    /// The type of exception.
    /// </typeparam>
    public static bool TryFixStackTrace<TException>(this TException ex, out TException fixedException)
      where TException : Exception
    {
      bool output;

      try
      {
        fixedException = ex.FixStackTrace();
        output = true;
      }
      catch(CannotFixStackTraceException)
      {
        fixedException = ex;
        output = false;
      }

      return output;
    }

    #endregion

    #region static methods

    /// <summary>
    /// Attempts to fix an exception's stack trace using the <c>InternalPreserveStackTrace</c> method.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should never be called directly, because it is unsafe and makes use of unofficial/unsupported
    /// internal framework methods.  This means that these methods may not exist in the framework that the application
    /// is currently running on top of.  This unsupported internal method may even be removed from the official .NET
    /// framework someday.
    /// </para>
    /// <para>
    /// This method should only ever be used as part of <see cref="M:FixStackTrace{TException}"/>, which makes a number
    /// of priority-ordered attempts to fix the stack trace, sticking with the one that works.
    /// </para>
    /// </remarks>
    /// <returns>
    /// If this mechanism of fixing an exception's stack trace worked, then the exception is returned.  If this method
    /// was unable to fix the stack trace then a null reference is returned.
    /// </returns>
    /// <param name='ex'>
    /// The exception for which to preserve the stack trace.
    /// </param>
    /// <typeparam name='TException'>
    /// The type of exception.
    /// </typeparam>
    public static TException FixStackTraceUsingInternalPreserve<TException>(TException ex)
      where TException : Exception
    {
      if(ex == null)
      {
        throw new ArgumentNullException("ex");
      }

      TException output = null;

      if(InternalPreserveStackTrace != null)
      {
        InternalPreserveStackTrace(ex);
        output = ex;
      }

      return output;
    }

    /// <summary>
    /// Attempts to fix an exception's stack trace using the <c>PrepForRemoting</c> method.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should never be called directly, because it is unsafe and makes use of unofficial/unsupported
    /// internal framework methods.  This means that these methods may not exist in the framework that the application
    /// is currently running on top of.  This unsupported internal method may even be removed from the official .NET
    /// framework someday.
    /// </para>
    /// <para>
    /// This method should only ever be used as part of <see cref="M:FixStackTrace{TException}"/>, which makes a number
    /// of priority-ordered attempts to fix the stack trace, sticking with the one that works.
    /// </para>
    /// </remarks>
    /// <returns>
    /// If this mechanism of fixing an exception's stack trace worked, then the exception is returned.  If this method
    /// was unable to fix the stack trace then a null reference is returned.
    /// </returns>
    /// <param name='ex'>
    /// The exception for which to preserve the stack trace.
    /// </param>
    /// <typeparam name='TException'>
    /// The type of exception.
    /// </typeparam>
    public static TException FixStackTraceUsingPrepForRemoting<TException>(TException ex)
      where TException : Exception
    {
      if(ex == null)
      {
        throw new ArgumentNullException("ex");
      }

      TException output = null;

      if(PrepForRemoting != null)
      {
        PrepForRemoting(ex);
        output = ex;
      }

      return output;
    }

    /// <summary>
    /// Attempts to fix an exception's stack trace using the 'serialization' method.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should never be called directly, because it will not always succeed - especially with custom
    /// exception types that do not provide a deserialization constructor.  That deserialization constructor is a
    /// public constructor of signature:
    /// <c>(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)</c>.
    /// </para>
    /// <para>
    /// The advantage of this method is that it relies only on publicly-available functionality and not
    /// unofficial/unsupported internal framework methods.  The downside being of course that it does not work for every
    /// exception type.  Of course - if the developer controls the exception type being 'fixed' then they could always
    /// add a deserialization constructor.
    /// </para>
    /// <para>
    /// This method should only ever be used as part of <see cref="M:FixStackTrace{TException}"/>, which makes a number
    /// of priority-ordered attempts to fix the stack trace, sticking with the one that works.
    /// </para>
    /// <para>
    /// Credit where it's due; this method almost direct copy-paste of the following StackOverflow answer:
    /// http://stackoverflow.com/a/2085377
    /// </para>
    /// </remarks>
    /// <returns>
    /// If this mechanism of fixing an exception's stack trace worked, then the exception is returned.  If this method
    /// was unable to fix the stack trace then a null reference is returned.
    /// </returns>
    /// <param name='ex'>
    /// The exception for which to preserve the stack trace.
    /// </param>
    /// <typeparam name='TException'>
    /// The type of exception.
    /// </typeparam>
    public static TException FixStackTraceUsingSerialization<TException>(TException ex)
      where TException : Exception
    {
      if(ex == null)
      {
        throw new ArgumentNullException("ex");
      }

      var ctx = new StreamingContext(StreamingContextStates.CrossAppDomain);
      var mgr = new ObjectManager(null, ctx);
      var si  = new SerializationInfo(ex.GetType(), new FormatterConverter());

      ex.GetObjectData(si, ctx);
      mgr.RegisterObject(ex, 1, si);

      try
      {
        mgr.DoFixups();
      }
      catch(SerializationException)
      {
        throw new CannotFixStackTraceException(ex);
      }

      return ex;
    }

    #endregion

    #region private methods

    /// <summary>
    /// Creates a cached delegate that represents the internal <c>Exception.InternalPreserveStackTrace</c> method.
    /// </summary>
    /// <returns>
    /// A delegate instance, or a null reference if the method does not exist.
    /// </returns>
    private static Action<Exception> GetInternalPreserveStackTrace()
    {
      Action<Exception> output;
      MethodInfo method = typeof(Exception).GetMethod(InternalPreserveMethod,
                                                      BindingFlags.Instance | BindingFlags.NonPublic);

      if(method != null)
      {
        output = (Action<Exception>) Delegate.CreateDelegate(typeof(Action<Exception>), method);
      }
      else
      {
        output = null;
      }

      return output;
    }

    /// <summary>
    /// Creates a cached delegate that represents the internal <c>Exception.PrepForRemoting</c> method.
    /// </summary>
    /// <returns>
    /// A delegate instance, or a null reference if the method does not exist.
    /// </returns>
    private static Action<Exception> GetPrepForRemoting()
    {
      Action<Exception> output;
      MethodInfo method = typeof(Exception).GetMethod(PrepForRemotingMethod,
                                                      BindingFlags.Instance | BindingFlags.NonPublic);

      if(method != null)
      {
        output = (Action<Exception>) Delegate.CreateDelegate(typeof(Action<Exception>), method);
      }
      else
      {
        output = null;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes the <see cref="CSF.ExceptionExtensions"/> type, initialising cached delegates for the internal
    /// methods used by this type.
    /// </summary>
    static ExceptionExtensions()
    {
      InternalPreserveStackTrace = GetInternalPreserveStackTrace();
      PrepForRemoting = GetPrepForRemoting();
    }

    #endregion
  }
}

