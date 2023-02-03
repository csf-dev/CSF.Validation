using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// Represents a recursive reference to a manifest value which already appears within the current manifest.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The validation manifest is the model by which validators are described, including how they should
    /// validate objects and values.
    /// </para>
    /// <para>
    /// The validation manifest objects are not particularly suited to serialization,
    /// as they support the use of types that cannot be easily serialized.
    /// If you are looking for a way to create/define a validator using serialized data then please read the
    /// article <xref href="ManifestModelIndexPage?text=Using+the+Manifest+Model"/>.
    /// </para>
    /// <para>
    /// Recursive validation is used when validating an object model which may contain references back to a type which
    /// already appears in the validation manifest and has appropriate validation configuration.
    /// This would be appropriate when validating a node-based object graph such as an HTML DOM.  In HTML a
    /// <c>&lt;div&gt;</c> element is permitted to contain <c>&lt;div&gt;</c> elements.  If writing a validator for this
    /// you would not want to write an endless nested series of validation manifest values, rather we would like something
    /// which essentially means "If this div contains a div, validate the child in the same way we would validate the parent,
    /// including its children".
    /// </para>
    /// <para>
    /// The recursive manifest value wraps another manifest value (which would have already appeared in the same manifest
    /// as a value).  It means "Validate in the same way as the wrapped value", except that it may have a different accessor
    /// function and member name.
    /// </para>
    /// </remarks>
    /// <seealso cref="ManifestRule"/>
    /// <seealso cref="ManifestRuleIdentifier"/>
    /// <seealso cref="ValidationManifest"/>
    /// <seealso cref="ManifestItem"/>
    /// <seealso cref="Manifest.ManifestValue"/>
    /// <seealso cref="ManifestCollectionItem"/>
    /// <seealso cref="ManifestPolymorphicType"/>
    public class RecursiveManifestValue : ManifestValue
    {
        readonly ManifestItem wrapped;

        /// <summary>
        /// Gets a reference to the original value that current instance wraps.
        /// </summary>
        public ManifestItem WrappedValue => wrapped;

        /// <summary>
        /// Gets the type of the object which the current manifest value describes.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="RecursiveManifestValue"/> does not support setting this property value, any usage of
        /// the setter will raise <see cref="NotSupportedException"/>.
        /// The value of this property always be derived from the <see cref="WrappedValue"/>.
        /// </para>
        /// </remarks>
        public override Type ValidatedType
        {
            get => wrapped.ValidatedType;
            set => throw GetNoSetterException(nameof(ValidatedType));
        }

        /// <summary>
        /// Gets a function which retrieves a unique identity of the object being
        /// validated, given a reference to that object being validated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="RecursiveManifestValue"/> does not support setting this property value, any usage of
        /// the setter will raise <see cref="NotSupportedException"/>.
        /// The value of this property always be derived from the <see cref="WrappedValue"/>.
        /// </para>
        /// </remarks>
        public override Func<object, object> IdentityAccessor
        {
            get => wrapped.IdentityAccessor;
            set => throw GetNoSetterException(nameof(IdentityAccessor));
        }

        /// <summary>
        /// Gets an optional value object which indicates how items within a collection are to be validated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="RecursiveManifestValue"/> does not support setting this property value, any usage of
        /// the setter will raise <see cref="NotSupportedException"/>.
        /// The value of this property always be derived from the <see cref="WrappedValue"/>.
        /// </para>
        /// <para>
        /// If the value representd by the current instance is a collection/enumerable of items then these items may
        /// be validated individually.  In this scenario, the <see cref="ManifestItem.ValidatedType"/> must be a
        /// type that implements <see cref="System.Collections.Generic.IEnumerable{T}"/> for at least one generic type.
        /// </para>
        /// <para>
        /// If this property has a non-null value, then the <see cref="ManifestCollectionItem"/> will be used to validate
        /// each item within that collection.
        /// </para>
        /// <para>
        /// If the current manifest value does not represent a collection of items to be validated individually then this
        /// property must be <see langword="null" />.
        /// </para>
        /// </remarks>
        public override ManifestCollectionItem CollectionItemValue
        {
            get => wrapped.CollectionItemValue;
            set => throw GetNoSetterException(nameof(CollectionItemValue));
        }

        /// <summary>
        /// Gets a collection of the immediate descendents of the current manifest value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="RecursiveManifestValue"/> does not support setting this property value, any usage of
        /// the setter will raise <see cref="NotSupportedException"/>.
        /// The value of this property always be derived from the <see cref="WrappedValue"/>.
        /// Additionally, the getter of this property will always produce a new collection, copying the elements from
        /// the same collection in the <see cref="WrappedValue"/>.  This means that modifying the contents of this
        /// collection will not be effective.
        /// </para>
        /// </remarks>
        public override ICollection<ManifestValue> Children
        {
            get => new List<ManifestValue>(wrapped.Children);
            set => throw GetNoSetterException(nameof(Children));
        }

        /// <summary>
        /// Gets a collection of the rules associated with the current value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="RecursiveManifestValue"/> does not support setting this property value, any usage of
        /// the setter will raise <see cref="NotSupportedException"/>.
        /// The value of this property always be derived from the <see cref="WrappedValue"/>.
        /// Additionally, the getter of this property will always produce a new collection, copying the elements from
        /// the same collection in the <see cref="WrappedValue"/>.  This means that modifying the contents of this
        /// collection will not be effective.
        /// </para>
        /// </remarks>
        public override ICollection<ManifestRule> Rules
        {
            get => new List<ManifestRule>(wrapped.Rules);
            set => throw GetNoSetterException(nameof(Rules));
        }

        /// <summary>
        /// Gets an optional value which indicates the desired behaviour should the <see cref="ManifestValue.AccessorFromParent"/> raise an exception.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="RecursiveManifestValue"/> does not support setting this property value, any usage of
        /// the setter will raise <see cref="NotSupportedException"/>.
        /// The value of this property always be derived from the <see cref="WrappedValue"/>.
        /// </para>
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
        public override ValueAccessExceptionBehaviour? AccessorExceptionBehaviour
        {
            get => (wrapped is ManifestValue val) ? val.AccessorExceptionBehaviour : null;
            set => throw GetNoSetterException(nameof(AccessorExceptionBehaviour));
        }

        static NotSupportedException GetNoSetterException(string propertyName)
        {
            var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("PropertyMayNotBeSetForRecursiveItem"),
                                        propertyName,
                                        nameof(RecursiveManifestValue));
            return new NotSupportedException(message);
        }

        /// <summary>
        /// Initialises a new <see cref="RecursiveManifestValue"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped manifest value.</param>
        public RecursiveManifestValue(ManifestItem wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}