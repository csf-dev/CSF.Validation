using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSF.Validation
{
    /// <summary>
    /// An object which can query for specific validation rule results matching specified predicates.
    /// </summary>
    public interface IQueryableValidationResult
    {
        /// <summary>
        /// Queries for validation rule results which validate a specified member of an object
        /// and which optionally also match all other specified predicates.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is used by specifying the validated-object-type which has the member (by which you wish to query) as the generic
        /// type parameter for this method, and then using an expression to select the member.
        /// </para>
        /// <para>
        /// Any predicate parameters which are not specified or have a <see langword="null"/> value, apart from the
        /// <paramref name="memberAccessor"/> which is mandatory in this method, are ignored.  Any which are specified
        /// with a non-null value are used to filter the validation rule results.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// If you wished to query for rules for the <c>Age</c> property of a validated <c>Pet</c> instance, then you
        /// would use this method like so.
        /// </para>
        /// <code>
        /// var results = queryableResult.QueryByMember&lt;Pet&gt;(x => x.Age);
        /// </code>
        /// </example>
        /// <typeparam name="TValidated">The validated-object-type from which the member is available.</typeparam>
        /// <param name="memberAccessor">An expression indicating the member for which rules are to be queried.</param>
        /// <param name="objectIdentity">An optional object identity predicate.</param>
        /// <param name="ruleTypeName">An optional validation-rule-type name predicate.</param>
        /// <param name="ruleName">An optional rule-name predicate.</param>
        /// <returns>A collection of the validation rule results for the specified member, which also match all of the provided predicate values.</returns>
        IEnumerable<ValidationRuleResult> QueryByMember<TValidated>(Expression<Func<TValidated, object>> memberAccessor,
                                                                    object objectIdentity = null,
                                                                    string ruleTypeName = null,
                                                                    string ruleName = null);

        /// <summary>
        /// Queries for validation rule results which match all of the specified predicate values.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Any predicate parameters which are not specified or have a <see langword="null"/> value, are ignored.
        /// Any which are specified with a non-null value are used to filter the validation rule results.
        /// This means that <c>Query()</c> on its own with no parameters specified will return all of the validation
        /// rule results with no filtering.
        /// </para>
        /// </remarks>
        /// <param name="objectIdentity">An optional object identity predicate.</param>
        /// <param name="ruleTypeName">An optional validation-rule-type name predicate.</param>
        /// <param name="ruleName">An optional rule-name predicate.</param>
        /// <returns>A collection of the validation rule results which match all of the provided predicate values.</returns>
        IEnumerable<ValidationRuleResult> Query(object objectIdentity = null,
                                                string ruleTypeName = null,
                                                string ruleName = null);
    }
}