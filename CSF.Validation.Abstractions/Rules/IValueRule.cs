using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which validates a value of a specified type, which is retrieved (in some way) from an instance of a specified object type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Both generic types of this interface are contravariant.  That means that (for example) an <c>IValueRule&lt;Animal,Person&gt;</c>
    /// may be used as if it were an <c>IValueRule&lt;Dog,Customer&gt;</c>.
    /// </para>
    /// <para>
    /// This covariance may be used to craft value rules which are neutral to the overall validated object type.
    /// For example an <c>IValueRule&lt;string,object&gt;</c> may be used to validate any string that is taken from
    /// any object.
    /// </para>
    /// </remarks>
    /// <typeparam name="TValue">The type of the value which this rule validates</typeparam>
    /// <typeparam name="TValidated">The type of the object which this rule validates</typeparam>
    public interface IValueRule<in TValue, in TValidated>
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="value">The value being validated</param>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        Task<RuleResult> GetResultAsync(TValue value, TValidated validated, ValueRuleContext context, CancellationToken token = default);
    }
}