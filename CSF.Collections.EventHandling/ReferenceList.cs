//
// ReferenceList.cs
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

namespace CSF.Collections.EventHandling
{
  /// <summary>
  /// Helper type dedicated to providing 'reference list' functionality, such as for entities.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Firstly, credit where credit is due.  This system is heavily based upon the excellent work found at
  /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.  It has been expanded
  /// and refined a little here but the core concept of using private fields wrapped with custom collections is an idea
  /// that I would not have come up with on my own.
  /// </para>
  /// <para>
  /// The concept of a 'reference list' is used frequently throughout domain models that feature 1-to-N relationships.
  /// A type contains a collection of items that are related.  Each of those contained items should hold a reference
  /// back to their 'parent'.  Consider the following:
  /// </para>
  /// <example>
  /// <code>
  /// public class ShoppingBasket
  /// {
  ///   public IList&lt;BasketItem&gt; Items { get; set; }
  /// }
  /// 
  /// public class BasketItem
  /// {
  ///   public ShoppingBasket Basket { get; set; }
  /// }
  /// </code>
  /// </example>
  /// <para>
  /// In the above example, any time a new item is added to the <c>Items</c> collection upon the parent basket, ideally
  /// the <c>Basket</c> property of that item should be automatically populated appropriately.  This keeps the API
  /// simple and ensures that developers don't have to remember to set the 'back reference' every time they add an item
  /// to such a collection.
  /// </para>
  /// <para>
  /// Typically, this could be resolved by using a custom collection type, which updates the reference on the item as it
  /// is added.  This solution breaks down however when we begin using an ORM package, since those custom collection
  /// types then need extra work performing in order to map them to the database.
  /// </para>
  /// <para>
  /// This is where the use of a generic <c>EventBoundListWrapper&lt;T&gt;</c> comes into play.  The wrapper is a custom
  /// collection type, that is able to maintain 'before' and 'after' events for both adding items and removing items.
  /// The trick however is that it simply wraps a plain generic <c>System.Collections.Generic.IList&lt;T&gt;</c>.  The
  /// ORM is configured to perform its mappings using the standard list, but the wrapper instance is exposed via the
  /// public property.  The wrapper keeps the contained list in sync, and everything works as intended.
  /// </para>
  /// <example>
  /// <para>
  /// Typical usage, continuing the example above, is as such:
  /// </para>
  /// <code>
  /// public class ShoppingBasket
  /// {
  ///   private IList&lt;BasketItem&gt; _items, _wrappedItems;
  /// 
  ///   public IList&lt;BasketItem&gt; Items
  ///   {
  ///     get {
  ///       return ReferenceList.GetOrInit&lt;BasketItem&gt;(ref _wrappedItems, ref _items, x => x.Basket, this);
  ///     }
  ///     set {
  ///       ReferenceList.Replace&lt;BasketItem&gt;(ref _wrappedItems, value, x => x.Basket, this);
  ///       _items = value;
  ///     }
  ///   }
  /// }
  /// 
  /// public class BasketItem
  /// {
  ///   public ShoppingBasket Basket { get; set; }
  /// }
  /// </code>
  /// <para>
  /// The getter ensures that the <c>_wrappedItems</c> collection is initialised as a list wrapper, wrapping the
  /// <c>_items</c> collection.  If it is not initialised, or if the given original collection is not the
  /// collection that the wrapper currently wraps then the wrapper is created/recreated as a new wrapper, wrapping the
  /// original collection given.
  /// </para>
  /// <para>
  /// The expression <c>x => x.Basket</c> is a LINQ expression for the property of <c>BasketItem</c> which should be
  /// manipulated when any given basket item is added/removed to/from a shopping basket.  In the case of an addition,
  /// the <c>Basket</c> property is set to <c>this</c> (the fourth parameter, representing the current basket item).  In
  /// the case of a removal from the shopping basket's collection, the <c>Basket</c> property is set to null.
  /// </para>
  /// <para>
  /// The property setter determines whether the <c>_wrappedItems</c> is currently initialised as an event-bound list
  /// wrapper.  If it is, then the removal actions are performed upon every item in the collection, setting each of
  /// their <c>Basket</c> properties to null.  Secondly, every item currently within the new list (passed in as
  /// <c>value</c>) goes through a one-time process of setting their <c>Basket</c> properties to <c>this</c> (the fourth
  /// parameter, representing the current basket item).
  /// </para>
  /// <para>
  /// Finally, the setter performs all of the initialisation actions upon the <c>_wrappedItems</c> collection as would
  /// be performed by the getter.  The wrapper is now ready to use.  The final step is to overwrite the origin list with
  /// the collection passed in by <c>value</c>.
  /// </para>
  /// </example>
  /// <para>
  /// A small note, particularly important when using NHibernate and "Cascade All &amp; Delete Orphans" cascade rules.
  /// It is often a good idea to avoid making collection properties settable.  The NHibernate ORM will complain if a
  /// collection instance mapped as cascade-all-delete-orphans is replaced with a new collection instance.  Replacing
  /// the collection can only be performed in this scenario by clearing all of the elements of the original collection
  /// and then replacing the elements.  There is an extension method provided for this at
  /// <c>ICollectionExtensions.ReplaceContents&lt;T&gt;(IEnumerable&lt;T&gt; replacement);</c>
  /// </para>
  /// </remarks>
  public static class ReferenceList
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
    public static IList<T> GetOrInit<T>(ref IList<T> wrapper,
                                        ref IList<T> original,
                                        Expression<Func<T, object>> referenceProperty,
                                        object referenceItem) where T : class
    {
      EventBoundListWrapper<T> typedList = wrapper as EventBoundListWrapper<T>;

      if(typedList == null || !typedList.IsWrapping(original))
      {
        original = original?? new List<T>();
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
    public static void Replace<T>(ref IList<T> wrapperToOverwrite,
                                  IList<T> replacement,
                                  Expression<Func<T, object>> referenceProperty,
                                  object referenceItem) where T : class
    {
      if(replacement == null)
      {
        throw new ArgumentNullException("replacement");
      }

      EventBoundListWrapper<T> typedList = wrapperToOverwrite as EventBoundListWrapper<T>;

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
    private static EventBoundListWrapper<T> GetOrInit<T>(IList<T> original,
                                                         Expression<Func<T, object>> referenceProperty,
                                                         object referenceItem) where T : class
    {
      if(original == null)
      {
        throw new ArgumentNullException("original");
      }

      EventBoundListWrapper<T> output = original as EventBoundListWrapper<T>;

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
    private static bool BeforeAdd<T>(IList<T> list,
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
    private static bool BeforeRemove<T>(IList<T> list,
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

