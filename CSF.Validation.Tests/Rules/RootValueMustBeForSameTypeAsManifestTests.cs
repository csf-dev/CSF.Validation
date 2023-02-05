using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class RootValueMustBeForSameTypeAsManifestTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheParentValidatedTypeIsNull(RootValueMustBeForSameTypeAsManifest sut,
                                                                                 [ManifestModel] ManifestValue value,
                                                                                 [ManifestModel] ValidationManifest manifest,
                                                                                 [RuleContext] RuleContext context)
        {
            value.ValidatedType = typeof(int);
            manifest.ValidatedType = null;
            Assert.That(() => sut.GetResultAsync(value, manifest, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValidatedTypeIsNull(RootValueMustBeForSameTypeAsManifest sut,
                                                                           [ManifestModel] ManifestValue value,
                                                                           [ManifestModel] ValidationManifest manifest,
                                                                           [RuleContext] RuleContext context)
        {
            value.ValidatedType = null;
            manifest.ValidatedType = typeof(int);
            Assert.That(() => sut.GetResultAsync(value, manifest, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValidatedTypesAreCompatible(RootValueMustBeForSameTypeAsManifest sut,
                                                                                   [ManifestModel] ManifestValue value,
                                                                                   [ManifestModel] ValidationManifest manifest,
                                                                                   [RuleContext] RuleContext context)
        {
            value.ValidatedType = typeof(Pet);
            manifest.ValidatedType = typeof(Cat);
            Assert.That(() => sut.GetResultAsync(value, manifest, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheValidatedTypesAreNotCompatible(RootValueMustBeForSameTypeAsManifest sut,
                                                                                      [ManifestModel] ManifestValue value,
                                                                                      [ManifestModel] ValidationManifest manifest,
                                                                                      [RuleContext] RuleContext context)
        {
            value.ValidatedType = typeof(Cat);
            manifest.ValidatedType = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(value, manifest, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessage(RootValueMustBeForSameTypeAsManifest sut,
                                                                        [ManifestModel] ManifestValue value,
                                                                        [ManifestModel] ValidationManifest manifest,
                                                                        [RuleResult] ValidationRuleResult result)
        {
            value.ValidatedType = typeof(Cat);
            manifest.ValidatedType = typeof(Pet);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, manifest, result),
                        Is.EqualTo(@"The ValidatedType of a ValidationManifest must be be assignable to the ValidatedType of the ManifestValue used as the manifest's RootValue.
ValidationManifest.ValidatedType = CSF.Validation.Rules.RootValueMustBeForSameTypeAsManifestTests+Pet
ValidationManifest.RootValue.ValidatedType = CSF.Validation.Rules.RootValueMustBeForSameTypeAsManifestTests+Cat"));
        }

        public class Pet {}

        public class Cat : Pet {}
    }
}