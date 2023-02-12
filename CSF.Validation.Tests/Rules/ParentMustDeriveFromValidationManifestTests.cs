using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class ParentMustDeriveFromValidationManifestTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheRootItemIsNull(RootManifestValueMustHaveParentThatIsValidationManifest sut,
                                                                      [ManifestModel] ValidationManifest value,
                                                                      RuleContext context)
        {
            value.RootValue = null;
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheRootItemParentIsNull(RootManifestValueMustHaveParentThatIsValidationManifest sut,
                                                                            [ManifestModel] ValidationManifest value,
                                                                            [ManifestModel] ManifestItem item,
                                                                            RuleContext context)
        {
            value.RootValue = item;
            value.RootValue.Parent = null;
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheParentIsTheSameValidationManifest(RootManifestValueMustHaveParentThatIsValidationManifest sut,
                                                                                         [ManifestModel] ManifestItem value,
                                                                                         [ManifestModel] ValidationManifest manifest,
                                                                                         RuleContext context)
        {
            value.Parent = manifest;
            manifest.RootValue = value;
            Assert.That(() => sut.GetResultAsync(manifest, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheParentIsNotAValidationManifest(RootManifestValueMustHaveParentThatIsValidationManifest sut,
                                                                                      [ManifestModel] ManifestItem value,
                                                                                      [ManifestModel] ManifestItem other,
                                                                                      [ManifestModel] ValidationManifest manifest,
                                                                                      RuleContext context)
        {
            manifest.RootValue = value;
            value.Parent = other;
            Assert.That(() => sut.GetResultAsync(manifest, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(RootManifestValueMustHaveParentThatIsValidationManifest sut,
                                                                     [ManifestModel] ValidationManifest value,
                                                                     [RuleResult] ValidationRuleResult result)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The Parent property of a ManifestItem that is the root of a validation manifest must be a reference to that same validation manifest."));
        }
    }
}