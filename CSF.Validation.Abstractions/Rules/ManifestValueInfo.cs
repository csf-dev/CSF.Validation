using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An immutable model which provides information about the configuration of a value to be validated.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type roughly corresponds to a <see cref="ManifestValueBase"/> and its derived types.  The key difference
    /// between that and this 'info' class is that this type is immutable and presents a read-only API.
    /// </para>
    /// </remarks>
    public class ManifestValueInfo
    {
        readonly ManifestValueBase manifestValue;

        /// <summary>
        /// Gets the type of the object which the current manifest value describes.
        /// </summary>
        public Type ValidatedType => manifestValue.ValidatedType;

        /// <summary>
        /// Where the current value represents a member access invocation (such as
        /// a property getter), this property gets the name of that member.
        /// </summary>
        public string MemberName => manifestValue.MemberName;

        /// <summary>
        /// Gets an optional value object which indicates how items within a collection are to be validated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the value representd by the current instance is a collection/enumerable of items then these items may
        /// be validated individually.  In this scenario, the <see cref="ValidatedType"/> must be a
        /// type that implements <see cref="System.Collections.Generic.IEnumerable{T}"/> for at least one generic type.
        /// </para>
        /// <para>
        /// If the current value does not represent a collection of items to be validated individually then this
        /// property will be <see langword="null" />.
        /// </para>
        /// </remarks>
        public ManifestValueInfo CollectionItemValue { get; }

        /// <summary>
        /// Gets a collection of the immediate descendents of the current manifest value.
        /// </summary>
        public IReadOnlyCollection<ManifestValueInfo> Children { get; }

        /// <summary>
        /// Gets a value which indicates that the validator should ignore any exceptions encountered whilst accessing
        /// this value from its parent.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This option is irrelevant if <see cref="ValidationOptions.IgnoreValueAccessExceptions"/> is set to <see langword="true"/>
        /// when validation occurs, because that option ignores all value-access exceptions globally.
        /// </para>
        /// <para>
        /// If the global validation options are not configured to globally-ignore value access exceptions then this option may be
        /// used to ignore exceptions on an accessor-by-accessor basis.  This is not recommended because it can lead to the
        /// hiding of logic errors within the accessor.
        /// </para>
        /// <para>
        /// See the information about the global setting for more information about what it means to ignore exceptions for
        /// value accessors.
        /// </para>
        /// </remarks>
        /// <seealso cref="ValidationOptions.IgnoreValueAccessExceptions"/>
        public bool IgnoreAccessorExceptions { get; }

        /// <summary>
        /// Initialises an instance of <see cref="ManifestValueInfo"/>.
        /// This is essentially a copy-constructor for a <see cref="ManifestValueBase"/>.
        /// </summary>
        /// <param name="manifestValue">The manifest value from which to create this instance.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="manifestValue"/> is <see langword="null" />.</exception>
        public ManifestValueInfo(ManifestValueBase manifestValue)
        {
            this.manifestValue = manifestValue ?? throw new ArgumentNullException(nameof(manifestValue));
            CollectionItemValue = manifestValue.CollectionItemValue is null
                ? null
                : new ManifestValueInfo(manifestValue.CollectionItemValue);
            Children = new List<ManifestValueInfo>(manifestValue.Children.Select(x => new ManifestValueInfo(x)));
            if(manifestValue is ManifestValue val)
                IgnoreAccessorExceptions = val.IgnoreAccessorExceptions;
        }
    }
}