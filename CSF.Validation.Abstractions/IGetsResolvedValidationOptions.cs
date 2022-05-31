namespace CSF.Validation
{
    /// <summary>
    /// An object which can get the resolved/final/effective validation options, combining all of the ways
    /// in which they might be specified.
    /// </summary>
    /// <remarks>
    /// <para>
    /// As you will notice, the <see cref="ResolvedValidationOptions"/> class has the same 'shape' as
    /// <see cref="ValidationOptions"/>, except that every property is non-nullable.
    /// The resolved options are created by taking the first non-null value from each of:
    /// </para>
    /// <list type="number">
    /// <item><description>The specified options</description></item>
    /// <item><description>The default options configured at DI registration</description></item>
    /// <item><description>Hard-coded default options (if nothing else is specified)</description></item>
    /// </list>
    /// </remarks>
    public interface IGetsResolvedValidationOptions
    {
        /// <summary>
        /// Gets the resolved validation options.
        /// </summary>
        /// <param name="specifiedOptions">Options which have been specified by a developer via the API; may be <see langword="null" />.</param>
        /// <returns>The resolved (or effective) validation options.</returns>
        ResolvedValidationOptions GetResolvedValidationOptions(ValidationOptions specifiedOptions);
    }
}