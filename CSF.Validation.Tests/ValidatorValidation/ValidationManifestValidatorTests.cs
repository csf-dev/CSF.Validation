using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.ValidatorValidation
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValidationManifestValidatorTests
    {
        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnPassResultForItsOwnManifest([IntegrationTesting] IValidatesValidationManifest sut)
        {
            var options = new ValidationOptions { RuleThrowingBehaviour = RuleThrowingBehaviour.Never };
            var result = await sut.ValidateAsync(new ValidationManifestValidatorBuilder(), options);
            Assert.That(result, Is.PassingValidationResult);
        }
    }
}