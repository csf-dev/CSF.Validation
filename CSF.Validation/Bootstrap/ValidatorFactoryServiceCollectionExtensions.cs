using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Bootstrap
{
    static class ValidatorFactoryServiceCollectionExtensions
    {
        internal static IServiceCollection AddValidatorFactory(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IGetsValidator, ValidatorFactory>()
                .AddTransient<IGetsBaseValidator, BaseValidatorFactory>()
                .AddTransient<IWrapsValidatorWithExceptionBehaviour, ExceptionThrowingValidatorWrapper>()
                .AddTransient<IWrapsValidatorWithMessageSupport, MessageSupportValidatorWrapper>()
                .AddTransient<IGetsResolvedValidationOptions,ValidationOptionsResolver>()
                ;
        }
    }
}