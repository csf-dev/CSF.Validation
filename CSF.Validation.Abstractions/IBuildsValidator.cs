using System;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation
{
    /// <summary>
    /// An object which may be used to build a validator, specifying its configuration such as
    /// the rules which the validator will use.
    /// </summary>
    /// <typeparam name="TValidated">The object type which the created validator will be used to validate.</typeparam>
    public interface IBuildsValidator<TValidated>
    {
        /// <summary>
        /// Configures the validator using a helper object.
        /// </summary>
        /// <param name="config">A helper object which is used to configure the validator.</param>
        void ConfigureValidator(IConfiguresValidator<TValidated> config);
    }
}