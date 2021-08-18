namespace CSF.Validation
{
    /// <summary>
    /// An exception which may be raised when building a validator.
    /// </summary>
#if !NETSTANDARD1_0
    [System.Serializable]
#endif
    public class ValidatorBuildingException : System.Exception
    {
        /// <summary>
        /// Gets the validation result, where one is associated with the current exception.
        /// </summary>
        public ValidationResult ValidationResult { get; }

        /// <summary>
        /// Initialises a new <see cref="ValidatorBuildingException"/>.
        /// </summary>
        public ValidatorBuildingException() { }

        /// <summary>
        /// Initialises a new <see cref="ValidatorBuildingException"/>.
        /// </summary>
        /// <param name="message">The error message.</param>
        public ValidatorBuildingException(string message) : base(message) { }

        /// <summary>
        /// Initialises a new <see cref="ValidatorBuildingException"/>.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="validationResult">A validation result associated with the current exception.</param>
        public ValidatorBuildingException(string message, ValidationResult validationResult) : base(message)
        {
            ValidationResult = validationResult;
        }

        /// <summary>
        /// Initialises a new <see cref="ValidatorBuildingException"/>.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The inner exception.</param>
        public ValidatorBuildingException(string message, System.Exception inner) : base(message, inner) { }

#if !NETSTANDARD1_0
        /// <summary>
        /// Initialises a new <see cref="ValidatorBuildingException"/>.
        /// </summary>
        /// <param name="info">Serialisation info.</param>
        /// <param name="context">Streaming context.</param>
        protected ValidatorBuildingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
#endif
    }
}