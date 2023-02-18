using System;
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
        ManifestItem ManifestValue { get; }

        /// <summary>
        /// Gets a value indicating how long the validation process took.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value will only be populated if <see cref="ValidationOptions.InstrumentRuleExecution"/> is set to <see langword="true" />.
        /// Otherwise it will be <see langword="null" />.
        /// </para>
        /// <para>
        /// When <see cref="ValidationOptions.EnableRuleParallelization"/> is <see langword="true" /> the sum of the times reported within each
        /// <see cref="ValidationRuleResult.InstrumentationData"/> will likely not total the amount reported by this property.  That is because
        /// many of the reported times for each of the rules will have run concurrently with other rules.
        /// On the other hand, this time does correspond to real "wall clock" time to perform validation.
        /// </para>
        /// </remarks>
        TimeSpan? ValidationTime { get; }

        /// <summary>
        /// Gets a collection of the results of individual validation rules, making up
        /// the current validation result.
        /// </summary>
        IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }

        /// <summary>
        /// Gets a representation of the current queryable result as a strongly-typed queryable result.
        /// </summary>
        /// <typeparam name="T">The validated-type for the queryable result.</typeparam>
        /// <returns>The same conceptual queryable result, cast to a specified type.</returns>
        /// <exception cref="System.InvalidCastException">If the <typeparamref name="T"/> is incompatible with the validated type for the current results.</exception>
        IQueryableValidationResult<T> AsResultFor<T>();

        /// <summary>
        /// Gets a representation of the current result as a serializable validation result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Whilst the validation result types represented by <see cref="IQueryableValidationResult"/> &amp;
        /// <see cref="IQueryableValidationResult{TValidated}"/> are powerful and include querying/matching
        /// functionality, they are cumbersome to serialize and/or transmit via network APIs such as JSON or XML.
        /// The <see cref="SerializableValidationResult"/> trades the powerful functionality for a simplified,
        /// flattened object model which is easy to serialize, and convert to other formats such as JSON, XML
        /// or data to be persisted in a relational database.
        /// </para>
        /// </remarks>
        /// <returns>A copy of the current validation result, in an easily-serializable format.</returns>
        SerializableValidationResult ToSerializableResult();
    }
}