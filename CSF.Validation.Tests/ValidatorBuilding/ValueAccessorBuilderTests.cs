using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable]
    public class ValueAccessorBuilderTests
    {
        [Test,AutoMoqData]
        public void GetManifestRulesShouldReturnOneRulePerRuleAdded([Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    ValueAccessorBuilder<ValidatedObject,string> sut)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<ARule>(It.IsAny<RuleBuilderContext>(), It.IsAny<Action<IConfiguresRule<ARule>>>()))
                .Returns((RuleBuilderContext ctx, Action<IConfiguresRule<ARule>> configAction) => new RuleBuilder<ARule>(ctx, Mock.Of<IGetsManifestRuleIdentifierFromRelativeIdentifier>()));

            sut.AddRule<ARule>();
            sut.AddRule<ARule>();

            var manifestRules = sut.GetManifestRules().ToList();

            Assert.That(manifestRules, Has.Count.EqualTo(2));
        }

        [Test,AutoMoqData]
        public void AddRuleShouldProvideConfigFunctionToRuleBuilder([Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    ValueAccessorBuilder<ValidatedObject,string> sut)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<ARule>(It.IsAny<RuleBuilderContext>(), It.IsAny<Action<IConfiguresRule<ARule>>>()))
                .Returns((RuleBuilderContext ctx, Action<IConfiguresRule<ARule>> configAction) => new RuleBuilder<ARule>(ctx, Mock.Of<IGetsManifestRuleIdentifierFromRelativeIdentifier>()));

            Action<IConfiguresRule<ARule>> configFunction = r => { };
            sut.AddRule<ARule>(configFunction);

            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<ARule>(It.IsAny<RuleBuilderContext>(), configFunction), Times.Once);
        }

        [Test,AutoMoqData]
        public void AddRulesShouldAddBuilderReturnedFromManifestFactory([Frozen] IGetsValidatorManifest manifestFactory,
                                                                        ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                        IGetsManifestRules manifest,
                                                                        ManifestRule rule)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorManifest(typeof(StringValidator), It.IsAny<RuleBuilderContext>()))
                .Returns(manifest);
            Mock.Get(manifest).Setup(x => x.GetManifestRules()).Returns(() => new[] { rule });

            sut.AddRules<StringValidator>();

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        #region Stub types

        public class ValidatedObject
        {
            public string AProperty { get; set; }
        }

        public class ARule : IValueRule<string, ValidatedObject>
        {
            public Task<RuleResult> GetResultAsync(string value, ValidatedObject validated, ValueRuleContext context, CancellationToken token = default)
                => throw new NotImplementedException();
        }

        public class StringValidator : IBuildsValidator<string>
        {
            public void ConfigureValidator(IConfiguresValidator<string> config)
                => throw new NotImplementedException();
        }

        #endregion
    }
}