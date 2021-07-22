using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    public interface IConfiguresValidator<TValidated> : IConfiguresObjectIdentity<TValidated>
    {
        void AddRule<TRule>(Action<IConfiguresRule<TRule>> ruleConfig = default) where TRule : IRule<TValidated>;

        void AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValidated>;

        void ForMember<TValue>(Expression<Func<TValidated, TValue>> memberAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig);

        void ForItemsMember<TValue>(Expression<Func<TValidated, IEnumerable<TValue>>> memberAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig);

        void ForValue<TValue>(Func<TValidated, TValue> valueAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig);

        void ForValues<TValue>(Func<TValidated, IEnumerable<TValue>> valuesAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig);
    }
}