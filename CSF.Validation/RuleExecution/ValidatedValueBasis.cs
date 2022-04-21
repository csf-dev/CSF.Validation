using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{

    /// <summary>
    /// A small model used as an intermediate state in order to get a <see cref="ValidatedValue"/>.
    /// It holds the information required in order to get the validated value at a point in the future.
    /// </summary>
    public sealed class ValidatedValueBasis
    {
        /// <summary>
        /// Gets the manifest value.
        /// </summary>
        public ManifestValueBase ManifestValue { get; }

        /// <summary>
        /// Gets the actual value.
        /// </summary>
        public object ActualValue { get; }

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
            => $"[{nameof(ValidatedValueBasis)}: Type = {ManifestValue.ValidatedType.Name}, Value = {ActualValue}]";

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatedValueBasis"/>.
        /// </summary>
        /// <param name="manifestValue">The manifest value for this basis.</param>
        /// <param name="actualValue">The actual value for this basis.</param>
        /// <param name="parent">An optional parent validated value for this basis.</param>
        /// <param name="collectionOrder">An optional collection order for this basis.</param>
        public ValidatedValueBasis(ManifestValueBase manifestValue, object actualValue, ValidatedValue parent, long? collectionOrder = default)
        {
            ManifestValue = manifestValue;
            ActualValue = actualValue;
            Parent = parent;
            CollectionOrder = collectionOrder;
        }
    }
}