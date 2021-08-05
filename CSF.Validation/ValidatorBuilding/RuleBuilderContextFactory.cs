using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CSF.Reflection;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service for getting instances of <see cref="ValidatorBuilderContext"/>.
    /// </summary>
    public class ValidatorBuilderContextFactory : IGetsValidatorBuilderContext
    {
        readonly IStaticallyReflects reflect;

        /// <summary>
        /// Gets the root validator builder context.
        /// </summary>
        /// <returns>A validator builder context.</returns>
        public ValidatorBuilderContext GetRootContext() => new ValidatorBuilderContext(new ManifestValue());

        /// <summary>
        /// Gets the rule builder context for validating values of a specified member of a validated object.
        /// </summary>
        /// <typeparam name="TValidated">The type of the primary object under validation.</typeparam>
        /// <typeparam name="TValue">The type of the member being validated.</typeparam>
        /// <param name="memberAccessor">An expression which indicates the member of <typeparamref name="TValidated"/> to validate.</param>
        /// <param name="validatorContext">Contextual information relating to how a validator is built.</param>
        /// <param name="enumerateItems">
        /// A value that indicates whether or not the items within the validated member should be enumerated
        /// and validated independently.  This is relevant when <typeparamref name="TValue"/> implements
        /// <see cref="IEnumerable{T}"/>.
        /// </param>
        /// <returns>A context object for building validation rules.</returns>
        public ValidatorBuilderContext GetContextForMember<TValidated, TValue>(Expression<Func<TValidated, TValue>> memberAccessor, ValidatorBuilderContext validatorContext, bool enumerateItems = false)
        {
            var member = reflect.Member(memberAccessor);
            var accessor = memberAccessor.Compile();

            var manifestValue = new ManifestValue
            {
                Parent = validatorContext.ManifestValue,
                AccessorFromParent = obj => accessor((TValidated) obj),
                MemberName = member.Name,
                EnumerateItems = enumerateItems,
            };
            validatorContext.ManifestValue.Children.Add(manifestValue);
            return new ValidatorBuilderContext(manifestValue);
        }

        /// <summary>
        /// Gets the rule builder context for validating values that have been derived from a validated object.
        /// </summary>
        /// <typeparam name="TValidated">The type of the primary object under validation.</typeparam>
        /// <typeparam name="TValue">The type of the derived value being validated.</typeparam>
        /// <param name="valueAccessor">An accessor function which gets the value to be validated from a <typeparamref name="TValidated"/>.</param>
        /// <param name="validatorContext">Contextual information relating to how a validator is built.</param>
        /// <param name="enumerateItems">
        /// A value that indicates whether or not the items within the derived value should be enumerated
        /// and validated independently.  This is relevant when <typeparamref name="TValue"/> implements
        /// <see cref="IEnumerable{T}"/>.
        /// </param>
        /// <returns>A context object for building validation rules.</returns>
        public ValidatorBuilderContext GetContextForValue<TValidated, TValue>(Func<TValidated, TValue> valueAccessor, ValidatorBuilderContext validatorContext, bool enumerateItems = false)
        {
            var manifestValue = new ManifestValue
            {
                Parent = validatorContext.ManifestValue,
                AccessorFromParent = obj => valueAccessor((TValidated) obj),
                EnumerateItems = enumerateItems,
            };
            validatorContext.ManifestValue.Children.Add(manifestValue);
            return new ValidatorBuilderContext(manifestValue);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorBuilderContextFactory"/>.
        /// </summary>
        /// <param name="reflect">A service which provides static reflection.</param>
        public ValidatorBuilderContextFactory(IStaticallyReflects reflect)
        {
            this.reflect = reflect ?? throw new ArgumentNullException(nameof(reflect));
        }
    }
}