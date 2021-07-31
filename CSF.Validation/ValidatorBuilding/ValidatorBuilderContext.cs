using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A model for contextual information about a validator-builder.
    /// </summary>
    public class ValidatorBuilderContext
    {
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
    }
}