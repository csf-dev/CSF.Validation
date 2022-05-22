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
    public class ThrowingBehaviourValidatorWithMessagesDecoratorTests
    {
        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldThrowIfBehaviourIsOnErrorAndResultsContainAnError([Frozen] IValidatorWithMessages<string> wrapped,
                                                                                                ThrowingBehaviourValidatorWithMessagesDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnError, Error()),
                        Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldNotThrowIfBehaviourIsOnErrorAndResultsContainOnlyFailures([Frozen] IValidatorWithMessages<string> wrapped,
                                                                                                ThrowingBehaviourValidatorWithMessagesDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnError, Fail()),
                        Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldNotThrowIfBehaviourIsOnErrorAndResultsContainNoErrorsOrFailures([Frozen] IValidatorWithMessages<string> wrapped,
                                                                                                ThrowingBehaviourValidatorWithMessagesDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnError, Pass()),
                        Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldThrowIfBehaviourIsOnFailureAndResultsContainAnError([Frozen] IValidatorWithMessages<string> wrapped,
                                                                                                ThrowingBehaviourValidatorWithMessagesDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnFailure, Error()),
                        Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldThrowIfBehaviourIsOnFailureAndResultsContainAFailure([Frozen] IValidatorWithMessages<string> wrapped,
                                                                                                ThrowingBehaviourValidatorWithMessagesDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnFailure, Fail()),
                        Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldNotThrowIfBehaviourIsOnFailureAndResultsContainNoErrorsOrFailures([Frozen] IValidatorWithMessages<string> wrapped,
                                                                                                ThrowingBehaviourValidatorWithMessagesDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.OnFailure, Pass()),
                        Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void GenericValidateAsyncShouldNotThrowIfBehaviourIsNeverAndResultContainsBothErrorsAndFailures([Frozen] IValidatorWithMessages<string> wrapped,
                                                                                                ThrowingBehaviourValidatorWithMessagesDecorator<string> sut,
                                                                                                string validated,
                                                                                                ValidationOptions options)
        {
            Assert.That(async () => await ExerciseGenericSut(sut, validated, options, wrapped, RuleThrowingBehaviour.Never, Error(), Fail()),
                        Throws.Nothing);
        }

        async Task<ValidationResultWithMessages> ExerciseGenericSut(ThrowingBehaviourValidatorWithMessagesDecorator<string> sut,
                                                                    string validated,
                                                                    ValidationOptions options,
                                                                    IValidatorWithMessages<string> wrapped,
                                                                    RuleThrowingBehaviour behaviour,
                                                                    params RuleResult[] ruleResults)
        {
            var result = new ValidationResultWithMessages(ruleResults.Select(x => new ValidationRuleResultWithMessage(new ValidationRuleResult(x, null, null))));
            Mock.Get(wrapped).Setup(x => x.ValidateAsync(validated, options, default)).Returns(Task.FromResult(result));
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

        async Task<ValidationResultWithMessages> ExerciseNonGenericSut(string validated,
                                                                       ValidationOptions options,
                                                                       RuleThrowingBehaviour behaviour,
                                                                       params RuleResult[] ruleResults)
        {
            var result = new ValidationResultWithMessages(ruleResults.Select(x => new ValidationRuleResultWithMessage(new ValidationRuleResult(x, null, null))));
            var wrapped = new Mock<IValidatorWithMessages<string>>();
            wrapped.As<IValidatorWithMessages>().Setup(x => x.ValidateAsync(validated, options, default)).Returns(Task.FromResult(result));
            var sut = new ThrowingBehaviourValidatorWithMessagesDecorator<string>(wrapped.Object);
            options.RuleThrowingBehaviour = behaviour;
            return await sut.ValidateAsync((object) validated, options).ConfigureAwait(false);
        }
    }
}