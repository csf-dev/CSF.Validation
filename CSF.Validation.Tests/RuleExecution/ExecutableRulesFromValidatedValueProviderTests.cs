using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class ExecutableRulesFromValidatedValueProviderTests
    {
        [Test,AutoMoqData]
        public void GetExecutableRulesShouldReturnAFlattenedListOfRulesIncludingFromGrandchildValidatedValues([Frozen] IGetsValidatedValue validatedValueProvider,
                                                                                                              [ExecutableModel] ValidatedValue rootValue,
                                                                                                              [ExecutableModel] ValidatedValue childVal1,
                                                                                                              [ExecutableModel] ValidatedValue childVal2,
                                                                                                              [ExecutableModel] ValidatedValue grandchildVal1,
                                                                                                              [ExecutableModel] ValidatedValue grandchildVal2,
                                                                                                              [ExecutableModel] ExecutableRule rule1,
                                                                                                              [ExecutableModel] ExecutableRule rule2,
                                                                                                              [ExecutableModel] ExecutableRule rule3,
                                                                                                              [ExecutableModel] ExecutableRule rule4,
                                                                                                              [ExecutableModel] ExecutableRule rule5,
                                                                                                              [ExecutableModel] ExecutableRule rule6,
                                                                                                              [ExecutableModel] ExecutableRule rule7,
                                                                                                              ExecutableRulesFromValidatedValueProvider sut,
                                                                                                              [ManifestModel] ManifestValue manifestValue,
                                                                                                              object valueToBeValidated)
        {
            rootValue.ChildValues.Add(childVal1);
            rootValue.ChildValues.Add(childVal2);
            childVal1.ChildValues.Add(grandchildVal1);
            childVal1.ChildValues.Add(grandchildVal2);
            rootValue.Rules.Add(rule1);
            childVal1.Rules.Add(rule2);
            childVal1.Rules.Add(rule3);
            childVal2.Rules.Add(rule4);
            grandchildVal1.Rules.Add(rule5);
            grandchildVal2.Rules.Add(rule6);
            grandchildVal2.Rules.Add(rule7);
            Mock.Get(validatedValueProvider).Setup(x => x.GetValidatedValue(manifestValue, valueToBeValidated)).Returns(rootValue);

            var expected = new[] { rule1, rule2, rule3, rule4, rule5, rule6, rule7 };

            Assert.That(() => sut.GetExecutableRules(manifestValue, valueToBeValidated), Is.EquivalentTo(expected));
        }
    }
}