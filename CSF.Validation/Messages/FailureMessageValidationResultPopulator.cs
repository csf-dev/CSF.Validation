using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A service which converts a <see cref="ValidationResult"/> into a <see cref="ValidationResult"/> by
    /// getting messages for each rule result within the original result and adding a message, where appropriate.
    /// </summary>
    public class FailureMessageValidationResultPopulator : IAddsFailureMessagesToResult
    {
        readonly IGetsRuleWithMessageProvider providerFactory;

        /// <inheritdoc/>
        public async Task<IQueryableValidationResult<TValidated>> GetResultWithMessagesAsync<TValidated>(IQueryableValidationResult<TValidated> result,
                                                                                                         ResolvedValidationOptions options,
                                                                                                         CancellationToken cancellationToken = default)
        {
            var resultsWithMessages = await GetRuleResultsWithMessagesAsync(result, options, cancellationToken).ConfigureAwait(false);
            return new ValidationResult<TValidated>(resultsWithMessages, ((ValidationResult<TValidated>) result).Manifest);
        }

        async Task<IEnumerable<ValidationRuleResult>> GetRuleResultsWithMessagesAsync(IQueryableValidationResult result,
                                                                                      ResolvedValidationOptions options,
                                                                                      CancellationToken cancellationToken)
        {
            var provider = providerFactory.GetRuleWithMessageProvider(options);
            var results = new List<ValidationRuleResult>();

            foreach (var ruleResult in result.RuleResults)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    results.Add(await provider.GetRuleResultWithMessageAsync(ruleResult, cancellationToken).ConfigureAwait(false));
                }
                catch(Exception e)
                {
                    if(options.TreatMessageGenerationErrorsAsRuleErrors)
                    {
                        var exception = new ValidationException(Resources.ExceptionMessages.GetExceptionMessage("ErrorGeneratingFailureMessage"), e);
                        var replacementRuleResult = new RuleResult(RuleOutcome.Errored, ruleResult.Data.ToDictionary(k => k.Key, v => v.Value), exception);
                        results.Add(new ValidationRuleResult(replacementRuleResult, ruleResult.RuleContext, ruleResult.ValidationLogic));
                    }
                    else
                    {
                        results.Add(ruleResult);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="FailureMessageValidationResultPopulator"/>.
        /// </summary>
        /// <param name="providerFactory">A rule/message provider factory.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="providerFactory"/> is <see langword="null" />.</exception>
        public FailureMessageValidationResultPopulator(IGetsRuleWithMessageProvider providerFactory)
        {
            this.providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
        }
    }
}