using CSF.Validation.Stubs;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture,Parallelizable,Ignore("Temporarily broken, to be restored")]
    public class RuleTypeResolverTests
    {
        [Test,AutoMoqData]
        public void GetRuleTypeShouldReturnNullForANonExistentType(RuleTypeResolver sut)
        {
            Assert.That(() => sut.GetRuleType("NopeThisTypeDoesNotExist"), Is.Null);
        }

        [Test,AutoMoqData]
        public void GetRuleTypeShouldGetTheCorrectTypeForARuleInTheCurrentAssemblyUsingAnAssemblyQualifiedName(RuleTypeResolver sut)
        {
            Assert.That(() => sut.GetRuleType("CSF.Validation.Stubs.CharValueRule, CSF.Validation.Tests"), Is.EqualTo(typeof(CharValueRule)));
        }

        [Test,AutoMoqData]
        public void GetRuleTypeShouldGetTheCorrectTypeForARuleInAPluginAssemblyUsingAnAssemblyQualifiedName(RuleTypeResolver sut)
        {
            Assert.That(() => sut.GetRuleType(SamplePluginAssembly.SampleRuleAqn), Is.Not.Null);
        }
    }
}