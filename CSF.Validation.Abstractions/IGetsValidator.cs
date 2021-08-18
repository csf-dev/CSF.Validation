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
        /// <exception cref="ArgumentNullException">If the <paramref name="builderType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the <paramref name="builderType"/> is not a concrete (non-abstract) class derived from <see cref="IBuildsValidator{TValidated}"/>.</exception>
        IValidator GetValidator(Type builderType);

        /// <summary>
        /// Gets a validator instance using a specified validator-builder instance which specifies a validator via code.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to be validated.</typeparam>
        /// <param name="builder">An instance of a validator-builder.</param>
        /// <returns>A strongly-typed validator implementation.</returns>
        IValidator<TValidated> GetValidator<TValidated>(IBuildsValidator<TValidated> builder);

        /// <summary>
        /// Gets a validator instance using a specified validation manifest.
        /// A validation manifest is a model which may specify a validator using data.
        /// </summary>
        /// <param name="manifest">The validation manifest.</param>
        /// <returns>A validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="manifest"/> is <see langword="null"/>.</exception>
        /// <exception cref="ValidatorBuildingException">If the <paramref name="manifest"/> is not valid to create a validator instance.</exception>
        IValidator GetValidator(Manifest.ValidationManifest manifest);

        /// <summary>
        /// Gets a validator instance using a specified validation manifest.
        /// A validation manifest is a model which may specify a validator using data.
        /// This overload uses a simplified/serialization-friendly model.
        /// </summary>
        /// <param name="manifestModel">A simplified validation manifest model.</param>
        /// <param name="validatedType">The type of object to be validated.</param>
        /// <returns>A validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="manifestModel"/> is <see langword="null"/>.</exception>
        /// <exception cref="ValidatorBuildingException">If the <paramref name="manifestModel"/> is not valid to create a validator instance.</exception>
        IValidator GetValidator(ManifestModel.Value manifestModel, Type validatedType);
    }
}