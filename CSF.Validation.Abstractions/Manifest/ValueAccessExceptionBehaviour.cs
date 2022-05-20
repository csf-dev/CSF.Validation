
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
    /// </remarks>
    public enum ValueAccessExceptionBehaviour
    {
        /// <summary>
        /// Continue validation of the overall object but add a validation result with an outcome of <see cref="RuleOutcome.Errored"/>
        /// to represent this error.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will result in the overall validation failing, but the immediate validation process will continue, so that the consumer
        /// will receive an otherwise-complete validation result.
        /// </para>
        /// </remarks>
        TreatAsError = 0,

        /// <summary>
        /// Immediately throw the exception caught by the value accessor, and allow validation to be halted with an uncaught exception.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will halt validation immediately and consumers will not receive a validation result.  They will need to catch the exception
        /// raised by the validation process.
        /// </para>
        /// </remarks>
        Throw,

        /// <summary>
        /// Ignore the exception and do not add any error or failure to the validation result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This behaviour allows validation to continue, ignoring the exception which occurred in the value accessor.
        /// Validation rules for that value, as well as any further derived values, will not be executed.
        /// </para>
        /// </remarks>
        Ignore
    }    
}
