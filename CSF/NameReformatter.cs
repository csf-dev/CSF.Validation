//
// NameReformatter.cs
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
  /// Naming formatter type, reformats a name using the naming convention rules defined by the implementation type.
  /// </summary>
  public abstract class NameReformatter
  {
    #region methods

    /// <summary>
    /// Formats the specified name using the rules defined by the current implementation.
    /// </summary>
    /// <param name='objectName'>
    /// The name to re-format.
    /// </param>
    public abstract string Format(string objectName);

    /// <summary>
    /// Concatenates and formats a number of object names, using the rules defined by the current implementation.
    /// </summary>
    /// <param name='objectNames'>
    /// A collection of the object names to re-format.
    /// </param>
    public virtual string Format(params string[] objectNames)
    {
      return this.Format(String.Concat(objectNames));
    }

    #endregion
  }
}

