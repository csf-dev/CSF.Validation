using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A model which describes the contextual state required in order to convert
    /// a validation model into a validation manifest.
    /// </summary>
    public class ModelToManifestConversionContext
    {
        /// <summary>
        /// Gets or sets the current model <see cref="Value"/> being converted.
        /// </summary>
        public ValueBase CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets an optional reference to the parent (already-converted) <see cref="ManifestValue"/>
        /// which should be used as the parent of the <see cref="CurrentValue"/>.
        /// </summary>
        public ManifestItem ParentManifestValue { get; set; }

        /// <summary>
        /// Gets or sets an optional member name which would be used to traverse from the
        /// <see cref="ParentManifestValue"/> to the <see cref="CurrentValue"/>.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets an optional accessor function which would be used to traverse from
        /// the <see cref="ParentManifestValue"/> to the <see cref="CurrentValue"/>.
        /// </summary>
        public Func<object,object> AccessorFromParent { get; set; }

        /// <summary>
        /// Gets or sets the type of the object which would be validated by the <see cref="CurrentValue"/>.
        /// </summary>
        public Type ValidatedType { get; set; }

        /// <summary>
        /// Gets a value that indicates the conversion type.
        /// </summary>
        public ModelToManifestConversionType ConversionType { get; set; }

        /// <summary>
        /// Gets or sets the string name of the polymorphic type.  Unused when <see cref="ConversionType"/> is
        /// not <see cref="ModelToManifestConversionType.PolymorphicType"/>.
        /// </summary>
        public string PolymorphicTypeName { get; set; }
    }
}