namespace CSF.Validation.ValidatorBuilding
{

    /// <summary>
    /// A builder object which may indicate optional information about the rule being referred-to.
    /// </summary>
    public interface IBuildsRelativeRuleIdentifier
    {
        /// <summary>
        /// Optionally indicates the name of the rule which is being referred-to.
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <returns>A builder instance so that methods may be chained.</returns>
        IBuildsRelativeRuleIdentifier Named(string ruleName);

        /// <summary>
        /// Optionally indicates the member name which is validated by the rule which is being referred-to.
        /// </summary>
        /// <param name="memberName">The member name</param>
        /// <returns>A builder instance so that methods may be chained.</returns>
        IBuildsRelativeRuleIdentifier ForMember(string memberName);

        /// <summary>
        /// Optionally indicates the number of ancestor levels (within the validated-object hierarchy)
        /// which must be traversed to reach the rule which is being referred-to.
        /// </summary>
        /// <param name="ancestorLevels">The number of ancestor levels.</param>
        /// <returns>A builder instance so that methods may be chained.</returns>
        IBuildsRelativeRuleIdentifier FromAncestorLevel(int ancestorLevels);
    }
}