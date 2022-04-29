using System;

namespace CSF.Validation.Messages
{
    public interface IGetsNonGenericMessageProvider
    {
        IGetsFailureMessage GetNonGenericFailureMessageProvider(Type type, Type ruleInterface);
    }
}