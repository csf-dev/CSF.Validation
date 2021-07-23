using System;

namespace CSF.Validation
{
    /// <summary>
    /// An object which may be used as the factory from which to create validator instances.
    /// </summary>
    public interface IGetsValidator
    {
        /// <summary>
        /// Gets a validator instance using a specified builder type which specifies a validator via code.
        /// </summary>
        /// <param name="builderType">The type of a class which implements <see cref="IBuildsValidator{TValidated}"/> for the desired validated type.</param>
        /// <returns>A validator implementation.</returns>
        IValidator GetValidator(Type builderType);

        /// <summary>
        /// Gets a validator instance using a specified validation manifest.
        /// A validation manifest is a model which may specify a validator using data.
        /// </summary>
        /// <param name="manifest">The validation manifest.</param>
        /// <returns>A validator implementation.</returns>
        IValidator GetValidator(Manifest.ValidationManifest manifest);
    }
}