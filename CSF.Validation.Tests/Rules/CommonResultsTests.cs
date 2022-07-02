using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class CommonResultsTests
    {
        [Test,AutoMoqData]
        public void PassShouldCreateAPassingResultWithTheAppropriateData(Dictionary<string,object> data)
        {
            AssertResultIsCorrect(Pass(data), RuleOutcome.Passed, data);
        }

        [Test,AutoMoqData]
        public async Task PassAsyncShouldCreateATaskOfPassingResultWithTheAppropriateData(Dictionary<string,object> data)
        {
            AssertResultIsCorrect(await PassAsync(data), RuleOutcome.Passed, data);
        }

        [Test,AutoMoqData]
        public void FailShouldCreateAFailureResultWithTheAppropriateData(Dictionary<string,object> data)
        {
            AssertResultIsCorrect(Fail(data), RuleOutcome.Failed, data);
        }

        [Test,AutoMoqData]
        public async Task FailAsyncShouldCreateATaskOfFailureResultWithTheAppropriateData(Dictionary<string,object> data)
        {
            AssertResultIsCorrect(await FailAsync(data), RuleOutcome.Failed, data);
        }
        [Test,AutoMoqData]
        public void ErrorShouldCreateAnErrorResultWithTheAppropriateData(Dictionary<string,object> data)
        {
            AssertResultIsCorrect(Error(data: data), RuleOutcome.Errored, data);
        }

        [Test,AutoMoqData]
        public async Task ErrorAsyncShouldCreateATaskOfErrorResultWithTheAppropriateData(Dictionary<string,object> data)
        {
            AssertResultIsCorrect(await ErrorAsync(data: data), RuleOutcome.Errored, data);
        }

        [Test,AutoMoqData]
        public void ResultShouldCreateAResultWithTheAppropriateOutcomeAndData(Dictionary<string,object> data)
        {
            AssertResultIsCorrect(Result(false, data), RuleOutcome.Failed, data);
        }

        [Test,AutoMoqData]
        public async Task ResultAsyncShouldCreateATaskOfResultWithTheAppropriateOutcomeAndData(Dictionary<string,object> data)
        {
            AssertResultIsCorrect(await ResultAsync(true, data), RuleOutcome.Passed, data);
        }

        void AssertResultIsCorrect(RuleResult result, RuleOutcome expectedOutcome, IDictionary<string,object> expectedData)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Property(nameof(RuleResult.Outcome)).EqualTo(expectedOutcome), "Outcome is correct");
                Assert.That(result, Has.Property(nameof(RuleResult.Data)).EqualTo(expectedData), "Result data is correct");
            });
        }
    }
}