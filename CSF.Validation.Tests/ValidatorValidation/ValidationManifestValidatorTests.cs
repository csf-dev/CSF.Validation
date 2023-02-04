using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.ValidatorValidation
{
    [TestFixture,Parallelizable]
    public class ValidationManifestValidatorTests
    {
        [Test,AutoMoqData]
        public void ValidateAsyncShouldReturnPassResultForItsOwnManifest([IntegrationTesting] IValidatesValidationManifest sut)
        {
            var options = new ValidationOptions { RuleThrowingBehaviour = RuleThrowingBehaviour.Never };
            Assert.That(async () => await sut.ValidateAsync(new ValidationManifestValidatorBuilder(), options), Is.PassingValidationResult);
        }
    }
}