namespace CSF.Validation
{
    /// <summary>
    /// An exception which may be raised when building a validator.
    /// </summary>
    [System.Serializable]
    public class ValidatorBuildingException : System.Exception
    {
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
        /// <param name="inner">The inner exception.</param>
        public ValidatorBuildingException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initialises a new <see cref="ValidatorBuildingException"/>.
        /// </summary>
        /// <param name="info">Serialisation info.</param>
        /// <param name="context">Streaming context.</param>
        protected ValidatorBuildingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}