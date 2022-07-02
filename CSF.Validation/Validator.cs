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
        readonly IGetsResolvedValidationOptions optionsResolver;
        readonly IGetsRuleExecutionContext contextFactory;

        /// <inheritdoc/>
        public Type ValidatedType => typeof(TValidated);

        /// <inheritdoc/>
        public async Task<IQueryableValidationResult<TValidated>> ValidateAsync(TValidated validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var resolvedOptions = optionsResolver.GetResolvedValidationOptions(options);
            var rules = ruleFactory.GetRulesWithDependencies(manifest.RootValue, validatedObject, resolvedOptions);
            var executor = await executorFactory.GetRuleExecutorAsync(resolvedOptions, cancellationToken).ConfigureAwait(false);
            var context = contextFactory.GetExecutionContext(rules, resolvedOptions);
            var ruleResults = await executor.ExecuteAllRulesAsync(context, cancellationToken).ConfigureAwait(false);

            return new ValidationResult<TValidated>(ruleResults, manifest);
        }

        async Task<ValidationResult> IValidator.ValidateAsync(object validatedObject, ValidationOptions options, CancellationToken cancellationToken)
        {
            var result = await ValidateAsync((TValidated)validatedObject, options, cancellationToken).ConfigureAwait(false);
            return (ValidationResult) result;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Validator{TValidated}"/>.
        /// </summary>
        /// <param name="manifest">A validation manifest from which to creator the validator.</param>
        /// <param name="executorFactory">The rule-executor factory.</param>
        /// <param name="ruleFactory">The rule factory.</param>
        /// <param name="optionsResolver">An options resolver.</param>
        /// <param name="contextFactory">A rule execution context factory.</param>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public Validator(ValidationManifest manifest,
                         IGetsRuleExecutor executorFactory,
                         IGetsAllExecutableRulesWithDependencies ruleFactory,
                         IGetsResolvedValidationOptions optionsResolver,
                         IGetsRuleExecutionContext contextFactory)
        {
            this.manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            this.executorFactory = executorFactory ?? throw new ArgumentNullException(nameof(executorFactory));
            this.ruleFactory = ruleFactory ?? throw new ArgumentNullException(nameof(ruleFactory));
            this.optionsResolver = optionsResolver ?? throw new ArgumentNullException(nameof(optionsResolver));
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }
    }
}