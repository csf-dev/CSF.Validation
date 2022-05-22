namespace CSF.Validation
{
    /// <summary>
    /// An exception which may be raised by an <see cref="IValidator"/> or an <see cref="IValidator{TValidated}"/>.
    /// </summary>
    [System.Serializable]
    public class ValidationException : System.Exception
    {
        /// <summary>
        /// Gets the validation result, where one is associated with the current exception.
        /// </summary>
        public IHasValidationRuleResults ValidationResult { get; }

        /// <summary>
        /// Initialises a new <see cref="ValidationException"/>.
        /// </summary>
        public ValidationException() {}

        /// <summary>
        /// Initialises a new <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="message">The error message.</param>
        public ValidationException(string message) : base(message) {}

        /// <summary>
        /// Initialises a new <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="validationResult">A validation result associated with the current exception.</param>
        public ValidationException(string message, IHasValidationRuleResults validationResult) : base(message)
        {
            ValidationResult = validationResult;
        }

        /// <summary>
        /// Initialises a new <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The inner exception.</param>
        public ValidationException(string message, System.Exception inner) : base(message, inner) {}

        /// <summary>
        /// Initialises a new <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="info">Serialisation info.</param>
        /// <param name="context">Streaming context.</param>
        protected ValidationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}