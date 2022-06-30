namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A <see cref="GetValueToBeValidatedResponse"/> which indicates success.
    /// </summary>
    public sealed class SuccessfulGetValueToBeValidatedResponse : GetValueToBeValidatedResponse
    {
        /// <inheritdoc/>
        public override bool IsSuccess => true;

        /// <summary>
        /// Gets the value derived from the accessor function.
        /// </summary>
        public object Value { get; }
        
        /// <inheritdoc/>
        public override bool Equals(GetValueToBeValidatedResponse other)
        {
            if(ReferenceEquals(this, other)) return true;
            
            if(!(other is SuccessfulGetValueToBeValidatedResponse successResponse))
                return false;

            return Equals(Value, successResponse.Value);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => (Value?.GetHashCode()).GetValueOrDefault();

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