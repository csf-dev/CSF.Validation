using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A model for contextual information about a validator-builder.
    /// </summary>
    public class ValidatorBuilderContext
    {
        /// <summary>
        /// Gets an optional rule builder context from which this validator context has been created.
        /// If this is not <see langword="null"/> then this represents a 'child' validation context.
        /// </summary>
        public RuleBuilderContext RuleBuilderContext { get; }

        /// <summary>
        /// Gets or sets an accessor function which is used to get the primary
        /// object under validation for this rule.  The input to this function is
        /// the original object under validation for the validator at the root of
        /// the manifest.
        /// </summary>
        public Func<object,object> ValidatedObjectAccessor { get; set; }

        /// <summary>
        /// Gets or sets a function which retrieves a unique identity of the object being
        /// validated, given a reference to that object being validated.
        /// </summary>
        public Func<object,object> ObjectIdentityAccessor { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorBuilderContext"/>.
        /// </summary>
        public ValidatorBuilderContext() {}

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorBuilderContext"/> which shall be a child
        /// context of a specified rule-building context.
        /// </summary>
        /// <param name="ruleBuilderContext">A parent rule builder context.</param>
        public ValidatorBuilderContext(RuleBuilderContext ruleBuilderContext)
        {
            RuleBuilderContext = ruleBuilderContext ?? throw new ArgumentNullException(nameof(ruleBuilderContext));
        }
    }
}