//
// ValidationTest`2.cs
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
  /// Immutable specification type for a validation test, to be used with an <c>IValidator&lt;TTarget&gt;</c>
  /// </summary>
  public class ValidationTest<TTarget, TValue> : ValidationTest<TTarget>, IValidationTest<TTarget>
  {
    #region fields
    
    private ValidationFunction<TValue> _test;
    
    #endregion
    
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
    /// If this property is non-null then the <see cref="Test"/> function will be performed upon the value of the
    /// member.
    /// </para>
    /// </remarks>
    /// <value>
    /// The member.
    /// </value>
    public override MemberInfo Member
    {
      get {
        return base.Member;
      }
      protected set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        base.Member = value;
      }
    }
    
    /// <summary>
    /// Gets the test function that this instance performs.
    /// </summary>
    /// <value>
    /// The test function.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public new ValidationFunction<TValue> Test
    {
      get {
        return _test;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _test = value;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Executes/performs this test against the specified target object instance.
    /// </summary>
    /// <param name='target'>
    /// The object instance to perform this test against.
    /// </param>
    public override bool Execute(TTarget target)
    {
      TValue testValue;
      
      if(target == null)
      {
        throw new ArgumentNullException ("target");
      }
      
      PropertyInfo property = this.Member as PropertyInfo;
      FieldInfo field = this.Member as FieldInfo;
      
      if(property != null)
      {
        testValue = (TValue) property.GetValue(target, null);
      }
      else if(field != null)
      {
        testValue = (TValue) field.GetValue(target);
      }
      else
      {
        throw new InvalidOperationException("Member must be either a PropertyInfo or a FieldInfo.");
      }
      
      return this.Test(testValue);
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new validation test instance.
    /// </summary>
    /// <param name='test'>
    /// The test function that this instance performs.
    /// </param>
    /// <param name='member'>
    /// The member that this test is associated with.
    /// </param>
    /// <param name='identifier'>
    /// The identifier for this test, to distinguish it from other tests.
    /// </param>
    public ValidationTest(ValidationFunction<TValue> test,
                          MemberInfo member,
                          object identifier) : base(identifier, member)
    {
      this.Test = test;
    }
    
    #endregion
  }
}

