using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Helper class to create instances of <see cref="RuleResult"/> instances from commonly-used scenarios.
    /// For simplicity of usage consider including <c>using static CSF.Validation.Rules.CommonResults;</c> in
    /// your rule implementations.
    /// </summary>
    public static class CommonResults
    {
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
        public static RuleResult Pass(IDictionary<string, object> data = null) => new RuleResult(RuleOutcome.Passed, data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for passing validation, returned within a completed task.
        /// This API is provided for convenience when validation rules do not contain any asynchronous logic.
        /// </summary>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <returns>A completed task of <see cref="RuleResult"/>.</returns>
        public static Task<RuleResult> PassAsync(Dictionary<string, object> data) => Task.FromResult(Pass(data));

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for passing validation, returned within a completed task.
        /// This API is provided for convenience when validation rules do not contain any asynchronous logic.
        /// </summary>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <returns>A completed task of <see cref="RuleResult"/>.</returns>
        public static Task<RuleResult> PassAsync(IDictionary<string, object> data = null) => Task.FromResult(Pass(data));

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
        public static RuleResult Fail(IDictionary<string, object> data = null) => new RuleResult(RuleOutcome.Failed, data);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for failing validation, returned within a completed task.
        /// This API is provided for convenience when validation rules do not contain any asynchronous logic.
        /// </summary>
        /// <param name="data">A key/value collection of arbitrary validation data.</param>
        /// <returns>A completed task of <see cref="RuleResult"/>.</returns>
        public static Task<RuleResult> FailAsync(Dictionary<string, object> data) => Task.FromResult(Fail(data));

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for failing validation, returned within a completed task.
        /// This API is provided for convenience when validation rules do not contain any asynchronous logic.
        /// </summary>
        /// <param name="data">An optional key/value collection of arbitrary validation data.</param>
        /// <returns>A <see cref="RuleResult"/>.</returns>
        public static Task<RuleResult> FailAsync(IDictionary<string, object> data = null) => Task.FromResult(Fail(data));

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
            => new RuleResult(RuleOutcome.Errored, data, exception);

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for validation which has encountered an unexpected error,
        /// returned within a completed task. This API is provided for convenience when validation rules do not
        /// contain any asynchronous logic.
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
        public static Task<RuleResult> ErrorAsync(Exception exception, Dictionary<string, object> data)
            => Task.FromResult(Error(exception, data));

        /// <summary>
        /// Creates an instance of <see cref="RuleResult"/> for validation which has encountered an unexpected error,
        /// returned within a completed task. This API is provided for convenience when validation rules do not
        /// contain any asynchronous logic.
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
        public static Task<RuleResult> ErrorAsync(Exception exception = null, IDictionary<string, object> data = null)
            => Task.FromResult(Error(exception, data));

        #endregion
    }
}