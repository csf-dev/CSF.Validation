using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValidatedValueBasisTests
    {
        [Test,AutoMoqData]
        public void GetChildManifestValuesShouldCombineApplicablePolymorphicValuesWithManifestValue([ManifestModel] ManifestValue value,
                                                                                                    [ManifestModel] ManifestPolymorphicType type1,
                                                                                                    [ManifestModel] ManifestPolymorphicType type2,
                                                                                                    [ManifestModel] ManifestPolymorphicType type3,
                                                                                                    [ManifestModel] ManifestValue child1,
                                                                                                    [ManifestModel] ManifestValue child2,
                                                                                                    [ManifestModel] ManifestValue child3,
                                                                                                    [ManifestModel] ManifestValue child4)
        {
            value.ValidatedType = typeof(Person);
            type1.ValidatedType = typeof(Employee);
            type2.ValidatedType = typeof(Manager);
            type3.ValidatedType = typeof(Cleaner);
            value.Children = new[] { child1 };
            type1.Children = new[] { child2 };
            type2.Children = new[] { child3 };
            type3.Children = new[] { child4 };
            value.PolymorphicTypes = new[] { type1, type2, type3 };
            var response = new SuccessfulGetValueToBeValidatedResponse(new Manager());
            var sut = new ValidatedValueBasis(value, response, null);

            Assert.That(() => sut.GetChildManifestValues(), Is.EquivalentTo(new[] { child1, child2, child3 }));
        }

        [Test,AutoMoqData]
        public void GetManifestRulesShouldCombineApplicablePolymorphicRulesWithManifestValue([ManifestModel] ManifestValue value,
                                                                                             [ManifestModel] ManifestPolymorphicType type1,
                                                                                             [ManifestModel] ManifestPolymorphicType type2,
                                                                                             [ManifestModel] ManifestPolymorphicType type3,
                                                                                             [ManifestModel] ManifestRule rule1,
                                                                                             [ManifestModel] ManifestRule rule2,
                                                                                             [ManifestModel] ManifestRule rule3,
                                                                                             [ManifestModel] ManifestRule rule4)
        {
            value.ValidatedType = typeof(Person);
            type1.ValidatedType = typeof(Employee);
            type2.ValidatedType = typeof(Manager);
            type3.ValidatedType = typeof(Cleaner);
            value.Rules = new[] { rule1 };
            type1.Rules = new[] { rule2 };
            type2.Rules = new[] { rule3 };
            type3.Rules = new[] { rule4 };
            value.PolymorphicTypes = new[] { type1, type2, type3 };
            var response = new SuccessfulGetValueToBeValidatedResponse(new Manager());
            var sut = new ValidatedValueBasis(value, response, null);

            Assert.That(() => sut.GetManifestRules(), Is.EquivalentTo(new[] { rule1, rule2, rule3 }));
        }

        class Person {}
        class Employee : Person {}
        class Manager : Employee {}
        class Cleaner : Employee {}
    }
}