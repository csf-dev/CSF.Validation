using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A service that gets the validated type from a value type.
    /// </summary>
    public class ValidatedTypeProvider : IGetsValidatedType
    {
        /// <summary>
        /// Gets the validated type from the value type, and whether or not items should be enumerated.
        /// </summary>
        /// <param name="type">The value type.</param>
        /// <param name="enumerateItems">A value indicating whether or not items within the value type are to be enumerated.</param>
        /// <returns>The validated type.</returns>
        public Type GetValidatedType(Type type, bool enumerateItems)
        {
            if(!enumerateItems) return type;

            return (from @interface in GetTypeInterfaces(type)
                    let interfaceInfo = @interface.GetTypeInfo()
                    where
                        interfaceInfo.IsGenericType
                     && interfaceInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                    select @interface.GenericTypeArguments.Single())
                .First();
        }

        static IEnumerable<Type> GetTypeInterfaces(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if(!typeInfo.IsInterface)
                return typeInfo.ImplementedInterfaces;

            return new[] { type }.Union(typeInfo.ImplementedInterfaces);
        }
    }
}