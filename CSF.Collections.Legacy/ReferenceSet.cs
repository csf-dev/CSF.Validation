//
// ReferenceSet.cs
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
using System.Linq.Expressions;
using System.Reflection;
using CSF.Reflection;
using Iesi.Collections.Generic;

namespace CSF.Collections.Legacy
{
  /// <summary>
  /// Helper type very similar to <see cref="CSF.Collections.ReferenceList"/> but providing the same functionality for
  /// implementors of the generic <c>ISet</c> type.
  /// </summary>
  public static class ReferenceSet
  {
    #region public api

    /// <summary>
    /// Gets a wrapped reference list, initialising it if required.
    /// </summary>
    /// <returns>
    /// A generic IList, with added addition and removal handlers.
    /// </returns>
    /// <param name='wrapper'>
    /// The list wrapper instance to use.
    /// </param>
    /// <param name='original'>
    /// The original/source list.
    /// </param>
    /// <param name='referenceProperty'>
    /// An expression indicating a property upon the contained type.
    /// </param>
    /// <param name='referenceItem'>
    /// The reference item to be stored in the <paramref name="referenceProperty"/>.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the list.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// See the documentation of this type for detailled information on how this works and what it does.
    /// </para>
    /// </remarks>
    public static ISet<T> GetOrInit<T>(ref ISet<T> wrapper,
                                       ref ISet<T> original,
                                       Expression<Func<T, object>> referenceProperty,
                                       object referenceItem) where T : class
    {
      EventBoundSetWrapper<T> typedList = wrapper as EventBoundSetWrapper<T>;

      if(typedList == null || !typedList.IsWrapping(original))
      {
        original = original?? new HashedSet<T>();
        wrapper = GetOrInit(original, referenceProperty, referenceItem);
      }

      return wrapper;
    }

    /// <summary>
    /// Replaces a wrapped reference list, detaching contained items if needed, and confiures the wrapper to wrap a
    /// replacement list.
    /// </summary>
    /// <param name='wrapperToOverwrite'>
    /// The wrapper instance to overwrite.
    /// </param>
    /// <param name='replacement'>
    /// The replacement source list.
    /// </param>
    /// <param name='referenceProperty'>
    /// An expression indicating a property upon the contained type.
    /// </param>
    /// <param name='referenceItem'>
    /// The reference item to be stored in the <paramref name="referenceProperty"/>.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the list.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// See the documentation of this type for detailled information on how this works and what it does.
    /// </para>
    /// </remarks>
    public static void Replace<T>(ref ISet<T> wrapperToOverwrite,
                                  ISet<T> replacement,
                                  Expression<Func<T, object>> referenceProperty,
                                  object referenceItem) where T : class
    {
      if(replacement == null)
      {
        throw new ArgumentNullException("replacement");
      }

      EventBoundSetWrapper<T> typedList = wrapperToOverwrite as EventBoundSetWrapper<T>;

      if(typedList != null)
      {
        typedList.DetachAll();
      }

      PropertyInfo propInfo = Reflect.Property(referenceProperty);
      foreach(T item in replacement)
      {
        propInfo.SetValue(item, referenceItem, null);
      }

      wrapperToOverwrite = GetOrInit(replacement, referenceProperty, referenceItem);
    }

    #endregion

    #region private methods

    /// <summary>
    /// Initialises and returns a new list wrapper, wrapping the given <paramref name="original"/> collection.
    /// </summary>
    /// <returns>
    /// A generic IList, with added addition and removal handlers.
    /// </returns>
    /// <param name='original'>
    /// The original/source list.
    /// </param>
    /// <param name='referenceProperty'>
    /// An expression indicating a property upon the contained type.
    /// </param>
    /// <param name='referenceItem'>
    /// The reference item to be stored in the <paramref name="referenceProperty"/>.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the list.
    /// </typeparam>
    private static EventBoundSetWrapper<T> GetOrInit<T>(ISet<T> original,
                                                        Expression<Func<T, object>> referenceProperty,
                                                        object referenceItem) where T : class
    {
      if(original == null)
      {
        throw new ArgumentNullException("original");
      }

      EventBoundSetWrapper<T> output = original as EventBoundSetWrapper<T>;

      if(output == null)
      {
        PropertyInfo propInfo = Reflect.Property(referenceProperty);

        output = original.WrapWithBeforeActions((list, item) => BeforeAdd(list, item, propInfo, referenceItem),
                                                (list, item) => BeforeRemove(list, item, propInfo));
      }

      return output;
    }

    /// <summary>
    /// Method referenced by a delegate, represents the "before addition" action.
    /// </summary>
    /// <returns>
    /// Always returns <c>true</c>.
    /// </returns>
    /// <param name='list'>
    /// The wrapped/original list instance that the addition action is occurring to.
    /// </param>
    /// <param name='item'>
    /// The item that is about to be added to the <paramref name="list"/> and that must be manipulated.
    /// </param>
    /// <param name='referenceProperty'>
    /// An expression indicating a property upon the contained type.
    /// </param>
    /// <param name='referenceItem'>
    /// The reference item to be stored in the <paramref name="referenceProperty"/>.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the list.
    /// </typeparam>
    private static bool BeforeAdd<T>(ISet<T> list,
                                     T item,
                                     PropertyInfo referenceProperty,
                                     object referenceItem) where T : class
    {
      if(item == null)
      {
        throw new ArgumentNullException("item");
      }

      referenceProperty.SetValue(item, referenceItem, null);

      return true;
    }

    /// <summary>
    /// Method referenced by a delegate, represents the "before removal" action.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the original <paramref name="list"/> contains the <paramref name="item"/> to be removed;
    /// <c>false</c> otherwise.
    /// </returns>
    /// <param name='list'>
    /// The wrapped/original list instance that the removal action is occurring to.
    /// </param>
    /// <param name='item'>
    /// The item that is about to be removed from the <paramref name="list"/> and that must be manipulated.
    /// </param>
    /// <param name='referenceProperty'>
    /// An expression indicating a property upon the contained type.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the list.
    /// </typeparam>
    private static bool BeforeRemove<T>(ISet<T> list,
                                        T item,
                                        PropertyInfo referenceProperty) where T : class
    {
      if(item == null)
      {
        throw new ArgumentNullException("item");
      }

      bool contained = list.Contains(item);
      if(contained)
      {
        referenceProperty.SetValue(item, null, null);
      }

      return contained;
    }

    #endregion
  }
}

