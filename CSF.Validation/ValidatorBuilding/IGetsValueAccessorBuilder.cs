using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which gets instances of <see cref="ValueAccessorBuilder{TValidated, TValue}"/> for a specified rule builder context.
    /// </summary>
    public interface IGetsValueAccessorBuilder
    {
        /// <summary>
        /// Gets a builder for validating a value of a validated object (typically retrieved via member access).
        /// </summary>
        /// <typeparam name="TValidated">The type of the primary object under validation.</typeparam>
        /// <typeparam name="TValue">The type of the derived value to be validated.</typeparam>
        /// <param name="ValidatorBuilderContext">Contextual information about how validation rules should be built.</param>
        /// <param name="valueConfig">An action which configures the value accessor-builder.</param>
        /// <returns>A builder for validating the derived value.</returns>
        IConfiguresValueAccessor<TValidated, TValue> GetValueAccessorBuilder<TValidated, TValue>(ValidatorBuilderContext ValidatorBuilderContext, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig);
    }
}