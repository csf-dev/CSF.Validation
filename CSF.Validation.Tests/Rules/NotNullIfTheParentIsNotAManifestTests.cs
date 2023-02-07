using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class NotNullIfTheParentIsNotAManifestTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheParentValueIsAValidationManifest(NotNullIfTheParentIsNotAManifest sut,
                                                                                        [ManifestModel] ManifestItem parent,
                                                                                        [RuleContext] RuleContext context)
        {
            parent.Parent = new ValidationManifest();
            Assert.That(() => sut.GetResultAsync(null, parent, context), Is.PassingRuleResult);
        }

        [Test, AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValueIsNotNull(NotNullIfTheParentIsNotAManifest sut,
                                                                      object value,
                                                                      [ManifestModel] ManifestItem parent,
                                                                      [ManifestModel] ManifestItem grandParent,
                                                                      [RuleContext] RuleContext context)
        {
            parent.Parent = grandParent;
            Assert.That(() => sut.GetResultAsync(value, parent, context), Is.PassingRuleResult);
    }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheValueIsNull(NotNullIfTheParentIsNotAManifest sut,
                                                                   [ManifestModel] ManifestItem parent,
                                                                   [ManifestModel] ManifestItem grandParent,
                                                                   [RuleContext] RuleContext context)
        {
            parent.Parent = grandParent;
            Assert.That(() => sut.GetResultAsync(null, parent, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(NotNullIfTheParentIsNotAManifest sut,
                                                                     object value,
                                                                     [ManifestModel] ManifestItem parent,
                                                                     [RuleResult] ValidationRuleResult result)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(value, parent, result),
                        Is.EqualTo("The value must not be null if the ManifestItem is not the root of a ValidationManifest."));
        }
    }
}