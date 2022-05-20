using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A service which resolves the effective accessor-exception-handling behaviour.
    /// </summary>
    public class ValueAccessExceptionBehaviourProvider : IGetsAccessorExceptionBehaviour
    {
        /// <inheritdoc/>
        public ValueAccessExceptionBehaviour GetBehaviour(ManifestValue manifestValue, ValidationOptions validationOptions)
        {
            return manifestValue.AccessorExceptionBehaviour.HasValue
                ? manifestValue.AccessorExceptionBehaviour.Value
                : validationOptions.AccessorExceptionBehaviour;
        }
    }
}