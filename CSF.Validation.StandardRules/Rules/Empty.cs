using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the value being validated is either <see langword="null" /> or is a collection or
    /// a string that has no items/is empty.
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
    /// returns <see langword="false" />.  
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
    /// Note that when using this class to provide a failure message for an <see cref="IEnumerable"/>, the failure message will not
    /// indicate the actual count of elements exposed by the enumerable instance.  This is for both performance reasons and because the
    /// <see cref="IEnumerable"/> interface does not guarantee that the collection is finite.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class Empty : IRuleWithMessage<ICollection>, IRuleWithMessage<IEnumerable>, IRuleWithMessage<Array>, IRuleWithMessage<string>
    {
        internal const string CountKey = "Count";

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ICollection validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            var data = new Dictionary<string, object> { { Empty.CountKey, validated.Count } };
            return validated.Count == 0 ? PassAsync() : FailAsync(data);
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(IEnumerable validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.GetEnumerator().MoveNext() ? FailAsync() : PassAsync();
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(Array validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            var data = new Dictionary<string, object> { { Empty.CountKey, validated.Length } };
            return validated.Length == 0 ? PassAsync() : FailAsync(data);
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            var data = new Dictionary<string, object> { { Empty.CountKey, validated.Length } };
            return validated.Length == 0 ? PassAsync() : FailAsync(data);
        }

        internal static string GetFailureMessage(ValidationRuleResult result)
        {
            if(!result.Data.TryGetValue(CountKey, out var countObj))
                return Resources.FailureMessages.GetFailureMessage("EmptyWithNoCount");

            var count = (int) countObj;
            return count == 1
                ? Resources.FailureMessages.GetFailureMessage("EmptyWithCountOne")
                : String.Format(Resources.FailureMessages.GetFailureMessage("EmptyWithCount"), count);
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ICollection value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(GetFailureMessage(result));

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(Array value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(GetFailureMessage(result));

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(String.Format(Resources.FailureMessages.GetFailureMessage("EmptyString"), value));

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(IEnumerable value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(GetFailureMessage(result));
    }
}