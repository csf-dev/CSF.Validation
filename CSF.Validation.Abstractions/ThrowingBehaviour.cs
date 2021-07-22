namespace CSF.Validation
{
    /// <summary>
    /// Enumerates the available scenarios in which a validator will throw an exception
    /// for failed validation.
    /// </summary>
    public enum ThrowingBehaviour
    {
        /// <summary>
        /// The validator will throw an exception if any part of the validation process
        /// raises an exception, such as when one or more validation rules raise uncaught exceptions.
        /// </summary>
        /// <remarks>
        /// <para>This is the default behaviour and is recommended for normal use.</para>
        /// </remarks>
        OnError,

        /// <summary>
        /// The validator will throw an exception at any time when the validation fails, even if the reason
        /// for the failure was not exceptional.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This behaviour is not recommended except for scenarios where the validated object is completely
        /// expected to be valid and "an invalid object represents an exceptional case".
        /// If you expect that the validated object could be invalid then use either <see cref="OnError"/>
        /// or <see cref="Never"/> to avoid throwing an exception for a 'normal' failure.
        /// </para>
        /// </remarks>
        OnFailure,

        /// <summary>
        /// The validator will not throw an exception under normal operation, even if validation rules raise
        /// uncaught exceptions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If validation rules raise uncaught exceptions which are not re-thrown because of this behaviour,
        /// those exceptions may be expected within the <see cref="Rules.RuleResult.Exception"/> property of
        /// the corresponding rule-results.
        /// </para>
        /// <para>
        /// This behaviour is not recommended unless it is expected that validation rules could raise uncaught
        /// exceptions under normal operation (which would suggest bad design).  This means that uncaught exceptions
        /// from validation rules will be treated somewhat equally to unexceptional rule failures.
        /// </para>
        /// </remarks>
        Never,
    }
}