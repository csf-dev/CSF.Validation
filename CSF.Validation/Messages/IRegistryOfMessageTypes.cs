using System;
using System.Collections.Generic;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which may serve as a registry of the available <see cref="Type"/> of message-provider services.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A message provider service is class which implements one of the following interfaces.
    /// It is OK if a single class implements more of these interfaces.  It is also acceptable if it
    /// implements more than one closed generic form of either (or both) of the generic interfaces.
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="IGetsFailureMessage"/></description></item>
    /// <item><description><see cref="IGetsFailureMessage{TValidated}"/></description></item>
    /// <item><description><see cref="IGetsFailureMessage{TValidated, TParent}"/></description></item>
    /// </list>
    /// <para>
    /// The class may optionally also implement any (or all) of the following, again perhaps implementing multiple
    /// closed generic forms of the same generic interface an unlimited number of times.
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="IHasFailureMessageUsageCriteria"/></description></item>
    /// <item><description><see cref="IHasFailureMessageUsageCriteria{TValidated}"/></description></item>
    /// <item><description><see cref="IHasFailureMessageUsageCriteria{TValidated, TParent}"/></description></item>
    /// </list>
    /// <para>
    /// The implementation of this service should be registered in dependency injection as a singleton.
    /// </para>
    /// </remarks>
    public interface IRegistryOfMessageTypes
    {
        /// <summary>
        /// Add a number of types (of message provider classes) to this registry.
        /// </summary>
        /// <param name="types">The types to register.</param>
        void RegisterMessageProviderTypes(IEnumerable<Type> types);

        /// <summary>
        /// Gets a collection of message provider types from the registry which are candidates to
        /// provide a message for the specified validation rule result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method does not take into account the <see cref="IHasFailureMessageUsageCriteria.CanGetFailureMessage(ValidationRuleResult)"/>
        /// method, or any of the equivalent methods upon <see cref="IHasFailureMessageUsageCriteria{TValidated}"/> or
        /// <see cref="IHasFailureMessageUsageCriteria{TValidated, TParent}"/>.
        /// This method only filters/selects candidate types based upon the <see cref="FailureMessageStrategyAttribute"/> and the
        /// predicate values stored there.
        /// </para>
        /// <para>
        /// Further logic should be executed afterward to determine whether or not the type information provided is suitable to select
        /// as the message provider for the specified validation rule result.
        /// </para>
        /// </remarks>
        /// <param name="result">A validation rule result.</param>
        /// <returns>A collection of candidate message provider types and their priority.</returns>
        IEnumerable<MessageProviderTypeInfo> GetCandidateMessageProviderTypes(ValidationRuleResult result);
    }
}