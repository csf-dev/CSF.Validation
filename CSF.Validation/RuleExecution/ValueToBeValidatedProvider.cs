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
        /// <summary>
        /// Attempts to get the value to be validatded and returns a value indicating whether or not this was successful.
        /// </summary>
        /// <param name="manifestValue">The manifest value describing the value.</param>
        /// <param name="parentValue">The previous/parent value, from which the validated value should be derived.</param>
        /// <param name="validationOptions">Validation options.</param>
        /// <param name="valueToBeValidated">If this method returns <see langword="true" /> then this parameter
        /// exposes the validated value.  This parameter must be ignored if the method returns <see langword="false" />.</param>
        /// <returns><see langword="true" /> if getting the validated value is a success; <see langword="false" /> otherwise.</returns>
        public bool TryGetValueToBeValidated(ManifestValue manifestValue,
                                             object parentValue,
                                             ValidationOptions validationOptions,
                                             out object valueToBeValidated)
        {
            valueToBeValidated = null;
            if(parentValue is null) return false;

            try
            {
                valueToBeValidated = manifestValue.AccessorFromParent(parentValue);
                return true;
            }
            catch(Exception e)
            {
                if(!manifestValue.IgnoreAccessorExceptions && !validationOptions.IgnoreValueAccessExceptions)
                {
                    var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("ErrorAccessingValue"),
                                                manifestValue,
                                                parentValue);
                    throw new ValidationException(message, e);
                }

                valueToBeValidated = GetDefaultOfType(manifestValue.ValidatedType);
                return false;
            }
        }

        static object GetDefaultOfType(Type t) => t.GetTypeInfo().IsValueType ? Activator.CreateInstance(t) : null;
    }
}