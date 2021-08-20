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
        /// Gets or sets the type of the object which shall be validated in this value.
        /// Where <see cref="EnumerateItems"/> is <see langword="true"/>, this property should
        /// contain the type of the collection items, not the collection itself.
        /// </summary>
        public Type ValidatedType { get; set; }

        /// <summary>
        /// Gets or sets an optional parent manifest value.
        /// Where this is <see langword="null"/> that indicates that this model is the root of the validation hierarchy.
        /// If it is non-<see langword="null"/> then it is a descendent of the root of the hierarchy.
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
        /// This should be set to <see langword="true"/> if the <see cref="AccessorFromParent"/> returns
        /// an <see cref="IEnumerable{T}"/> which should be enumerated and each of its items validated
        /// independently. This should be set to <see langword="false"/> if not.
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