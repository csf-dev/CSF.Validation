using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A factory which builds up a decorator stack for the <see cref="IGetsMessageProviderInfo"/> service.
    /// </summary>
    public class DecoratorBasedMessageProviderInfoFactory : IGetsMessageProviderInfoFactory
    {
        readonly IServiceProvider serviceProvider;

        /// <inheritdoc/>
        public IGetsMessageProviderInfo GetProviderInfoFactory()
        {
            var result = GetBaseFactory();
            result = WrapWithNullExcludingDecorator(result);
            result = WrapWithCriteriaApplyingDecorator(result);
            return result;
        }

        IGetsMessageProviderInfo GetBaseFactory() => serviceProvider.GetRequiredService<MessageProviderInfoFactory>();

        IGetsMessageProviderInfo WrapWithNullExcludingDecorator(IGetsMessageProviderInfo wrapped)
            => new NullExcludingMessageProviderInfoDecorator(wrapped);

        IGetsMessageProviderInfo WrapWithCriteriaApplyingDecorator(IGetsMessageProviderInfo wrapped)
            => new CriteriaApplyingMessageProviderInfoDecorator(wrapped, serviceProvider.GetRequiredService<IGetsNonGenericMessageCriteria>());

        /// <summary>
        /// Initialises a new instance of <see cref="DecoratorBasedMessageProviderInfoFactory"/>.
        /// </summary>
        /// <param name="serviceProvider">A service provider.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="serviceProvider"/> is <see langword="null" />.</exception>
        public DecoratorBasedMessageProviderInfoFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}