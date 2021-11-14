using System;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture,Parallelizable]
    public class ValidationManifestFromModelConverterTests
    {
        [Test,AutoMoqData]
        public void GetValidationManifestShouldGetAValidationManifestUsingServices([Frozen] IConvertsModelValuesToManifestValues valueConverter,
                                                                                   [Frozen] IConvertsModelRulesToManifestRules ruleConverter,
                                                                                   ValidationManifestFromModelConverter sut,
                                                                                   [ManifestModel] Value rootValue,
                                                                                   Type validatedType,
                                                                                   [ManifestModel] ModelToManifestValueConversionResult conversionResult)
        {
            Mock.Get(valueConverter)
                .Setup(x => x.ConvertAllValues(It.Is<ModelToManifestConversionContext>(c => c.CurrentValue == rootValue && c.ValidatedType == validatedType)))
                .Returns(conversionResult);

            var result = sut.GetValidationManifest(rootValue, validatedType);

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Property(nameof(ValidationManifest.RootValue)).SameAs(conversionResult.RootValue), "Correct root value");
                Assert.That(result, Has.Property(nameof(ValidationManifest.ValidatedType)).SameAs(validatedType), "Correct validated type");
            });

            Mock.Get(ruleConverter).Verify(x => x.ConvertAllRulesAndAddToManifestValues(conversionResult.ConvertedValues), Times.Once, "Rule converter was used correctly");
        }
    }
}