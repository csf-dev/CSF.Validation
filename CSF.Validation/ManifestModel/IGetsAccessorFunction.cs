using System;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// An object which can provide an accessor function which represents
    /// getting a value from a named member of a specified type.
    /// </summary>
    public interface IGetsAccessorFunction
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
        AccessorFunctionAndType GetAccessorFunction(Type type, string memberName);
    }
}