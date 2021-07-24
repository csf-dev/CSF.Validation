namespace CSF.Validation
{
    /// <summary>
    /// A model for options which affect how a validator should behave whilst it is performing its validation.
    /// </summary>
    public class ValidationOptions
    {
        /// <summary>
        /// Gets or sets the exception-throwing behaviour for the validator.
        /// The default &amp; recommended behaviour is <see cref="ThrowingBehaviour.OnError"/>.
        /// </summary>
        public ThrowingBehaviour ThrowingBehaviour { get; set; }
    }
}