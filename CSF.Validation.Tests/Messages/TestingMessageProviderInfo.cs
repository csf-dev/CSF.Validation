namespace CSF.Validation.Messages
{
    public class TestingMessageProviderInfo : MessageProviderInfo
    {
        public override IGetsFailureMessage MessageProvider { get; }

        public TestingMessageProviderInfo(IGetsFailureMessage messageProvider, int priority = default)
            : base(new MessageProviderTypeInfo(messageProvider.GetType(), priority))
        {
            MessageProvider = messageProvider ?? throw new System.ArgumentNullException(nameof(messageProvider));
        }
    }
}