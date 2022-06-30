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
    /// <seealso cref="IManifestItem"/>
    /// <seealso cref="IManifestValue"/>
    /// <seealso cref="IHasPolymorphicTypes"/>
    /// <seealso cref="ManifestValueBase"/>
    /// <seealso cref="ManifestValue"/>
    /// <seealso cref="ManifestCollectionItem"/>
    /// <seealso cref="ManifestPolymorphicType"/>
    public class RecursiveManifestValue : IManifestValue
    {
        readonly IManifestValue wrapped;

        /// <summary>
        /// Gets a reference to the original value that current instance wraps.
        /// </summary>
        public IManifestValue WrappedValue => wrapped;

        /// <summary>
        /// Gets or sets a function which gets (from the object represented by the <see cref="IManifestItem.Parent"/>)
        /// the value for the current instance.
        /// </summary>
        public Func<object, object> AccessorFromParent { get; set; }

        /// <summary>
        /// Where the current value represents a member access invocation (such as
        /// a property getter), this property gets or sets the name of that member.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets an optional parent manifest value.
        /// Where this is <see langword="null"/> that indicates that this model is the root of the validation hierarchy.
        /// If it is non-<see langword="null"/> then it is a descendent of the root of the hierarchy.
        /// </summary>
        public IManifestItem Parent { get; set; }

        /// <inheritdoc/>
        public Type ValidatedType => wrapped.ValidatedType;

        /// <inheritdoc/>
        public Func<object, object> IdentityAccessor => wrapped.IdentityAccessor;

        /// <inheritdoc/>
        public ManifestCollectionItem CollectionItemValue => wrapped.CollectionItemValue;

        /// <inheritdoc/>
        public ICollection<IManifestValue> Children => wrapped.Children;

        /// <inheritdoc/>
        public ICollection<ManifestRule> Rules => wrapped.Rules;

        /// <inheritdoc/>
        public ValueAccessExceptionBehaviour? AccessorExceptionBehaviour => wrapped.AccessorExceptionBehaviour;

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string which represents the current instance.</returns>
        public override string ToString()
        {
            var properties = new Dictionary<string, string>
            {
                { nameof(Type), ValidatedType?.Name },
                { nameof(MemberName), MemberName },
            };
            var propertyStrings = properties
                .Where(x => !(x.Value is null))
                .Select(x => $"{x.Key} = {x.Value}").ToList();
            
            return $"[{nameof(RecursiveManifestValue)}: {String.Join(", ",  propertyStrings)}]";
        }

        /// <summary>
        /// Initialises a new <see cref="RecursiveManifestValue"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped manifest value.</param>
        public RecursiveManifestValue(IManifestValue wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}