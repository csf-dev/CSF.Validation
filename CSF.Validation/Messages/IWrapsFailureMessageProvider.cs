namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which exposes a wrapped/original message provider object.
    /// </summary>
    public interface IWrapsFailureMessageProvider
    {
        /// <summary>
        /// Gets the wrapped message provider instance.
        /// </summary>
        object WrappedProvider { get; }
    }
}