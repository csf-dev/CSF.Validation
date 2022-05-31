using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class SingleRuleExecutorFactoryTests
    {
        [Test,AutoMoqData]
        public void GetRuleExecutorShouldReturnAnInstanceOfSingleRuleExecutor(ResolvedValidationOptions options,
                                                                              SingleRuleExecutorFactory sut)
        {
            Assert.That(() => sut.GetRuleExecutor(options), Is.InstanceOf<SingleRuleExecutor>());
        }
    }
}