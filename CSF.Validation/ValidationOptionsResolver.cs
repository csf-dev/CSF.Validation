using System;
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
        static readonly ResolvedValidationOptions hardcodedDefaults = new ResolvedValidationOptions();

        readonly IOptions<ValidationOptions> defaultOptions;

        /// <inheritdoc/>
        public ResolvedValidationOptions GetResolvedValidationOptions(ValidationOptions specifiedOptions)
        {
            return new ResolvedValidationOptions
            {
                RuleThrowingBehaviour = GetEffectiveValue(x => x.RuleThrowingBehaviour, specifiedOptions, hardcodedDefaults.RuleThrowingBehaviour),
                AccessorExceptionBehaviour = GetEffectiveValue(x => x.AccessorExceptionBehaviour, specifiedOptions, hardcodedDefaults.AccessorExceptionBehaviour),
                EnableMessageGeneration = GetEffectiveValue(x => x.EnableMessageGeneration, specifiedOptions, hardcodedDefaults.EnableMessageGeneration),
                EnableRuleParallelization = GetEffectiveValue(x => x.EnableRuleParallelization, specifiedOptions, hardcodedDefaults.EnableRuleParallelization),
            };
        }

        /// <summary>
        /// Gets the effective value of an options, either from the specified options, from the <see cref="defaultOptions"/> or
        /// from the <see cref="hardcodedDefaults"/>.  The first non-null value found is used.
        /// </summary>
        /// <typeparam name="T">The type of option value.</typeparam>
        /// <param name="getter">A getter func</param>
        /// <param name="specifiedOptions">The specified options</param>
        /// <param name="hardcodedDefault">The hard-coded default value</param>
        /// <returns>The effective option value</returns>
        T GetEffectiveValue<T>(Func<ValidationOptions, Nullable<T>> getter,
                               ValidationOptions specifiedOptions,
                               T hardcodedDefault)
                where T : struct
        {
            if(!(specifiedOptions is null))
            {
                var specifiedValue = getter(specifiedOptions);
                if(specifiedValue.HasValue) return specifiedValue.Value;
            }
            
            if(!(defaultOptions.Value is null))
            {
                var defaultValue = getter(defaultOptions.Value);
                if(defaultValue.HasValue) return defaultValue.Value;
            }

            return hardcodedDefault;
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