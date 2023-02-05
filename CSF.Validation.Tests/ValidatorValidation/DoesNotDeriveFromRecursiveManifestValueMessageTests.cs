using NUnit.Framework;
using CSF.Validation.Rules;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorValidation
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class DoesNotDeriveFromRecursiveManifestValueMessageTests
    {
        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(DoesNotDeriveFromRecursiveManifestValueMessage sut,
                                                                     [RuleResult] ValidationRuleResult result,
                                                                     [ManifestModel] ManifestValue manifest)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(manifest, result),
                        Is.EqualTo("The RootValue property of a ValidationManifest must not be an object that derives from RecursiveManifestValue."));
        }
    }
}