using System;

namespace CSF.Validation
{
    /// <summary>
    /// A small model which represents instrumentation data about the execution of a single
    /// validation rule.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In order to collect this data, you must enable <see cref="ValidationOptions.InstrumentRuleExecution"/>.
    /// </para>
    /// </remarks>
    public class RuleInstrumentationData
    {
        /// <summary>
        /// Gets a value indicating how long this rule took to execute.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that if <see cref="ParallelizationEnabled"/> is <see langword="true" /> then this time may
        /// have run concurrently with the execution of other rules.  That means that the sum of time shown
        /// by this property across all rules executed will not be equal to "real world" (or "wall clock")
        /// time.
        /// </para>
        /// </remarks>
        public TimeSpan RuleExecutionTime { get; }

        /// <summary>
        /// Gets a value indicating how long it took to generate a validation feedback message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that this property value will be <see langword="null" /> if either the generation of messages is
        /// disabled via the validation options, or if the associated validation rule result did not have any
        /// feedback message applicable (typical of rules which pass).
        /// </para>
        /// </remarks>
        public TimeSpan? MessageGenerationTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not the associated validation rule was executed using
        /// a parallel-execution strategy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The value of this property is not in fact a 100% guarantee that the associated rule logic executed in
        /// parallel with other rules, nor does it give an indication of how many rules executed concurrently with
        /// the associated rule.
        /// </para>
        /// <para>
        /// This property will be set to <see langword="true" /> if <see cref="ValidationOptions.EnableRuleParallelization"/>
        /// was <see langword="true" /> and the rule was marked with <see cref="Rules.ParallelizableAttribute"/>.
        /// This will mean that the rule's execution would have been placed into a "task pool" of rules to be executed in
        /// parallel.
        /// The actual order in which rules run and how many rules may run concurrently is unpredictable though,
        /// which is why this value provides no certainty that parallelization actually occurred.
        /// The parallel rule-execution algorithm used by CSF.Validation attempts to run as many
        /// rules in parallel as possible, when the feature is enabled by the options.
        /// </para>
        /// </remarks>
        public bool ParallelizationEnabled { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleInstrumentationData"/>.
        /// </summary>
        /// <param name="parallelizationEnabled">Whether or not parallelization was enabled for the rule.</param>
        /// <param name="ruleExecutionTime">The time the rule took to execute.</param>
        /// <param name="messageGenerationTime">The time it took to generate the rule's feedback message.</param>
        public RuleInstrumentationData(bool parallelizationEnabled, TimeSpan ruleExecutionTime, TimeSpan? messageGenerationTime = null)
        {
            ParallelizationEnabled = parallelizationEnabled;
            RuleExecutionTime = ruleExecutionTime;
            MessageGenerationTime = messageGenerationTime;
        }
    }
}