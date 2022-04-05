using System;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A manifest model class that represents the relative identifier of a rule, within the context of
    /// another validation rule.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type roughly corresponds to <see cref="CSF.Validation.ValidatorBuilding.RelativeRuleIdentifier"/>.
    /// The manifest model classes are simplified when compared with the validation manifest
    /// and offer only a subset of functionality.  Importantly though, manifest model classes
    /// such as this are suitable for easy serialization to/from various data formats, such as
    /// JSON or relational database tables.
    /// </para>
    /// <para>
    /// For more information about when and how to use the manifest model, see the article
    /// <xref href="ManifestModelIndexPage?text=Using+the+Manifest+Model"/>
    /// </para>
    /// </remarks>
    /// <seealso cref="Value"/>
    /// <seealso cref="Rule"/>
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