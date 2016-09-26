//
// Reflect.cs
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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CSF.Reflection.Resources;

namespace CSF.Reflection
{
  /// <summary>
  /// Helper class for reflection-related tasks.
  /// </summary>
  public static class Reflect
  {
    #region constants

    private const string MONO_TYPE = "Mono.Runtime";

    #endregion

    #region static member reflection
    
    /// <summary>
    /// Gets a <see cref="MemberInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The member information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MemberInfo Member<TObject>(Expression<Func<TObject, object>> expression)
    {
      return Member(expression.Body);
    }
    
    /// <summary>
    /// Gets a <see cref="MemberInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The member information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <typeparam name='TReturn'>
    /// The return/output type of the member.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MemberInfo Member<TObject,TReturn>(Expression<Func<TObject,TReturn>> expression)
    {
      return Member(expression.Body);
    }
      
    /// <summary>
    /// Gets a <see cref="MemberInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The member information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MemberInfo Member<TObject>(Expression<Action<TObject>> expression)
    {
      return Member(expression.Body);
    }
    
    /// <summary>
    /// Gets a <see cref="PropertyInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The property information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static PropertyInfo Property<TObject>(Expression<Func<TObject, object>> expression)
    {
      return Member<TObject>(expression) as PropertyInfo;
    }
    
    /// <summary>
    /// Gets a <see cref="PropertyInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The property information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static PropertyInfo Property<TObject,TReturn>(Expression<Func<TObject,TReturn>> expression)
    {
      return Member<TObject,TReturn>(expression) as PropertyInfo;
    }

    /// <summary>
    /// Gets a <see cref="FieldInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The field information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static FieldInfo Field<TObject>(Expression<Func<TObject, object>> expression)
    {
      return Member<TObject>(expression) as FieldInfo;
    }

    /// <summary>
    /// Gets a <see cref="FieldInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The field information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static FieldInfo Field<TObject,TReturn>(Expression<Func<TObject,TReturn>> expression)
    {
      return Member<TObject,TReturn>(expression) as FieldInfo;
    }
    
    /// <summary>
    /// Gets a <see cref="MethodInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The method information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MethodInfo Method<TObject>(Expression<Func<TObject, object>> expression)
    {
      return Member<TObject>(expression) as MethodInfo;
    }
    
    /// <summary>
    /// Gets a <see cref="MethodInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The method information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MethodInfo Method<TObject,TReturn>(Expression<Func<TObject,TReturn>> expression)
    {
      return Member<TObject,TReturn>(expression) as MethodInfo;
    }
    
    /// <summary>
    /// Gets a <see cref="MethodInfo"/> from an expression that indicates a member of a specified type.
    /// </summary>
    /// <returns>
    /// The method information.
    /// </returns>
    /// <param name='expression'>
    /// The lambda expression that indicates a type, such as <c>x => x.MyProperty</c>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type that contains the member which we are interested in.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public static MethodInfo Method<TObject>(Expression<Action<TObject>> expression)
    {
      return Member<TObject>(expression) as MethodInfo;
    }

    #endregion

    #region static reflection extension methods

    /// <summary>
    /// Gets information about a member from the current instance.
    /// </summary>
    /// <returns>
    /// Member information.
    /// </returns>
    /// <param name='type'>
    /// The type from which to get the member information.
    /// </param>
    /// <param name='expression'>
    /// A linq expression that identifies a member from the given <paramref name="type"/>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type from which to get the member.
    /// </typeparam>
    public static MemberInfo GetMember<TObject>(this TObject type, Expression<Func<TObject,object>> expression)
    {
      return Member<TObject>(expression);
    }

    /// <summary>
    /// Gets information about a member from the current instance.
    /// </summary>
    /// <returns>
    /// Member information.
    /// </returns>
    /// <param name='type'>
    /// The type from which to get the member information.
    /// </param>
    /// <param name='expression'>
    /// A linq expression that identifies a member from the given <paramref name="type"/>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type from which to get the member.
    /// </typeparam>
    public static MemberInfo GetMember<TObject>(this TObject type, Expression<Action<TObject>> expression)
    {
      return Member<TObject>(expression);
    }

    /// <summary>
    /// Gets information about a property from the current instance.
    /// </summary>
    /// <returns>
    /// Member information.
    /// </returns>
    /// <param name='type'>
    /// The type from which to get the property information.
    /// </param>
    /// <param name='expression'>
    /// A linq expression that identifies a property from the given <paramref name="type"/>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type from which to get the property.
    /// </typeparam>
    public static PropertyInfo GetProperty<TObject>(this TObject type, Expression<Func<TObject,object>> expression)
    {
      return Property<TObject>(expression);
    }

    /// <summary>
    /// Gets information about a field from the current instance.
    /// </summary>
    /// <returns>
    /// Member information.
    /// </returns>
    /// <param name='type'>
    /// The type from which to get the field information.
    /// </param>
    /// <param name='expression'>
    /// A linq expression that identifies a field from the given <paramref name="type"/>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type from which to get the field.
    /// </typeparam>
    public static FieldInfo GetField<TObject>(this TObject type, Expression<Func<TObject,object>> expression)
    {
      return Field<TObject>(expression);
    }

    /// <summary>
    /// Gets information about a function-method from the current instance.
    /// </summary>
    /// <returns>
    /// Member information.
    /// </returns>
    /// <param name='type'>
    /// The type from which to get the function-method information.
    /// </param>
    /// <param name='expression'>
    /// A linq expression that identifies a function-method from the given <paramref name="type"/>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type from which to get the function-method.
    /// </typeparam>
    public static MethodInfo GetMethod<TObject>(this TObject type, Expression<Func<TObject,object>> expression)
    {
      return Method<TObject>(expression);
    }

    /// <summary>
    /// Gets information about a action-method from the current instance.
    /// </summary>
    /// <returns>
    /// Member information.
    /// </returns>
    /// <param name='type'>
    /// The type from which to get the action-method information.
    /// </param>
    /// <param name='expression'>
    /// A linq expression that identifies an action-method from the given <paramref name="type"/>.
    /// </param>
    /// <typeparam name='TObject'>
    /// The type from which to get the action-method.
    /// </typeparam>
    public static MethodInfo GetMethod<TObject>(this TObject type, Expression<Action<TObject>> expression)
    {
      return Method<TObject>(expression);
    }

    #endregion

    #region other reflection-related functionality

    /// <summary>
    /// Similar to <c>Type.GetType</c> but searches every assembly within an <see cref="AppDomain"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Type"/>.
    /// </returns>
    /// <param name='typeName'>
    /// The full name of the type to find and return, does not need to be assembly-qualified.
    /// </param>
    public static Type TypeFromAppDomain(string typeName)
    {
      return TypeFromAppDomain(typeName, AppDomain.CurrentDomain);
    }

    /// <summary>
    /// Similar to <c>Type.GetType</c> but searches every assembly within an <see cref="AppDomain"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Type"/>.
    /// </returns>
    /// <param name='domain'>
    /// The AppDomain to search.
    /// </param>
    /// <param name='typeName'>
    /// The full name of the type to find and return, does not need to be assembly-qualified.
    /// </param>
    public static Type TypeFromAppDomain(string typeName, AppDomain domain)
    {
      if(domain == null)
      {
        throw new ArgumentNullException(nameof(domain));
      }
      if(typeName == null)
      {
        throw new ArgumentNullException(nameof(typeName));
      }
      if(typeName.Length == 0)
      {
        throw new ArgumentException(ExceptionMessages.TypeNameMustNotBeEmpty, nameof(typeName));
      }

      return domain.GetAssemblies()
        .Where(x => !x.IsDynamic)
        .SelectMany(s => s.GetExportedTypes())
        .Where(x => x.FullName == typeName)
        .SingleOrDefault();
    }

    /// <summary>
    /// Determines whether the application is executing using the Mono framework.  This uses the supported manner of
    /// detecting mono.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the application is executing on the mono framework; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsMono()
    {
      return (Type.GetType(MONO_TYPE) != null);
    }

    #endregion
    
    #region private methods

    /// <summary>
    /// Gets a <see cref="System.Reflection.MemberInfo"/> from the given linq expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This private method wraps the differing mechanisms by which static reflection can be performed:
    /// </para>
    /// <list type="number">
    /// <item>
    /// Determine whether or not the expression is a unary (value type) expression or not.  If it is then use the
    /// operand for further analysis.
    /// </item>
    /// <item>
    /// Determine whether the expression-to-analyse is a member expression or a method call expression.
    /// </item>
    /// <item>
    /// Cast the expression appropriately and retirve the member.
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name='expression'>
    /// The expression to parse for a member.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Raised if the <paramref name="expression"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Raised if the <paramref name="expression"/> does not reflect a single member.
    /// </exception>
    private static MemberInfo Member(Expression expression)
    {
      if(expression == null)
      {
        throw new ArgumentNullException(nameof(expression));
      }

      if(expression is UnaryExpression)
      {
        return Member(((UnaryExpression) expression).Operand);
      }

      if(expression is MemberExpression)
      {
        return ((MemberExpression) expression).Member;
      }
      else if(expression is MethodCallExpression)
      {
        return ((MethodCallExpression) expression).Method;
      }
      else
      {
        string message = String.Format(ExceptionMessages.ExpressionMustIndicateMember, expression.ToString());
        throw new ArgumentException(message, nameof(expression));
      }
    }
    
    #endregion
  }
}

