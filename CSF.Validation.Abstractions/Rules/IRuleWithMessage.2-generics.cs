using CSF.Validation.Messages;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An object which contains both validation rule logic and also can provide a human-readable failure message.
    /// </summary>
    /// <typeparam name="TValidated">The validated type.</typeparam>
    /// <typeparam name="TParent">The parent validated type.</typeparam>
    public interface IRuleWithMessage<in TValidated,in TParent> : IRule<TValidated,TParent>, IGetsFailureMessage<TValidated,TParent> {}
}