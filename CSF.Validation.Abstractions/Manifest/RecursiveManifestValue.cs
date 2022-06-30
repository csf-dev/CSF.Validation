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
    /// </remarks>
    /// <seealso cref="ManifestRule"/>
    /// <seealso cref="ValidationManifest"/>
    /// <seealso cref="ManifestRuleIdentifier"/>
    /// <seealso cref="ManifestValue"/>
    /// <seealso cref="ManifestCollectionItem"/>
    public class RecursiveManifestValue : IManifestValue
    {
        readonly IManifestValue wrapped;

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

        /// <inheritdoc/>
        public Type ValidatedType => wrapped.ValidatedType;

        /// <inheritdoc/>
        public IManifestItem Parent => wrapped.Parent;

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