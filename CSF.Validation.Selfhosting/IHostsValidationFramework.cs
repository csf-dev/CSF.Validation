using CSF.Validation.Manifest;
using CSF.Validation.ManifestModel;
using CSF.Validation.ValidatorValidation;

namespace CSF.Validation
{
    /// <summary>
    /// An object which hosts a validation framework in a self-contained manner, for applications which do not use dependency injection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The recommended way in which to consume the CSF.Validation framework is via dependency injection, in which case the types
    /// within this assembly/package are not required.
    /// </para>
    /// <para>
    /// This interface acts as a root object providing access to the the entry-points into the validation framework.
    /// </para>
    /// </remarks>
    public interface IHostsValidationFramework
    {
        /// <summary>
        /// Gets an object from which validator instances may be created.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the primary property of use provided by <see cref="IHostsValidationFramework"/>.
        /// From here you may create validator instances from any of:
        /// </para>
        /// <list type="bullet">
        /// <item><description>Validator builders</description></item>
        /// <item><description>A validation manifest model</description></item>
        /// <item><description>A validation manifest</description></item>
        /// </list>
        /// <para>
        /// Validator builders are the recommended mechanism for creating validators.  For more information
        /// please read the documentation website.
        /// </para>
        /// </remarks>
        IGetsValidator ValidatorFactory { get; }

        /// <summary>
        /// Gets an object which may provide a <see cref="ValidationManifest"/> from a validator-builder.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Validator builders are types which implement <see cref="IBuildsValidator{TValidated}"/> and specify
        /// the definition/configuration of a validator in code.  This is the recommended way of specifying
        /// validators.
        /// </para>
        /// </remarks>
        IGetsManifestFromBuilder ManifestFromBuilderProvider { get; }

        /// <summary>
        /// Gets an object which may provide a <see cref="ValidationManifest"/> from a manifest model.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The manifest model is a simplified API, similar to a validation manifest but suitable for serializing
        /// to/from data such as relational databases or document structures such as JSON or XML.  This provides
        /// a mechanism for specifying validators-as-data.
        /// validators.
        /// </para>
        /// </remarks>
        IGetsValidationManifestFromModel ManifestFromModelProvider { get; }

        /// <summary>
        /// Gets an object which may be used to validate a validation manifest.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is essentially "a validator-validator".  Particularly of use if you are manipulating validation
        /// manifests directly, or if you are using the manifest model.  It allows developers to test their validators
        /// for common mistakes.
        /// </para>
        /// </remarks>
        IValidatesValidationManifest ManifestValidator { get; }

        /// <summary>
        /// Gets an object which may be used to serialize instances of manifest model <see cref="ManifestModel.Value"/> to/from JSON.
        /// </summary>
        ISerializesManifestModelToFromJson JsonSerializer { get; }

        /// <summary>
        /// Gets an object which may be used to serialize instances of manifest model <see cref="ManifestModel.Value"/> to/from XML.
        /// </summary>
        ISerializesManifestModelToFromXml XmlSerializer { get; }
    }
}