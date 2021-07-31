namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder object which implements both <see cref="IConfiguresValueAccessor{TValidated, TValue}"/> &amp; <see cref="IGetsManifestRules"/>.
    /// </summary>
    /// <typeparam name="TValidated">The type of the primary object being validated.</typeparam>
    /// <typeparam name="TValue">The type of the derived value being validated.</typeparam>
    public interface IBuildsValueAccessor<TValidated, TValue> : IConfiguresValueAccessor<TValidated, TValue>, IGetsManifestRules {}
}