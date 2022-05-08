namespace CSF.Validation.Messages
{
    /// <summary>
    /// A special case of <see cref="IHasFailureMessageUsageCriteria"/> which represents "no criteria".
    /// It is hard-coded to return <see langword="true" /> for all scenarios.
    /// </summary>
    public sealed class AllowAllUsageCriteriaProvider : IHasFailureMessageUsageCriteria
    {
        /// <summary>
        /// Gets a value (which will always be <see langword="true" />) which indicates whether or not the current
        /// class may be used to provide a failure message for the specified validation rule result.
        /// </summary>
        /// <param name="result">A validation rule result.</param>
        /// <returns>This implementation always returns <see langword="true" />.</returns>
        public bool CanGetFailureMessage(ValidationRuleResult result) => true;
    }
}