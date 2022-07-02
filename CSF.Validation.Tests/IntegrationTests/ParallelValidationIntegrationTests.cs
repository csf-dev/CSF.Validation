using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using NUnit.Framework;

namespace CSF.Validation.IntegrationTests
{
    [TestFixture,NonParallelizable]
    public class ParallelValidationIntegrationTests
    {
        int workerThreads, completionThreads;
        Stopwatch stopwatch;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            stopwatch = new Stopwatch();

            ThreadPool.GetMaxThreads(out workerThreads, out completionThreads);
            // Configure the thread pool so that we can run 4 rules concurrently
            ThreadPool.SetMaxThreads(4, 4);
        }

        [TearDown]
        public void TearDown()
        {
            stopwatch.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Restore to the previous environment setting
            ThreadPool.SetMaxThreads(workerThreads, completionThreads);
        }


        /// <summary>
        /// An amount of time in milliseconds that we allow the validator to run, beyond
        /// what we would expect in the best case scenario.
        /// </summary>
        const int millisecondsGrace = 150;

        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldTakeApproximatelyTheCorrectTimeToRunAParallelRuleset([IntegrationTesting] IGetsValidator validatorFactory,
                                                                                                  [NoAutoProperties] Person person)
        {
            var validator = validatorFactory.GetValidator<Person>(typeof(ParallelValidatorBuilder));

            stopwatch.Start();
            await validator.ValidateAsync(person, new ValidationOptions { EnableRuleParallelization = true }).ConfigureAwait(false);
            stopwatch.Stop();

            // Best case time :  [largest of (300, 300, 200, 200)] + 200 + 150 = 650    (represents perfect parallelisation with no overhead)
            // Worst case time:  300 + 300 + 200 + 200             + 200 + 150 = 1350   (represents serial execution, although still no overhead)
            // 
            // The total time for validation should be a little over "the best case time".
            // We allow a short grace period because that best case is totally hypothetical and impossible to actually achieve.
            // Even with the grace period, the target time is well below the worst case (representing totally non-parallel execution).
            var bestCaseMilliseconds = 650;

            Assert.That(stopwatch.ElapsedMilliseconds,
                        Is.GreaterThan(bestCaseMilliseconds).And.LessThan(bestCaseMilliseconds + millisecondsGrace),
                        "The validator is expected to take between 650 and 800 milliseconds to complete validation.");
        }
    }
}