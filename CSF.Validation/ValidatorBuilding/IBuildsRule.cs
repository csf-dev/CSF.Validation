namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder object which implements both <see cref="IConfiguresRule{TRule}"/> &amp; <see cref="IGetsManifestRules"/>.
    /// </summary>
    /// <typeparam name="TRule">The rule type.</typeparam>
    public interface IBuildsRule<TRule> : IConfiguresRule<TRule>, IGetsManifestRules {}
}