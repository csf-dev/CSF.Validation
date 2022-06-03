using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which passes if the validated string is either <see langword="null" /> or is a match for the specified
    /// regular expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Pattern"/> property upon this rule must be set before the rule is used.  Specifying <see cref="RegexOptions"/>
    /// is optional, as this property is initialised to <see cref="RegexOptions.None"/> by default.
    /// </para>
    /// <para>
    /// This rule will pass if the validated string is <see langword="null" />.  Combine this rule with <see cref="NotNull"/> if null
    /// strings are not permitted.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class MatchesRegex : IRule<string>
    {
        /// <summary>
        /// Gets or sets the regular expression pattern.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets or sets the regular expression options to be used in the match.
        /// </summary>
        public RegexOptions RegexOptions { get; set; } = RegexOptions.None;

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null)
                return PassAsync();
            if(Pattern is null)
                throw new InvalidOperationException(Resources.ExceptionMessages.GetExceptionMessage("RegexPatternMustNotBeNull"));

            return Regex.IsMatch(validated, Pattern, RegexOptions) ? PassAsync() : FailAsync();
        }
    }
}