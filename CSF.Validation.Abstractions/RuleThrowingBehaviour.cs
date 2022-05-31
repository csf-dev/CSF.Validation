namespace CSF.Validation
{
    /// <summary>
    /// Enumerates the available scenarios in which a validator will throw an exception
    /// due to either failure or unexpected exceptions from validation rules.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default behaviour if this value is not specified upon the <see cref="ResolvedValidationOptions"/> is
    /// <see cref="OnError"/>.
    /// This is recommended for normal use.
    /// </para>
    /// <para>
    /// The <see cref="OnFailure"/> behaviour is not recommended except for scenarios where the validated
    /// object is expected to be valid and where an invalid object represents an exceptional case.
    /// If you expect that the validated object could be invalid then use either <see cref="OnError"/>
    /// or <see cref="Never"/> to avoid throwing an exception for a 'normal' failure.
    /// </para>
    /// <para>
    /// The <see cref="OnFailure"/> behaviour includes the behaviour specified by <see cref="OnError"/>.
    /// </para>
    /// <para>
    /// When the <see cref="Never"/> behaviour is specified, no exception will be raised at the completion
    /// of validation.
    /// Rule results with outcomes of <see cref="Rules.RuleOutcome.Errored"/> will still cause the validation
    /// result to indicate failure but without throwing an exception.
    /// </para>
    /// <para>
    /// The <see cref="Never"/> behaviour is not recommended unless it is expected that validation rules may
    /// raise uncaught exceptions.  Under normal operation this would usually suggest bad design.
    /// The disadvantage of this approach is that it may hide logic errors and/or other problems because
    /// an unexpected exception will be treated very similarly to normal validation failures.
    /// </para>
    /// </remarks>
    /// <seealso cref="ResolvedValidationOptions"/>
    /// <seealso cref="Manifest.ValueAccessExceptionBehaviour"/>
    public enum RuleThrowingBehaviour
    {
        /// <summary>
        /// The validator will throw an exception at the completion of validation if any result
        /// has an outcome of <see cref="Rules.RuleOutcome.Errored"/>, such as when a rule
        /// raises an exception.
        /// </summary>
        OnError = 0,

        /// <summary>
        /// The validator will throw an exception at any time when the validation fails, even if the reason
        /// for the failure was not exceptional.
        /// </summary>
        OnFailure,

        /// <summary>
        /// The validator will not throw an exception under normal operation, even if validation rules raise
        /// uncaught exceptions.
        /// </summary>
        Never,
    }
}