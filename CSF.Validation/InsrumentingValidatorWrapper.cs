using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation
{
    /// <summary>
    /// Default implementation of <see cref="IWrapsValidatorWithInstrumentationSupport"/>.
    /// </summary>
    public class InsrumentingValidatorWrapper : IWrapsValidatorWithInstrumentationSupport
    {
        static readonly MethodInfo getValidatorPrivateMethod = typeof(InsrumentingValidatorWrapper).GetTypeInfo().GetDeclaredMethod(nameof(GetValidatorWithInstrumentationSupportPrivate));

        readonly IServiceProvider services;

        /// <inheritdoc/>
        public IValidator WrapValidator(IValidator validator)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var method = getValidatorPrivateMethod.MakeGenericMethod(validator.ValidatedType);
            return (IValidator)method.Invoke(this, new[] { validator });
        }

        /// <inheritdoc/>
        public IValidator<TValidated> WrapValidator<TValidated>(IValidator<TValidated> validator)
            => GetValidatorWithInstrumentationSupportPrivate(validator);

        IValidator<TValidated> GetValidatorWithInstrumentationSupportPrivate<TValidated>(IValidator<TValidated> validator)
            => ActivatorUtilities.CreateInstance<InstrumentingValidatorDecorator<TValidated>>(services, validator);

        /// <summary>
        /// Initialises a new instance of <see cref="InsrumentingValidatorWrapper"/>.
        /// </summary>
        /// <param name="services">A service provider.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="services"/> is <see langword="null" />.</exception>
        public InsrumentingValidatorWrapper(IServiceProvider services)
        {
            this.services = services ?? throw new System.ArgumentNullException(nameof(services));
        }
    }
}