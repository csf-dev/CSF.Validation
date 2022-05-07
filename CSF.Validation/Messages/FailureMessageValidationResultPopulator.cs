using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A service which converts a <see cref="ValidationResult"/> into a <see cref="ValidationResultWithMessages"/> by
    /// getting messages for each rule result within the original result and adding a message, where appropriate.
    /// </summary>
    public class FailureMessageValidationResultPopulator : IAddsFailureMessagesToResult
    {
        readonly IGetsFailureMessageProvider messageProviderFactory;

        /// <inheritdoc/>
        public async Task<ValidationResultWithMessages> GetResultWithMessagesAsync(ValidationResult result, CancellationToken cancellationToken = default)
        {
            var resultsWithMessages = await GetRuleResultsWithMessagesAsync(result, cancellationToken);
            return new ValidationResultWithMessages(resultsWithMessages);
        }

        async Task<IEnumerable<ValidationRuleResultWithMessage>> GetRuleResultsWithMessagesAsync(ValidationResult result, CancellationToken cancellationToken)
        {
            var results = new List<ValidationRuleResultWithMessage>();

            foreach (var ruleResult in result.RuleResults)
                results.Add(await GetRuleResultWithMessageAsync(ruleResult, cancellationToken));

            return results;
        }

        async Task<ValidationRuleResultWithMessage> GetRuleResultWithMessageAsync(ValidationRuleResult ruleResult, CancellationToken cancellationToken)
        {
            var messageProvider = messageProviderFactory.GetProvider(ruleResult);
            var message = (messageProvider is null)? null : await messageProvider.GetFailureMessageAsync(ruleResult, cancellationToken);
            return new ValidationRuleResultWithMessage(ruleResult, message);
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