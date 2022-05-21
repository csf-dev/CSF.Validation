namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A response/result type for the <see cref="IGetsValueToBeValidated"/> service.
    /// </summary>
    public abstract class GetValueToBeValidatedResponse
    {
        /// <summary>
        /// Gets a value that indicates whether ornot the current instance represents success.
        /// </summary>
        public abstract bool IsSuccess { get; }
    }
}