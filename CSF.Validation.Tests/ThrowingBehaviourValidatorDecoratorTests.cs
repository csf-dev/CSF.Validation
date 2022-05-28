using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class ThrowingBehaviourValidatorDecoratorTests
    {
        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldThrowIfBehaviourIsOnErrorAndResultsContainAnError([Frozen] IValidator<string> wrapped,
                                                                                                ThrowingBehaviourValidatorDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnError, Error()),
                        Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldNotThrowIfBehaviourIsOnErrorAndResultsContainOnlyFailures([Frozen] IValidator<string> wrapped,
                                                                                                ThrowingBehaviourValidatorDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnError, Fail()),
                        Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldNotThrowIfBehaviourIsOnErrorAndResultsContainNoErrorsOrFailures([Frozen] IValidator<string> wrapped,
                                                                                                ThrowingBehaviourValidatorDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnError, Pass()),
                        Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldThrowIfBehaviourIsOnFailureAndResultsContainAnError([Frozen] IValidator<string> wrapped,
                                                                                                ThrowingBehaviourValidatorDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnFailure, Error()),
                        Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldThrowIfBehaviourIsOnFailureAndResultsContainAFailure([Frozen] IValidator<string> wrapped,
                                                                                                ThrowingBehaviourValidatorDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnFailure, Fail()),
                        Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldNotThrowIfBehaviourIsOnFailureAndResultsContainNoErrorsOrFailures([Frozen] IValidator<string> wrapped,
                                                                                                ThrowingBehaviourValidatorDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnFailure, Pass()),
                        Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldNotThrowIfBehaviourIsNeverAndResultContainsBothErrorsAndFailures([Frozen] IValidator<string> wrapped,
                                                                                                ThrowingBehaviourValidatorDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.Never, Error(), Fail()),
                        Throws.Nothing);
        }

        async Task<IQueryableValidationResult<string>> ExerciseGenericSut(ThrowingBehaviourValidatorDecorator<string> sut,
                                                                          string validated,
                                                                          ValidationOptions options,
                                                                          IValidator<string> wrapped,
                                                                          RuleThrowingBehaviour behaviour,
                                                                          params RuleResult[] ruleResults)
        {
            var result = new ValidationResult<string>(ruleResults.Select(x => new ValidationRuleResult(x, null, null)), new Manifest.ValidationManifest { ValidatedType = typeof(string) });
            Mock.Get(wrapped).Setup(x => x.ValidateAsync(validated, options, default)).Returns(Task.FromResult<IQueryableValidationResult<string>>(result));
            options.RuleThrowingBehaviour = behaviour;
            return await sut.ValidateAsync(validated, options).ConfigureAwait(false);
        }
        
        [Test,AutoMoqData]
        public void NonGenericValidateAsyncShouldThrowIfBehaviourIsOnErrorAndResultsContainAnError(string validated,
                                                                                                   ValidationOptions options)
        {
            Assert.That(async () => await ExerciseNonGenericSut(validated, options, RuleThrowingBehaviour.OnError, Error()),
                        Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void NonGenericValidateAsyncShouldNotThrowIfBehaviourIsOnErrorAndResultsContainOnlyFailures(string validated,
                                                                                                   ValidationOptions options)
        {
            Assert.That(async () => await ExerciseNonGenericSut(validated, options, RuleThrowingBehaviour.OnError, Fail()),
                        Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void NonGenericValidateAsyncShouldNotThrowIfBehaviourIsOnErrorAndResultsContainNoErrorsOrFailures(string validated,
                                                                                                   ValidationOptions options)
        {
            Assert.That(async () => await ExerciseNonGenericSut(validated, options, RuleThrowingBehaviour.OnError, Pass()),
                        Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void NonGenericValidateAsyncShouldThrowIfBehaviourIsOnFailureAndResultsContainAnError(string validated,
                                                                                                   ValidationOptions options)
        {
            Assert.That(async () => await ExerciseNonGenericSut(validated, options, RuleThrowingBehaviour.OnFailure, Error()),
                        Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void NonGenericValidateAsyncShouldThrowIfBehaviourIsOnFailureAndResultsContainAFailure(string validated,
                                                                                                   ValidationOptions options)
        {
            Assert.That(async () => await ExerciseNonGenericSut(validated, options, RuleThrowingBehaviour.OnFailure, Fail()),
                        Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void NonGenericValidateAsyncShouldNotThrowIfBehaviourIsOnFailureAndResultsContainNoErrorsOrFailures(string validated,
                                                                                                   ValidationOptions options)
        {
            Assert.That(async () => await ExerciseNonGenericSut(validated, options, RuleThrowingBehaviour.OnFailure, Pass()),
                        Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void NonGenericValidateAsyncShouldNotThrowIfBehaviourIsNeverAndResultContainsBothErrorsAndFailures(string validated,
                                                                                                   ValidationOptions options)
        {
            Assert.That(async () => await ExerciseNonGenericSut(validated, options, RuleThrowingBehaviour.Never, Error(), Fail()),
                        Throws.Nothing);
        }

        async Task<ValidationResult> ExerciseNonGenericSut(string validated,
                                                           ValidationOptions options,
                                                           RuleThrowingBehaviour behaviour,
                                                           params RuleResult[] ruleResults)
        {
            var result = new ValidationResult<string>(ruleResults.Select(x => new ValidationRuleResult(x, null, null)), new Manifest.ValidationManifest { ValidatedType = typeof(string) });
            var wrapped = new Mock<IValidator<string>>();
            wrapped.As<IValidator>().Setup(x => x.ValidateAsync(validated, options, default)).Returns(Task.FromResult<ValidationResult>(result));
            var sut = new ThrowingBehaviourValidatorDecorator<string>(wrapped.Object);
            options.RuleThrowingBehaviour = behaviour;
            return await sut.ValidateAsync((object) validated, options).ConfigureAwait(false);
        }
    }
}