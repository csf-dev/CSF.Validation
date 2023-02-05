using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class ParentMustDeriveFromValidationManifestTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheParentIsNull(ParentMustDeriveFromValidationManifest sut,
                                                                    [ManifestModel] ManifestValue value,
                                                                    RuleContext context)
        {
            value.Parent = null;
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheParentIsAValidationManifest(ParentMustDeriveFromValidationManifest sut,
                                                                                   [ManifestModel] ManifestValue value,
                                                                                   [ManifestModel] ValidationManifest manifest,
                                                                                   RuleContext context)
        {
            value.Parent = manifest;
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheParentIsNotAValidationManifest(ParentMustDeriveFromValidationManifest sut,
                                                                                      [ManifestModel] ManifestValue value,
                                                                                      [ManifestModel] ManifestValue other,
                                                                                      RuleContext context)
        {
            value.Parent = other;
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(ParentMustDeriveFromValidationManifest sut,
                                                                     [ManifestModel] ManifestValue value,
                                                                     [RuleResult] ValidationRuleResult result)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The Parent property of a ManifestValue that is the root of a validation manifest must be an instance of ValidationManifest."));
        }
    }
}