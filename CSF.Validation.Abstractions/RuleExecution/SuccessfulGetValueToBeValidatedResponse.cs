namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A <see cref="GetValueToBeValidatedResponse"/> which indicates success.
    /// </summary>
    public class SuccessfulGetValueToBeValidatedResponse : GetValueToBeValidatedResponse
    {
        /// <inheritdoc/>
        public override bool IsSuccess => true;

        /// <summary>
        /// Gets the value derived from the accessor function.
        /// </summary>
        public object Value { get; }
        
        /// <summary>
        /// Initialises a new instance of <see cref="SuccessfulGetValueToBeValidatedResponse"/>.
        /// </summary>
        /// <param name="value">The value derived from the accessor function.</param>
        public SuccessfulGetValueToBeValidatedResponse(object value)
        {
            Value = value;
        }
    }
}