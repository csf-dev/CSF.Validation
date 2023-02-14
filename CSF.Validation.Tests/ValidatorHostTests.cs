using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class ValidatorHostTests
    {
        [Test, Category(TestCategory.Integration)]
        public void BuildShouldBeAbleToBuildAnObjectForWhichAllPropertiesAreAccessible()
        {
            var host = ValidatorHost.Build();

            Assert.Multiple(() =>
            {
                Assert.That(() => host.ValidatorFactory, Throws.Nothing, nameof(ValidatorHost.ValidatorFactory));
                Assert.That(() => host.ManifestFromBuilderProvider, Throws.Nothing, nameof(ValidatorHost.ManifestFromBuilderProvider));
                Assert.That(() => host.ManifestFromModelProvider, Throws.Nothing, nameof(ValidatorHost.ManifestFromModelProvider));
                Assert.That(() => host.ManifestValidator, Throws.Nothing, nameof(ValidatorHost.ManifestValidator));
            });
        }
    }
}