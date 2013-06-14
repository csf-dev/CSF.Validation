using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace CSF.Collections
{
  /// <summary>
  /// Provides collection extension methods specific to the <c>Iesi.Collections</c> library.
  /// </summary>
  public static class IesiCollectionExtensions
  {
    /// <summary>
    /// Converts a generic collection of elements into an equivalent set.
    /// </summary>
    /// <returns>
    /// A generic <c>ISet</c> containing equivalent elements to the source collection.
    /// </returns>
    /// <param name='source'>
    /// A collection.
    /// </param>
    /// <typeparam name='T'>
    /// The type of item contained within the collection.
    /// </typeparam>
    public static ISet<T> ToSet<T>(this ICollection<T> source)
    {
      if(source == null)
      {
        throw new ArgumentNullException("source");
      }

      return new HashedSet<T>(source);
    }
  }
}

