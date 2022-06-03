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
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValidatorTests
    {
        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnReturnValidationResultContainingRuleResults([ManifestModel,Frozen] ValidationManifest manifest,
                                                                                               [Frozen] IGetsRuleExecutor executorFactory,
                                                                                               [Frozen] IGetsAllExecutableRulesWithDependencies ruleFactory,
                                                                                               [Frozen] IGetsRuleExecutionContext contextFactory,
                                                                                               IExecutesAllRules executor,
                                                                                               Validator<object> sut,
                                                                                               object validatedObject,
                                                                                               ValidationOptions options,
                                                                                               CancellationToken cancellationToken,
                                                                                               IRuleExecutionContext context,
                                                                                               [ExecutableModel] ExecutableRuleAndDependencies[] ruleAndDependencies,
                                                                                               [RuleResult] ValidationRuleResult[] results)
        {
            Mock.Get(executorFactory)
                .Setup(x => x.GetRuleExecutorAsync(It.IsAny<ResolvedValidationOptions>(), cancellationToken))
                .Returns(Task.FromResult(executor));
            Mock.Get(contextFactory)
                .Setup(x => x.GetExecutionContext(ruleAndDependencies, It.IsAny<ResolvedValidationOptions>()))
                .Returns(context);
            Mock.Get(executor)
                .Setup(x => x.ExecuteAllRulesAsync(context, cancellationToken))
                .Returns(Task.FromResult((IReadOnlyCollection<ValidationRuleResult>) results));
            Mock.Get(ruleFactory)
                .Setup(x => x.GetRulesWithDependencies(manifest.RootValue, validatedObject, It.IsAny<ResolvedValidationOptions>()))
                .Returns(ruleAndDependencies);

            var result = await sut.ValidateAsync(validatedObject, options, cancellationToken);

            Assert.That(result.RuleResults, Is.EqualTo(results));
        }

        [Test,AutoMoqData]
        public async Task ValidateAsyncNonGenericShouldReturnReturnValidationResultContainingRuleResults([ManifestModel,Frozen] ValidationManifest manifest,
                                                                                               [Frozen] IGetsRuleExecutor executorFactory,
                                                                                               [Frozen] IGetsAllExecutableRulesWithDependencies ruleFactory,
                                                                                               [Frozen] IGetsRuleExecutionContext contextFactory,
                                                                                               IExecutesAllRules executor,
                                                                                               Validator<object> sut,
                                                                                               object validatedObject,
                                                                                               ValidationOptions options,
                                                                                               CancellationToken cancellationToken,
                                                                                               IRuleExecutionContext context,
                                                                                               [ExecutableModel] ExecutableRuleAndDependencies[] ruleAndDependencies,
                                                                                               [RuleResult] ValidationRuleResult[] results)
        {
            Mock.Get(executorFactory)
                .Setup(x => x.GetRuleExecutorAsync(It.IsAny<ResolvedValidationOptions>(), cancellationToken))
                .Returns(Task.FromResult(executor));
            Mock.Get(contextFactory)
                .Setup(x => x.GetExecutionContext(ruleAndDependencies, It.IsAny<ResolvedValidationOptions>()))
                .Returns(context);
            Mock.Get(executor)
                .Setup(x => x.ExecuteAllRulesAsync(context, cancellationToken))
                .Returns(Task.FromResult((IReadOnlyCollection<ValidationRuleResult>) results));
            Mock.Get(ruleFactory)
                .Setup(x => x.GetRulesWithDependencies(manifest.RootValue, validatedObject, It.IsAny<ResolvedValidationOptions>()))
                .Returns(ruleAndDependencies);

            var result = await ((IValidator) sut).ValidateAsync(validatedObject, options, cancellationToken);

            Assert.That(result.RuleResults, Is.EqualTo(results));
        }
    }
}