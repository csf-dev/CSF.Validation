namespace CSF.Validation
{
    /// <summary>
    /// Enumerates the available scenarios in which a validator will throw an exception
    /// due to either failure or unexpected exceptions from validation rules.
    /// </summary>
    public enum RuleThrowingBehaviour
    {
        /// <summary>
        /// The validator will throw an exception if any part of the validation process
        /// raises an exception, such as when one or more validation rules raise uncaught exceptions.
        /// </summary>
        /// <remarks>
        /// <para>This is the default behaviour and is recommended for normal use.</para>
        /// </remarks>
        OnError = 0,

        /// <summary>
        /// The validator will throw an exception at any time when the validation fails, even if the reason
        /// for the failure was not exceptional.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This behaviour is not recommended except for scenarios where the validated object is expected to be
        /// valid and where an invalid object represents an exceptional case.
        /// If you expect that the validated object could be invalid then use either <see cref="OnError"/>
        /// or <see cref="Never"/> to avoid throwing an exception for a 'normal' failure.
        /// </para>
        /// <para>
        /// This behaviour includes the behaviour specified by <see cref="OnError"/>.
        /// </para>
        /// </remarks>
        OnFailure,

        /// <summary>
        /// The validator will not throw an exception under normal operation, even if validation rules raise
        /// uncaught exceptions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When this behaviour is specified, when any validation rule raises an exception, that exception will
        /// be caught and will not be re-thrown.  The rule will complete with an outcome of <see cref="Rules.RuleOutcome.Errored"/>.
        /// The overall result will indicate that validation has failed.
        /// </para>
        /// <para>
        /// The exceptions which are caught when this behaviour is specified may be accessed and inspected from
        /// the validation result.  The exception will be available at the <see cref="Rules.RuleResult.Exception"/>
        /// property of the corresponding rule-results.
        /// </para>
        /// <para>
        /// This behaviour is not recommended unless it is expected that validation rules may raise uncaught
        /// exceptions.  Under normal operation this would usually suggest bad design.
        /// The disadvantage of this approach is that it may hide logic errors and/or other problems because
        /// an unexpected exception will be treated very similarly to normal validation failures.
        /// </para>
        /// </remarks>
        Never,
    }
}