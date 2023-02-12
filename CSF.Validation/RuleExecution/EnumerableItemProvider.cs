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

        /// <inheritdoc/>
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