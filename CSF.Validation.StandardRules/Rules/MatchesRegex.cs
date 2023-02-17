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
    public class MatchesRegex : IRuleWithMessage<string>
    {
        /// <summary>
        /// Gets or sets the regular expression pattern.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is mandatory when using this rule.  If it is unset/has a value of <see langword="null" /> when the rule
        /// is executed then this rule will raise an exception.
        /// </para>
        /// </remarks>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets or sets the regular expression options to be used in the match.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property defaults to a value of <see cref="RegexOptions.None"/>.
        /// </para>
        /// <para>
        /// If you are using .NET 7 or newer and your <see cref="Pattern"/> does not require regular expression backtracking
        /// then consider adding <c>NonBacktracking</c> to your options, to prevent a potential DoS attack upon the regular
        /// expression engine.  For more information see the documentation comments upon the <see cref="ExecutionTimeout"/>
        /// property.
        /// </para>
        /// </remarks>
        public RegexOptions RegexOptions { get; set; } = RegexOptions.None;

        /// <summary>
        /// Gets or sets a representation of <see cref="ExecutionTimeout"/> using only a number of milliseconds.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is considered a potential security risk (DoS) to evaluate a regular expression without any timeout.
        /// An attacker could send maliciously-crafted data to force the regular expression engine to perform a lot of work
        /// and degrade system performance.
        /// </para>
        /// <para>
        /// The default timeout is 50 milliseconds, which should be more than ample for matching typical regular expressions.
        /// If this is insufficient or if your own use-case expects a much faster match then you may alter this property
        /// or you may instead alter <see cref="ExecutionTimeout"/>.  Both of these two properties control the same underlying
        /// value.  If, for example, you are using <xref href="ManifestModelIndexPage?text=the+Manifest+Model"/> then you may
        /// find this property easier to maintiain using a serializable value.
        /// </para>
        /// <para>
        /// For more information and an example of such an attack, see https://www.regular-expressions.info/catastrophic.html.
        /// Another mitigation technique would be to include <c>NonBacktracking</c> amongst the <see cref="RegexOptions"/>.
        /// The non-backtracking option is only available from .NET 7 and onward, though.
        /// </para>
        /// </remarks>
        /// <seealso cref="ExecutionTimeout"/>
        public double ExecutionTimeoutMs
        {
            get => ExecutionTimeout.TotalMilliseconds;
            set => ExecutionTimeout = TimeSpan.FromMilliseconds(value);
        }

        /// <summary>
        /// Gets or sets a timeout for the evaluation of the regular expression match.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is considered a potential security risk (DoS) to evaluate a regular expression without any timeout.
        /// An attacker could send maliciously-crafted data to force the regular expression engine to perform a lot of work
        /// and degrade system performance.
        /// </para>
        /// <para>
        /// The default timeout is 50 milliseconds, which should be more than ample for matching typical regular expressions.
        /// If this is insufficient or if your own use-case expects a much faster match then you may alter this property
        /// or you may instead alter <see cref="ExecutionTimeoutMs"/>.  Both of these two properties control the same underlying
        /// value.  If, for example, you are using <xref href="ManifestModelIndexPage?text=the+Manifest+Model"/> then you may
        /// find the <see cref="ExecutionTimeoutMs"/> property easier to maintiain using a serializable value.
        /// </para>
        /// <para>
        /// For more information and an example of such an attack, see https://www.regular-expressions.info/catastrophic.html.
        /// Another mitigation technique would be to include <c>NonBacktracking</c> amongst the <see cref="RegexOptions"/>.
        /// The non-backtracking option is only available from .NET 7 and onward, though.
        /// </para>
        /// </remarks>
        /// <seealso cref="ExecutionTimeoutMs"/>
        public TimeSpan ExecutionTimeout { get; set; } = TimeSpan.FromMilliseconds(50);

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null)
                return PassAsync();
            if(Pattern is null)
                throw new InvalidOperationException(Resources.ExceptionMessages.GetExceptionMessage("RegexPatternMustNotBeNull"));

            return Regex.IsMatch(validated, Pattern, RegexOptions, ExecutionTimeout) ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = String.Format(Resources.FailureMessages.GetFailureMessage("MatchesRegex"),
                                        Pattern, RegexOptions, value);
            return new ValueTask<string>(message);
        }
    }
}