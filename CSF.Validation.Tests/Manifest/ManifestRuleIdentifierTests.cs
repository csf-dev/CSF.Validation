using System;
using NUnit.Framework;

namespace CSF.Validation.Manifest
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ManifestRuleIdentifierTests
    {
        [Test,AutoMoqData]
        public void EqualsShouldReturnTrueForTheSameInstance([ManifestModel] ManifestRuleIdentifier sut)
        {
            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test,AutoMoqData]
        public void EqualsShouldReturnFalseForAString([ManifestModel] ManifestRuleIdentifier sut, string aString)
        {
            Assert.That(sut.Equals(aString), Is.False);
        }

        [Test,AutoMoqData]
        public void EqualsShouldReturnFalseForAnInstanceWithDifferentRuleName([ManifestModel] ManifestItem value, Type aType)
        {
            var first = new ManifestRuleIdentifier(value, aType, "One");
            var second = new ManifestRuleIdentifier(value, aType, "Two");
            
            Assert.That(first.Equals(second), Is.False);
        }

        [Test,AutoMoqData]
        public void EqualsShouldReturnFalseForADifferentInstanceWithEqualPropertyValues([ManifestModel] ManifestItem value, Type aType, string aString)
        {
            var first = new ManifestRuleIdentifier(value, aType, aString);
            var second = new ManifestRuleIdentifier(value, aType, aString);
            
            Assert.That(first.Equals(second), Is.True);
        }

        [Test,AutoMoqData]
        public void GetHashCodeShouldReturnDifferentResultsForAnInstanceWithDifferentRuleName([ManifestModel] ManifestItem value, Type aType)
        {
            var first = new ManifestRuleIdentifier(value, aType, "One");
            var second = new ManifestRuleIdentifier(value, aType, "Two");
            
            Assert.That(first.GetHashCode(), Is.Not.EqualTo(second.GetHashCode()));
        }

        [Test,AutoMoqData]
        public void GetHashCodeShouldReturnSameResultForADifferentInstanceWithEqualPropertyValues([ManifestModel] ManifestItem value, Type aType, string aString)
        {
            var first = new ManifestRuleIdentifier(value, aType, aString);
            var second = new ManifestRuleIdentifier(value, aType, aString);
            
            Assert.That(first.GetHashCode(), Is.EqualTo(second.GetHashCode()));
        }
    }
}