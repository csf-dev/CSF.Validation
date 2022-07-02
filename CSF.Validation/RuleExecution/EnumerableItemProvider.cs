using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Reflection;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A service which gets an enumerable collection of items to be validated.
    /// </summary>
    public class EnumerableItemProvider : IGetsEnumerableItemsToBeValidated
    {
        const string uniqueMessage = "3d27ef38-0104-4803-bf45-36b5602d386f";

        static MethodInfo openGenericMethod = Reflect.Method<EnumerableItemProvider>(x => x.GetEnumerableItemsPrivate<object>(default))
            .GetGenericMethodDefinition();

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
        public IEnumerable<object> GetEnumerableItems(object enumerableValue, Type itemType)
        {
            if (itemType is null)
                throw new ArgumentNullException(nameof(itemType));
            if(enumerableValue is null)
                return null;

            var method = openGenericMethod.MakeGenericMethod(itemType);

            try
            {
                return method.Invoke(this, new[] { enumerableValue }) as IEnumerable<object>;
            }
            catch(TargetInvocationException ex)
            {
                if(ex.InnerException is InvalidCastException && ex.InnerException.Message == uniqueMessage)
                {
                    var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("ObjectMustBeEnumerable"),
                                                nameof(IEnumerable<object>),
                                                enumerableValue.GetType().FullName,
                                                itemType.Name);
                    throw new ValidationException(message);
                }

                throw;
            }
        }

        IEnumerable<object> GetEnumerableItemsPrivate<T>(object enumerableValue)
        {
            if(enumerableValue is IEnumerable<T> enumerable)
                return enumerable.ToList().Cast<object>();
            
            // We are intentionally throwing invalid cast here because we're going to
            // catch it (through the TargetInvocationException it'll be wrapped within) and
            // throw a different kind of exception instead.  We use a GUID for the message
            // too, so that we can be absolutely certain we are catching the right one.
            throw new InvalidCastException(uniqueMessage);
        }
    }
}