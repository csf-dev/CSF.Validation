using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.RuleExecution;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class ValidatorTests
    {
        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnReturnValidationResultContainingRuleResults([ManifestModel,Frozen] ValidationManifest manifest,
                                                                                               [Frozen] IGetsRuleExecutor executorFactory,
                                                                                               [Frozen] IGetsAllExecutableRulesWithDependencies ruleFactory,
                                                                                               IExecutesAllRules executor,
                                                                                               Validator<object> sut,
                                                                                               object validatedObject,
                                                                                               ValidationOptions options,
                                                                                               CancellationToken cancellationToken,
                                                                                               [ExecutableModel] ExecutableRuleAndDependencies[] ruleAndDependencies,
                                                                                               [RuleResult] ValidationRuleResult[] results)
        {
            Mock.Get(executorFactory)
                .Setup(x => x.GetRuleExecutorAsync(options, cancellationToken))
                .Returns(Task.FromResult(executor));
            Mock.Get(executor)
                .Setup(x => x.ExecuteAllRulesAsync(ruleAndDependencies, cancellationToken))
                .Returns(Task.FromResult((IReadOnlyCollection<ValidationRuleResult>) results));
            Mock.Get(ruleFactory)
                .Setup(x => x.GetRulesWithDependencies(manifest.RootValue, validatedObject, options))
                .Returns(ruleAndDependencies);

            var result = await sut.ValidateAsync(validatedObject, options, cancellationToken);

            Assert.That(result.RuleResults, Is.EqualTo(results));
        }
    }
}