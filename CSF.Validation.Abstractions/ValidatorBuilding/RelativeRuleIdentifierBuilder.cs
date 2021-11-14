using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder object which is used to create a <see cref="RelativeRuleIdentifier"/>.
    /// </summary>
    class RelativeRuleIdentifierBuilder : IBuildsRelativeRuleIdentifierType, IBuildsRelativeRuleIdentifier
    {
        Type ruleType;
        string memberName, ruleName;
        int ancestorLevels;

        /// <summary>
        /// Optionally indicates the member name which is validated by the rule which is being referred-to.
        /// </summary>
        /// <param name="memberName">The member name</param>
        /// <returns>A builder instance so that methods may be chained.</returns>
        public IBuildsRelativeRuleIdentifier ForMember(string memberName)
        {
            this.memberName = memberName;
            return this;
        }

        /// <summary>
        /// Optionally indicates the number of ancestor levels (within the validated-object hierarchy)
        /// which must be traversed to reach the rule which is being referred-to.
        /// </summary>
        /// <param name="ancestorLevels">The number of ancestor levels.</param>
        /// <returns>A builder instance so that methods may be chained.</returns>
        public IBuildsRelativeRuleIdentifier FromAncestorLevel(int ancestorLevels)
        {
            this.ancestorLevels = ancestorLevels;
            return this;
        }

        /// <summary>
        /// Gets the relative identifier built by the current instance.
        /// </summary>
        /// <returns>A relative rule identifier.</returns>
        public RelativeRuleIdentifier GetIdentifier()
        {
            return new RelativeRuleIdentifier(ruleType, memberName, ruleName, ancestorLevels);
        }

        /// <summary>
        /// Optionally indicates the name of the rule which is being referred-to.
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <returns>A builder instance so that methods may be chained.</returns>
        public IBuildsRelativeRuleIdentifier Named(string ruleName)
        {
            this.ruleName = ruleName;
            return this;
        }

        /// <summary>
        /// Indicates the type of validation rule which is being referred-to.
        /// </summary>
        /// <typeparam name="T">The rule type.</typeparam>
        /// <returns>A builder instance so that methods may be chained.</returns>
        public IBuildsRelativeRuleIdentifier RuleType<T>()
        {
            ruleType = typeof(T);
            return this;
        }
    }
}