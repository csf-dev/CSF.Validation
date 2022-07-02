using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which can get the generic validated type, given the <see cref="Type"/> of
    /// a validator builder class.
    /// </summary>
    public interface IGetsValidatedTypeForBuilderType
    {
        /// <summary>
        /// Gets the generic validated type appropriate for the specified <paramref name="builderType"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A builder type is one which implements the interface <see cref="IBuildsValidator{TValidated}"/>.
        /// This method gets the value of the generic type parameter used for that interface.
        /// </para>
        /// <para>
        /// In order to be used with this method, the <paramref name="builderType"/> must implement
        /// <see cref="IBuildsValidator{TValidated}"/> for precisely one generic type.
        /// If the builder type implements the interface more than once then this method will be unable to return
        /// a definitive result and will throw an exception.
        /// </para>
        /// </remarks>
        /// <param name="builderType">A type that indicates a validator builder.</param>
        /// <returns>The <see cref="Type"/> for which the specified builder type validates.</returns>
        Type GetValidatedType(Type builderType);
    }
}