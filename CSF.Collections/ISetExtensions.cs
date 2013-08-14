using System;
using System.Linq;
using Iesi.Collections.Generic;

namespace CSF.Collections
{
  /// <summary>
  /// Extension methods to the generic <c>ISet</c> type.
  /// </summary>
  public static class ISetExtensions
  {
    /// <summary>
    /// Creates a wrapped copy of the <paramref name="sourceList"/> and associates a pair of actions with it that are
    /// executed upon items before they are added/removed to/from the list.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The wrapped copy of the list, with the new behaviour added.
    /// </returns>
    /// <param name='sourceList'>
    /// The source <c>IList</c>, which is unmodified in the process.
    /// </param>
    /// <param name='beforeAdd'>
    /// An action to perform upon an item before it is added to the list.
    /// </param>
    /// <param name='beforeRemove'>
    /// An action to perform upon an item before it is removed from the list.
    /// </param>
    /// <typeparam name='T'>
    /// The type of object contained within the list.
    /// </typeparam>
    public static EventBoundSetWrapper<T> WrapWithBeforeActions<T>(this ISet<T> sourceList,
                                                                    Action<T> beforeAdd,
                                                                    Action<T> beforeRemove)
      where T : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      EventBoundSetWrapper<T> output;
      output = (sourceList as EventBoundSetWrapper<T>)?? new EventBoundSetWrapper<T>(sourceList);

      output.BeforeAdd = (list, item) => {
        beforeAdd(item);
        return true;
      };
      output.BeforeRemove = (list, item) => {
        beforeRemove(item);
        return true;
      };

      return output;
    }

    /// <summary>
    /// Creates a wrapped copy of the <paramref name="sourceList"/> and associates a pair of functions with it that are
    /// executed upon items before they are added/removed to/from the list.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The wrapped copy of the list, with the new behaviour added.
    /// </returns>
    /// <param name='sourceList'>
    /// The source <c>IList</c>, which is unmodified in the process.
    /// </param>
    /// <param name='beforeAdd'>
    /// A function to execute before an item is added to the list.  If the function returns false then addition is
    /// aborted.
    /// </param>
    /// <param name='beforeRemove'>
    /// A function to execute before an item  is removed from the list.  If the function returns false then addition is
    /// aborted.
    /// </param>
    /// <typeparam name='T'>
    /// The type of object contained within the list.
    /// </typeparam>
    public static EventBoundSetWrapper<T> WrapWithBeforeActions<T>(this ISet<T> sourceList,
                                                                    Func<ISet<T>, T, bool> beforeAdd,
                                                                    Func<ISet<T>, T, bool> beforeRemove)
      where T : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      EventBoundSetWrapper<T> output;
      output = (sourceList as EventBoundSetWrapper<T>)?? new EventBoundSetWrapper<T>(sourceList);

      output.BeforeAdd = beforeAdd;
      output.BeforeRemove = beforeRemove;

      return output;
    }
  }
}

