using System.Collections.Generic;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A manifest model object which supports a collection of polymorphic values.
    /// </summary>
    public interface IHasPolymorphicValues
    {
        /// <summary>
        /// Gets or sets a dictionary of polymorphic values, which describe the configuration of a validator
        /// for a type which is derived from the validated type of the current <see cref="Value"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Each key of this dictionary must be the name of a <c>System.Type</c>, as would return
        /// a type instance if used with <c>Type.GetType(string, bool)</c>.
        /// This means that the key might need to be a full assembly-qualified type name with the correct formatting
        /// for usage with the GetType method.
        /// </para>
        /// <para>
        /// If this string type name is incorrect then the exception raised upon conversion to a validation
        /// manifest will be the same exception as <c>Type.GetType(string, bool)</c> would raise,
        /// if the <c>throwOnError</c> parameter were set to <see langword="true" />.
        /// </para>
        /// </remarks>
        IDictionary<string,PolymorphicValue> PolymorphicValues { get; set; }
    }
}