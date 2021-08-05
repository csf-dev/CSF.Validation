using System;
using System.Collections.Generic;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model which represents a value which is validated within a validation hierarchy.
    /// </summary>
    public class ManifestValue
    {
        ICollection<ManifestValue> children = new List<ManifestValue>();
        ICollection<ManifestRule> rules = new List<ManifestRule>();

        /// <summary>
        /// Gets or sets an optional parent object, indicating that this instance is a descendent of the root of the
        /// validation hierarchy.
        /// </summary>
        public ManifestValue Parent { get; set; }

        /// <summary>
        /// Gets or sets a function which retrieves a unique identity of the object being
        /// validated, given a reference to that object being validated.
        /// </summary>
        public Func<object, object> IdentityAccessor { get; set; }

        /// <summary>
        /// Gets or sets a function which gets (from the object represented by the <see cref="Parent"/>)
        /// the value for the current instance.
        /// </summary>
        public Func<object, object> AccessorFromParent { get; set; }

        /// <summary>
        /// Where the <see cref="AccessorFromParent"/> represents a member invocation (such as
        /// a property getter), this property gets or sets the name of that member.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets <see langword="true"/> <see cref="AccessorFromParent"/> returns an <see cref="IEnumerable{T}"/> which
        /// should be enumerated and each of its items validated independently.
        /// This should be set to <see langword="false"/> if not.
        /// </summary>
        public bool EnumerateItems { get; set; }

        /// <summary>
        /// Gets or sets a collection of the immediate descendents of the current manifest value.
        /// </summary>
        public ICollection<ManifestValue> Children
        {
            get => children;
            set => children = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets a collection of the rules associated with the current value.
        /// </summary>
        public ICollection<ManifestRule> Rules
        {
            get => rules;
            set => rules = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}