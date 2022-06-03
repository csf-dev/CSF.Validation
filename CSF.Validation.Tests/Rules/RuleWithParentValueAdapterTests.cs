using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class RuleWithParentValueAdapterTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnResultFromWrappedRule([Frozen] IRule<string,ComplexObject> wrapped,
                                                                    RuleWithParentValueAdapter<string,ComplexObject> sut,
                                                                    string value,
                                                                    [NoAutoProperties] ComplexObject parentValue,
                                                                    [RuleContext] RuleContext context,
                                                                    [RuleResult] RuleResult result)
        {
            Mock.Get(wrapped)
                .Setup(x => x.GetResultAsync(value, parentValue, context, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(result));
            Assert.That(async () => await sut.GetResultAsync(value, parentValue, context, default), Is.SameAs(result));
        }
    }
}