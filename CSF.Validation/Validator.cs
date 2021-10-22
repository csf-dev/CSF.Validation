using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.RuleExecution;

namespace CSF.Validation
{
    /// <summary>
    /// The validator service, which validates instances of <typeparamref name="TValidated"/>
    /// and returns a result.
    /// </summary>
    /// <typeparam name="TValidated">The type of the object which is validated.</typeparam>
    public class Validator<TValidated> : IValidator<TValidated>, IValidator
    {
        readonly ValidationManifest manifest;
        readonly IGetsRuleExecutor executorFactory;
        readonly IGetsAllExecutableRulesWithDependencies ruleFactory;

        /// <summary>
        /// Validate the specified object instance asynchronously and get a validation result.
        /// </summary>
        /// <param name="validatedObject">The object to be validated.</param>
        /// <param name="options">An optional object containing configuration options related to the validation process.</param>
        /// <param name="cancellationToken">An optional object which enables premature cancellation of the validation process.</param>
        /// <returns>A task containing the result of the validation process.</returns>
        /// <exception cref="ValidationException">
        /// If the validation process fails or errors and the <see cref="ValidationOptions.RuleThrowingBehaviour"/>
        /// of the <paramref name="options"/> indicate that an exception should be thrown.
        /// </exception>
        public async Task<ValidationResult> ValidateAsync(TValidated validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var rules = ruleFactory.GetRulesWithDependencies(manifest.RootValue, validatedObject);
            var executor = await executorFactory.GetRuleExecutorAsync(options, cancellationToken).ConfigureAwait(false);
            var ruleResults = await executor.ExecuteAllRulesAsync(rules, cancellationToken).ConfigureAwait(false);

            return new ValidationResult(ruleResults);
        }

        Task<ValidationResult> IValidator.ValidateAsync(object validatedObject, ValidationOptions options, CancellationToken cancellationToken)
            => ValidateAsync((TValidated)validatedObject, options, cancellationToken);

        /// <summary>
        /// Initialises a new instance of <see cref="Validator{TValidated}"/>.
        /// </summary>
        /// <param name="manifest">A validation manifest from which to creator the validator.</param>
        /// <param name="executorFactory">The rule-executor factory.</param>
        /// <param name="ruleFactory">The rule factory.</param>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public Validator(ValidationManifest manifest, IGetsRuleExecutor executorFactory, IGetsAllExecutableRulesWithDependencies ruleFactory)
        {
            this.manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            this.executorFactory = executorFactory ?? throw new ArgumentNullException(nameof(executorFactory));
            this.ruleFactory = ruleFactory ?? throw new ArgumentNullException(nameof(ruleFactory));
        }
    }
}