namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which can get the appropriate failure message provider for a specified <see cref="ValidationRuleResult"/>.
    /// </summary>
    public interface IGetsFailureMessageProvider
    {
        /// <summary>
        /// Gets the most appropriate message provider implementation for getting a feedback message for the
        /// specified <see cref="ValidationRuleResult"/>.
        /// </summary>
        /// <param name="ruleResult">The validation rule result for which to get a message provider.</param>
        /// <returns>Either an implementation of <see cref="IGetsFailureMessage"/>, or a <see langword="null" />
        /// reference, if no message provider is suitable for the result.</returns>
        IGetsFailureMessage GetProvider(ValidationRuleResult ruleResult);
    }
}