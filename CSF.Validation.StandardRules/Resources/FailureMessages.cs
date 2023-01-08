using System.Reflection;
using System.Resources;

namespace CSF.Validation.Resources
{
    /// <summary>
    /// A singleton service class which gets human-readable validation failure messages for the current assembly.
    /// </summary>
    internal static class FailureMessages
    {
        static readonly System.Type thisType = typeof(FailureMessages);
        static readonly ResourceManager resourceManager = new ResourceManager(thisType.FullName, thisType.GetTypeInfo().Assembly);

        /// <summary>
        /// Gets the named failure message.
        /// </summary>
        /// <param name="name">The failure message name.</param>
        /// <returns>The failure message string.</returns>
        internal static string GetFailureMessage(string name) => resourceManager.GetString(name);
    }
}