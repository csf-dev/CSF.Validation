using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class RuleAdapterTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnResultFromWrappedRule([Frozen] IRule<string> wrapped,
                                                                    RuleAdapter<string> sut,
                                                                    string value,
                                                                    object parentValue,
                                                                    [RuleContext] RuleContext context,
                                                                    [RuleResult] RuleResult result)
        {
            Mock.Get(wrapped)
                .Setup(x => x.GetResultAsync(value, context, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(result));
            Assert.That(async () => await sut.GetResultAsync(value, parentValue, context, default), Is.SameAs(result));
        }
    }
}