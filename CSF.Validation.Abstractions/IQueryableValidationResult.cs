using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation
{
    /// <summary>
    /// An object which contains validation rule results, which may be queried for specific results.
    /// </summary>
    public interface IQueryableValidationResult : IEnumerable<ValidationRuleResult>
    {
        /// <summary>
        /// Gets a value that indicates whether or not the current instance represents passing validation.
        /// </summary>
        bool Passed { get; }

        /// <summary>
        /// Gets a reference to the manifest value which forms the logical
        /// root of the results in the current instance.
        /// </summary>
        ManifestValueBase ManifestValue { get; }

        /// <summary>
        /// Gets a collection of the results of individual validation rules, making up
        /// the current validation result.
        /// </summary>
        IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }
    }
}