namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which can get an instance of <see cref="IGetsValidationRuleResultWithMessage"/> applicable for the specified options.
    /// </summary>
    public interface IGetsRuleWithMessageProvider
    {
        /// <summary>
        /// Gets a the rule/message provider applicable for the specified options.
        /// </summary>
        /// <param name="options">Validation options.</param>
        /// <returns>A rule/message provider.</returns>
        IGetsValidationRuleResultWithMessage GetRuleWithMessageProvider(ResolvedValidationOptions options);
    }
}