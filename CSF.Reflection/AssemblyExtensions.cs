//
// AssemblyExtensions.cs
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
using System.IO;
using CSF.Reflection.Resources;


namespace CSF.Reflection
{
  /// <summary>
  /// Extension methods for the Assembly type.
  /// </summary>
  public static class AssemblyExtensions
  {
    #region embedded resources

    /// <summary>
    /// Extension method reads a text-based resource stored within an assembly.
    /// </summary>
    /// <returns>
    /// The manifest resource text.
    /// </returns>
    /// <param name='assembly'>
    /// The assembly
    /// </param>
    /// <param name='resourceName'>
    /// Resource name.
    /// </param>
    public static string GetManifestResourceText(this Assembly assembly, string resourceName)
    {
      string output;

      if(assembly == null)
      {
        throw new ArgumentNullException(nameof(assembly));
      }

      using(Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
      {
        if(resourceStream == null)
        {
          var message = String.Format(ExceptionMessages.ResourceNotPresent,
                                      resourceName,
                                      assembly.FullName);
          throw new ResourceNotFoundException(message);
        }

        output = GetResourceText(resourceStream);
      }

      return output;
    }

    /// <summary>
    /// Extension method reads a text-based resource stored within an assembly.
    /// </summary>
    /// <returns>
    /// The manifest resource text.
    /// </returns>
    /// <param name='assembly'>
    /// The assembly.
    /// </param>
    /// <param name='type'>
    /// A type by which to namespace the resource.
    /// </param>
    /// <param name='resourceName'>
    /// Resource name.
    /// </param>
    public static string GetManifestResourceText(this Assembly assembly, Type type, string resourceName)
    {
      string output;

      if(assembly == null)
      {
        throw new ArgumentNullException(nameof(assembly));
      }
      else if(type == null)
      {
        throw new ArgumentNullException(nameof(type));
      }

      using(Stream resourceStream = assembly.GetManifestResourceStream(type, resourceName))
      {
        if(resourceStream == null)
        {
          var message = String.Format(ExceptionMessages.ResourceNotPresent,
                                      String.Format("{0}.{1}", type.Namespace, resourceName),
                                      assembly.FullName);
          throw new ResourceNotFoundException(message);
        }

        output = GetResourceText(resourceStream);
      }

      return output;
    }

    /// <summary>
    /// Private helper method gets the textual content of a stream.
    /// </summary>
    /// <returns>
    /// The text content of the stream.
    /// </returns>
    /// <param name='resourceStream'>
    /// A resource stream.
    /// </param>
    private static string GetResourceText(Stream resourceStream)
    {
      string output;

      using(TextReader reader = new StreamReader(resourceStream))
      {
        output = reader.ReadToEnd();
      }

      return output;
    }

    #endregion
  }
}

