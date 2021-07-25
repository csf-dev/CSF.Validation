using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A model of contextual information used by a <see cref="RuleBuilder{TRule}"/>.
    /// </summary>
    public class RuleBuilderContext
    {
        /// <summary>
        /// Gets or sets an accessor function which is used to get the primary
        /// object under validation for this rule.  The input to this function is
        /// the original object under validation for the validator at the root of
        /// the manifest.
        /// </summary>
        public Func<object,object> ValidatedObjectAccessor { get; set; }

        /// <summary>
        /// Gets or sets an accessor function which is used to get a value to be
        /// validated for this rule.  The input to this function is
        /// the original object under validation for the validator at the root of
        /// the manifest.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For a rule that does not validate a specific value, this property will be <see langword="null"/>.
        /// </para>
        /// </remarks>
        public Func<object,object> ValueAccessor { get; set; }

        /// <summary>
        /// Gets or sets a function which retrieves a unique identity of the object being
        /// validated, given a reference to that object being validated.
        /// </summary>
        public Func<object,object> ObjectIdentityAccessor { get; set; }

        /// <summary>
        /// Gets or sets a member name associated with the rule builder context.
        /// </summary>
        public string MemberName { get; set; }
    }
}