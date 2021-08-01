using System;
using System.Collections.Generic;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model which represents a value which is validated within a validation hierarchy.
    /// </summary>
    public class ManifestValue
    {
        /// <summary>
        /// An optional parent object, indicating that this instance is a descendent of the root of the validation hierarchy.
        /// </summary>
        public ManifestValue Parent { get; }

        /// <summary>
        /// A function which retrieves a unique identity of the object being
        /// validated, given a reference to that object being validated.
        /// </summary>
        public Func<object,object> IdentityAccessor { get; set; }

        /// <summary>
        /// A function which gets (from the object represented by the <see cref="Parent"/>)
        /// the value for the current instance.
        /// </summary>
        public Func<object,object> AccessorFromParent { get; }

        /// <summary>
        /// Where the <see cref="AccessorFromParent"/> represents a member invocation (such as
        /// a property getter), this property gets the name of that member.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets <see langword="true"/> <see cref="AccessorFromParent"/> returns an <see cref="IEnumerable{T}"/> which
        /// should be enumerated and each of its items validated independently.  Gets <see langword="false"/> otherwise.
        /// </summary>
        public bool EnumerateItems { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="ManifestValue"/>, used for the root manifest value in a validation hierarchy.
        /// </summary>
        public ManifestValue() {}

        /// <summary>
        /// Initialises a new instance of <see cref="ManifestValue"/>.
        /// </summary>
        /// <param name="parent">An parent manifest value.</param>
        /// <param name="accessorFromParent">An optional accessor function.</param>
        /// <param name="memberName">An optional member name.</param>
        /// <param name="enumerateItems">An optional value indicating whether items should be enumerated.</param>
        public ManifestValue(ManifestValue parent,
                             Func<object, object> accessorFromParent = default,
                             string memberName = default,
                             bool enumerateItems = default)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            AccessorFromParent = accessorFromParent;
            MemberName = memberName;
            EnumerateItems = enumerateItems;
        }
    }
}