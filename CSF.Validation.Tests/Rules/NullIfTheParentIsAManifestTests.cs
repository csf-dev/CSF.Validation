using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class NullIfTheParentIsAManifestTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheParentValueIsNotAValidationManifest(NullIfTheParentIsAManifest sut,
                                                                                           [ManifestModel] ManifestValue parent,
                                                                                           [ManifestModel] ManifestValue grandParent,
                                                                                           object value,
                                                                                           [RuleContext] RuleContext context)
        {
            parent.Parent = grandParent;
            Assert.That(() => sut.GetResultAsync(value, parent, context), Is.PassingRuleResult);
        }

        [Test, AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValueIsNull(NullIfTheParentIsAManifest sut,
                                                                   [ManifestModel] ManifestValue parent,
                                                                   [ManifestModel] ValidationManifest manifest,
                                                                   [RuleContext] RuleContext context)
        {
            parent.Parent = manifest;
            Assert.That(() => sut.GetResultAsync(null, parent, context), Is.PassingRuleResult);
    }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheValueIsNotNull(NullIfTheParentIsAManifest sut,
                                                                      [ManifestModel] ManifestValue parent,
                                                                      [ManifestModel] ValidationManifest manifest,
                                                                      object value,
                                                                      [RuleContext] RuleContext context)
        {
            parent.Parent = manifest;
            Assert.That(() => sut.GetResultAsync(value, parent, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(NullIfTheParentIsAManifest sut,
                                                                     object value,
                                                                     [ManifestModel] ManifestValue parent,
                                                                     [RuleResult] ValidationRuleResult result)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(value, parent, result),
                        Is.EqualTo("The value must be null if the Parent is an instance of ValidationManifest."));
        }
    }
}