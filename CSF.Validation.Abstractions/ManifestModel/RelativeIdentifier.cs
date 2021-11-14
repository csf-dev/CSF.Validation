using System;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A model which may be used to indicate another rule, based upon a relative position within
    /// a validation hierarchy.  This is used to indicate dependencies between validation rules.
    /// The models in this namespace provide a serialization-friendly mechanism by which to describe
    /// a validation manifest.
    /// </summary>
    public class RelativeIdentifier
    {
        private int ancestorLevels;

        /// <summary>
        /// Gets or sets the type name of the validation rule logic class.
        /// </summary>
        public string RuleTypeName { get; set; }

        /// <summary>
        /// Gets or sets an optional name for the rule, which uniquely identifies it
        /// from other rules of the same type upon the same validated value.
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// Where the referenced rule is for a member of a value/object, this property gets or sets the member name.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets the number of levels of ancestry (within the parent/child validation hierarchy) which must be traversed
        /// from the 'current' rule, in order to reach the validator which contains the identified rule.
        /// </summary>
        public int AncestorLevels
        {
            get => ancestorLevels;
            set
            {
                if(ancestorLevels < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                ancestorLevels = value;
            }
        }
    }
}