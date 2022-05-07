namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which can get an instance of <see cref="IGetsMessageProviderInfo"/>.
    /// </summary>
    public interface IGetsMessageProviderInfoFactory
    {
        /// <summary>
        /// Gets the message provider info factory.
        /// </summary>
        /// <returns>A message provider info factory instance.</returns>
        IGetsMessageProviderInfo GetProviderInfoFactory();
    }
}