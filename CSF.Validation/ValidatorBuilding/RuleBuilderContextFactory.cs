using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CSF.Reflection;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service for getting instances of <see cref="RuleBuilderContext"/>.
    /// </summary>
    public class RuleBuilderContextFactory : IGetsRuleBuilderContext
    {
        readonly IStaticallyReflects reflect;

        /// <summary>
        /// Gets a rule builder context for a specified validator builder context.
        /// </summary>
        /// <param name="validatorContext">Contextual information relating to how a validator is built.</param>
        /// <returns>A context object for building validation rules.</returns>
        public RuleBuilderContext GetContext(ValidatorBuilderContext validatorContext) => new RuleBuilderContext(validatorContext);

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
        public RuleBuilderContext GetContextForMember<TValidated, TValue>(Expression<Func<TValidated, TValue>> memberAccessor, ValidatorBuilderContext validatorContext, bool enumerateItems = false)
        {
            var member = reflect.Member(memberAccessor);
            var accessor = memberAccessor.Compile();

            return new RuleBuilderContext(validatorContext)
            {
                EnumerateValueItems = enumerateItems,
                MemberName = member.Name,
                ValueAccessor = obj => accessor((TValidated) obj),
            };
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
        public RuleBuilderContext GetContextForValue<TValidated, TValue>(Func<TValidated, TValue> valueAccessor, ValidatorBuilderContext validatorContext, bool enumerateItems = false)
        {
            return new RuleBuilderContext(validatorContext)
            {
                EnumerateValueItems = enumerateItems,
                ValueAccessor = obj => valueAccessor((TValidated) obj),
            };
        }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleBuilderContextFactory"/>.
        /// </summary>
        /// <param name="reflect">A service which provides static reflection.</param>
        public RuleBuilderContextFactory(IStaticallyReflects reflect)
        {
            this.reflect = reflect ?? throw new ArgumentNullException(nameof(reflect));
        }
    }
}