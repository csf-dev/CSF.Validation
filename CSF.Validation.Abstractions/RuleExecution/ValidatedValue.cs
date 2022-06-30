using System;
using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A model which represents a value which is being validated by the validator.
    /// </summary>
    public class ValidatedValue
    {
        /// <summary>
        /// Gets or sets the manifest value to which the current instance relates.
        /// </summary>
        public IManifestItem ManifestValue { get; set;  }

        /// <summary>
        /// Gets or sets a response object which may expose an "actual value" of the validated value.
        /// </summary>
        /// <seealso cref="GetActualValue"/>
        public GetValueToBeValidatedResponse ValueResponse { get; set; }

        /// <summary>
        /// Gets or sets the identity of the value to be validated.
        /// </summary>
        public object ValueIdentity { get; set; }

        /// <summary>
        /// Gets or sets a parent value, where applicable.
        /// </summary>
        public ValidatedValue ParentValue { get; set; }

        /// <summary>
        /// Gets a collection of the child values, which treat the current instance as their parent.
        /// </summary>
        public IList<ValidatedValue> ChildValues { get; } = new List<ValidatedValue>();

        /// <summary>
        /// Gets a collection of the rules which should be executed upon the current value.
        /// </summary>
        public IList<ExecutableRule> Rules { get; set; } = new List<ExecutableRule>();

        /// <summary>
        /// Where this validated value represents a collection of values, and there is a separate
        /// validated value representing the items of that collection, this property should contain
        /// a collection of the values which represent the items of that collection.
        /// </summary>
        public IList<ValidatedValue> CollectionItems { get; set; } = new List<ValidatedValue>();

        /// <summary>
        /// Gets or sets a numeric item order, indicating the order in which this value was retrieved from a collection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is not neccesarily a collection index, because a collection of values to be validated
        /// might only be <see cref="IEnumerable{T}"/> and not (for example) <see cref="IList{T}"/>.
        /// Thus, the order in which values are retrieved might not be meaningful and might not even
        /// be stable.
        /// For informational purposes, this value is still retrieved and made available by
        /// this property.
        /// </para>
        /// <para>
        /// If the current validated value does not represent an item from a collection then this property will contain
        /// <see langword="null" />.
        /// </para>
        /// </remarks>
        public long? CollectionItemOrder { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the current instance matches the specified manifest item and value response.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used only to detect circular references in the validated object model.
        /// Circular references are not validated, because they would lead to an endless loop.
        /// </para>
        /// </remarks>
        /// <param name="item">A manifest item</param>
        /// <param name="valueResponse">A value response</param>
        /// <returns><see langword="true" /> if the current instance matches the item and value response; <see langword="false" /> otherwise.</returns>
        public bool IsMatch(IManifestItem item, GetValueToBeValidatedResponse valueResponse)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (valueResponse is null)
                throw new ArgumentNullException(nameof(valueResponse));

            IManifestItem
                thisItem = GetManifestItemForMatching(ManifestValue),
                thatItem = GetManifestItemForMatching(item);
            
            return ReferenceEquals(thisItem, thatItem) && Equals(ValueResponse, valueResponse);
        }

        static IManifestItem GetManifestItemForMatching(IManifestItem item)
        {
            if(item is RecursiveManifestValue recursiveItem)
                return recursiveItem.WrappedValue;
            return item;
        }

        /// <summary>
        /// Gets the 'actual value' from the <see cref="ValueResponse"/>.  If that is not an instance of
        /// <see cref="SuccessfulGetValueToBeValidatedResponse"/> then this method will return <see langword="null" />.
        /// </summary>
        /// <returns>Either an actual value or a <see langword="null" /> reference.</returns>
        public object GetActualValue()
        {
            if(!(ValueResponse is SuccessfulGetValueToBeValidatedResponse successResponse))
                return null;
            return successResponse.Value;
        }
    }
}