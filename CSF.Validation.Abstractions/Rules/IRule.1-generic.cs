using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A service for a validation rule which validates an instance of a specified object type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A validation rule should be a single piece of logic which tests a single value against
    /// a business rule/requirement to determine whether it is valid according to that business rule
    /// or not.
    /// Whilst it is possible to write dependencies between validation rules (EG:  Don't run this rule
    /// if that other rule failed), validation rules should aim to be as independent as possible.
    /// </para>
    /// <para>
    /// The generic type of this interface is contravariant.  That means that (for example) an <c>IRule&lt;Animal&gt;</c>
    /// may be used as if it were an <c>IRule&lt;Dog&gt;</c>.
    /// </para>
    /// <para>
    /// You are encouraged to read more at <xref href="WritingValidationRules?text=the+documentation+for+writing+rule+classes"/>
    /// &amp; <xref href="ValidationRuleBestPractices?text=best+practices+for+writing+rules"/>.
    /// </para>
    /// </remarks>
    /// <typeparam name="TValidated">The type of the object which this rule validates</typeparam>
    public interface IRule<in TValidated>
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        Task<RuleResult> GetResultAsync(TValidated validated, RuleContext context, CancellationToken token = default);
    }
}