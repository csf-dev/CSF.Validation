using System;
using System.Reflection;

namespace CSF.Validation
{
    /// <summary>
    /// A service which wraps instances of validators with exception-throwing behaviour.
    /// </summary>
    public class ExceptionThrowingValidatorWrapper : IWrapsValidatorWithExceptionBehaviour
    {
        static readonly MethodInfo
            getValidatorPrivateMethod = typeof(ExceptionThrowingValidatorWrapper).GetTypeInfo().GetDeclaredMethod(nameof(WrapValidatorPrivate));
        
        /// <inheritdoc/>
        public IValidator WrapValidator(IValidator validator)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var method = getValidatorPrivateMethod.MakeGenericMethod(validator.ValidatedType);
            return (IValidator) method.Invoke(this, new[] { validator });
        }

        /// <inheritdoc/>
        public IValidator<TValidated> WrapValidator<TValidated>(IValidator<TValidated> validator)
            => WrapValidatorPrivate(validator);

        IValidator<TValidated> WrapValidatorPrivate<TValidated>(IValidator<TValidated> validator)
            => new ThrowingBehaviourValidatorDecorator<TValidated>(validator);
    }
}