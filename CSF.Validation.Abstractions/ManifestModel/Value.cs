using System;
using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A simple model object which represents a simplified model for <see cref="ManifestValue"/>.
    /// The models in this namespace provide a serialization-friendly mechanism by which to describe
    /// a validation manifest.
    /// </summary>
    public class Value
    {
        IDictionary<string,Value> children = new Dictionary<string,Value>();
        ICollection<Rule> rules = new List<Rule>();

        /// <summary>
        /// Gets or sets a collection of the child values from the current instance.
        /// Each value is recorded using its member-name as a key.
        /// </summary>
        public IDictionary<string,Value> Children
        {
            get => children;
            set => children = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a collection of rules for the current value.
        /// </summary>
        public ICollection<Rule> Rules
        {
            get => rules;
            set => rules = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets an optional member name which provides the identity value for the current object.
        /// </summary>
        public string IdentityMemberName { get; set; }
        
        /// <summary>
        /// This should be set to <see langword="true"/> if the object represented by this value is an <see cref="IEnumerable{T}"/> which
        /// should be enumerated and each of its items validated independently.
        /// This should be set to <see langword="false"/> if not.
        /// </summary>
        public bool EnumerateItems { get; set; }

        /// <summary>
        /// Indicates that the validator should ignore any exceptions encountered whilst getting the value from
        /// the member accessor for this value.
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
    }
}