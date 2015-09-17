//
// VersionExtensions.cs
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

namespace CSF
{
  /// <summary>
  /// Container for extension methods to the <see cref="System.Version"/> type.
  /// </summary>
  public static class VersionExtensions
  {
    /// <summary>
    /// Returns the version number formatted as a "semantic versioning" tag-name.
    /// </summary>
    /// <remarks>
    /// See <c>http://semver.org/</c> for more information about the semantic versioning specification.
    /// </remarks>
    /// <returns>
    /// The semantic version number.
    /// </returns>
    /// <param name='version'>
    /// A <see cref="System.Version"/> to operate upon.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public static string ToSemanticVersion(this Version version)
    {
      if(version == null)
      {
        throw new ArgumentNullException ("version");
      }
      
      return String.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
    }
  }
}

