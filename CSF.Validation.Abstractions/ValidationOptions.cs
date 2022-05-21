using CSF.Validation.Manifest;
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
    }
}