using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A small model used as an intermediate state in order to get a <see cref="ValidatedValue"/>.
    /// It holds the information required in order to get the validated value at a point in the future.
    /// </summary>
    public sealed class ValidatedValueBasis
    {
        readonly IEnumerable<ManifestItem> polymorphicTypes;

        /// <summary>
        /// Gets the manifest value.
        /// </summary>
        public ManifestItem ManifestValue { get; }

        /// <summary>
        /// Gets the actual value.
        /// </summary>
        public GetValueToBeValidatedResponse ValidatedValueResponse { get; }

        /// <summary>
        /// Gets an optional parent validated value.
        /// </summary>
        public ValidatedValue Parent { get; }

        /// <summary>
        /// Gets an optional collection order.
        /// </summary>
        public long? CollectionOrder { get; }

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string that represents this validated value basis.</returns>
        public override string ToString()
            => $"[{nameof(ValidatedValueBasis)}: Type = {ManifestValue.ValidatedType.Name}, Value = {ValidatedValueResponse}]";

        /// <summary>
        /// Gets the actual value from the <see cref="ValidatedValueResponse"/>, or <see langword="null" /> if the response is not a success.
        /// </summary>
        /// <returns>Either the <see cref="SuccessfulGetValueToBeValidatedResponse.Value"/> or a
        /// <see langword="null" /> reference if the response is not a success.</returns>
        public object GetActualValue()
            => (ValidatedValueResponse is SuccessfulGetValueToBeValidatedResponse successResponse) ? successResponse.Value : null;

        /// <summary>
        /// Gets a collection of the <see cref="ManifestItem"/> which should be considered as children of the current
        /// manifest value basis.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method considers not only the <see cref="ManifestItem.Children"/> of <see cref="ValidatedValueBasis.ManifestValue"/>
        /// but also the children of every value returned by <see cref="GetPolymorphicTypes"/>.
        /// </para>
        /// </remarks>
        /// <returns>A collection of manifest values.</returns>
        public IEnumerable<ManifestItem> GetChildManifestValues()
        {
            return new [] { ManifestValue }
                .Union(polymorphicTypes)
                .SelectMany(x => x.Children)
                .ToList();
        }

        /// <summary>
        /// Gets a collection of the <see cref="ManifestRule"/> which should be applied to the current
        /// manifest value basis.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method considers not only the <see cref="ManifestItem.Rules"/> of <see cref="ValidatedValueBasis.ManifestValue"/>
        /// but also the rules of every value returned by <see cref="GetPolymorphicTypes"/>.
        /// </para>
        /// </remarks>
        /// <returns>A collection of manifest rules.</returns>
        public IEnumerable<ManifestRule> GetManifestRules()
        {
            return new [] { ManifestValue }
                .Union(polymorphicTypes)
                .SelectMany(x => x.Rules)
                .ToList();
        }

        /// <summary>
        /// Gets a value which indicates whether the current validated value basis represents a
        /// circular reference to a value which has already been converted to a <see cref="ValidatedValue"/>.
        /// </summary>
        /// <returns><see langword="true" /> if the current instance appears as a <see cref="ValidatedValue"/>
        /// amongst the chain of <see cref="Parent"/> validated values; <see langword="false" /> otherwise.</returns>
        public bool IsCircularReference() => GetAllParents().Any(IsMatch);

        IEnumerable<ValidatedValue> GetAllParents()
        {
            var current = Parent;
            while(!(current is null))
            {
                yield return current;
                current = current.ParentValue;
            }
        }

        bool IsMatch(ValidatedValue value) => value.IsMatch(ManifestValue, ValidatedValueResponse);

        /// <summary>
        /// Gets a collection of applicable polymorphic types which are applicable to the current validated value.
        /// </summary>
        IEnumerable<ManifestItem> GetPolymorphicTypes()
        {
            if (ManifestValue.IsPolymorphicType
             || !(ValidatedValueResponse is SuccessfulGetValueToBeValidatedResponse successResponse))
                return Enumerable.Empty<ManifestItem>();

            return ManifestValue
                .PolymorphicTypes
                .Where(x => x.ValidatedType.IsInstanceOfType(successResponse.Value))
                .ToList();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatedValueBasis"/>.
        /// </summary>
        /// <param name="manifestValue">The manifest value for this basis.</param>
        /// <param name="actualValue">The actual value for this basis.</param>
        /// <param name="parent">An optional parent validated value for this basis.</param>
        /// <param name="collectionOrder">An optional collection order for this basis.</param>
        public ValidatedValueBasis(ManifestItem manifestValue,
                                   GetValueToBeValidatedResponse actualValue,
                                   ValidatedValue parent,
                                   long? collectionOrder = default)
        {
            ManifestValue = manifestValue;
            ValidatedValueResponse = actualValue;
            Parent = parent;
            CollectionOrder = collectionOrder;

            polymorphicTypes = GetPolymorphicTypes();
        }
    }
}