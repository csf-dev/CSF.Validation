using System;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// An object which can provide a setter action/delegate which represents
    /// setting a value into a named property of a specified type.
    /// </summary>
    public interface IGetsPropertySetterAction
    {
        /// <summary>
        /// Gets the action delegate which may be used to set the property value.
        /// The first parameter of the delegate is the target object and the second parameter
        /// is the property value to set.
        /// </summary>
        /// <param name="type">The type of object for which to set the property value.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <returns>An action delegate.</returns>
        Action<object, object> GetSetterAction(Type type, string propertyName);
    }
}