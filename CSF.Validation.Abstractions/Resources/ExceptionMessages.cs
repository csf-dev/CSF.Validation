using System.Resources;

namespace CSF.Validation.Resources
{
    internal static class ExceptionMessages
    {
        static readonly ResourceManager resourceManager = new ResourceManager("CSF.Validation.Resources.ExceptionMessages.resx", typeof(ExceptionMessages).Assembly);

        internal static string GetMessage(string name) => resourceManager.GetString(name);
    }
}