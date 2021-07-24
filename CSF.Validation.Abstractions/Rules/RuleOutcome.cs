namespace CSF.Validation.Rules
{
    /// <summary>
    /// Enumerates the possible outcomes from an attempt to run a single validation rule.
    /// </summary>
    public enum RuleOutcome
    {
        /// <summary>
        /// The rule executed and it explicitly passed.
        /// </summary>
        Passed = 1,

        /// <summary>
        /// The rule executed and it explicitly failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The rule was not executed because one or more of its dependency rules had failed.  This is
        /// treated in the same way as if the rule had <see cref="Failed"/> but it is distinct because
        /// the way that the validation result is communicated (such as to a user) might differ.
        /// </summary>
        DependencyFailed,

        /// <summary>
        /// The rule partially executed but raised an exception.
        /// </summary>
        Errored
    }
}