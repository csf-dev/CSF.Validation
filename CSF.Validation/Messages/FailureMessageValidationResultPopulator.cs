using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A service which converts a <see cref="ValidationResult"/> into a <see cref="ValidationResult"/> by
    /// getting messages for each rule result within the original result and adding a message, where appropriate.
    /// </summary>
    public class FailureMessageValidationResultPopulator : IAddsFailureMessagesToResult
    {
        readonly IGetsFailureMessageProvider messageProviderFactory;

        /// <inheritdoc/>
        public async Task<IQueryableValidationResult<TValidated>> GetResultWithMessagesAsync<TValidated>(IQueryableValidationResult<TValidated> result, CancellationToken cancellationToken = default)
        {
            var resultsWithMessages = await GetRuleResultsWithMessagesAsync(result, cancellationToken).ConfigureAwait(false);
            return new ValidationResult<TValidated>(resultsWithMessages, ((ValidationResult<TValidated>) result).Manifest);
        }

        async Task<IEnumerable<ValidationRuleResult>> GetRuleResultsWithMessagesAsync(IQueryableValidationResult result, CancellationToken cancellationToken)
        {
            var results = new List<ValidationRuleResult>();

            foreach (var ruleResult in result.RuleResults)
            {
                cancellationToken.ThrowIfCancellationRequested();
                results.Add(await GetRuleResultWithMessageAsync(ruleResult, cancellationToken).ConfigureAwait(false));
            }

            return results;
        }

        async Task<ValidationRuleResult> GetRuleResultWithMessageAsync(ValidationRuleResult ruleResult, CancellationToken cancellationToken)
        {
            var messageProvider = messageProviderFactory.GetProvider(ruleResult);
            var message = (messageProvider is null)? null : await messageProvider.GetFailureMessageAsync(ruleResult, cancellationToken).ConfigureAwait(false);
            return new ValidationRuleResult(ruleResult, ruleResult.RuleContext, ruleResult.ValidationLogic, message);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="FailureMessageValidationResultPopulator"/>.
        /// </summary>
        /// <param name="messageProviderFactory"></param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="messageProviderFactory"/> is <see langword="null" />.</exception>
        public FailureMessageValidationResultPopulator(IGetsFailureMessageProvider messageProviderFactory)
        {
            this.messageProviderFactory = messageProviderFactory ?? throw new System.ArgumentNullException(nameof(messageProviderFactory));
        }
    }
}