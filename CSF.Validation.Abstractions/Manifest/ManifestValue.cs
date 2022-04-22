using System;

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
    /// <seealso cref="ValidationManifest"/>
    /// <seealso cref="ManifestRuleIdentifier"/>
    /// <seealso cref="ManifestValueBase"/>
    /// <seealso cref="ManifestCollectionItem"/>
    public class ManifestValue : ManifestValueBase
    {
        /// <summary>
        /// Gets or sets a function which gets (from the object represented by the <see cref="ManifestValueBase.Parent"/>)
        /// the value for the current instance.
        /// </summary>
        public Func<object, object> AccessorFromParent { get; set; }

        /// <summary>
        /// Where the current value represents a member access invocation (such as
        /// a property getter), this property gets or sets the name of that member.
        /// </summary>
        public new string MemberName
        {
            get => base.MemberName;
            set => base.MemberName = value;
        }

        /// <summary>
        /// Indicates that the validator should ignore any exceptions encountered whilst getting the value from
        /// the <see cref="AccessorFromParent"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This option is irrelevant if <see cref="ValidationOptions.IgnoreValueAccessExceptions"/> is set to <see langword="true"/>,
        /// because that option ignores all value-access exceptions globally.
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
        public bool IgnoreAccessorExceptions { get; set; }

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string which represents the current instance.</returns>
        public override string ToString()
            => $"[{nameof(ManifestValue)}: Type = {ValidatedType.Name}, Member = {MemberName}]";
    }
}