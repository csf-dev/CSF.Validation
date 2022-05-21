
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// Enumerates the possible behaviours which the validator should take when a value-accessor raises an exception.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Typically, a value-accessor: <see cref="ManifestValue.AccessorFromParent"/> should not raise exceptions.  If an exception
    /// occurs then it will be impossible to validate this value or any of the values derived from it.
    /// The validator has a number of ways in which it can respond to such exceptions.
    /// </para>
    /// <para>
    /// It is down to implementations of <see cref="CSF.Validation.RuleExecution.IGetsValueToBeValidated"/> to make use of this behaviour,
    /// returning an appropriate instance of an object derived from <see cref="CSF.Validation.RuleExecution.GetValueToBeValidatedResponse"/>
    /// for the value accessor, and whether or not it raises an exception.
    /// </para>
    /// <para>
    /// There is more information available about these behaviours available in the
    /// <xref href="HandlingAccessorExceptions?text=Handling+accessor+exceptions"/> article.
    /// </para>
    /// <para>
    /// When the behaviour <see cref="TreatAsError"/> is indicated and when a value accessor throws an exception, an implementation of
    /// <see cref="CSF.Validation.RuleExecution.IGetsValueToBeValidated"/> should return an instance of
    /// <see cref="CSF.Validation.RuleExecution.ErrorGetValueToBeValidatedResponse"/>.
    /// </para>
    /// <para>
    /// This means that the accessor exception will be caught and added to the value-to-be-validated response (above).
    /// When a validator is performing validation, an additional validation result (not associated with any rule) should
    /// be added to the final results.  One such result should be added for each distinct instance of
    /// <see cref="CSF.Validation.RuleExecution.ErrorGetValueToBeValidatedResponse"/>.  That result will have a
    /// <see cref="RuleOutcome.Errored"/> and will indicate that the validation has failed with one or more errors.
    /// The result will also contain a reference to the original exception which was thrown by the accessor.
    /// </para>
    /// <para>
    /// When a value has an error-result in this way, no child values or collection item values should be evaluated from it.
    /// Additionally, no rules associated with that value will be executed, nor will any rules for any child values.
    /// </para>
    /// <para>
    /// Whilst overall validation will fail if an exception is thrown by this behaviour, because of the additional error
    /// result, the remaining validation will take place.  This means that the consumer of validation will still receive
    /// the most comprehensive validation result that is possible, with actual results for any rules which were able to run.
    /// </para>
    /// <para>
    /// When the behaviour <see cref="Throw"/> is indicated and when a value accessor throws an exception, an implementation of
    /// <see cref="CSF.Validation.RuleExecution.IGetsValueToBeValidated"/> should throw <see cref="ValidationException"/>
    /// with the original exception contained as the inner exception.
    /// This exception will then be thrown by the overall validation framework, so the consumer of validation must take
    /// responsibility for catching this validation exception.
    /// </para>
    /// <para>
    /// When the behaviour <see cref="Ignore"/> is indicated and when a value accessor throws an exception, an implementation of
    /// <see cref="CSF.Validation.RuleExecution.IGetsValueToBeValidated"/> should return an instance of
    /// <see cref="CSF.Validation.RuleExecution.IgnoredGetValueToBeValidatedResponse"/>.
    /// </para>
    /// <para>
    /// This means that the accessor exception will be caught but ignored.
    /// When a validator is performing validation, any value which has an ignored response is simply skipped, along
    /// with all of its child/collection values and all associated rules.  Value accessors which raise exceptions and
    /// which are ignored by this behaviour will not prevent the validation process from succeeding.
    /// </para>
    /// <para>
    /// The final validation result will include all other results which are unaffected by the value accessor that
    /// raised an exception.
    /// </para>
    /// <para>
    /// Use this behaviour with caution; it is usually a sign of poor object design if accessors such as getters raise
    /// exceptions. It is much better to create accessors which do not raise exceptions.
    /// </para>
    /// </remarks>
    /// <seealso cref="ManifestValue.AccessorExceptionBehaviour"/>
    /// <seealso cref="ValidationOptions.AccessorExceptionBehaviour"/>
    /// <seealso cref="CSF.Validation.RuleExecution.IGetsValueToBeValidated"/>
    /// <seealso cref="CSF.Validation.RuleExecution.IGetsAccessorExceptionBehaviour"/>
    public enum ValueAccessExceptionBehaviour
    {
        /// <summary>
        /// Continue validation of the overall object but add a validation result with an outcome of <see cref="RuleOutcome.Errored"/>
        /// to the overall validation result.  This is the default behaviour.
        /// </summary>
        TreatAsError = 0,

        /// <summary>
        /// Immediately throw the exception caused by the value accessor; validation will be halted with an exception.
        /// </summary>
        Throw,

        /// <summary>
        /// Similar to <see cref="TreatAsError"/>, but no additional error results will be added, meaning that overall
        /// validation may pass.
        /// </summary>
        Ignore
    }    
}
