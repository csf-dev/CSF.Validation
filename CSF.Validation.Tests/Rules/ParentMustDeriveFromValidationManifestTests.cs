using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class ParentMustDeriveFromValidationManifestTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheParentIsNull(ParentMustDeriveFromValidationManifest sut,
                                                                    [ManifestModel] ManifestItem value,
                                                                    RuleContext context)
        {
            value.Parent = null;
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheParentIsAValidationManifest(ParentMustDeriveFromValidationManifest sut,
                                                                                   [ManifestModel] ManifestItem value,
                                                                                   [ManifestModel] ValidationManifest manifest,
                                                                                   RuleContext context)
        {
            value.Parent = manifest;
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheParentIsNotAValidationManifest(ParentMustDeriveFromValidationManifest sut,
                                                                                      [ManifestModel] ManifestItem value,
                                                                                      [ManifestModel] ManifestItem other,
                                                                                      RuleContext context)
        {
            value.Parent = other;
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(ParentMustDeriveFromValidationManifest sut,
                                                                     [ManifestModel] ManifestItem value,
                                                                     [RuleResult] ValidationRuleResult result)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The Parent property of a ManifestItem that is the root of a validation manifest must be an instance of ValidationManifest."));
        }
    }
}