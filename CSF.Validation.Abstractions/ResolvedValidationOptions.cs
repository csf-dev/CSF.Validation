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
        /// The default &amp; recommended behaviour is <see cref="RuleThrowingBehaviour.OnError"/>.
        /// </summary>
        public RuleThrowingBehaviour RuleThrowingBehaviour { get; set; }

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
        /// <para>
        /// The default behaviour for this property if unset is <see cref="ValueAccessExceptionBehaviour.TreatAsError"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="IConfiguresValueAccessor{TValidated,TValue}.AccessorExceptionBehaviour"/>
        /// <seealso cref="ManifestValue.AccessorExceptionBehaviour"/>
        public ValueAccessExceptionBehaviour AccessorExceptionBehaviour { get; set; }

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
        public bool EnableMessageGeneration { get; set; }
    }
}