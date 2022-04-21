using System;
using System.Collections.Generic;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which can get a <see cref="IEnumerable{T}"/> of individual items to be validated, based upon
    /// an object which should implement the correct enumerable interface and an intended type.
    /// </summary>
    public interface IGetsEnumerableItemsToBeValidated
    {
        /// <summary>
        /// Gets an enumerable collection of item values to be validatated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that executing this method will enumerate the source collection and crystalise it into a finite collection.
        /// The order of items is defined by the order in which the source <see cref="IEnumerable{T}"/> provided them.
        /// There is no certainty that executing this method more than once would provide the items in the same
        /// order (or even the same items each time).
        /// </para>
        /// </remarks>
        /// <param name="enumerableValue">A value to be validated, which must implement
        /// <see cref="IEnumerable{T}"/> for a generic type specified by <paramref name="itemType"/>.</param>
        /// <param name="itemType">The expected generic type of the enumerable collection.</param>
        /// <returns>Either a collection of item values from the enumerable, or a <see langword="null" /> reference
        /// if <paramref name="enumerableValue"/> is <see langword="null" />.</returns>
        /// <exception cref="ValidationException">If the <paramref name="enumerableValue"/> does not implement
        /// <see cref="IEnumerable{T}"/> for the type <paramref name="itemType"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="itemType"/> is <see langword="null" />.</exception>
        IEnumerable<object> GetEnumerableItems(object enumerableValue, Type itemType);
    }
}