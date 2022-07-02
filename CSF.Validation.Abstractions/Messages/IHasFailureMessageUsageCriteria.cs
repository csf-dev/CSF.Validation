namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which can use custom logic to determine whether it may be used to provide a failure message.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface is only useful when combined with one of the failure-message-provision interfaces:
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="IGetsFailureMessage"/></description></item>
    /// <item><description><see cref="IGetsFailureMessage{TValidated}"/></description></item>
    /// <item><description><see cref="IGetsFailureMessage{TValidated, TParent}"/></description></item>
    /// </list>
    /// <para>
    /// Generally, using <see cref="FailureMessageStrategyAttribute"/> is a superior and more performant mechanism
    /// for selecting an appropriate message-provider class.  However, for situations where that attribute alone is
    /// insufficent and custom/arbitrary logic must be used to determine whether a class may provide a message or not,
    /// this interface may be implemented and the <see cref="CanGetFailureMessage(ValidationRuleResult)"/> method used
    /// to provide that logic.
    /// </para>
    /// <para>
    /// You are encouraged to read more about how validation message providers are used and selected
    /// at <xref href="WritingMessageProviders?text=the+documentation+relating+to+message+providers"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="IGetsFailureMessage{TValidated, TParent}"/>
    /// <seealso cref="IGetsFailureMessage{TValidated}"/>
    /// <seealso cref="IGetsFailureMessage"/>
    /// <seealso cref="IHasFailureMessageUsageCriteria{TValidated}"/>
    /// <seealso cref="IHasFailureMessageUsageCriteria{TValidated, TParent}"/>
    /// <seealso cref="Rules.IRuleWithMessage{TValidated}"/>
    /// <seealso cref="Rules.IRuleWithMessage{TValidated, TParent}"/>
    /// <seealso cref="FailureMessageStrategyAttribute"/>
    public interface IHasFailureMessageUsageCriteria
    {
        /// <summary>
        /// Gets a value which indicates whether or not the current class may be used to provide a failure message
        /// for the specified validation rule result.
        /// </summary>
        /// <param name="result">A validation rule result.</param>
        /// <returns><see langword="true" /> if the current instance may provide a message for the result;
        /// <see langword="false" /> otherwise.</returns>
        bool CanGetFailureMessage(ValidationRuleResult result);
    }
}