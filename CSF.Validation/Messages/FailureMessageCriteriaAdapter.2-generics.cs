using System.Linq;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// Adapter class which allows a <see cref="IHasFailureMessageUsageCriteria{TValidated}"/> to be used
    /// as a <see cref="IHasFailureMessageUsageCriteria"/>.
    /// </summary>
    /// <typeparam name="TValidated">The validated type.</typeparam>
    /// <typeparam name="TParent">The parent validated type.</typeparam>
    public class FailureMessageCriteriaAdapter<TValidated,TParent> : IHasFailureMessageUsageCriteria
    {
        readonly IHasFailureMessageUsageCriteria<TValidated,TParent> wrapped;

        /// <summary>
        /// Gets a value which indicates whether or not the current class may be used to provide a failure message
        /// for the specified validation rule result.
        /// </summary>
        /// <param name="result">A validation rule result.</param>
        /// <returns><see langword="true" /> if the current instance may provide a message for the result;
        /// <see langword="false" /> otherwise.</returns>
        public bool CanGetFailureMessage(ValidationRuleResult result)
            => wrapped.CanGetFailureMessage((TValidated)result.ValidatedValue,
                                            (TParent) result.RuleContext.AncestorContexts.FirstOrDefault()?.ActualValue,
                                            result);

        /// <summary>
        /// Initialises a new instance of <see cref="FailureMessageCriteriaAdapter{TValidated,TParent}"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped service.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="wrapped"/> is <see langword="null" />.</exception>
        public FailureMessageCriteriaAdapter(IHasFailureMessageUsageCriteria<TValidated,TParent> wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
        }
    }
}