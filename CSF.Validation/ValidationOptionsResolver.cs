using Microsoft.Extensions.Options;

namespace CSF.Validation
{
    /// <summary>
    /// A service which gets the resolved validation options.
    /// </summary>
    /// <remarks>
    /// <para>
    /// As you will notice, the <see cref="ResolvedValidationOptions"/> class has the same 'shape' as
    /// <see cref="ValidationOptions"/>, except that every property is non-nullable.
    /// The resolved options are created by taking the first non-null value from each of:
    /// </para>
    /// <list type="number">
    /// <item><description>The specified options</description></item>
    /// <item><description>The default options configured at DI registration</description></item>
    /// <item><description>Hard-coded default options (if nothing else is specified)</description></item>
    /// </list>
    /// </remarks>
    public class ValidationOptionsResolver : IGetsResolvedValidationOptions
    {
        static readonly ResolvedValidationOptions hardcodedDefaults = new ResolvedValidationOptions
        {
            RuleThrowingBehaviour = RuleThrowingBehaviour.OnError,
            AccessorExceptionBehaviour = Manifest.ValueAccessExceptionBehaviour.TreatAsError,
            EnableMessageGeneration = false,
        };

        readonly IOptions<ValidationOptions> defaultOptions;

        /// <inheritdoc/>
        public ResolvedValidationOptions GetResolvedValidationOptions(ValidationOptions specifiedOptions)
        {
            return new ResolvedValidationOptions
            {
                RuleThrowingBehaviour = specifiedOptions?.RuleThrowingBehaviour
                                            ?? defaultOptions.Value.RuleThrowingBehaviour
                                            ?? hardcodedDefaults.RuleThrowingBehaviour,
                AccessorExceptionBehaviour = specifiedOptions?.AccessorExceptionBehaviour
                                            ?? defaultOptions.Value.AccessorExceptionBehaviour
                                            ?? hardcodedDefaults.AccessorExceptionBehaviour,
                EnableMessageGeneration = specifiedOptions?.EnableMessageGeneration
                                            ?? defaultOptions.Value.EnableMessageGeneration
                                            ?? hardcodedDefaults.EnableMessageGeneration,
            };
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidationOptionsResolver"/>.
        /// </summary>
        /// <param name="defaultOptions">The default options, possibly configured during DI setup.</param>
        public ValidationOptionsResolver(IOptions<ValidationOptions> defaultOptions)
        {
            this.defaultOptions = defaultOptions;
        }
    }
}