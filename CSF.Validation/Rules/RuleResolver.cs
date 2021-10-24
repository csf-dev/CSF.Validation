using System;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A service that provides resolution to get concrete implementations of rule types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This implementation will make use of a Dependency Injection <see cref="IServiceProvider"/> by
    /// default, but if that fails it will make an attempt to resolve the rule via <see cref="Activator.CreateInstance(Type)"/>.
    /// </para>
    /// </remarks>
    public class RuleResolver : IResolvesRule
    {
        readonly IServiceProvider resolver;

        /// <summary>
        /// Gets/resolves an instance of the specified <paramref name="ruleType"/>.
        /// </summary>
        /// <param name="ruleType">The concrete type of rule to resolve.</param>
        /// <returns>The resolved validation rule instance.</returns>
        public object ResolveRule(Type ruleType)
        {
            var output = resolver.GetService(ruleType);
            if(!(output is null)) return output;

            try
            {
                return Activator.CreateInstance(ruleType);
            }
            catch(Exception e)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("CannotResolveRule"), ruleType.FullName);
                throw new ValidationException(message, e);
            }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleResolver"/>.
        /// </summary>
        /// <param name="resolver">A DI resolver.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="resolver"/> is <see langword="null" />.</exception>
        public RuleResolver(IServiceProvider resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }
    }
}