using System.Linq;
using CSF.Validation.RuleExecution;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class RuleContextFactoryTests
    {
        [Test,AutoMoqData]
        public void GetRuleContextShouldGetCorrectAncestorContexts(RuleContextFactory sut,
                                                                   [ExecutableModel] ExecutableRule rule,
                                                                   ValidatedValue value,
                                                                   ValidatedValue parentValue,
                                                                   ValidatedValue grandparentValue,
                                                                   ValidatedValue greatGrandparentValue)
        {
            rule.ValidatedValue = value;
            value.ParentValue = parentValue;
            parentValue.ParentValue = grandparentValue;
            grandparentValue.ParentValue = greatGrandparentValue;
            greatGrandparentValue.ParentValue = null;

            var result = sut.GetRuleContext(rule);

            Assert.Multiple(() =>
            {
                Assert.That(result?.RuleIdentifier,
                            Is.SameAs(rule.RuleIdentifier),
                            nameof(RuleContext.RuleIdentifier));
                Assert.That(result?.AncestorContexts.Select(x => x.ActualValue).ToList(),
                            Is.EqualTo(new [] { parentValue.ValueResponse, grandparentValue.ValueResponse, greatGrandparentValue.ValueResponse }),
                            nameof(RuleContext.AncestorContexts));
            });
        }
    }
}