using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValidatedValueBasisTests
    {
        [Test,AutoMoqData]
        public void GetChildManifestValuesShouldCombineApplicablePolymorphicValuesWithManifestValue([ManifestModel] ManifestItem value,
                                                                                                    [ManifestModel] ManifestItem type1,
                                                                                                    [ManifestModel] ManifestItem type2,
                                                                                                    [ManifestModel] ManifestItem type3,
                                                                                                    [ManifestModel] ManifestItem child1,
                                                                                                    [ManifestModel] ManifestItem child2,
                                                                                                    [ManifestModel] ManifestItem child3,
                                                                                                    [ManifestModel] ManifestItem child4)
        {
            value.ValidatedType = typeof(Person);
            type1.ValidatedType = typeof(Employee);
            type1.ItemType = ManifestItemType.PolymorphicType;
            type2.ValidatedType = typeof(Manager);
            type2.ItemType = ManifestItemType.PolymorphicType;
            type3.ValidatedType = typeof(Cleaner);
            type3.ItemType = ManifestItemType.PolymorphicType;
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
        public void GetManifestRulesShouldCombineApplicablePolymorphicRulesWithManifestValue([ManifestModel] ManifestItem value,
                                                                                             [ManifestModel] ManifestItem type1,
                                                                                             [ManifestModel] ManifestItem type2,
                                                                                             [ManifestModel] ManifestItem type3,
                                                                                             [ManifestModel] ManifestRule rule1,
                                                                                             [ManifestModel] ManifestRule rule2,
                                                                                             [ManifestModel] ManifestRule rule3,
                                                                                             [ManifestModel] ManifestRule rule4)
        {
            value.ValidatedType = typeof(Person);
            type1.ValidatedType = typeof(Employee);
            type1.ItemType = ManifestItemType.PolymorphicType;
            type2.ValidatedType = typeof(Manager);
            type2.ItemType = ManifestItemType.PolymorphicType;
            type3.ValidatedType = typeof(Cleaner);
            type3.ItemType = ManifestItemType.PolymorphicType;
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