namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which exposes a <see cref="ValidatorBuilderContext"/> as a property.
    /// </summary>
    public interface IHasValidationBuilderContext
    {
        /// <summary>
        /// Gets the validator builder context associated with the current instance.
        /// </summary>
        ValidatorBuilderContext Context { get; }
    }
}