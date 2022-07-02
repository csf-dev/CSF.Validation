using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class CircularDependencyPreventingRulesWithDependenciesDecoratorTests
    {
        [Test,AutoMoqData]
        public void GetRulesWithDependenciesShouldNotThrowIfThereAreNoCircularDependencies([Frozen] IGetsAllExecutableRulesWithDependencies wrapped,
                                                                                           [Frozen] IDetectsCircularDependencies circularDependencyDetector,
                                                                                           CircularDependencyPreventingRulesWithDependenciesDecorator sut,
                                                                                           [ManifestModel] ManifestValue manifestValue,
                                                                                           object objectToBeValidated,
                                                                                           ResolvedValidationOptions validationOptions)
        {
            Mock.Get(wrapped)
                .Setup(x => x.GetRulesWithDependencies(It.IsAny<ManifestValue>(), It.IsAny<object>(), validationOptions))
                .Returns(new ExecutableRuleAndDependencies[0]);
            Mock.Get(circularDependencyDetector)
                .Setup(x => x.GetCircularDependencies(It.IsAny<IEnumerable<ExecutableRuleAndDependencies>>()))
                .Returns(Enumerable.Empty<CircularDependency>());
            Assert.That(() => sut.GetRulesWithDependencies(manifestValue, objectToBeValidated, validationOptions), Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void GetRulesWithDependenciesShouldThrowValidationExceptionWithCorrectMessageIfThereAreCircularDependencies([Frozen] IGetsAllExecutableRulesWithDependencies wrapped,
                                                                                                                           [Frozen] IDetectsCircularDependencies circularDependencyDetector,
                                                                                                                           CircularDependencyPreventingRulesWithDependenciesDecorator sut,
                                                                                                                           [ManifestModel] ManifestValue manifestValue,
                                                                                                                           object objectToBeValidated,
                                                                                                                           ResolvedValidationOptions validationOptions)
        {
            var circularDependencies = GetSomeCircularDependencies(manifestValue);
            var expectedMessage = @"Validation rules may not have circular dependencies.  Following is a list of the circular dependencies found, to a maximum of the first 10.

Type               = System.String
Name               = Foo
Validated type     = System.Int32
Validated identity = Identity 1
->  Type           = System.DateTime
    Validated type = System.Int64
    ->  Type               = System.String
        Name               = Foo
        Validated type     = System.Int32
        Validated identity = Identity 1

Type               = System.String
Name               = Bar
Validated type     = System.Int32
Validated identity = Identity 2
->  Type           = System.Object
    Validated type = System.Int64
    ->  Type               = System.String
        Name               = Bar
        Validated type     = System.Int32
        Validated identity = Identity 2
";

            Mock.Get(wrapped)
                .Setup(x => x.GetRulesWithDependencies(It.IsAny<ManifestValue>(), It.IsAny<object>(), validationOptions))
                .Returns(new ExecutableRuleAndDependencies[0]);
            Mock.Get(circularDependencyDetector)
                .Setup(x => x.GetCircularDependencies(It.IsAny<IEnumerable<ExecutableRuleAndDependencies>>()))
                .Returns(circularDependencies);
            
            Assert.That(() => sut.GetRulesWithDependencies(manifestValue, objectToBeValidated, validationOptions), Throws.InstanceOf<ValidationException>().And.Message.EqualTo(expectedMessage));
        }

        static IEnumerable<CircularDependency> GetSomeCircularDependencies(ManifestValue manifestValue)
        {
            return new[] {
                new CircularDependency
                {
                    DependencyChain = new List<ExecutableRule>
                    {
                        new ExecutableRule
                        { RuleIdentifier = new RuleIdentifier(typeof(string), typeof(int), "Identity 1", ruleName: "Foo"), },
                        new ExecutableRule
                        { RuleIdentifier = new RuleIdentifier(typeof(DateTime), typeof(long), null), },
                        new ExecutableRule
                        { RuleIdentifier = new RuleIdentifier(typeof(string), typeof(int), "Identity 1", ruleName: "Foo"), },
                    },
                },
                new CircularDependency
                {
                    DependencyChain = new List<ExecutableRule>
                    {
                        new ExecutableRule
                        { RuleIdentifier = new RuleIdentifier(typeof(string), typeof(int), "Identity 2", ruleName: "Bar"), },
                        new ExecutableRule
                        { RuleIdentifier = new RuleIdentifier(typeof(object), typeof(long), null), },
                        new ExecutableRule
                        { RuleIdentifier = new RuleIdentifier(typeof(string), typeof(int), "Identity 2", ruleName: "Bar"), },
                    },
                },
            };
        }
    }
}