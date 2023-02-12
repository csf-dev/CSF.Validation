namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which can configure a <see cref="ValidatorBuilderContext"/>.
    /// </summary>
    public interface IConfiguresContext
    {
        /// <summary>
        /// Performs configuration upon the context.
        /// </summary>
        /// <param name="context">The builder context to configure.</param>
        void ConfigureContext(ValidatorBuilderContext context);
    }
}