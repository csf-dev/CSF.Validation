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
        /// <summary>
        /// An amount of time in milliseconds that we allow the validator to run, beyond
        /// what we would expect in the best case scenario.
        /// </summary>
        const int millisecondsGrace = 150;

        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldTakeApproximatelyTheCorrectTimeToRunAParallelRuleset([IntegrationTesting] IGetsValidator validatorFactory,
                                                                                                  [NoAutoProperties] Person person)
        {
            // Get the current setting
            ThreadPool.GetMaxThreads(out var workerThreads, out var completionThreads);
            var stopwatch = new Stopwatch();

            try
            {
                // Ensure that we can run 4 rules concurrently
                ThreadPool.SetMaxThreads(4, 4);

                var validator = validatorFactory.GetValidator<Person>(typeof(ParallelValidatorBuilder));

                stopwatch.Start();
                await validator.ValidateAsync(person, new ValidationOptions { EnableRuleParallelization = true }).ConfigureAwait(false);
                stopwatch.Stop();
            }
            finally
            {
                // Restore to the previous environment setting
                ThreadPool.SetMaxThreads(workerThreads, completionThreads);
            }

            // Best case :  [largest of (200, 200, 50, 50)] + 500 + 300 = 1000
            // Worst case:  200 + 200 + 50 + 50             + 500 + 300 = 1300
            // We are giving "best case" + "a small grace period" which is well below the worst case for non-parallel execution.
            var expectedMilliseconds = 1000;

            Assert.That(stopwatch.ElapsedMilliseconds,
                        Is.GreaterThan(expectedMilliseconds).And.LessThan(expectedMilliseconds + millisecondsGrace),
                        "The validator is expected to take around 1000 milliseconds to complete validation.");
        }
    }
}