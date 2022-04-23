using System;
using System.Reflection;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation
{
    /// <summary>
    /// Extensionm methods for <see cref="IGetsValidator"/>.
    /// </summary>
    public static class ValidatorFactoryExtensions
    {
        /// <summary>
        /// Gets a validator instance for a specified generic type, using a specified builder type.
        /// The builder type must implement <see cref="IBuildsValidator{TValidated}"/> for the generic
        /// type <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to validate.</typeparam>
        /// <param name="factory">The validator factory.</param>
        /// <param name="builderType">A type indicating the type of validator-builder service to use.</param>
        /// <returns>A strongly-typed validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If either <paramref name="factory"/> or <paramref name="builderType"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="builderType"/> is not of a type appropriate to validate <typeparamref name="TValidated"/>.</exception>
        public static IValidator<TValidated> GetValidator<TValidated>(this IGetsValidator factory, Type builderType)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
            if (builderType is null)
                throw new ArgumentNullException(nameof(builderType));
            if (!IsCorrectBuilderType<TValidated>(builderType))
                throw new ArgumentException(String.Format(GetExceptionMessage("BuilderTypeMustBeForSelectedValidatedType"), GetBuilderType<TValidated>()), nameof(builderType));

            return (IValidator<TValidated>) factory.GetValidator(builderType);
        }

        /// <summary>
        /// Gets a validator instance for a specified generic type, using a specified validation manifest.
        /// The manifest must describe a validator which is able to validate the object type <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to validate.</typeparam>
        /// <param name="factory">The validator factory.</param>
        /// <param name="manifest">The validation manifest.</param>
        /// <returns>A strongly-typed validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If either <paramref name="factory"/> or <paramref name="manifest"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="manifest"/> does not describe a validator for <typeparamref name="TValidated"/>.</exception>
        public static IValidator<TValidated> GetValidator<TValidated>(this IGetsValidator factory, Manifest.ValidationManifest manifest)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
            if (manifest is null)
                throw new ArgumentNullException(nameof(manifest));
            if (!IsCorrectValidatedType<TValidated>(manifest.ValidatedType))
                throw new ArgumentException(String.Format(GetExceptionMessage("ValidatedTypeMustBeSelectedValidatedType"), typeof(TValidated).Name), nameof(manifest));

            return (IValidator<TValidated>) factory.GetValidator(manifest);
        }

        /// <summary>
        /// Gets a validator instance for a specified generic type, using a specified manifest model.
        /// The model must describe a validator which is able to validate the object type <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to validate.</typeparam>
        /// <param name="factory">The validator factory.</param>
        /// <param name="manifestModel">The validation manifest model.</param>
        /// <returns>A strongly-typed validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If either <paramref name="factory"/> or <paramref name="manifestModel"/> are <see langword="null"/>.</exception>
        public static IValidator<TValidated> GetValidator<TValidated>(this IGetsValidator factory, ManifestModel.Value manifestModel)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
            if (manifestModel is null)
                throw new ArgumentNullException(nameof(manifestModel));

            return (IValidator<TValidated>) factory.GetValidator(manifestModel, typeof(TValidated));
        }
        /// <summary>
        /// Gets a validator instance which includes failure message support, for a specified generic type, using a specified builder type.
        /// The builder type must implement <see cref="IBuildsValidator{TValidated}"/> for the generic
        /// type <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to validate.</typeparam>
        /// <param name="factory">The validator factory.</param>
        /// <param name="builderType">A type indicating the type of validator-builder service to use.</param>
        /// <returns>A strongly-typed validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If either <paramref name="factory"/> or <paramref name="builderType"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="builderType"/> is not of a type appropriate to validate <typeparamref name="TValidated"/>.</exception>
        public static IValidatorWithMessages<TValidated> GetValidatorWithMessageSupport<TValidated>(this IGetsValidator factory, Type builderType)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
            if (builderType is null)
                throw new ArgumentNullException(nameof(builderType));
            if (!IsCorrectBuilderType<TValidated>(builderType))
                throw new ArgumentException(String.Format(GetExceptionMessage("BuilderTypeMustBeForSelectedValidatedType"), GetBuilderType<TValidated>()), nameof(builderType));

            return (IValidatorWithMessages<TValidated>) factory.GetValidatorWithMessageSupport(builderType);
        }

        /// <summary>
        /// Gets a validator instance which includes failure message support, for a specified generic type, using a specified validation manifest.
        /// The manifest must describe a validator which is able to validate the object type <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to validate.</typeparam>
        /// <param name="factory">The validator factory.</param>
        /// <param name="manifest">The validation manifest.</param>
        /// <returns>A strongly-typed validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If either <paramref name="factory"/> or <paramref name="manifest"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="manifest"/> does not describe a validator for <typeparamref name="TValidated"/>.</exception>
        public static IValidatorWithMessages<TValidated> GetValidatorWithMessageSupport<TValidated>(this IGetsValidator factory, Manifest.ValidationManifest manifest)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
            if (manifest is null)
                throw new ArgumentNullException(nameof(manifest));
            if (!IsCorrectValidatedType<TValidated>(manifest.ValidatedType))
                throw new ArgumentException(String.Format(GetExceptionMessage("ValidatedTypeMustBeSelectedValidatedType"), typeof(TValidated).Name), nameof(manifest));

            return (IValidatorWithMessages<TValidated>) factory.GetValidatorWithMessageSupport(manifest);
        }

        /// <summary>
        /// Gets a validator instance which includes failure message support, for a specified generic type, using a specified manifest model.
        /// The model must describe a validator which is able to validate the object type <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to validate.</typeparam>
        /// <param name="factory">The validator factory.</param>
        /// <param name="manifestModel">The validation manifest model.</param>
        /// <returns>A strongly-typed validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If either <paramref name="factory"/> or <paramref name="manifestModel"/> are <see langword="null"/>.</exception>
        public static IValidatorWithMessages<TValidated> GetValidatorWithMessageSupport<TValidated>(this IGetsValidator factory, ManifestModel.Value manifestModel)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
            if (manifestModel is null)
                throw new ArgumentNullException(nameof(manifestModel));

            return (IValidatorWithMessages<TValidated>) factory.GetValidatorWithMessageSupport(manifestModel, typeof(TValidated));
        }

        static string GetBuilderType<T>() => $"{nameof(IBuildsValidator<object>)}<{typeof(T).Name}>";

        static bool IsCorrectBuilderType<T>(Type builderType)
            => typeof(IBuildsValidator<T>).GetTypeInfo().IsAssignableFrom(builderType.GetTypeInfo());

        static bool IsCorrectValidatedType<T>(Type validatedType)
            => typeof(T).GetTypeInfo().IsAssignableFrom(validatedType.GetTypeInfo());
    }
}