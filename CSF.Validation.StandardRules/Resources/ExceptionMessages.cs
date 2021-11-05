using System.Reflection;
using System.Resources;

namespace CSF.Validation.Rules.Resources
{
    /// <summary>
    /// A singleton service class which gets human-readable exception messages for the current assembly.
    /// </summary>
    internal static class ExceptionMessages
    {
        static readonly System.Type thisType = typeof(ExceptionMessages);
        static readonly ResourceManager resourceManager = new ResourceManager(thisType.FullName, thisType.GetTypeInfo().Assembly);

        /// <summary>
        /// Gets the named exception message.
        /// </summary>
        /// <param name="name">The exception message name.</param>
        /// <returns>The exception message string.</returns>
        internal static string GetExceptionMessage(string name) => resourceManager.GetString(name);
    }
}