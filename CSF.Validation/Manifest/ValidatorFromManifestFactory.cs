using System;
using System.Reflection;
using CSF.Validation.RuleExecution;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A factory service which gets a validator from a validation manifest.
    /// </summary>
    public class ValidatorFromManifestFactory : IGetsValidatorFromManifest
    {
        readonly IServiceProvider resolver;

        /// <summary>
        /// Gets a validator from a validation manifest.
        /// </summary>
        /// <param name="manifest">The validation manifest.</param>
        /// <returns>A validator.</returns>
        public IValidator GetValidator(ValidationManifest manifest)
        {
            if (manifest is null)
                throw new ArgumentNullException(nameof(manifest));

            var method = GetType().GetTypeInfo().GetDeclaredMethod(nameof(GetValidatorPrivate)).MakeGenericMethod(manifest.ValidatedType);
            return (IValidator) method.Invoke(this, new [] { manifest });
        }

        IValidator<TValidated> GetValidatorPrivate<TValidated>(ValidationManifest manifest)
        {
            return new Validator<TValidated>(manifest,
                                             resolver.GetRequiredService<IGetsRuleExecutor>(),
                                             resolver.GetRequiredService<IGetsAllExecutableRulesWithDependencies>());
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorFromManifestFactory"/>.
        /// </summary>
        /// <param name="resolver">A DI resolver.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="resolver"/> is <see langword="null" />.</exception>
        public ValidatorFromManifestFactory(IServiceProvider resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }
    }
}