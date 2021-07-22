using System;

namespace CSF.Validation.ValidatorBuilding
{
    public interface IConfiguresObjectIdentity<out T>
    {
        void UseObjectIdentity(Func<T, object> identityAccessor);
    }
}