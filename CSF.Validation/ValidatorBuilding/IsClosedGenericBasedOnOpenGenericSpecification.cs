using System;
using System.Linq.Expressions;
using System.Reflection;
using CSF.Specifications;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A specification for types that are a closed form of an open generic type.
    /// </summary>
    public class IsClosedGenericBasedOnOpenGenericSpecification : ISpecificationExpression<Type>
    {
        readonly Type type;

        /// <summary>
        /// Gets the match expression.
        /// </summary>
        /// <returns>An expression.</returns>
        public Expression<Func<Type, bool>> GetExpression()
        {
            return t => t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsClosedGenericBasedOnOpenGenericSpecification"/> class.
        /// </summary>
        /// <param name="type">An open generic type.</param>
        public IsClosedGenericBasedOnOpenGenericSpecification(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (!type.GetTypeInfo().IsGenericTypeDefinition)
                throw new ArgumentException("The base type must be an open generic type.", nameof(type));

            this.type = type;
        }
    }
}