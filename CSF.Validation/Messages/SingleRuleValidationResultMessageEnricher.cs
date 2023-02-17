using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// Default implementation of <see cref="IGetsValidationRuleResultWithMessage"/>.
    /// </summary>
    public class SingleRuleValidationResultMessageEnricher : IGetsValidationRuleResultWithMessage
    {
        static readonly RuleOutcome[] outcomesWhichDontGetMessages = { RuleOutcome.Passed, RuleOutcome.DependencyFailed };

        readonly IGetsFailureMessageProvider messageProviderFactory;

        /// <inheritdoc/>
        public ValueTask<ValidationRuleResult> GetRuleResultWithMessageAsync(ValidationRuleResult ruleResult, CancellationToken cancellationToken = default)
        {
            if(outcomesWhichDontGetMessages.Contains(ruleResult.Outcome))
                return new ValueTask<ValidationRuleResult>(ruleResult);

            var messageProvider = messageProviderFactory.GetProvider(ruleResult);
            if(messageProvider is null) return new ValueTask<ValidationRuleResult>(ruleResult);

            return GetRuleResultWithMessagePrivateAsync(ruleResult, messageProvider, cancellationToken);
        }

        static async ValueTask<ValidationRuleResult> GetRuleResultWithMessagePrivateAsync(ValidationRuleResult ruleResult,
                                                                                     IGetsFailureMessage messageProvider,
                                                                                     CancellationToken cancellationToken)
        {
            var message = await messageProvider.GetFailureMessageAsync(ruleResult, cancellationToken).ConfigureAwait(false);
            return new ValidationRuleResult(ruleResult, ruleResult.RuleContext, ruleResult.ValidationLogic, message);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="SingleRuleValidationResultMessageEnricher"/>.
        /// </summary>
        /// <param name="messageProviderFactory">A factory for message providers.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="messageProviderFactory"/> is <see langword="null" />.</exception>
        public SingleRuleValidationResultMessageEnricher(IGetsFailureMessageProvider messageProviderFactory)
        {
            this.messageProviderFactory = messageProviderFactory ?? throw new System.ArgumentNullException(nameof(messageProviderFactory));
        }
    }
}