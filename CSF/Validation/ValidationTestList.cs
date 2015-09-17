//
// ValidationTestList.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CSF.Reflection;

namespace CSF.Validation
{
  /// <summary>
  /// Represents a collection of validation failures.
  /// </summary>
  public class ValidationTestList<TTarget> : List<IValidationTest<TTarget>>
  {
    #region methods
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) are for the specified <paramref name="member"/>.
    /// </summary>
    /// <returns>
    /// A collection of validation failures, possibly empty.
    /// </returns>
    /// <param name='member'>
    /// The member that we are interested in.
    /// </param>
    public ValidationTestList<TTarget> ByMember(Expression<Func<TTarget, object>> member)
    {
      return this.ByMember(Reflect.Member<TTarget>(member));
    }
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) are for the specified <paramref name="member"/>.
    /// </summary>
    /// <returns>
    /// A collection of validation failures, possibly empty.
    /// </returns>
    /// <param name='member'>
    /// The member that we are interested in.
    /// </param>
    public ValidationTestList<TTarget> ByMember(MemberInfo member)
    {
      return this.ByIdentifier(null, member);
    }
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) have the specified <paramref name="identifier"/>.
    /// </summary>
    /// <returns>
    /// The identifier.
    /// </returns>
    /// <param name='identifier'>
    /// The identifier value to search for.
    /// </param>
    public ValidationTestList<TTarget> ByIdentifier(object identifier)
    {
      return this.ByIdentifier(identifier, (MemberInfo) null);
    }
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) have the specified
    /// <paramref name="identifier"/> and are for the specified <paramref name="member"/>.
    /// </summary>
    /// <returns>
    /// A collection of validation failures, possibly empty.
    /// </returns>
    /// <param name='identifier'>
    /// The identifier value to search for.
    /// </param>
    /// <param name='member'>
    /// The member that we are interested in.
    /// </param>
    public ValidationTestList<TTarget> ByIdentifier(object identifier, Expression<Func<TTarget, object>> member)
    {
      return this.ByIdentifier(identifier, Reflect.Member<TTarget>(member));
    }
    
    /// <summary>
    /// Gets a subset of the current collection where the failed test(s) have the specified
    /// <paramref name="identifier"/> and are for the specified <paramref name="member"/>.
    /// </summary>
    /// <returns>
    /// A collection of validation failures, possibly empty.
    /// </returns>
    /// <param name='identifier'>
    /// The identifier value to search for.
    /// </param>
    /// <param name='member'>
    /// The member that we are interested in.
    /// </param>
    public ValidationTestList<TTarget> ByIdentifier(object identifier, MemberInfo member)
    {
      ValidationTestList<TTarget> output = new ValidationTestList<TTarget>();
      
      if(identifier == null && member == null)
      {
        throw new InvalidOperationException("Both identifier and member cannot be null.");
      }
      
      var subset = (from x in this
                    where
                      (identifier == null || x.Identifier == identifier)
                      && (member == null || x.Member == member)
                    select x);
      output.AddRange(subset);
      
      return output;
    }
    
    #endregion
  }
}

