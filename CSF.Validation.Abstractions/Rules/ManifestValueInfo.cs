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
        /// <summary>
        /// Gets the type of the object which the current manifest value describes.
        /// </summary>
        public Type ValidatedType { get; }

        /// <summary>
        /// Where the current value represents a member access invocation (such as
        /// a property getter), this property gets the name of that member.
        /// </summary>
        public string MemberName { get; }

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
        /// Gets or sets an optional value which indicates the desired behaviour should the value-accessor raise an exception.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This option will override the behaviour specified at <see cref="ValidationOptions.AccessorExceptionBehaviour"/>
        /// for the current manifest value, if this property is set to any non-<see langword="null" /> value.
        /// </para>
        /// <para>
        /// If this property is set to <see langword="null" /> then the behaviour at <see cref="ValidationOptions.AccessorExceptionBehaviour"/>
        /// will be used.
        /// </para>
        /// </remarks>
        /// <seealso cref="ValidationOptions.AccessorExceptionBehaviour"/>
        public ValueAccessExceptionBehaviour? AccessorExceptionBehaviour { get; }

        /// <summary>
        /// Initialises an instance of <see cref="ManifestValueInfo"/>.
        /// This is essentially a copy-constructor for a <see cref="ManifestValueBase"/>.
        /// </summary>
        /// <param name="manifestValue">The manifest value from which to create this instance.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="manifestValue"/> is <see langword="null" />.</exception>
        public ManifestValueInfo(ManifestValueBase manifestValue)
        {
            if (manifestValue is null)
                throw new ArgumentNullException(nameof(manifestValue));

            ValidatedType = manifestValue.ValidatedType;
            MemberName = manifestValue.MemberName;
            CollectionItemValue = manifestValue.CollectionItemValue is null
                ? null
                : new ManifestValueInfo(manifestValue.CollectionItemValue);
            Children = new List<ManifestValueInfo>(manifestValue.Children.Select(x => new ManifestValueInfo(x)));
            if(manifestValue is ManifestValue val)
                AccessorExceptionBehaviour = val.AccessorExceptionBehaviour;
        }
    }
}