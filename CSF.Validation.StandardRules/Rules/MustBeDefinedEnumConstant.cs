using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which asserts that the validated value (which may be nullable) is a defined member
    /// of the specified enumerated type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In order to use this validation rule, either of <see cref="EnumType"/> or <see cref="EnumTypeName"/> must be set to a not-<see langword="null" />
    /// value.  If <see cref="EnumType"/> is not <see langword="null" /> then this will be used as the type of the enum against which to validate the
    /// validated value, and <see cref="EnumTypeName"/> will not be used.
    /// If <see cref="EnumType"/> is <see langword="null" /> then the <see cref="EnumTypeName"/> must not be <see langword="null" /> and it must correspond
    /// to a type that could be loaded via <see cref="Type.GetType(string)"/>.
    /// </para>
    /// <para>
    /// This rule will raise an exception if both <see cref="EnumType"/> &amp; <see cref="EnumTypeName"/> are <see langword="null" /> or if <see cref="Type.GetType(string)"/>
    /// raises any exception when <see cref="EnumTypeName"/> is used as its parameter, or if <see cref="Type.GetType(string)"/> returns a <see langword="null" />
    /// result.
    /// </para>
    /// <para>
    /// This validation rule is also compatible with nullable values of enum types, and will return a passing
    /// result if the validated value is <see langword="null" />.
    /// </para>
    /// </remarks>
    public class MustBeDefinedEnumConstant : IRuleWithMessage<object>
    {
        const string enumTypeKey = "Enum type";

        /// <summary>
        /// Gets or sets a <see cref="System.Type"/> of an enum type for which to validate the validated value is a
        /// defined member.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this value is non-<see langword="null" /> then <see cref="EnumTypeName"/> will not be used.
        /// </para>
        /// </remarks>
        public Type EnumType { get; set; }

        /// <summary>
        /// Gets or sets a string which used to get a <see cref="System.Type"/> of an enum type for which to validate
        /// the validated value is a defined member.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is only used if <see cref="EnumType"/> is <see langword="null" />.
        /// </para>
        /// </remarks>
        public string EnumTypeName { get; set; }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
        {
            if(Equals(validated, null)) return PassAsync();
            var enumType = GetEnumType();
            return Enum.IsDefined(enumType, validated)
                ? PassAsync(new Dictionary<string, object> {{enumTypeKey, enumType}})
                : FailAsync(new Dictionary<string, object> {{enumTypeKey, enumType}});
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(object value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = String.Format(Resources.FailureMessages.GetFailureMessage("MustBeDefinedEnumConstant"),
                                        result.Data[enumTypeKey]);
            return Task.FromResult(message);
        }

        Type GetEnumType() 
        {
            if(EnumType != null) return EnumType;
            
            try
            {
                var type = Type.GetType(EnumTypeName);
                if(type != null) return type;
            }
            catch(Exception e)
            {
                throw new InvalidOperationException(GetExceptionMessage(), e);
            }

            throw new InvalidOperationException(GetExceptionMessage());
        }

        static string GetExceptionMessage()
            => String.Format(Resources.ExceptionMessages.GetExceptionMessage("TypeNotFound"),
                             nameof(EnumType),
                             nameof(EnumTypeName),
                             $"{nameof(Type)}.{nameof(Type.GetType)}({nameof(String)})");
    }
}