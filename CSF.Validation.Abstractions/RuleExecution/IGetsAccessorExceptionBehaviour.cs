using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which can get the behaviour to use for unhandled exceptions thrown by accessor functions.
    /// </summary>
    public interface IGetsAccessorExceptionBehaviour
    {
        /// <summary>
        /// Gets the behaviour to use for a specified manifest value.
        /// </summary>
        /// <param name="manifestValue">The manifest value for which behaviour is required.</param>
        /// <param name="validationOptions">The validation options.</param>
        /// <returns>The exception behaviour to use.</returns>
        ValueAccessExceptionBehaviour GetBehaviour(ManifestValue manifestValue, ResolvedValidationOptions validationOptions);
    }
}