using System;
using System.Linq;
using System.Reflection;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A factory service which gets an accessor function via reflection.
    /// </summary>
    public class ReflectionDelegateFactory : IGetsAccessorFunction, IGetsPropertySetterAction
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
                throw GetArgumentException("MissingMember", type, memberName, nameof(memberName));

            if(member is PropertyInfo prop)
                return GetAccessor(prop);
            if(member is FieldInfo field)
                return GetAccessor(field);
            if(member is MethodInfo method)
                return GetAccessor(method);

            throw GetArgumentException("MustBeAGettableMember", type, memberName, nameof(memberName));
        }

        /// <summary>
        /// Gets the action delegate which may be used to set the property value.
        /// The first parameter of the delegate is the target object and the second parameter
        /// is the property value to set.
        /// </summary>
        /// <param name="type">The type of object for which to set the property value.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <returns>An action delegate.</returns>
        public Action<object, object> GetSetterAction(Type type, string propertyName)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            var property = type.GetTypeInfo().DeclaredProperties.FirstOrDefault(x => x.Name == propertyName);
            if(property is null)
                throw GetArgumentException("MissingProperty", type, propertyName, nameof(propertyName));

            if(!property.CanWrite)
                throw GetArgumentException("PropertyMustBeWritable", property, nameof(propertyName));
            if(property.GetIndexParameters().Any())
                throw GetArgumentException("PropertyMustNotBeIndexer", property, nameof(propertyName));

            return GetSetterDelegate(property.SetMethod);
        }

        static AccessorFunctionAndType GetAccessor(PropertyInfo property)
        {
            if(!property.CanRead)
                throw GetArgumentException("PropertyMustBeReadable", property, nameof(property));
            if(property.GetIndexParameters().Any())
                throw GetArgumentException("PropertyMustNotBeIndexer", property, nameof(property));
            
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
                throw GetArgumentException("MethodMustHaveNonVoidReturn", method, nameof(method));
            if(method.GetParameters().Any() || method.IsGenericMethod)
                throw GetArgumentException("MethodMustNotHaveParameters", method, nameof(method));

            var accessor = GetAccessDelegate(method);

            return new AccessorFunctionAndType
            {
                AccessorFunction = accessor,
                ExpectedType = method.ReturnType,
            };
        }

        static AccessorFunctionAndType GetAccessor(FieldInfo field)
        {
            return new AccessorFunctionAndType
            {
                AccessorFunction = obj => field.GetValue(obj),
                ExpectedType = field.FieldType,
            };
        }

        static Func<object,object> GetAccessDelegate(MethodInfo method)
        {
            var targetType = method.DeclaringType;
            var returnType = method.ReturnType;
            var genericFactory = typeof(ReflectionDelegateFactory).GetTypeInfo()
                .DeclaredMethods.Single(x => x.Name == nameof(GetAccessDelegateGeneric))
                .MakeGenericMethod(targetType, returnType);
            return (Func<object, object>) genericFactory.Invoke(null, new[] { method });
        }

        static Func<object,object> GetAccessDelegateGeneric<TTarget,TReturn>(MethodInfo method)
        {
            var @delegate = (Func<TTarget,TReturn>) method.CreateDelegate(typeof(Func<TTarget, TReturn>));
            return obj => @delegate((TTarget) obj);
        }

        static Action<object,object> GetSetterDelegate(MethodInfo method)
        {
            var targetType = method.DeclaringType;
            var valueType = method.GetParameters().Single().ParameterType;
            var genericFactory = typeof(ReflectionDelegateFactory).GetTypeInfo()
                .DeclaredMethods.Single(x => x.Name == nameof(GetSetterDelegateGeneric))
                .MakeGenericMethod(targetType, valueType);
            return (Action<object, object>) genericFactory.Invoke(null, new[] { method });
        }

        static Action<object,object> GetSetterDelegateGeneric<TTarget,TValue>(MethodInfo method)
        {
            var @delegate = (Action<TTarget,TValue>) method.CreateDelegate(typeof(Action<TTarget, TValue>));
            return (obj, val) => @delegate((TTarget) obj, (TValue) val);
        }

        static ArgumentException GetArgumentException(string messageTemplateName, MemberInfo member, string paramName)
            => GetArgumentException(messageTemplateName, member.DeclaringType, member.Name, paramName);

        static ArgumentException GetArgumentException(string messageTemplateName, Type type, string memberName, string paramName)
        {
            var message = String.Format(GetExceptionMessage(messageTemplateName), type.Name, memberName);
            return new ArgumentException(message, paramName);
        }
    }
}