using CSF.Validation.Messages;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An object which contains both validation rule logic and also can provide a human-readable failure message.
    /// </summary>
    /// <typeparam name="TValidated">The validated type.</typeparam>
    public interface IRuleWithMessage<TValidated> : IRule<TValidated>, IGetsFailureMessage<TValidated> {}
}