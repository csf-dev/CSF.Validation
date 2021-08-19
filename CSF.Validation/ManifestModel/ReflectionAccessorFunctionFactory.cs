using System;
using System.Linq;
using System.Reflection;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A factory service which gets an accessor function via reflection.
    /// </summary>
    public class ReflectionAccessorFunctionFactory : IGetsAccessorFunction
    {
        /// <summary>
        /// Gets the accessor function which corresponds to getting a value from the
        /// specified <paramref name="type"/> by using the specified <paramref name="memberName"/>.
        /// </summary>
        /// <param name="type">The type of object which shall be the input to the function.</param>
        /// <param name="memberName">The name of the member to get/access/execute in order to get the output of the function.</param>
        /// <returns>An <see cref="AccessorFunctionAndType"/> containing both the function and also the expected return-type of that function.</returns>
        /// <exception cref="ArgumentNullException">If either <paramref name="memberName"/> or <paramref name="type"/> are <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// If either the <paramref name="type"/> does not have an accessible member named <paramref name="memberName"/> or if
        /// that member has a <see langword="void"/> return-type.
        /// </exception>
        public AccessorFunctionAndType GetAccessorFunction(Type type, string memberName)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (memberName is null)
                throw new ArgumentNullException(nameof(memberName));

            var member = type.GetTypeInfo().DeclaredMembers.FirstOrDefault(x => x.Name == memberName);
            if(member is null)
            {
                var exMessage = String.Format(GetExceptionMessage("MissingMember"), type.Name, memberName);
                throw new ArgumentException(exMessage, nameof(memberName));
            }

            if(member is PropertyInfo prop)
                return GetAccessor(prop);
            if(member is FieldInfo field)
                return GetAccessor(field);
            if(member is MethodInfo method)
                return GetAccessor(method);

            var message = String.Format(GetExceptionMessage("MustBeAGettableMember"), type.Name, memberName);
            throw new ArgumentException(message, nameof(memberName));
        }

        static AccessorFunctionAndType GetAccessor(PropertyInfo property)
        {
            if(!property.CanRead)
            {
                var message = String.Format(GetExceptionMessage("PropertyMustBeReadable"), property.DeclaringType, property.Name);
                throw new ArgumentException(message, nameof(property));
            }

            if(property.GetIndexParameters().Any())
            {
                var message = String.Format(GetExceptionMessage("PropertyMustNotBeIndexer"), property.DeclaringType, property.Name);
                throw new ArgumentException(message, nameof(property));
            }
            var accessor = GetAccessDelegate(property.GetMethod);

            return new AccessorFunctionAndType
            {
                AccessorFunction = accessor,
                ExpectedType = property.PropertyType,
            };
        }

        static AccessorFunctionAndType GetAccessor(MethodInfo method)
        {
            if(method.ReturnType == typeof(void))
            {
                var message = String.Format(GetExceptionMessage("MethodMustHaveNonVoidReturn"), method.DeclaringType, method.Name);
                throw new ArgumentException(message, nameof(method));
            }

            if(method.GetParameters().Any() || method.IsGenericMethod)
            {
                var message = String.Format(GetExceptionMessage("MethodMustNotHaveParameters"), method.DeclaringType, method.Name);
                throw new ArgumentException(message, nameof(method));
            }

            var accessor = GetAccessDelegate(method);

            return new AccessorFunctionAndType
            {
                AccessorFunction = accessor,
                ExpectedType = method.ReturnType,
            };
        }

        static Func<object,object> GetAccessDelegate(MethodInfo method)
        {
            var targetType = method.DeclaringType;
            var returnType = method.ReturnType;
            var genericFactory = typeof(ReflectionAccessorFunctionFactory).GetTypeInfo()
                .DeclaredMethods.Single(x => x.Name == nameof(GetAccessDelegateGeneric))
                .MakeGenericMethod(targetType, returnType);
            return (Func<object, object>) genericFactory.Invoke(null, new[] { method });
        }

        static Func<object,object> GetAccessDelegateGeneric<TTarget,TReturn>(MethodInfo method)
        {
            var @delegate = (Func<TTarget,TReturn>) method.CreateDelegate(typeof(Func<TTarget, TReturn>));
            return obj => @delegate((TTarget) obj);
        }

        static AccessorFunctionAndType GetAccessor(FieldInfo field)
        {
            return new AccessorFunctionAndType
            {
                AccessorFunction = obj => field.GetValue(obj),
                ExpectedType = field.FieldType,
            };
        }
    }
}