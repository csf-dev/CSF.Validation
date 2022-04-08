namespace CSF.Validation.Bootstrap
{
    /// <summary>
    /// An exception raised by <see cref="IResolvesServices"/> if it is unable to resolve a service instance.
    /// </summary>
#if !NETSTANDARD1_0
    [System.Serializable]
#endif
    public class ResolutionException : ValidationException
    {
        /// <summary>
        /// Initialises an instance of <see cref="ResolutionException"/>.
        /// </summary>
        public ResolutionException() {}

        /// <summary>
        /// Initialises an instance of <see cref="ResolutionException"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        public ResolutionException(string message) : base(message) {}

        /// <summary>
        /// Initialises an instance of <see cref="ResolutionException"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public ResolutionException(string message, System.Exception inner) : base(message, inner) {}

#if !NETSTANDARD1_0
        /// <summary>
        /// Initialises an instance of <see cref="ResolutionException"/>.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected ResolutionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
#endif
    }
}