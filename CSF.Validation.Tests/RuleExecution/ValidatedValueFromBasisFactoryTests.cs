using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class ValidatedValueFromBasisFactoryTests
    {
        [Test,AutoMoqData]
        public void GetValidatedValueShouldReturnAValidatedValueWithCorrectBasicPropertyValues(ValidatedValueFromBasisFactory sut,
                                                                                               [ExecutableModel] ValidatedValueBasis basis)
        {
            var result = sut.GetValidatedValue(basis);
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Property(nameof(ValidatedValue.ManifestValue)).SameAs(basis.ManifestValue));
                Assert.That(result, Has.Property(nameof(ValidatedValue.ValueResponse)).SameAs(basis.ValidatedValueResponse));
                Assert.That(result, Has.Property(nameof(ValidatedValue.ParentValue)).SameAs(basis.Parent));
                Assert.That(result, Has.Property(nameof(ValidatedValue.CollectionItemOrder)).EqualTo(basis.CollectionOrder));
            });
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldGetIdentityFromAccessorIfThereIsOne(ValidatedValueFromBasisFactory sut,
                                                                               [ExecutableModel] ValidatedValueBasis basis,
                                                                               int identity)
        {
            basis.ManifestValue.IdentityAccessor = obj => identity;
            Assert.That(() => sut.GetValidatedValue(basis).ValueIdentity, Is.EqualTo(identity));
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldLeaveIdentityNullIfThereIsNoAccessor(ValidatedValueFromBasisFactory sut,
                                                                                [ExecutableModel] ValidatedValueBasis basis)
        {
            basis.ManifestValue.IdentityAccessor = null;
            Assert.That(() => sut.GetValidatedValue(basis).ValueIdentity, Is.Null);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldGetAnExecutableRuleUsingTheLogicFactoryFromEachManifestRule([Frozen] IGetsValidationLogic validationLogicFactory,
                                                                                                       ValidatedValueFromBasisFactory sut,
                                                                                                       [ExecutableModel] ValidatedValueBasis basis,
                                                                                                       [ManifestModel] ManifestRule rule1,
                                                                                                       [ManifestModel] ManifestRule rule2,
                                                                                                       IValidationLogic logic1,
                                                                                                       IValidationLogic logic2)
        {
            Mock.Get(validationLogicFactory).Setup(x => x.GetValidationLogic(rule1)).Returns(logic1);
            Mock.Get(validationLogicFactory).Setup(x => x.GetValidationLogic(rule2)).Returns(logic2);
            basis.ManifestValue.Rules.Add(rule1);
            basis.ManifestValue.Rules.Add(rule2);

            var result = sut.GetValidatedValue(basis);

            Assert.That(result.Rules.Select(x => x.RuleLogic).ToList(), Is.EqualTo(new[] { logic1, logic2 }));
        }
    }
}