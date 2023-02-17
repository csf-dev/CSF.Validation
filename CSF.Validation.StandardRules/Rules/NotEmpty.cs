using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the value being validated is either <see langword="null" /> or is a collection or
    /// a string that has at least one item/is not empty.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For a validated object which implements the non-generic <see cref="ICollection"/>, this validation rule
    /// makes use of the <see cref="ICollection.Count"/> property.
    /// For a validated object which derives from <see cref="Array"/> or is a <see cref="String"/>, this validation rule makes use of the
    /// <see cref="Array.Length"/> or <see cref="String.Length"/> property.
    /// </para>
    /// <para>
    /// For an object which is simply <see cref="IEnumerable"/>, this validation rule will get and make use of the enumerator once,
    /// attempting to read a single item from the enumeration.  This rule will return a pass result if <see cref="IEnumerator.MoveNext"/>
    /// returns <see langword="true" />.  
    /// </para>
    /// <para>
    /// In some cases enumerating an enumerable in this way will not have undesirable consequences; for some implementations of
    /// <see cref="IEnumerable"/> though, it could.
    /// By way of an example of a common mistake, consider when using an <see cref="System.Linq.IQueryable{T}"/> which comes from an ORM such
    /// as Entity Framework or NHibernate.
    /// Whilst the generic queryable interface does implement the non-generic enumerable interface and this rule would
    /// operate with the correct results, it is not recommended to use it in this way.
    /// If a Linq queryable which comes from an ORM is treated as <see cref="IEnumerable"/>, enumerating it will cause that ORM to
    /// execute the full query in the database and load/fetch all of the results, similar to a <c>SELECT * FROM ...</c>.  This could
    /// incur significant performance degradation.  
    /// </para>
    /// <para>
    /// If treating the validated value as non-generic <see cref="IEnumerable"/> and enumerating it is unacceptable, it is
    /// recommended to use the generic <see cref="Empty{T}"/> rule instead.
    /// The generic rule works with <see cref="System.Linq.IQueryable{T}"/> in a way that avoids enumerating it as described above
    /// and thus provides superior performance when used with an ORM.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class NotEmpty : IRuleWithMessage<ICollection>, IRuleWithMessage<IEnumerable>, IRuleWithMessage<Array>, IRuleWithMessage<string>
    {
        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ICollection validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Count > 0 ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(IEnumerable validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.GetEnumerator().MoveNext() ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(Array validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Length > 0 ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Length > 0 ? PassAsync() : FailAsync();
        }

        static ValueTask<string> GetFailureMessageAsync() => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NotEmpty"));

        ValueTask<string> IGetsFailureMessage<string>.GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync();

        ValueTask<string> IGetsFailureMessage<Array>.GetFailureMessageAsync(Array value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync();

        ValueTask<string> IGetsFailureMessage<IEnumerable>.GetFailureMessageAsync(IEnumerable value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync();

        ValueTask<string> IGetsFailureMessage<ICollection>.GetFailureMessageAsync(ICollection value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync();
    }
}