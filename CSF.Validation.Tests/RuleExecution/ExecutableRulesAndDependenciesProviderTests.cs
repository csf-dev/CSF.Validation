using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class ExecutableRulesAndDependenciesProviderTests
    {
        [Test,AutoMoqData]
        public void GetRulesWithDependenciesShouldNotReturnAnyDependenciesForARuleWhichHasNone([Frozen] IGetsAllExecutableRules executableRulesProvider,
                                                                                               [ManifestModel] ManifestValue manifestValue,
                                                                                               object objectToBeValidated,
                                                                                               [ExecutableModel] ExecutableRule rule,
                                                                                               ExecutableRulesAndDependenciesProvider sut,
                                                                                               ValidationOptions validationOptions)
        {
            Mock.Get(executableRulesProvider)
                .Setup(x => x.GetExecutableRules(manifestValue, objectToBeValidated, validationOptions))
                .Returns(new [] { rule });
            
            var result = sut.GetRulesWithDependencies(manifestValue, objectToBeValidated, validationOptions);

            Assert.That(result.Single().Dependencies, Is.Empty);
        }

        [Test,AutoMoqData]
        public void GetRulesWithDependenciesShouldReturnAnObjectWithDependencyExecutableRulesWhereItHasADependencyUponTheSameValue([Frozen] IGetsAllExecutableRules executableRulesProvider,
                                                                                                                                   [ManifestModel] ManifestValue manifestValue,
                                                                                                                                   object objectToBeValidated,
                                                                                                                                   IValidationLogic logic,
                                                                                                                                   ManifestRule manifestRule,
                                                                                                                                   ManifestRule manifestDependency,
                                                                                                                                   [ExecutableModel] ValidatedValue validatedValue,
                                                                                                                                   ExecutableRulesAndDependenciesProvider sut,
                                                                                                                                   ValidationOptions validationOptions)
        {
            var rule = new ExecutableRule { ValidatedValue = validatedValue, ManifestRule = manifestRule, RuleLogic = logic };
            var dependency = new ExecutableRule { ValidatedValue = validatedValue, ManifestRule = manifestDependency, RuleLogic = logic };
            validatedValue.ManifestValue = manifestDependency.Identifier.ManifestValue;
            validatedValue.Rules.Add(dependency);
            Mock.Get(executableRulesProvider)
                .Setup(x => x.GetExecutableRules(manifestValue, objectToBeValidated, validationOptions))
                .Returns(new [] { rule, dependency });
            rule.ManifestRule.DependencyRules.Clear();
            rule.ManifestRule.DependencyRules.Add(manifestDependency.Identifier);

            var result = sut.GetRulesWithDependencies(manifestValue, objectToBeValidated, validationOptions);

            Assert.That(result.First(x => x.ExecutableRule == rule).Dependencies, Is.EqualTo(new[] { dependency }));
        }

        [Test,AutoMoqData]
        public void GetRulesWithDependenciesShouldReturnAnObjectWithDependencyExecutableRulesWhereItHasADependencyUponAParentValue([Frozen] IGetsAllExecutableRules executableRulesProvider,
                                                                                                                                   [ManifestModel] ManifestValue manifestValue,
                                                                                                                                   object objectToBeValidated,
                                                                                                                                   IValidationLogic logic,
                                                                                                                                   ManifestRule manifestRule,
                                                                                                                                   ManifestRule manifestDependency,
                                                                                                                                   [ExecutableModel] ValidatedValue validatedValue,
                                                                                                                                   [ExecutableModel] ValidatedValue parentValue,
                                                                                                                                   ExecutableRulesAndDependenciesProvider sut,
                                                                                                                                   ValidationOptions validationOptions)
        {
            var rule = new ExecutableRule { ValidatedValue = validatedValue, ManifestRule = manifestRule, RuleLogic = logic };
            validatedValue.ParentValue = parentValue;
            var dependency = new ExecutableRule { ValidatedValue = parentValue, ManifestRule = manifestDependency, RuleLogic = logic };
            parentValue.ManifestValue = manifestDependency.Identifier.ManifestValue;
            parentValue.Rules.Add(dependency);
            Mock.Get(executableRulesProvider)
                .Setup(x => x.GetExecutableRules(manifestValue, objectToBeValidated, validationOptions))
                .Returns(new [] { rule, dependency });
            rule.ManifestRule.DependencyRules.Clear();
            rule.ManifestRule.DependencyRules.Add(manifestDependency.Identifier);

            var result = sut.GetRulesWithDependencies(manifestValue, objectToBeValidated, validationOptions);

            Assert.That(result.First(x => x.ExecutableRule == rule).Dependencies, Is.EqualTo(new[] { dependency }));
        }

        [Test,AutoMoqData]
        public void GetRulesWithDependenciesShouldReturnAnObjectWithDependencyExecutableRulesWhereItHasADependencyUponAGrandparentValue([Frozen] IGetsAllExecutableRules executableRulesProvider,
                                                                                                                                        [ManifestModel] ManifestValue manifestValue,
                                                                                                                                        object objectToBeValidated,
                                                                                                                                        IValidationLogic logic,
                                                                                                                                        ManifestRule manifestRule,
                                                                                                                                        ManifestRule manifestDependency,
                                                                                                                                        [ExecutableModel] ValidatedValue validatedValue,
                                                                                                                                        [ExecutableModel] ValidatedValue parentValue,
                                                                                                                                        [ExecutableModel] ValidatedValue grandparentValue,
                                                                                                                                        ExecutableRulesAndDependenciesProvider sut,
                                                                                                                                        ValidationOptions validationOptions)
        {
            var rule = new ExecutableRule { ValidatedValue = validatedValue, ManifestRule = manifestRule, RuleLogic = logic };
            validatedValue.ParentValue = parentValue;
            parentValue.ParentValue = grandparentValue;
            var dependency = new ExecutableRule { ValidatedValue = grandparentValue, ManifestRule = manifestDependency, RuleLogic = logic };
            grandparentValue.ManifestValue = manifestDependency.Identifier.ManifestValue;
            grandparentValue.Rules.Add(dependency);
            Mock.Get(executableRulesProvider)
                .Setup(x => x.GetExecutableRules(manifestValue, objectToBeValidated, validationOptions))
                .Returns(new [] { rule, dependency });
            rule.ManifestRule.DependencyRules.Clear();
            rule.ManifestRule.DependencyRules.Add(manifestDependency.Identifier);

            var result = sut.GetRulesWithDependencies(manifestValue, objectToBeValidated, validationOptions);

            Assert.That(result.First(x => x.ExecutableRule == rule).Dependencies, Is.EqualTo(new[] { dependency }));
        }

        [Test,AutoMoqData]
        public void GetRulesWithDependenciesShouldReturnAnObjectWithDependencyExecutableRulesWhereItHasADependencyUponASiblingOfTheParentValue([Frozen] IGetsAllExecutableRules executableRulesProvider,
                                                                                                                                               [ManifestModel] ManifestValue manifestValue,
                                                                                                                                               object objectToBeValidated,
                                                                                                                                               IValidationLogic logic,
                                                                                                                                               ManifestRule manifestRule,
                                                                                                                                               ManifestRule manifestDependency,
                                                                                                                                               [ExecutableModel] ValidatedValue validatedValue,
                                                                                                                                               [ExecutableModel] ValidatedValue parentValue,
                                                                                                                                               [ExecutableModel] ValidatedValue grandparentValue,
                                                                                                                                               [ExecutableModel] ValidatedValue parentSiblingValue,
                                                                                                                                               ExecutableRulesAndDependenciesProvider sut,
                                                                                                                                               ValidationOptions validationOptions)
        {
            var rule = new ExecutableRule { ValidatedValue = validatedValue, ManifestRule = manifestRule, RuleLogic = logic };
            validatedValue.ParentValue = parentValue;
            parentValue.ParentValue = grandparentValue;
            grandparentValue.ChildValues.Add(parentSiblingValue);
            var dependency = new ExecutableRule { ValidatedValue = parentSiblingValue, ManifestRule = manifestDependency, RuleLogic = logic };
            parentSiblingValue.ManifestValue = manifestDependency.Identifier.ManifestValue;
            parentSiblingValue.Rules.Add(dependency);
            Mock.Get(executableRulesProvider)
                .Setup(x => x.GetExecutableRules(manifestValue, objectToBeValidated, validationOptions))
                .Returns(new [] { rule, dependency });
            rule.ManifestRule.DependencyRules.Clear();
            rule.ManifestRule.DependencyRules.Add(manifestDependency.Identifier);

            var result = sut.GetRulesWithDependencies(manifestValue, objectToBeValidated, validationOptions);

            Assert.That(result.First(x => x.ExecutableRule == rule).Dependencies, Is.EqualTo(new[] { dependency }));
        }

        [Test,AutoMoqData]
        public void GetRulesWithDependenciesShouldThrowIfNoAncestorOrSiblingOfAnyAncestorValueMatchesTheDependencyValue([Frozen] IGetsAllExecutableRules executableRulesProvider,
                                                                                                                        [ManifestModel] ManifestValue manifestValue,
                                                                                                                        object objectToBeValidated,
                                                                                                                        IValidationLogic logic,
                                                                                                                        ManifestRule manifestRule,
                                                                                                                        ManifestRule manifestDependency,
                                                                                                                        [ExecutableModel] ValidatedValue validatedValue,
                                                                                                                        [ExecutableModel] ValidatedValue parentValue,
                                                                                                                        ExecutableRulesAndDependenciesProvider sut,
                                                                                                                        ValidationOptions validationOptions)
        {
            var rule = new ExecutableRule { ValidatedValue = validatedValue, ManifestRule = manifestRule, RuleLogic = logic };
            validatedValue.ParentValue = parentValue;
            var dependency = new ExecutableRule { ValidatedValue = parentValue, ManifestRule = manifestDependency, RuleLogic = logic };
            Mock.Get(executableRulesProvider)
                .Setup(x => x.GetExecutableRules(manifestValue, objectToBeValidated, validationOptions))
                .Returns(new [] { rule, dependency });
            rule.ManifestRule.DependencyRules.Clear();
            rule.ManifestRule.DependencyRules.Add(manifestDependency.Identifier);

            Assert.That(() => sut.GetRulesWithDependencies(manifestValue, objectToBeValidated, validationOptions), Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void GetRulesWithDependenciesShouldThrowIfTheMatchingValueDoesNotHaveTheSpecifiedRule([Frozen] IGetsAllExecutableRules executableRulesProvider,
                                                                                                     [ManifestModel] ManifestValue manifestValue,
                                                                                                     object objectToBeValidated,
                                                                                                     IValidationLogic logic,
                                                                                                     ManifestRule manifestRule,
                                                                                                     ManifestRule manifestDependency,
                                                                                                     [ExecutableModel] ValidatedValue validatedValue,
                                                                                                     [ExecutableModel] ValidatedValue parentValue,
                                                                                                     ExecutableRulesAndDependenciesProvider sut,
                                                                                                     ValidationOptions validationOptions)
        {
            var rule = new ExecutableRule { ValidatedValue = validatedValue, ManifestRule = manifestRule, RuleLogic = logic };
            validatedValue.ParentValue = parentValue;
            var dependency = new ExecutableRule { ValidatedValue = parentValue, ManifestRule = manifestDependency, RuleLogic = logic };
            parentValue.ManifestValue = manifestDependency.Identifier.ManifestValue;
            Mock.Get(executableRulesProvider)
                .Setup(x => x.GetExecutableRules(manifestValue, objectToBeValidated, validationOptions))
                .Returns(new [] { rule, dependency });
            rule.ManifestRule.DependencyRules.Clear();
            rule.ManifestRule.DependencyRules.Add(manifestDependency.Identifier);

            Assert.That(() => sut.GetRulesWithDependencies(manifestValue, objectToBeValidated, validationOptions), Throws.InstanceOf<ValidationException>());
        }
    }
}