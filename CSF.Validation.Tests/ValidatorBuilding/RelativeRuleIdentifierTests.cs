using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture, Parallelizable]
    public class RelativeRuleIdentifierTests
    {
        [Test]
        public void ConstructorShouldThrowIfBothObjectIdentityAndAncestorLevelsAreNotNull()
        {
            Assert.That(() => new RelativeRuleIdentifier(typeof(string), ancestorLevels: 2, objectIdentity: "Foo bar"), Throws.ArgumentException);
        }

        [Test]
        public void ConstructorShouldNotThrowIfObjectIdentityIsNullButAncestorLevelsIsNot()
        {
            Assert.That(() => new RelativeRuleIdentifier(typeof(string), ancestorLevels: 2), Throws.Nothing);
        }

        [Test]
        public void ConstructorShouldNotThrowIfObjectIdentityIsNotNullButAncestorLevelsIs()
        {
            Assert.That(() => new RelativeRuleIdentifier(typeof(string), objectIdentity: "Foo bar"), Throws.Nothing);
        }

        [Test]
        public void ConstructorShouldNotThrowIfBothObjectIdentityAndAncestorLevelsAreNull()
        {
            Assert.That(() => new RelativeRuleIdentifier(typeof(string)), Throws.Nothing);
        }
    }
}