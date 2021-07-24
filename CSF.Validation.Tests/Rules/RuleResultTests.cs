using System;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class RuleResultTests
    {
        [Test,AutoMoqData]
        public void ConstructorShouldNotThrowIfOutcomeIsErrorAndExceptionIsNotNull(Exception exception)
        {
            Assert.That(() => new RuleResult(RuleOutcome.Errored, exception: exception), Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void ConstructorShouldThrowIfOutcomeIsPassAndExceptionIsNotNull(Exception exception)
        {
            Assert.That(() => new RuleResult(RuleOutcome.Passed, exception: exception), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void ConstructorShouldThrowIfOutcomeIsPassAndExceptionIsNull()
        {
            Assert.That(() => new RuleResult(RuleOutcome.Passed, exception: null), Throws.Nothing);
        }
    }
}