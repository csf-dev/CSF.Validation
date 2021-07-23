using System.Reflection;
using System.Resources;

namespace CSF.Validation.Resources
{
    internal static class ExceptionMessages
    {
        static readonly ResourceManager resourceManager;

        internal static string GetExceptionMessage(string name) => resourceManager.GetString(name);

        static ExceptionMessages()
        {
            var thisTypeInfo = typeof(ExceptionMessages).GetTypeInfo();
            var thisAssembly = thisTypeInfo.Assembly;

            resourceManager = new ResourceManager("CSF.Validation.Resources.ExceptionMessages.resx", thisAssembly);
        }
    }
}