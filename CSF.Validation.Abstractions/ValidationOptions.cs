using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation
{
    /// <summary>
    /// A model for options which affect how a validator should behave whilst it is performing its validation.
    /// </summary>
    public class ValidationOptions
    {
        /// <summary>
        /// Gets or sets the exception-throwing behaviour for validation rules.
        /// The default &amp; recommended behaviour is <see cref="RuleThrowingBehaviour.OnError"/>.
        /// </summary>
        public RuleThrowingBehaviour RuleThrowingBehaviour { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether or not exceptions should be globally ignored
        /// when accessing values from validated objects.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For example, if a validator is to validate a value from an object and that value is retrieved from
        /// a property or method access.  If that property/method raises an exception for retrieving the value
        /// then by default that exception will be rethrown by the validator and will cause validation to fail.
        /// </para>
        /// <para>
        /// If this option is set to <see langword="true"/> then all such exceptions will be caught and ignored.
        /// In this case, the validated value will be treated as the default of its data-type, such as
        /// <see langword="null"/> or zero.
        /// </para>
        /// <para>
        /// It is not recommended to enable this setting as it may hide logic errors in the value accessors used
        /// by the validator.  It could then lead to misleading validation results, as the rules are being run upon
        /// an incorrect value.
        /// </para>
        /// <para>
        /// If this option is set to <see langword="true"/> then the <see cref="IConfiguresValueAccessor{TValidated,TValue}.IgnoreExceptions"/>
        /// option will be irrelevant wherever it is used upon individual value accessors; exceptions will be ignored
        /// globally.  If this option is set to <see langword="false"/> then the <see cref="IConfiguresValueAccessor{TValidated,TValue}.IgnoreExceptions"/>
        /// option may be used on individual value accessors to ignore exceptions on a value-by-value basis.
        /// </para>
        /// </remarks>
        /// <seealso cref="IConfiguresValueAccessor{TValidated,TValue}.IgnoreExceptions"/>
        public bool IgnoreValueAccessExceptions { get; set; }
    }
}