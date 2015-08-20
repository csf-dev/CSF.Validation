//
// Validator.cs
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
using System.Linq.Expressions;
using System.Reflection;
using CSF.Reflection;

namespace CSF.Validation
{
  /// <summary>
  /// Validator type that provides validation services for object instances against a registered set of validation
  /// tests.
  /// </summary>
  /// <typeparam name='TTarget'>
  /// The type of object that this validator targets and provides validation for.
  /// </typeparam>
  public class Validator<TTarget> : IValidator<TTarget>
  {
    #region fields
    
    private IList<IValidationTest<TTarget>> _tests;
    private bool _throwOnFailure;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets or sets a collection of the validation tests defined for the current instance.
    /// </summary>
    /// <value>
    /// A collection of the validation rules.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public IList<IValidationTest<TTarget>> Tests
    {
      get {
        return _tests;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _tests = value;
      }
    }
    
    #endregion
    
    #region test registration
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test will validate an object in general terms, without
    /// an association to a particular member of the instance.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    public IValidator<TTarget> AddTest(ValidationFunction<TTarget> test)
    {
      return this.AddTest(test, null);
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test will validate an object in general terms, without
    /// an association to a particular member of the instance.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <param name='testIdentifier'>
    /// An identifier for the test, that allows it to be distinguished from other tests.
    /// </param>
    public IValidator<TTarget> AddTest(ValidationFunction<TTarget> test,
                                       object testIdentifier)
    {
      this.Tests.Add(new ValidationTest<TTarget>(test, testIdentifier));
      return this;
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test is associated with a specific member of the
    /// target object being validated and will be performed against the value of that member.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='member'>
    /// The member that this test is to be associated with.
    /// </param>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <typeparam name='TMember'>
    /// The output/return type of the <paramref name="member"/> that this test is associated with.
    /// </typeparam>
    public IValidator<TTarget> AddTest<TMember>(Expression<Func<TTarget, TMember>> member,
                                                ValidationFunction<TMember> test)
    {
      return this.AddTest<TMember>(member, test, null);
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test is associated with a specific member of the
    /// target object being validated and will be performed against the value of that member.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='member'>
    /// The member that this test is to be associated with.
    /// </param>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <typeparam name='TMember'>
    /// The output/return type of the <paramref name="member"/> that this test is associated with.
    /// </typeparam>
    public IValidator<TTarget> AddTest<TMember>(MemberInfo member,
                                                ValidationFunction<TMember> test)
    {
      return this.AddTest<TMember>(member, test, null);
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test is associated with a specific member of the
    /// target object being validated and will be performed against the value of that member.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='member'>
    /// The member that this test is to be associated with.
    /// </param>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <param name='testIdentifier'>
    /// An identifier for the test, that allows it to be distinguished from other tests.
    /// </param>
    /// <typeparam name='TMember'>
    /// The output/return type of the <paramref name="member"/> that this test is associated with.
    /// </typeparam>
    public IValidator<TTarget> AddTest<TMember>(Expression<Func<TTarget, TMember>> member,
                                                ValidationFunction<TMember> test,
                                                object testIdentifier)
    {
      return this.AddTest<TMember>(Reflect.Member<TTarget,TMember>(member), test, testIdentifier);
    }
    
    /// <summary>
    /// Adds a new validation test to the current instance.  The test is associated with a specific member of the
    /// target object being validated and will be performed against the value of that member.
    /// </summary>
    /// <returns>
    /// The current instance, permitting method-chaining (such as adding many tests together).
    /// </returns>
    /// <param name='member'>
    /// The member that this test is to be associated with.
    /// </param>
    /// <param name='test'>
    /// The test to add.
    /// </param>
    /// <param name='testIdentifier'>
    /// An identifier for the test, that allows it to be distinguished from other tests.
    /// </param>
    /// <typeparam name='TMember'>
    /// The output/return type of the <paramref name="member"/> that this test is associated with.
    /// </typeparam>
    public IValidator<TTarget> AddTest<TMember>(MemberInfo member,
                                                ValidationFunction<TMember> test,
                                                object testIdentifier)
    {
      this.Tests.Add(new ValidationTest<TTarget, TMember>(test, member, testIdentifier));
      return this;
    }

    /// <summary>
    /// Configures this validator instance such that it will throw an exception on a validation failure, instead of
    /// returning false.
    /// </summary>
    /// <returns>
    /// A reference to the validator instance.
    /// </returns>
    public IValidator<TTarget> ThrowOnFailure()
    {
      return this.ThrowOnFailure(true);
    }

    /// <summary>
    /// Configures this validator instance, determining whether or not it will throw an exception on a validation
    /// failure, instead of simply returning false.
    /// </summary>
    /// <returns>
    /// A reference to the validator instance.
    /// </returns>
    /// <param name='throwOnFailure'>
    /// A value that indicates whether the 'throw on failure' functionality should be enabled or disabled.
    /// </param>
    public IValidator<TTarget> ThrowOnFailure(bool throwOnFailure)
    {
      _throwOnFailure = throwOnFailure;
      return this;
    }
    
    #endregion
    
    #region validation
    
    /// <summary>
    /// Validates the specified object instance.
    /// </summary>
    /// <returns>
    /// A value that indicates whether validation was successful or not.
    /// </returns>
    /// <param name='target'>
    /// The target object instance to validate.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="target"/> is null.
    /// </exception>
    public bool Validate (TTarget target)
    {
      ValidationTestList<TTarget> failures;
      bool output = this.Validate(target, out failures);
      
      if(_throwOnFailure && !output)
      {
        throw new ValidationFailureException<TTarget>(failures);
      }
      
      return output;
    }
  
    /// <summary>
    /// Validates the specified object instance.
    /// </summary>
    /// <returns>
    /// A value that indicates whether validation was successful or not.
    /// </returns>
    /// <param name='target'>
    /// The target object instance to validate.
    /// </param>
    /// <param name='failures'>
    /// Exposes a collection of the validation failures.  If validation is a success this collection will be empty.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="target"/> is null.
    /// </exception>
    public bool Validate (TTarget target, out ValidationTestList<TTarget> failures)
    {
      if(target == null)
      {
        throw new ArgumentNullException ("target");
      }
      
      failures = new ValidationTestList<TTarget>();
      
      foreach(IValidationTest<TTarget> test in this.Tests)
      {
        try
        {
          bool testPassed = test.Execute(target);

          if(!testPassed)
          {
            failures.Add(test);
          }
        }
        catch(Exception ex)
        {
          failures = new ValidationTestList<TTarget>();
          failures.Add(test);
          throw new ValidationFailureException<TTarget>(failures, ex);
        }
      }
      
      return (failures.Count == 0);
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new validator instance.
    /// </summary>
    public Validator()
    {
      _throwOnFailure = false;
      this.Tests = new List<IValidationTest<TTarget>>();
    }
    
    #endregion
  }
}

