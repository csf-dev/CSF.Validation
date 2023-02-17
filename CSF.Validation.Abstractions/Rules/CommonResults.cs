using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Helper class to create instances of <see cref="RuleResult"/> instances from commonly-used scenarios.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For simplicity of usage consider including <c>using static CSF.Validation.Rules.CommonResults;</c> in
    /// your rule implementations.
    /// </para>
    /// <para>
    /// This class also provides a small performance optimisation for results which do not return a data dictionary (or exception,
    /// in the case of error results).
    /// When any of these methods are used without data or exceptions then flyweight validation rule result instances will be
    /// used.
    /// See https://en.wikipedia.org/wiki/Flyweight_pattern for more information.
    /// </para>
    /// <para>
    /// You may also use an overload of <see cref="Result(bool, IDictionary{string, object})"/> or
    /// <see cref="ResultAsync(bool, IDictionary{string, object})"/> if you wish to use a boolean to indicate pass or failure.
    /// </para>
    /// </remarks>
    public static class CommonResults
    {
        #region Cached/singleton results

        // These immutable singleton/cached instances are flyweights which may be reused to avoid
        // allocating new objects where the singleton would suffice.
        
        static readonly RuleResult
            passSingleton = new RuleResult(RuleOutcome.Passed),
            failSingleton = new RuleResult(RuleOutcome.Failed),
            errorSingleton = new RuleResult(RuleOutcome.Errored);

        #endregion

        #region Pass

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for passing validation.
        /// </summary>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static RuleResult Pass(Dictionary<string, object> data) => Pass((IDictionary<string, object>) data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for passing validation.
        /// </summary>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static RuleResult Pass(IDictionary<string, object> data = null)
            => IsEmpty(data) ? passSingleton : new RuleResult(RuleOutcome.Passed, data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for passing validation, returned within a completed task.
        /// </summary>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <returns>A completed task of <see cref="RuleResult"/>.</returns>
        public static ValueTask<RuleResult> PassAsync(Dictionary<string, object> data) => PassAsync((IDictionary<string, object>)data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for passing validation, returned within a completed task.
        /// </summary>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <returns>A completed task of <see cref="RuleResult"/>.</returns>
        public static ValueTask<RuleResult> PassAsync(IDictionary<string, object> data = null) => new ValueTask<RuleResult>(Pass(data));

        #endregion

        #region Fail

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for failing validation.
        /// </summary>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static RuleResult Fail(Dictionary<string, object> data) =>  Fail((IDictionary<string, object>) data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for failing validation.
        /// </summary>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static RuleResult Fail(IDictionary<string, object> data = null)
            => IsEmpty(data) ? failSingleton : new RuleResult(RuleOutcome.Failed, data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for failing validation, returned within a completed task.
        /// </summary>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <returns>A completed task of <see cref="RuleResult"/>.</returns>
        public static ValueTask<RuleResult> FailAsync(Dictionary<string, object> data) => FailAsync((IDictionary<string, object>)data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for failing validation, returned within a completed task.
        /// </summary>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static ValueTask<RuleResult> FailAsync(IDictionary<string, object> data = null) => new ValueTask<RuleResult>(Fail(data));

        #endregion

        #region Error

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for validation which has encountered an unexpected error.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Most validation rules do not need to explicitly return error results.
        /// If a rule raises an uncaught exception whilst it is being executed then the validation framework
        /// will automatically create an error result for that rule, using the unhandled exception raised by
        /// the rule.
        /// </para>
        /// <para>
        /// In normal operation the only times when a rule should explicitly return an error result is either
        /// when it also wants to return a dictionary of data, when there are reasons to catch/manipulate the
        /// exception before returning the error result or when the "error" does not involve an uncaught exception.
        /// </para>
        /// </remarks>
        /// <param name="exception">The exception which caused the error.</param>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static RuleResult Error(Exception exception, Dictionary<string, object> data)
            => Error(exception, (IDictionary<string, object>) data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for validation which has encountered an unexpected error.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Most validation rules do not need to explicitly return error results.
        /// If a rule raises an uncaught exception whilst it is being executed then the validation framework
        /// will automatically create an error result for that rule, using the unhandled exception raised by
        /// the rule.
        /// </para>
        /// <para>
        /// In normal operation the only times when a rule should explicitly return an error result is either
        /// when it also wants to return a dictionary of data, when there are reasons to catch/manipulate the
        /// exception before returning the error result or when the "error" does not involve an uncaught exception.
        /// </para>
        /// </remarks>
        /// <param name="exception">The exception which caused the error.</param>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static RuleResult Error(Exception exception = null, IDictionary<string, object> data = null)
            => IsEmpty(data) && exception is null ? errorSingleton : new RuleResult(RuleOutcome.Errored, data, exception);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for validation which has encountered an unexpected error,
        /// returned within a completed task.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Most validation rules do not need to explicitly return error results.
        /// If a rule raises an uncaught exception whilst it is being executed then the validation framework
        /// will automatically create an error result for that rule, using the unhandled exception raised by
        /// the rule.
        /// </para>
        /// <para>
        /// In normal operation the only times when a rule should explicitly return an error result is either
        /// when it also wants to return a dictionary of data, when there are reasons to catch/manipulate the
        /// exception before returning the error result or when the "error" does not involve an uncaught exception.
        /// </para>
        /// </remarks>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <param name="exception">The exception which caused the error.</param>
        /// <returns>A completed task of <see cref="RuleResult"/>.</returns>
        public static ValueTask<RuleResult> ErrorAsync(Exception exception, Dictionary<string, object> data)
            => ErrorAsync(exception, (IDictionary<string, object>)data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for validation which has encountered an unexpected error,
        /// returned within a completed task.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Most validation rules do not need to explicitly return error results.
        /// If a rule raises an uncaught exception whilst it is being executed then the validation framework
        /// will automatically create an error result for that rule, using the unhandled exception raised by
        /// the rule.
        /// </para>
        /// <para>
        /// In normal operation the only times when a rule should explicitly return an error result is either
        /// when it also wants to return a dictionary of data, when there are reasons to catch/manipulate the
        /// exception before returning the error result or when the "error" does not involve an uncaught exception.
        /// </para>
        /// </remarks>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <param name="exception">The exception which caused the error.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static ValueTask<RuleResult> ErrorAsync(Exception exception = null, IDictionary<string, object> data = null)
            => new ValueTask<RuleResult>(Error(exception, data));

        #endregion

        #region Result

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for a validation result, either a pass or a fail determined by a boolean parameter.
        /// </summary>
        /// <param name="pass">A value which indicates whether or not the result was a pass.
        /// <see langword="true" /> indicates pass, <see langword="false" /> indicates failure.</param>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static RuleResult Result(bool pass, Dictionary<string, object> data)
            => Result(pass, (IDictionary<string, object>) data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for a validation result, either a pass or a fail determined by a boolean parameter.
        /// </summary>
        /// <param name="pass">A value which indicates whether or not the result was a pass.
        /// <see langword="true" /> indicates pass, <see langword="false" /> indicates failure.</param>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static RuleResult Result(bool pass, IDictionary<string, object> data = null)
        {
            if(IsEmpty(data))
                return pass ? passSingleton : failSingleton;

            var result = pass ? RuleOutcome.Passed : RuleOutcome.Failed;
            return new RuleResult(result, data);
        }

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for a validation result, either a pass or a fail determined by
        /// a boolean parameter, returned within a completed task.
        /// </summary>
        /// <param name="pass">A value which indicates whether or not the result was a pass.
        /// <see langword="true" /> indicates pass, <see langword="false" /> indicates failure.</param>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <returns>A completed task of <see cref="RuleResult"/>.</returns>
        public static ValueTask<RuleResult> ResultAsync(bool pass, Dictionary<string, object> data)
            => ResultAsync(pass, (IDictionary<string, object>) data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for a validation result, either a pass or a fail determined by
        /// a boolean parameter, returned within a completed task.
        /// </summary>
        /// <param name="pass">A value which indicates whether or not the result was a pass.
        /// <see langword="true" /> indicates pass, <see langword="false" /> indicates failure.</param>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <returns>A completed task of <see cref="RuleResult"/>.</returns>
        public static ValueTask<RuleResult> ResultAsync(bool pass, IDictionary<string, object> data = null)
            => new ValueTask<RuleResult>(Result(pass, data));

        #endregion

        /// <summary>
        /// Gets a value indicating whether or not the specified data dictionary is empty or not.
        /// </summary>
        /// <param name="data">A data dictionary</param>
        /// <returns>
        /// <see langword="true" /> if the <paramref name="data"/> is <see langword="null" /> or has a count of zero; <see langword="false" /> otherwise.
        /// </returns>
        static bool IsEmpty(IDictionary<string, object> data) => data is null || data.Count == 0;
    }
}