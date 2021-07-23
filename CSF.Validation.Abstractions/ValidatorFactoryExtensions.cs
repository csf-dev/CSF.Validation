using System;
using System.Reflection;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation
{
    public static class ValidatorFactoryExtensions
    {
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

        static string GetBuilderType<T>() => $"{nameof(IBuildsValidator<object>)}<{typeof(T).Name}>";

        static bool IsCorrectBuilderType<T>(Type builderType)
            => typeof(IBuildsValidator<T>).GetTypeInfo().IsAssignableFrom(builderType.GetTypeInfo());

        static bool IsCorrectValidatedType<T>(Type validatedType)
            => typeof(T).GetTypeInfo().IsAssignableFrom(validatedType.GetTypeInfo());
    }
}