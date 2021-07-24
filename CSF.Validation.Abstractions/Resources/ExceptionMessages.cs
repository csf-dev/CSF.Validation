using System.Reflection;
using System.Resources;

namespace CSF.Validation.Resources
{
    /// <summary>
    /// A singleton service class which gets human-readable exception messages for the current assembly.
    /// </summary>
    internal static class ExceptionMessages
    {
        static readonly ResourceManager resourceManager;

        /// <summary>
        /// Gets the named exception message.
        /// </summary>
        /// <param name="name">The exception message name.</param>
        /// <returns>The exception message string.</returns>
        internal static string GetExceptionMessage(string name) => resourceManager.GetString(name);

        static ExceptionMessages()
        {
            var thisTypeInfo = typeof(ExceptionMessages).GetTypeInfo();
            var thisAssembly = thisTypeInfo.Assembly;

            resourceManager = new ResourceManager("CSF.Validation.Resources.ExceptionMessages.resx", thisAssembly);
        }
    }
}