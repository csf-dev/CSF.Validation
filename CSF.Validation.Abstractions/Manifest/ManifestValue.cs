using System;
using System.Collections.Generic;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model which represents a value which is validated within a validation hierarchy.
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
    /// <seealso cref="ManifestRuleIdentifier"/>
    /// <seealso cref="ValidationManifest"/>
    /// <seealso cref="IManifestItem"/>
    /// <seealso cref="IManifestValue"/>
    /// <seealso cref="IHasPolymorphicTypes"/>
    /// <seealso cref="ManifestValueBase"/>
    /// <seealso cref="ManifestCollectionItem"/>
    /// <seealso cref="ManifestPolymorphicType"/>
    /// <seealso cref="RecursiveManifestValue"/>
    public class ManifestValue : ManifestValueBase, IHasPolymorphicTypes, IManifestValue
    {
        ICollection<ManifestPolymorphicType> polymorphicTypes = new HashSet<ManifestPolymorphicType>();

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
        /// Gets or sets an optional value which indicates the desired behaviour should the <see cref="AccessorFromParent"/> raise an exception.
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
        public ValueAccessExceptionBehaviour? AccessorExceptionBehaviour { get; set; }

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string which represents the current instance.</returns>
        public override string ToString()
            => $"[{nameof(ManifestValue)}: Type = {ValidatedType.Name}, Member = {MemberName}]";

        /// <inheritdoc/>
        public ICollection<ManifestPolymorphicType> PolymorphicTypes
        {
            get => polymorphicTypes;
            set => polymorphicTypes = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}