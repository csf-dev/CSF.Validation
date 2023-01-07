using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which gets the value to be validated: the "actual" value.
    /// </summary>
    public interface IGetsValueToBeValidated
    {
        /// <summary>
        /// Attempts to get the value to be validatded from the specified manifest value and parent value instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method returns a result object indicating the outcome of the attempt.  Three different concrete classes
        /// may be returned by this method.
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="SuccessfulGetValueToBeValidatedResponse"/> if the attempt is a success.</description></item>
        /// <item><description><see cref="ErrorGetValueToBeValidatedResponse"/> if the attempt failed and this should result in an error being added to the validation result.</description></item>
        /// <item><description><see cref="IgnoredGetValueToBeValidatedResponse"/> if the attempt failed but this should be silently ignored.</description></item>
        /// </list>
        /// <para>
        /// This method might also raise a <see cref="ValidationException"/> if the attempt fails and the error-handling
        /// behaviour (decided between the manifest value and validation options) is <see cref="ValueAccessExceptionBehaviour.Throw"/>.
        /// </para>
        /// </remarks>
        /// <param name="manifestValue">The manifest value describing the value.</param>
        /// <param name="parentValue">The previous/parent value, from which the validated value should be derived.</param>
        /// <param name="validationOptions">Validation options.</param>
        /// <returns>A result object which indicates success/failure and provides further contextual information.</returns>
        /// <exception cref="ValidationException">If the attempt fails and the error-handling behaviour is <see cref="ValueAccessExceptionBehaviour.Throw"/>.</exception>
        GetValueToBeValidatedResponse GetValueToBeValidated(ManifestValue manifestValue,
                                                            object parentValue,
                                                            ResolvedValidationOptions validationOptions);
    }
}