using System.Linq;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using CSF.Validation.Stubs;
using NUnit.Framework;

namespace CSF.Validation.ValidatorValidation
{
    [TestFixture, NUnit.Framework.Parallelizable, Category(TestCategory.Integration)]
    public class ValidationManifestValidatorTests
    {
        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnPassResultForItsOwnManifest([IntegrationTesting] IValidatesValidationManifest sut)
        {
            var options = new ValidationOptions { RuleThrowingBehaviour = RuleThrowingBehaviour.Never };
            var result = await sut.ValidateAsync(new ValidationManifestValidatorBuilder(), options);
            Assert.That(result, Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnFailingResultForAValidationManifestWithAnIncompatibleRule([IntegrationTesting] IValidatesValidationManifest sut)
        {
            var options = new ValidationOptions { RuleThrowingBehaviour = RuleThrowingBehaviour.Never };
            var result = await sut.ValidateAsync(GetManifestThatIncludesIncompatibleRule(), options);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.FailingValidationResult, "Result is an overall failure");
                Assert.That(result?.RuleResults,
                            Has.One.Matches<ValidationRuleResult>(r => r.Outcome == RuleOutcome.Failed),
                            "Includes only one failing rule");
                Assert.That(result?.RuleResults,
                            Has.One.Matches<ValidationRuleResult>(r => r.Identifier.RuleType == typeof(RuleMustImplementCompatibleValidationLogic)
                                                                    && r.Outcome == RuleOutcome.Failed),
                            "Includes a failure for the incompatible-rule-type rule");
            });
        }

        static ValidationManifest GetManifestThatIncludesIncompatibleRule()
        {
            var manifest = new ValidationManifest
            {
                ValidatedType = typeof(ValidatedObject),
                RootValue = new ManifestItem
                {
                    ValidatedType = typeof(ValidatedObject),
                    Children = new []
                    {
                        new ManifestItem
                        {
                            ValidatedType = typeof(string),
                            AccessorFromParent = p => ((ValidatedObject) p).AProperty,
                            MemberName = nameof(ValidatedObject.AProperty),
                        }
                    },
                },
            };
            manifest.RootValue.Parent = manifest;
            var propertyValue = manifest.RootValue.Children.First();
            propertyValue.Parent = manifest.RootValue;
            propertyValue.Rules.Add(new ManifestRule(propertyValue, new ManifestRuleIdentifier(propertyValue, typeof(IntegerRule))));
            return manifest;
        }
    }
}