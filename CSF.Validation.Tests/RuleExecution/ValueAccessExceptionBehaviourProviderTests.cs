using NUnit.Framework;

namespace CSF.Validation.Tests.RuleExecution
{
    [TestFixture,Parallelizable]
    public class ValueAccessExceptionBehaviourProviderTests
    {
        [Test,AutoMoqData]
        public void GetBehaviourShouldReturnManifestBehaviourIfItIsNotNull()
        {
            Assert.Fail("TODO: Write this test");
        }

        [Test,AutoMoqData]
        public void GetBehaviourShouldReturnOptionsBehaviourIfManifestBehaviourIsNull()
        {
            Assert.Fail("TODO: Write this test");
        }
    }
}