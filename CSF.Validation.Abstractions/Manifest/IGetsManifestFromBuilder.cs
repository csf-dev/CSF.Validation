namespace CSF.Validation.Manifest
{
    public interface IGetsManifestFromBuilder
    {
        ValidationManifest GetManifest<TValidated>(IBuildsValidator<TValidated> builder);
    }
}