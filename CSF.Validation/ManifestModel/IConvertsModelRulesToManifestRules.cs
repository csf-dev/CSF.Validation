using System.Collections.Generic;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// An object which may be used to convert the rules within a <see cref="Value"/> into manifest rules
    /// and to add those rules to a corresponding <see cref="CSF.Validation.Manifest.ManifestValue"/>
    /// </summary>
    public interface IConvertsModelRulesToManifestRules
    {
        /// <summary>
        /// Converts all of the rules present in each of the <see cref="Value"/> models into
        /// <see cref="CSF.Validation.Manifest.ManifestRule"/> instances and then adds those rules to
        /// the corresponding <see cref="CSF.Validation.Manifest.ManifestValue"/> instance in
        /// the model/manifest value pair.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The input parameter is a collection of objects, each of which contains both a
        /// <see cref="CSF.Validation.Manifest.ManifestValue"/> and also the model <see cref="Value"/>
        /// from which that manifest value was created.  This method gets the rules from within
        /// that model value, converts them and then adds those the the corresponding manifest value.
        /// This process occurs for each item in the input collection.
        /// </para>
        /// </remarks>
        /// <param name="values">A collection of <see cref="ModelAndManifestValuePair"/>.</param>
        void ConvertAllRulesAndAddToManifestValues(IEnumerable<ModelAndManifestValuePair> values);
    }
}