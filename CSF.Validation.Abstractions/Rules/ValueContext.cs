using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A model which provides contextual information about a value which is validated.
    /// </summary>
    public class ValueContext
    {
        /// <summary>
        /// Gets the identity associated with the current value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The object (or value) identity is used to distinguish this value from other objects/values of
        /// the same type.
        /// This is particularly useful when validating collections of similar objects.
        /// </para>
        /// </remarks>
        public object ObjectIdentity { get; }

        /// <summary>
        /// Gets a <see cref="ManifestValueInfo"/> with information about the configuration of the current
        /// validated value.
        /// </summary>
        public ManifestValueInfo ValueInfo { get; }

        /// <summary>
        /// Gets the value which is being validated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Developers do not need to use this property to get the primary value under validation when they are using the
        /// <see cref="IRule{TValidated}"/> interface.  The first parameter passed to
        /// <see cref="IRule{TValidated}.GetResultAsync(TValidated, RuleContext, System.Threading.CancellationToken)"/>
        /// is the same as this property, but it is presented in a strongly-typed manner.
        /// </para>
        /// <para>
        /// Likewise, when developers are using the interface <see cref="IRule{TValue, TParent}"/>, they do not need to use
        /// this property to get the value from the first ancestor context (see <see cref="RuleContext.AncestorContexts"/>).
        /// That parent valus will be presented as the second parameter to
        /// <see cref="IRule{TValue, TParent}.GetResultAsync(TValue, TParent, RuleContext, System.Threading.CancellationToken)"/>.
        /// </para>
        /// <para>
        /// This property may be used within rule logic to access more distant ancestors, if they are required.
        /// </para>
        /// </remarks>
        public object ActualValue { get; }

        /// <summary>
        /// Gets a numeric item order, indicating the order in which this value was retrieved from a collection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is not neccesarily a collection index, because a collection of values to be validated
        /// might only be <see cref="System.Collections.Generic.IEnumerable{T}"/> and not (for example) <see cref="System.Collections.Generic.IList{T}"/>.
        /// Thus, the order in which values are retrieved might not be meaningful and might not even
        /// be stable.
        /// </para>
        /// <para>
        /// If the current validated value does not represent an item from a collection then this property will contain
        /// <see langword="null" />.
        /// </para>
        /// </remarks>
        public long? CollectionItemOrder { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ValueContext"/>.
        /// </summary>
        /// <param name="identity">The object identity associated with this ancestor context.</param>
        /// <param name="actualValue">The object being validated in this ancestor context.</param>
        /// <param name="manifestValue">The manifest value.</param>
        /// <param name="collectionItemOrder">The collection index by which you would traverse from this ancestor context to its immediate child (where applicable).</param>
        public ValueContext(object identity, object actualValue, IManifestItem manifestValue, long? collectionItemOrder = null)
        {
            if (manifestValue is null)
                throw new ArgumentNullException(nameof(manifestValue));

            ValueInfo = new ManifestValueInfo(manifestValue);
            ObjectIdentity = identity;
            ActualValue = actualValue;
            CollectionItemOrder = collectionItemOrder;
        }
    }
}