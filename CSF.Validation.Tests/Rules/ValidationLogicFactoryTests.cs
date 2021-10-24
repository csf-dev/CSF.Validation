using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class ValidationLogicFactoryTests
    {
        [Test,AutoMoqData]
        public async Task GetValidationLogicShouldReturnWorkingLogicForNormalRule([Frozen] IResolvesRule ruleResolver,
                                                                                  ValidationLogicFactory sut,
                                                                                  string str,
                                                                                  [RuleId] RuleIdentifier id)
        {
            var value = new ManifestValue { ValidatedType = typeof(string) };
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(StringRule)));
            value.Rules.Add(rule);
            var ruleBody = new StringRule();
            Mock.Get(ruleResolver).Setup(x => x.ResolveRule(typeof(StringRule))).Returns(ruleBody);

            var result = sut.GetValidationLogic(rule);
            await result.GetResultAsync(str, null, new RuleContext(id, typeof(string)));

            Assert.That(ruleBody.Executed, Is.True);
        }

        [Test,AutoMoqData]
        public async Task GetValidationLogicShouldReturnWorkingLogicForValueRule([Frozen] IResolvesRule ruleResolver,
                                                                                 ValidationLogicFactory sut,
                                                                                 string str,
                                                                                 [RuleId] RuleIdentifier id)
        {
            var value = new ManifestValue { ValidatedType = typeof(string), Parent = new ManifestValue { ValidatedType = typeof(ComplexObject) } };
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(StringValueRule)));
            value.Rules.Add(rule);
            var ruleBody = new StringValueRule();
            Mock.Get(ruleResolver).Setup(x => x.ResolveRule(typeof(StringValueRule))).Returns(ruleBody);

            var result = sut.GetValidationLogic(rule);
            await result.GetResultAsync(str, null, new RuleContext(id, typeof(string)));

            Assert.That(ruleBody.ExecutedAsValueRule, Is.True);
        }

        [Test,AutoMoqData]
        public async Task GetValidationLogicShouldReturnRuleThatUsesCorrectInterfaceWhenOriginalLogicWasAmbiguousBetweenRuleAndValueRule([Frozen] IResolvesRule ruleResolver,
                                                                                                                                         ValidationLogicFactory sut,
                                                                                                                                         string str,
                                                                                                                                         [RuleId] RuleIdentifier id)
        {
            var value = new ManifestValue { ValidatedType = typeof(string) };
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(StringValueRule)));
            value.Rules.Add(rule);
            var ruleBody = new StringValueRule();
            Mock.Get(ruleResolver).Setup(x => x.ResolveRule(typeof(StringValueRule))).Returns(ruleBody);

            var result = sut.GetValidationLogic(rule);
            await result.GetResultAsync(str, null, new RuleContext(id, typeof(string)));

            Assert.That(ruleBody.ExecutedAsRule, Is.True);
        }

        [Test,AutoMoqData]
        public void GetValidationLogicShouldConfigureRuleWithConfigurationAction([Frozen] IResolvesRule ruleResolver,
                                                                                 ValidationLogicFactory sut,
                                                                                 string str,
                                                                                 [RuleId] RuleIdentifier id,
                                                                                 string configValue)
        {
            var value = new ManifestValue { ValidatedType = typeof(string) };
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(StringRule)));
            rule.RuleConfiguration = obj => ((StringRule)obj).ConfigurableValue = configValue;
            value.Rules.Add(rule);
            var ruleBody = new StringRule();
            Mock.Get(ruleResolver).Setup(x => x.ResolveRule(typeof(StringRule))).Returns(ruleBody);

            var result = sut.GetValidationLogic(rule);

            Assert.That(ruleBody.ConfigurableValue, Is.EqualTo(configValue));
        }

        [Test,AutoMoqData]
        public void GetValidationLogicShouldThrowValidatorBuildingExceptionIfTheRuleConfigurationActionThrowsAnException([Frozen] IResolvesRule ruleResolver,
                                                                                                                         ValidationLogicFactory sut,
                                                                                                                         string str,
                                                                                                                         [RuleId] RuleIdentifier id)
        {
            var value = new ManifestValue { ValidatedType = typeof(string) };
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(StringRule)));
            rule.RuleConfiguration = obj => throw new Exception();
            value.Rules.Add(rule);
            var ruleBody = new StringRule();
            Mock.Get(ruleResolver).Setup(x => x.ResolveRule(typeof(StringRule))).Returns(ruleBody);

            Assert.That(() => sut.GetValidationLogic(rule), Throws.InstanceOf<ValidatorBuildingException>());
        }

        public class StringRule : IRule<string>
        {
            public bool Executed { get; private set; }

            public string ConfigurableValue { get; set; }

            public Task<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
            {
                Executed = true;
                return PassAsync();
            }
        }

        public class StringValueRule : IValueRule<string,ComplexObject>, IRule<string>
        {
            public bool ExecutedAsRule { get; private set; }

            public bool ExecutedAsValueRule { get; private set; }

            public Task<RuleResult> GetResultAsync(string value, ComplexObject validated, RuleContext context, CancellationToken token = default)
            {
                ExecutedAsValueRule = true;
                return PassAsync();
            }

            public Task<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
            {
                ExecutedAsRule = true;
                return PassAsync();
            }
        }
    }
}