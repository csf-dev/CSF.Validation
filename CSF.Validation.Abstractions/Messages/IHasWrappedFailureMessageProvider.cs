namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which wraps (like in the Adapter Pattern) an implementation of a failure message provider.
    /// </summary>
    public interface IHasWrappedFailureMessageProvider
    {
        /// <summary>
        /// Gets a reference to the wrapped failure message provider instance.
        /// </summary>
        /// <returns>The wrapped failure message provider.</returns>
        object GetWrappedProvider();
    }
}