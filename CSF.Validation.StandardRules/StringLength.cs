using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that passes is a validated string is between an inclusive minimum/maximum length.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Either of <see cref="Min"/> or <see cref="Max"/> may be <see langword="null" />, in which case they are ignored
    /// and not used.  If either are null then this rule becomes effectively "shorter than" or "longer than".  If both are
    /// null then this rull will always pass.
    /// </para>
    /// <para>
    /// If the string itself is <see langword="null" /> then this rule will pass.  To prevent null strings, combine this rule
    /// with <see cref="NotNull"/>.
    /// </para>
    /// <para>
    /// The logic of this rule does not verify that the Mininum length is not greater-than the Maximum length, or that neither
    /// of these values is negative.  Thus it is possible to set up scenarios where this rule will always return a failure
    /// result for any non-null string, as the pass criteria cannot be satisfied.
    /// </para>
    /// </remarks>
    public class StringLength : IRule<string>
    {
        /// <summary>
        /// Gets or sets the minimum inclusive string length.
        /// </summary>
        public int? Min { get; set; }
        
        /// <summary>
        /// Gets or sets the maximum inclusive string length.
        /// </summary>
        public int? Max { get; set; }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            var result = (Min.HasValue ? Min.Value        <= validated.Length : true)
                      && (Max.HasValue ? validated.Length <= Max.Value        : true);
            return result ? PassAsync() : FailAsync();
        }
    }
}