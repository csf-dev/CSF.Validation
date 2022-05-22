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
            getValidatorPrivateMethod = typeof(ExceptionThrowingValidatorWrapper).GetTypeInfo().GetDeclaredMethod(nameof(WrapValidatorPrivate)),
            getValidatorWithMessagesPrivateMethod = typeof(ExceptionThrowingValidatorWrapper).GetTypeInfo().GetDeclaredMethod(nameof(WrapValidatorWithMessagesPrivate));
        
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

        /// <inheritdoc/>
        public IValidatorWithMessages WrapValidator(IValidatorWithMessages validator)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var method = getValidatorWithMessagesPrivateMethod.MakeGenericMethod(validator.ValidatedType);
            return (IValidatorWithMessages) method.Invoke(this, new[] { validator });
        }

        /// <inheritdoc/>
        public IValidatorWithMessages<TValidated> WrapValidator<TValidated>(IValidatorWithMessages<TValidated> validator)
            => WrapValidatorWithMessagesPrivate(validator);

        IValidator<TValidated> WrapValidatorPrivate<TValidated>(IValidator<TValidated> validator)
            => new ThrowingBehaviourValidatorDecorator<TValidated>(validator);

        IValidatorWithMessages<TValidated> WrapValidatorWithMessagesPrivate<TValidated>(IValidatorWithMessages<TValidated> validator)
            => new ThrowingBehaviourValidatorWithMessagesDecorator<TValidated>(validator);
    }
}