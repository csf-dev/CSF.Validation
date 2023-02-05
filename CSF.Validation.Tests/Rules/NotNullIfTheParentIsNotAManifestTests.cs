using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class NotNullIfTheParentIsNotAManifestTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheParentValueIsAValidationManifest(NotNullIfTheParentIsNotAManifest sut,
                                                                                        [ManifestModel] ManifestValue parent,
                                                                                        [RuleContext] RuleContext context)
        {
            parent.Parent = new ValidationManifest();
            Assert.That(() => sut.GetResultAsync(null, parent, context), Is.PassingRuleResult);
        }

        [Test, AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValueIsNotNull(NotNullIfTheParentIsNotAManifest sut,
                                                                      object value,
                                                                      [ManifestModel] ManifestValue parent,
                                                                      [ManifestModel] ManifestValue grandParent,
                                                                      [RuleContext] RuleContext context)
        {
            parent.Parent = grandParent;
            Assert.That(() => sut.GetResultAsync(value, parent, context), Is.PassingRuleResult);
    }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheValueIsNull(NotNullIfTheParentIsNotAManifest sut,
                                                                   [ManifestModel] ManifestValue parent,
                                                                   [ManifestModel] ManifestValue grandParent,
                                                                   [RuleContext] RuleContext context)
        {
            parent.Parent = grandParent;
            Assert.That(() => sut.GetResultAsync(null, parent, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(NotNullIfTheParentIsNotAManifest sut,
                                                                     object value,
                                                                     [ManifestModel] ManifestValue parent,
                                                                     [RuleResult] ValidationRuleResult result)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(value, parent, result),
                        Is.EqualTo("The value must not be null if the ManifestValue is not the root of a ValidationManifest."));
        }
    }
}