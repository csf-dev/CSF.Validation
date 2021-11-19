using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A service for a validation rule which validates a value of a specified type and also makes use of a
    /// contextual 'parent' value, typically the object from which the first value was retrieved.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A validation rule should be a single piece of logic which tests a value against
    /// a business rule/requirement to determine whether it is valid according to that business rule
    /// or not.
    /// Whilst it is possible to write dependencies between validation rules (EG:  Don't run this rule
    /// if that other rule failed), validation rules should aim to be as independent as possible.
    /// </para>
    /// <para>
    /// Both generic types of this interface are contravariant.  That means that (for example) an <c>IRule&lt;Animal,Person&gt;</c>
    /// may be used as if it were an <c>IRule&lt;Dog,Customer&gt;</c>.
    /// </para>
    /// <para>
    /// You are encouraged to read more at <xref href="WritingValidationRules?text=the+documentation+for+writing+rule+classes"/>
    /// &amp; <xref href="BestPractices?text=best+practices+for+writing+rules"/>.
    /// </para>
    /// </remarks>
    /// <typeparam name="TValue">The type of the value which this rule validates</typeparam>
    /// <typeparam name="TParent">The type of the parent value used by this rule</typeparam>
    public interface IRule<in TValue, in TParent>
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="value">The value being validated</param>
        /// <param name="parentValue">The parent value</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        Task<RuleResult> GetResultAsync(TValue value, TParent parentValue, RuleContext context, CancellationToken token = default);
    }
}