using System;
using System.Reflection;
using CSF.Validation.Messages;

namespace CSF.Validation
{
    /// <summary>
    /// A service which wraps a validator with functionality which adds message support.
    /// </summary>
    public class MessageSupportValidatorWrapper : IWrapsValidatorWithMessageSupport
    {
        static readonly MethodInfo getValidatorPrivateMethod = typeof(MessageSupportValidatorWrapper).GetTypeInfo().GetDeclaredMethod(nameof(GetValidatorWithMessageSupportPrivate));

        readonly IAddsFailureMessagesToResult failureMessageEnricher;

        /// <inheritdoc/>
        public IValidatorWithMessages GetValidatorWithMessageSupport(IValidator validator)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var method = getValidatorPrivateMethod.MakeGenericMethod(validator.ValidatedType);
            return (IValidatorWithMessages)method.Invoke(this, new[] { validator });
        }

        /// <inheritdoc/>
        public IValidatorWithMessages<TValidated> GetValidatorWithMessageSupport<TValidated>(IValidator<TValidated> validator)
            => GetValidatorWithMessageSupportPrivate(validator);

        IValidatorWithMessages<TValidated> GetValidatorWithMessageSupportPrivate<TValidated>(IValidator<TValidated> validator)
            => new MessageEnrichingValidatorAdapter<TValidated>(validator, failureMessageEnricher);

        /// <summary>
        /// Initialises a new instance of <see cref="MessageSupportValidatorWrapper"/>.
        /// </summary>
        /// <param name="failureMessageEnricher">The failure message enriching service.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="failureMessageEnricher"/> is <see langword="null" />.</exception>
        public MessageSupportValidatorWrapper(IAddsFailureMessagesToResult failureMessageEnricher)
        {
            this.failureMessageEnricher = failureMessageEnricher ?? throw new ArgumentNullException(nameof(failureMessageEnricher));
        }
    }
}