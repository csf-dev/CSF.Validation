using System;
using System.Linq.Expressions;
using CSF.Specifications;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// A specification for validation value contexts that match a specified manifest value and
    /// optionally a validated item.
    /// </summary>
    public class ValueContextIsMatchForItem : ISpecificationExpression<ValueContext>
    {
        readonly ManifestItem value;
        readonly bool hasItem;
        readonly object item;

        ISpecificationExpression<ValueContext> IsMatchForValue
            => Spec.Expr<ValueContext>(x => x.ValueInfo.GetOriginalManifestValue() == value);

        ISpecificationExpression<ValueContext> IsMatchForItem
        {
            get {
                if(!hasItem) return Spec.Expr(Predicate.True<ValueContext>());
                if(value.IdentityAccessor is null || item is null)
                    return Spec.Expr<ValueContext>(x => Equals(x.ActualValue, item));
                return Spec.Expr<ValueContext>(x => Equals(value.IdentityAccessor(item), x.ObjectIdentity));
            }
        }


        /// <summary>
        /// Gets an expression which represents the current specification instance.
        /// </summary>
        /// <returns>The specification expression.</returns>
        public Expression<Func<ValueContext, bool>> GetExpression()
            => IsMatchForValue.And(IsMatchForItem).GetExpression();

        /// <summary>
        /// Initialises a new instance of <see cref="ValueContextIsMatchForItem"/> without a validated item instance.
        /// </summary>
        /// <param name="value">The manifest value which must be matched</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <see langword="null" />.</exception>
        public ValueContextIsMatchForItem(ManifestItem value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
            hasItem = false;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValueContextIsMatchForItem"/> with a validated item instance.
        /// </summary>
        /// <param name="value">The manifest value which must be matched</param>
        /// <param name="item">An optional item (of the type indicated by the <paramref name="value"/>) which the value content must match.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <see langword="null" />.</exception>
        public ValueContextIsMatchForItem(ManifestItem value, object item) : this(value)
        {
            this.item = item;
            hasItem = true;
        }
    }
}