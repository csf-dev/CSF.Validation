using System;

namespace CSF.Validation
{
    public static class SamplePluginAssembly
    {
        /// <summary>
        /// Gets the assembly-qualified type name of the <c>AssemblyMarker</c> type in the sample plugin assembly.
        /// </summary>
        internal const string AssemblyMarkerAqn = "CSF.Validation.AssemblyMarker, CSF.Validation.Tests.SamplePlugin";

        /// <summary>
        /// Gets the assembly-qualified type name of the <c>SampleRule</c> type in the sample plugin assembly.
        /// </summary>
        internal const string SampleRuleAqn = "CSF.Validation.SampleRule, CSF.Validation.Tests.SamplePlugin";

        public static Type GetAssemblyMarkerType() => Type.GetType(AssemblyMarkerAqn);

        public static System.Reflection.Assembly GetAssembly() => GetAssemblyMarkerType().Assembly;
    }
}