using System;

namespace CSF.Validation.Messages
{
    public interface IGetsNonGenericMessageCriteria
    {
        IHasFailureMessageUsageCriteria GetNonGenericMessageCriteria(IGetsFailureMessage messageProvider, Type ruleInterface);
    }
}