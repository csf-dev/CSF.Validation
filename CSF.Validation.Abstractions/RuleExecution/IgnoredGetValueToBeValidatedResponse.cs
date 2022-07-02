namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A <see cref="GetValueToBeValidatedResponse"/> which indicates failure but that the failure should be ignored.
    /// </summary>
    public sealed class IgnoredGetValueToBeValidatedResponse : GetValueToBeValidatedResponse
    {
        static readonly GetValueToBeValidatedResponse singleton = new IgnoredGetValueToBeValidatedResponse();

        /// <inheritdoc/>
        public override bool IsSuccess => false;

        /// <inheritdoc/>
        public override bool Equals(GetValueToBeValidatedResponse other) => ReferenceEquals(this, other);

        /// <summary>
        /// Gets a singleton/flyweight instance of this type.
        /// </summary>
        public static GetValueToBeValidatedResponse Default => singleton;
    }
}