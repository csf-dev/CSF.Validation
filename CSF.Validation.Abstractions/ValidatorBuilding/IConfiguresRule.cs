using System.Collections.Generic;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    public interface IConfiguresRule<TRule>
    {
        TRule Rule { get; }

        string Name { get; set; }

        ICollection<RelativeRuleIdentifier> Dependencies { get; }
    }
}