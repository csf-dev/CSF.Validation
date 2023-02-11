using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which gets <see cref="ValidatorBuilderContext"/> instances for a number of common scenarios.
    /// </summary>
    public interface IGetsValidatorBuilderContext
    {
        /// <summary>
        /// Gets the validator builder context for validating values of a specified member of a validated object.
        /// </summary>
        /// <typeparam name="TValidated">The type of the primary object under validation.</typeparam>
        /// <typeparam name="TValue">The type of the member being validated.</typeparam>
        /// <param name="memberAccessor">An expression which indicates the member of <typeparamref name="TValidated"/> to validate.</param>
        /// <param name="validatorContext">Contextual information relating to how a validator is built.</param>
        /// <returns>A context object for building a validator.</returns>
        ValidatorBuilderContext GetContextForMember<TValidated, TValue>(Expression<Func<TValidated, TValue>> memberAccessor,
                                                                        ValidatorBuilderContext validatorContext);

        /// <summary>
        /// Gets the validator builder context for validating values that have been retrieved from a validated object.
        /// </summary>
        /// <typeparam name="TValidated">The type of the primary object under validation.</typeparam>
        /// <typeparam name="TValue">The type of the retrieved value being validated.</typeparam>
        /// <param name="valueAccessor">An accessor function which gets the value to be validated from a <typeparamref name="TValidated"/>.</param>
        /// <param name="validatorContext">Contextual information relating to how a validator is built.</param>
        /// <returns>A context object for building a validator.</returns>
        ValidatorBuilderContext GetContextForValue<TValidated, TValue>(Func<TValidated, TValue> valueAccessor,
                                                                       ValidatorBuilderContext validatorContext);

        /// <summary>
        /// Gets a validator builder context for validating items within a collection.
        /// </summary>
        /// <param name="collectionContext">A validation context representing the collection itself.</param>
        /// <param name="collectionItemType">The type of items within the collection.</param>
        /// <returns>A validation builder context.</returns>
        ValidatorBuilderContext GetContextForCollection(ValidatorBuilderContext collectionContext, Type collectionItemType);

        /// <summary>
        /// Gets a validator context for polymorphic validation of a derived type.
        /// </summary>
        /// <param name="validatorContext">Contextual information relating to how a validator is built.</param>
        /// <param name="derivedType">The derived type for which the polymorphic context will be used.</param>
        /// <returns>A context object for building a validator.</returns>
        ValidatorBuilderContext GetPolymorphicContext(ValidatorBuilderContext validatorContext, Type derivedType);
    }
}