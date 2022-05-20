using System;
using System.Reflection;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A service which gets the value to be validated: the "actual" value.
    /// </summary>
    public class ValueToBeValidatedProvider : IGetsValueToBeValidated
    {
        readonly IGetsAccessorExceptionBehaviour behaviourProvider;

        /// <inheritdoc/>
        public GetValueToBeValidatedResponse GetValueToBeValidated(ManifestValue manifestValue, object parentValue, ValidationOptions validationOptions)
        {
            if(parentValue is null) return IgnoredGetValueToBeValidatedResponse.Default;

            try
            {
                var valueToBeValidated = manifestValue.AccessorFromParent(parentValue);
                return new SuccessfulGetValueToBeValidatedResponse(valueToBeValidated);
            }
            catch(Exception e)
            {
                var behaviour = behaviourProvider.GetBehaviour(manifestValue, validationOptions);
                if(behaviour == ValueAccessExceptionBehaviour.Throw)
                {
                    var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("ErrorAccessingValue"),
                                                manifestValue,
                                                parentValue);
                    throw new ValidationException(message, e);
                }
                else if(behaviour == ValueAccessExceptionBehaviour.TreatAsError)
                    return new ErrorGetValueToBeValidatedResponse(e);

                return IgnoredGetValueToBeValidatedResponse.Default;
            }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValueToBeValidatedProvider"/>.
        /// </summary>
        /// <param name="behaviourProvider">The exception behaviour provider.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="behaviourProvider"/> is <see langword="null" />.</exception>
        public ValueToBeValidatedProvider(IGetsAccessorExceptionBehaviour behaviourProvider)
        {
            this.behaviourProvider = behaviourProvider ?? throw new ArgumentNullException(nameof(behaviourProvider));
        }
    }
}