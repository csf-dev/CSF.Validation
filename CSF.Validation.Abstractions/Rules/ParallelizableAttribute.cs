using System;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Attribute used to mark rule classes which may be executed in parallel with other rules.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Validation rules are only executed in parallel if they are BOTH decorated with this attribute and
    /// <see cref="ValidationOptions.EnableRuleParallelization"/> is set to <see langword="true" />.
    /// </para>
    /// <para>
    /// If a validation rule is not suitable for execution in parallel with other rules then do not decorate
    /// it with this attribute.  This ensures that is run in serial, even when
    /// <see cref="ValidationOptions.EnableRuleParallelization"/> is enabled.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ParallelizableAttribute : Attribute {}
}