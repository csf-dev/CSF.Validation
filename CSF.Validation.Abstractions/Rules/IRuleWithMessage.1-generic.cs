using CSF.Validation.Messages;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An object which contains both validation rule logic and also can provide a human-readable failure message.
    /// </summary>
    /// <remarks>
    /// <para>
    /// You are encouraged to read more about how validation message providers are used and selected
    /// at <xref href="WritingMessageProviders?text=the+documentation+relating+to+message+providers"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="IGetsFailureMessage{TValidated, TParent}"/>
    /// <seealso cref="IGetsFailureMessage{TValidated}"/>
    /// <seealso cref="IGetsFailureMessage"/>
    /// <seealso cref="IHasFailureMessageUsageCriteria"/>
    /// <seealso cref="IHasFailureMessageUsageCriteria{TValidated}"/>
    /// <seealso cref="IHasFailureMessageUsageCriteria{TValidated, TParent}"/>
    /// <seealso cref="IRuleWithMessage{TValidated, TParent}"/>
    /// <seealso cref="FailureMessageStrategyAttribute"/>
    /// <typeparam name="TValidated">The validated type.</typeparam>
    public interface IRuleWithMessage<in TValidated> : IRule<TValidated>, IGetsFailureMessage<TValidated> {}
}