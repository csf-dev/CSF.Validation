using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the value being validated is either <see langword="null" /> or is a collection
    /// that has at least one item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For a validated object which implements <see cref="ICollection"/> or <see cref="ICollection{T}"/>, this validation rule
    /// makes use of the <see cref="ICollection.Count"/> or <see cref="ICollection{T}.Count"/> property.
    /// </para>
    /// <para>
    /// For a validated object which derives from <see cref="Array"/>, this validation rule makes use of the
    /// <see cref="Array.Length"/> property.
    /// </para>
    /// <para>
    /// For an object which is simply <see cref="IEnumerable"/>, this validation rule will use the enumerator at least once
    /// in order to attempt to read a single item from the object.  In many cases, enumerating the object will not have any
    /// adverse side-effects but for some implementations of <see cref="IEnumerable"/>, this could incur unwanted consequences.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    public class NotEmpty : IRule<ICollection>, IRule<IEnumerable>, IRule<ICollection<object>>, IRule<Array>
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(ICollection validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Count > 0 ? PassAsync() : FailAsync();
        }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(ICollection<object> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Count > 0 ? PassAsync() : FailAsync();
        }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(IEnumerable validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Cast<object>().Any() ? PassAsync() : FailAsync();
        }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(Array validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Length > 0 ? PassAsync() : FailAsync();
        }
    }
}