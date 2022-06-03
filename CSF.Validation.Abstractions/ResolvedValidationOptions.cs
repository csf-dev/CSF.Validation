using CSF.Validation.Manifest;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation
{
    /// <summary>
    /// A model for options which affect how a validator should behave whilst it is performing its validation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In this model, every property is non-nullable and has a concrete value. Do not use this class to
    /// specify options at the developer/API level.  Instead use <see cref="ValidationOptions"/> for that purpose.
    /// </para>
    /// </remarks>
    /// <seealso cref="ValidationOptions"/>
    public class ResolvedValidationOptions
    {
        /// <summary>
        /// Gets or sets the exception-throwing behaviour for validation rules.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When a validator - an <see cref="IValidator"/> or an <see cref="IValidator{TValidated}"/> - completes validation
        /// it might throw an exception, governed by this option setting.
        /// If this is set to <see cref="RuleThrowingBehaviour.OnError"/> then the validator will throw if any result
        /// indicates <see cref="Rules.RuleOutcome.Errored"/>.
        /// If this is set to <see cref="RuleThrowingBehaviour.OnFailure"/> then the validator will throw if any result
        /// indicates either <see cref="Rules.RuleOutcome.Failed"/> or <see cref="Rules.RuleOutcome.Errored"/>.
        /// </para>
        /// <para>
        /// The hard-coded default for this value (if not specified upon any <see cref="ValidationOptions"/> instance)
        /// is <see cref="RuleThrowingBehaviour.OnError"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="Validation.RuleThrowingBehaviour"/>
        /// <seealso cref="ValidationOptions.RuleThrowingBehaviour"/>
        public RuleThrowingBehaviour RuleThrowingBehaviour { get; set; } = RuleThrowingBehaviour.OnError;

        /// <summary>
        /// Gets or sets a value which indicates the default behaviour should a value-accessor raise an exception
        /// during validation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The validator automatically catches exceptions when executing validation rules and converts each of
        /// these into a rule result which has an outcome of <see cref="Rules.RuleOutcome.Errored"/>.
        /// This option configures what the validator should do by default when accessing a value to be validated
        /// throws an exception.
        /// </para>
        /// <para>
        /// Please note that the behaviour specified in this validation option can be overridden by individual
        /// values in the validation manifest.  The property <see cref="ManifestValue.AccessorExceptionBehaviour"/>,
        /// where set to a non-null value, will override the behaviour specified upon this options property.
        /// </para>
        /// <para>
        /// The hard-coded default for this value (if not specified upon any <see cref="ValidationOptions"/> instance)
        /// is <see cref="ValueAccessExceptionBehaviour.TreatAsError"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="IConfiguresValueAccessor{TValidated,TValue}.AccessorExceptionBehaviour"/>
        /// <seealso cref="ManifestValue.AccessorExceptionBehaviour"/>
        /// <seealso cref="ValueAccessExceptionBehaviour"/>
        /// <seealso cref="ValidationOptions.AccessorExceptionBehaviour"/>
        public ValueAccessExceptionBehaviour AccessorExceptionBehaviour { get; set; } = ValueAccessExceptionBehaviour.TreatAsError;

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
        /// <para>
        /// For more information about validation feedback message generation, you are encouraged to
        /// <xref href="GeneratingFeedbackMessages?text=read+the+corresponding+documentation"/>.
        /// </para>
        /// <para>
        /// The hard-coded default for this value (if not specified upon any <see cref="ValidationOptions"/> instance)
        /// is <see langword="false" />.
        /// </para>
        /// </remarks>
        /// <seealso cref="ValidationOptions.EnableMessageGeneration"/>
        public bool EnableMessageGeneration { get; set; } = false;

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
        /// <item><description>Individual rule classes must be decorated with the <see cref="Rules.ParallelizableAttribute"/>
        /// to be eligible for parallel execution.</description></item>
        /// </list>
        /// <para>
        /// The hard-coded default for this value (if not specified upon any <see cref="ValidationOptions"/> instance)
        /// is <see langword="false" />.
        /// </para>
        /// </remarks>
        /// <seealso cref="ValidationOptions.EnableRuleParallelization"/>
        /// <seealso cref="Rules.ParallelizableAttribute"/>
        public bool EnableRuleParallelization { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating how many rules are permitted to be run in parallel at once.
        /// This property is relevant only when <see cref="EnableRuleParallelization"/> is <see langword="true" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The hard-coded default for this value (if not specified upon any <see cref="ValidationOptions"/> instance)
        /// is to be equal to the number of CPU cores detected by the .NET runtime, as indicated by
        /// <see cref="System.Environment.ProcessorCount"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="EnableRuleParallelization"/>
        /// <seealso cref="ValidationOptions.DegreesOfParallelization"/>
        /// <seealso cref="Rules.ParallelizableAttribute"/>
        public int DegreesOfParallelization { get; set; } = System.Environment.ProcessorCount;
    }
}