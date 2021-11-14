namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder object which implements both <see cref="IConfiguresRule{TRule}"/> &amp; <see cref="IGetsManifestValue"/>.
    /// </summary>
    /// <typeparam name="TRule">The rule type.</typeparam>
    public interface IBuildsRule<TRule> : IConfiguresRule<TRule>, IGetsManifestValue {}
}