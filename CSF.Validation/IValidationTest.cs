//
// IValidationTest.cs
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

namespace CSF.Validation
{
  /// <summary>
  /// Interface for a validation test; a single test that may be performed against an object, which returns either
  /// <c>true</c> or <c>false</c> (pass or fail).
  /// </summary>
  public interface IValidationTest<TTarget>
  {
    #region properties
    
    /// <summary>
    /// Gets the member associated with this test.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this test is associated with a specific member then this will be non-null.  If the test is unassociated and
    /// is performed on the overall object instance then this property will return <c>null</c>.
    /// </para>
    /// <para>
    /// If this property is non-null then the test will be performed upon the value of the member.
    /// </para>
    /// </remarks>
    /// <value>
    /// The member.
    /// </value>
    MemberInfo Member { get; }
    
    /// <summary>
    /// Gets an identifier that may be used to distinguish this test from others.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    object Identifier { get; }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Executes/performs this test against the specified target object instance.
    /// </summary>
    /// <param name='target'>
    /// The object instance to perform this test against.
    /// </param>
    bool Execute(TTarget target);
    
    #endregion
  }
}

