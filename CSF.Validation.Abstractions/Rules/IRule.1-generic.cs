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
    /// Rules run until they complete and in the majority of cases this is not a problem.  When designing a rule class which
    /// has the potential to be long-running then there are a few best practices to consider, as noted in the article
    /// linked below.  One of these is that you should consider implementing <see cref="IHasRuleTimeout"/>.
    /// </para>
    /// <para>
    /// You are encouraged to read more at <xref href="WritingValidationRules?text=the+documentation+for+writing+rule+classes"/>
    /// &amp; <xref href="BestPractices?text=best+practices+for+writing+rules"/>.
    /// </para>
    /// </remarks>
    /// <typeparam name="TValidated">The type of the object which this rule validates</typeparam>
    public interface IRule<in TValidated>
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method receives the value to be validated as well as an object which represents the
        /// context in which this rule is running.
        /// It should return a task of <see cref="RuleResult"/>.
        /// </para>
        /// <para>
        /// In order to create the result object, particularly if your rule logic will run synchronously,
        /// consider using the <see cref="CommonResults"/> class via <c>using static CSF.Validation.Rules.CommonResults;</c> in your
        /// rule logic.
        /// The common results class has helper methods such as <see cref="CommonResults.PassAsync(System.Collections.Generic.IDictionary{string, object})"/>
        /// and <see cref="CommonResults.FailAsync(System.Collections.Generic.IDictionary{string, object})"/>
        /// which include optimisations for flyweight task instances that avoid allocating additional resources
        /// needlessly.
        /// </para>
        /// <para>
        /// It is acceptable to throw an uncaught exception from this method, as the validation framework will
        /// catch it and automatically convert it into an error result.
        /// Generally, developers do not need to manually return a result of outcome <see cref="RuleOutcome.Errored"/>
        /// manually.  This would be appropriate only in an unusual scenario that is considered an error, but
        /// which does not involve the throwing of an exception.
        /// Error results are generally harder for the consumer to deal with than failure results.
        /// </para>
        /// <para>
        /// The <paramref name="context"/> parameter may be used, amongst other things, to access 'ancestor'
        /// values.
        /// However, if this rule only needs access to an immediate parent value then consider using
        /// <see cref="IRule{TValue, TParent}"/> instead.
        /// </para>
        /// </remarks>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <exception cref="System.Exception">This method may raise any exception type</exception>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        Task<RuleResult> GetResultAsync(TValidated validated, RuleContext context, CancellationToken token = default);
    }
}