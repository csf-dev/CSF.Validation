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
    /// This type roughly corresponds to a <see cref="ManifestItem"/> and its derived types.  The key difference
    /// between that and this 'info' class is that this type is immutable and presents a read-only API.
    /// </para>
    /// </remarks>
    public class ManifestValueInfo
    {
        readonly ManifestItem manifestValue;
        readonly Lazy<ManifestValueInfo> collectionItemValue;
        readonly Lazy<IReadOnlyCollection<ManifestValueInfo>> children;

        /// <summary>
        /// Gets the type of the object which the current manifest value describes.
        /// </summary>
        public Type ValidatedType => manifestValue.ValidatedType;

        /// <summary>
        /// Where the current value represents a member access invocation (such as
        /// a property getter), this property gets the name of that member.
        /// </summary>
        public string MemberName => (manifestValue.IsValue)? manifestValue.MemberName : null;

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
        public ManifestValueInfo CollectionItemValue => collectionItemValue.Value;

        /// <summary>
        /// Gets a collection of the immediate descendents of the current manifest value.
        /// </summary>
        public IReadOnlyCollection<ManifestValueInfo> Children => children.Value;

        /// <summary>
        /// Gets or sets an optional value which indicates the desired behaviour should the value-accessor raise an exception.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This option will override the behaviour specified at <see cref="ResolvedValidationOptions.AccessorExceptionBehaviour"/>
        /// for the current manifest value, if this property is set to any non-<see langword="null" /> value.
        /// </para>
        /// <para>
        /// If this property is set to <see langword="null" /> then the behaviour at <see cref="ResolvedValidationOptions.AccessorExceptionBehaviour"/>
        /// will be used.
        /// </para>
        /// </remarks>
        /// <seealso cref="ResolvedValidationOptions.AccessorExceptionBehaviour"/>
        public ValueAccessExceptionBehaviour? AccessorExceptionBehaviour { get; }

        /// <summary>
        /// Gets a reference to the original manifest value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Be very careful not to mutate/alter any state of this object when using this method.
        /// The returned object is mutable, but must not be changed. 
        /// </para>
        /// </remarks>
        /// <returns>The original manifest value from which this instance was created.</returns>
        public ManifestItem GetOriginalManifestValue() => manifestValue;

        /// <summary>
        /// Initialises an instance of <see cref="ManifestValueInfo"/>.
        /// This is essentially a copy-constructor for a <see cref="ManifestItem"/>.
        /// </summary>
        /// <param name="manifestValue">The manifest value from which to create this instance.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="manifestValue"/> is <see langword="null" />.</exception>
        public ManifestValueInfo(ManifestItem manifestValue)
        {
            this.manifestValue = manifestValue ?? throw new ArgumentNullException(nameof(manifestValue));

            collectionItemValue = new Lazy<ManifestValueInfo>(() =>
            {
                return manifestValue.CollectionItemValue is null
                ? null
                : new ManifestValueInfo(manifestValue.CollectionItemValue);
            });

            children = new Lazy<IReadOnlyCollection<ManifestValueInfo>>(() =>
            {
                return manifestValue.Children
                .Select(x => new ManifestValueInfo(x))
                .ToList();
            });
            
            if(manifestValue.IsValue)
                AccessorExceptionBehaviour = manifestValue.AccessorExceptionBehaviour;
        }
    }
}