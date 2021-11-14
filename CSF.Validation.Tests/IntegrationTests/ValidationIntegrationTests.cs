using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.IntegrationTests
{
    [TestFixture,Parallelizable]
    public class ValidationIntegrationTests
    {
        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnPassResultForValidObject([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Person>(typeof(PersonValidatorBuilder));

            var person = new Person
            {
                Name = "Bobby",
                Birthday = new DateTime(2000, 1, 1),
                Pets = new[] {
                    new Pet {
                        Name = "Miffles",
                        NumberOfLegs = 4,
                        Type = "Cat"
                    },
                },
            };

            var result = await validator.ValidateAsync(person);

            Assert.That(result.Passed, Is.True);
        }
    }
}