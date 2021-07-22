using System;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    public interface IConfiguresValueAccessor<TValidated, TValue> : IConfiguresObjectIdentity<TValue>
    {
        void AddValueRule<TRule>(Action<IConfiguresRule<TRule>> ruleConfig = default) where TRule : IValueRule<TValue,TValidated>;

        void AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValue>;
    }
}