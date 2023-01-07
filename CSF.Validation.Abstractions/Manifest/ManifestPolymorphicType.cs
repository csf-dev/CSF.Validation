using System;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model which represents how a specific runtime type should be validated, when performing polymorphic validation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Polymorphic validation is a technique whereby a developer may define/configure validation for an object of a general type.
    /// If, during validation, that object's runtime type is a more derived type then additional validation configuration - specific
    /// to that derived type - may be conditionally activated.
    /// </para>
    /// <para>
    /// For example, if we are validating an instance of <c>Person</c>, then ordinarily the validation manifest for that validation
    /// may only assume that the validated object is an instance of <c>Person</c>.  If an instance of this polymorphic manifest is
    /// added to the <see cref="ManifestValue"/> for the <c>Person</c>, representing a runtime type of (again, for example) <c>Employee</c>
    /// then that instance may configure further validation for the object, should its runtime type be <c>Employee</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="ManifestRule"/>
    /// <seealso cref="ValidationManifest"/>
    /// <seealso cref="ManifestRuleIdentifier"/>
    /// <seealso cref="ManifestValue"/>
    /// <seealso cref="RecursiveManifestValue"/>
    public class ManifestPolymorphicType : ManifestItem {}
}