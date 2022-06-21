using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSF.Validation
{
    /// <summary>
    /// A strongly-typed specialisation of <see cref="IQueryableValidationResult"/>.
    /// </summary>
    public interface IQueryableValidationResult<TValidated> : IQueryableValidationResult
    {
        /// <summary>
        /// Gets a subset of the current validation result, including only results applicable to
        /// the specified member and descendent members.
        /// </summary>
        /// <typeparam name="TItem">The type of the member item.</typeparam>
        /// <param name="memberExpression">A expression indicating a member of the validated object, which returns a value.</param>
        /// <returns>A queryable result, filtered for results applicable to the specified member and its descendents.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="memberExpression"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If the <see cref="IQueryableValidationResult.ManifestValue"/> has no child values with the same name as the <paramref name="memberExpression"/>.</exception>
        IQueryableValidationResult<TItem> ForMember<TItem>(Expression<Func<TValidated, TItem>> memberExpression);

        /// <summary>
        /// Gets a subset of the current validation result, including only results applicable to
        /// a collection item that matches the specified item.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="memberExpression">A expression indicating a member of the validated object, which returns a collection of values.</param>
        /// <param name="item">The item (of the collection) for which a result is to be filtered</param>
        /// <returns>A queryable result, filtered for results applicable to the specified item of a collection, and its descendents.</returns>
        IQueryableValidationResult<TItem> ForMatchingMemberItem<TItem>(Expression<Func<TValidated, IEnumerable<TItem>>> memberExpression, TItem item);

        /// <summary>
        /// Gets a subset of the current validation result, including rule results only for the current value.
        /// Any rule results for descendent values are excluded.
        /// </summary>
        /// <returns>A queryable result, filtered for only rules directly associated with the current value.</returns>
        IQueryableValidationResult<TValidated> ForOnlyThisValue();

        /// <summary>
        /// Gets a subset of the current validation result, excluding any results which relate to a success.
        /// </summary>
        /// <returns>A queryable result, filtered for only rule results that do not indicate success.</returns>
        IQueryableValidationResult<TValidated> WithoutSuccesses();

        /// <summary>
        /// 'Casts' the current validation result for an object of type <typeparamref name="TDerived"/>, enabling access to
        /// rules and values added as part of polymorphic validation.
        /// </summary>
        /// <typeparam name="TDerived">A derived validated type.</typeparam>
        /// <returns>A queryable result, enabling access to members and rules for the derived type..</returns>
        IQueryableValidationResult<TDerived> PolymorphicAs<TDerived>() where TDerived : TValidated;
    }
}