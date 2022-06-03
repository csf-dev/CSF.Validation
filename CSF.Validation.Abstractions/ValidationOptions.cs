using CSF.Validation.Manifest;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation
{
    /// <summary>
    /// A model for options which affect how a validator should behave whilst it is performing its validation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In this model, every property is nullable.  If any given property is set to a non-null value, then that value is used.
    /// Property which are set to <see langword="null" /> are treated as unset/not-specified will use a default value instead.
    /// </para>
    /// <para>
    /// Validation options may be specified in either or both of two places.  Thay may be configured at the point where the
    /// validator is added to dependency injection, with the <c>UseValidationFramework()</c> method.
    /// Options may also be specified when using either
    /// <see cref="IValidator.ValidateAsync(object, ValidationOptions, System.Threading.CancellationToken)"/> or
    /// <see cref="IValidator{TValidated}.ValidateAsync(TValidated, ValidationOptions, System.Threading.CancellationToken)"/>.
    /// The order of precendence for any option value (properties of this type) is:
    /// </para>
    /// <list type="number">
    /// <item><description>If specified in the <c>ValidateAsync()</c> method then that specified value is used.</description></item>
    /// <item><description>If specified as a default value in <c>UseValidationFramework()</c> then that default value is used.</description></item>
    /// <item><description>If neither of the above is specified then a hard-coded default value is used.  See
    /// <see cref="ResolvedValidationOptions"/> for info on which values are used as the hard-coded defaults.</description></item>
    /// </list>
    /// <para>
    /// For more information about validation options, please <xref href="ValidationOptions?text=read+the+corresponding+documentation+article" />.
    /// </para>
    /// </remarks>
    /// <seealso cref="ResolvedValidationOptions"/>
    public class ValidationOptions
    {
        /// <summary>
        /// Gets or sets the exception-throwing behaviour for validation rules.
        /// </summary>
        public RuleThrowingBehaviour? RuleThrowingBehaviour { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates the default behaviour should a value-accessor raise an exception
        /// during validation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value indicates the default behaviour when an accessor raises an exception but this configuration
        /// value may be overridden by <see cref="ManifestValue.AccessorExceptionBehaviour"/> upon an individual
        /// manifest value if it is set to non-<see langword="null" /> value there.
        /// </para>
        /// </remarks>
        /// <seealso cref="IConfiguresValueAccessor{TValidated,TValue}.AccessorExceptionBehaviour"/>
        /// <seealso cref="ManifestValue.AccessorExceptionBehaviour"/>
        public ValueAccessExceptionBehaviour? AccessorExceptionBehaviour { get; set; }

        /// <summary>
        /// Gets or sets a value which determines whether or not the validator should generate validation feedback
        /// messages, such as for rules which fail validation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// An optional piece of functionality within this validation framework is the generation of human-readable feedback
        /// messages relating to validation, typically for failures or errors.  By default this functionality is disabled and
        /// the logic to generate messages is skipped, providing a performance boost when it is not going to be used.
        /// </para>
        /// <para>
        /// Setting this option value to <see langword="true" /> enables this functionality and means that every validation
        /// rule result has the opportunity to have a <see cref="ValidationRuleResult.Message"/> generated with feedback
        /// advising the user what to do about the validation failure.
        /// If thie functionality is not enabled then that message property will be left at its unset/default value
        /// of <see langword="null" />.
        /// </para>
        /// </remarks>
        public bool? EnableMessageGeneration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not rules are permitted to be executed/evaluated in parallel.
        /// This may offer a modest performance gain in the right scenarios.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Running rules in parallel requires both of the following:
        /// </para>
        /// <list type="bullet">
        /// <item><description>This configuration option must be set to <see langword="true" />.</description></item>
        /// <item><description>Individual rule classes must be decorated with the <see cref="Rules.ParallelizableAttribute"/> to be eligible for parallel execution.</description></item>
        /// </list>
        /// <para>
        /// If left unset, the default value for this is <see langword="false" />, which disables parallelization.
        /// </para>
        /// </remarks>
        public bool? EnableRuleParallelization { get; set; }
    }
}