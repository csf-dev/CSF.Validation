using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder service which may be used to specify a function that is
    /// used to get the identity of a validated object.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    public interface IConfiguresObjectIdentity<out T>
    {
        /// <summary>
        /// Specifies an accessor function which should be used to get the identity of the validated object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Object identities are useful &amp; important when validating complex object models which include
        /// object graphs (tree-structures) of related/connected objects.  This is particularly true where
        /// collections are child objects are involved.
        /// The identity is used to differentiate object instances.  For example, so that the validation rule
        /// results may be matched back to the object to which they relate.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// For example, if validating an instance of the following object:
        /// </para>
        /// <code>
        /// using System;
        /// using System.Collections.Generic;
        /// 
        /// public class HumanBeing
        /// {
        ///     public Guid Identity { get; set; } = new Guid();
        ///     public string Name { get; set; }
        ///     public List&lt;HumanBeing&gt; Children { get; } = new List&lt;HumanBeing&gt;();
        /// }
        /// </code>
        /// <para>
        /// Without any kind of identity, if we had a validation rule that validated the <c>Name</c> property is not null
        /// or whitespace-only, then what if more than one child failed this same rule?  In our feedback (for example some
        /// user interface), how would we indicate which children failed the validation and which children did not.
        /// </para>
        /// <para>
        /// By using (for example) the <c>Identity</c> property to uniquely identity each instance of <c>HumanBeing</c>
        /// we will recieve the related identities back with the validation rule results, allowing us to associate the
        /// results back to the appropriate object within the object graph.
        /// </para>
        /// </example>
        /// <param name="identityAccessor">The accessor function.</param>
        void UseObjectIdentity(Func<T, object> identityAccessor);
    }
}