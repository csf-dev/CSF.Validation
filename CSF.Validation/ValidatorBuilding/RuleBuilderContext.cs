using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A model of contextual information used by a <see cref="RuleBuilder{TRule}"/>.
    /// </summary>
    public class RuleBuilderContext
    {
        /// <summary>
        /// Gets contextual information about the validator which contains this rule building context.
        /// </summary>
        public ValidatorBuilderContext ValidatorBuilderContext { get; }

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
        /// Gets or sets a member name associated with the rule builder context.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets a valud which indicates whether or not the value retrieved from <see cref="ValueAccessor"/>
        /// should be enumerated and each of its items validated independently.
        /// </summary>
        public bool EnumerateValueItems { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="RuleBuilderContext"/>.
        /// </summary>
        /// <param name="validatorBuilderContext">The validator-building context.</param>
        public RuleBuilderContext(ValidatorBuilderContext validatorBuilderContext)
        {
            ValidatorBuilderContext = validatorBuilderContext ?? throw new ArgumentNullException(nameof(validatorBuilderContext));
        }
    }
}