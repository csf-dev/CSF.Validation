namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder object which may indicate the type of the rule being referred-to.
    /// The rule type is the only mandatory piece of information.
    /// </summary>
    public interface IBuildsRelativeRuleIdentifierType
    {
        /// <summary>
        /// Indicates the type of validation rule which is being referred-to.
        /// </summary>
        /// <typeparam name="T">The rule type.</typeparam>
        /// <returns>A builder instance so that methods may be chained.</returns>
        IBuildsRelativeRuleIdentifier RuleType<T>();
    }
}