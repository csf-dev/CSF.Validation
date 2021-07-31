using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable]
    public class ValidatorBuilderTests
    {
        [Test,AutoMoqData]
        public void UseObjectIdentityShouldAddIdentityAccessorToTheContext([Frozen] ValidatorBuilderContext context,
                                                                           ValidatorBuilder<ValidatedObject> sut,
                                                                           ValidatedObject obj)
        {
            sut.UseObjectIdentity(x => x.Identity);

            Assert.That(() => context.ObjectIdentityAccessor(obj), Is.EqualTo(obj.Identity));
        }

        [Test,AutoMoqData]
        public void AddRuleShouldAddRuleCreatedFromFactoryUsingContext([Frozen] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                       [Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                       IBuildsRule<ObjectRule> ruleBuilder,
                                                                       ValidatorBuilder<ValidatedObject> sut,
                                                                       RuleBuilderContext ruleContext,
                                                                       ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContext(context))
                .Returns(ruleContext);
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<ObjectRule>(ruleContext, It.IsAny<Action<IConfiguresRule<ObjectRule>>>()))
                .Returns(ruleBuilder);
            Mock.Get(ruleBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });
            
            sut.AddRule<ObjectRule>();

            Assert.That(() => sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void GetManifestRulesShouldReturnOneRulePerRuleAdded([Frozen] ValidatorBuilderContext context,
                                                                    [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                    [Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    RuleBuilderContext ruleContext,
                                                                    ValidatorBuilder<ValidatedObject> sut,
                                                                    ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContext(context))
                .Returns(ruleContext);
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<ObjectRule>(It.IsAny<RuleBuilderContext>(), It.IsAny<Action<IConfiguresRule<ObjectRule>>>()))
                .Returns(() => {
                    var ruleBuilder = new Mock<IBuildsRule<ObjectRule>>();
                    ruleBuilder
                        .Setup(x => x.GetManifestRules())
                        .Returns(() => new[] { rule });
                    return ruleBuilder.Object;
                });

            sut.AddRule<ObjectRule>();
            sut.AddRule<ObjectRule>();

            var manifestRules = sut.GetManifestRules().ToList();

            Assert.That(manifestRules, Has.Count.EqualTo(2));
        }

        [Test,AutoMoqData]
        public void AddRulesShouldAddBuilderReturnedFromManifestFactory([Frozen] ValidatorBuilderContext context,
                                                                        [Frozen] IGetsValidatorManifest manifestFactory,
                                                                        ValidatorBuilder<ValidatedObject> sut,
                                                                        IGetsManifestRules manifest,
                                                                        ManifestRule rule)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorManifest(typeof(ValidatedObjectValidator), context))
                .Returns(manifest);
            Mock.Get(manifest).Setup(x => x.GetManifestRules()).Returns(() => new[] { rule });

            sut.AddRules<ValidatedObjectValidator>();

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForMemberShouldAddBuilderReturnedFromValueBuilderFactory([Frozen] ValidatorBuilderContext context,
                                                                             [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                             [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                             ValidatorBuilder<ValidatedObject> sut,
                                                                             RuleBuilderContext ruleContext,
                                                                             IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                             ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,string>>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForMember(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForMemberItemsShouldAddBuilderReturnedFromValueBuilderFactory([Frozen] ValidatorBuilderContext context,
                                                                                  [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                                  [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                                  ValidatorBuilder<ValidatedObject> sut,
                                                                                  RuleBuilderContext ruleContext,
                                                                                  IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                                  ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,IEnumerable<char>>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForMemberItems(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForValueShouldAddBuilderReturnedFromValueBuilderFactory([Frozen] ValidatorBuilderContext context,
                                                                            [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                            [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                            ValidatorBuilder<ValidatedObject> sut,
                                                                            RuleBuilderContext ruleContext,
                                                                            IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                            ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,string>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForValue(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForValuesShouldAddBuilderReturnedFromValueBuilderFactory([Frozen] ValidatorBuilderContext context,
                                                                             [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                             [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                             ValidatorBuilder<ValidatedObject> sut,
                                                                             RuleBuilderContext ruleContext,
                                                                             IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                             ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,IEnumerable<char>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForValues(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForMemberShouldExecuteConfigurationActionOnBuilder([Frozen] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                       [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                       ValidatorBuilder<ValidatedObject> sut,
                                                                       RuleBuilderContext ruleContext,
                                                                       IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                       ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,string>>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForMember(x => x.AProperty, c => c.AddRule<StringValueRule>());

            Mock.Get(valueBuilder)
                .Verify(x => x.AddRule<StringValueRule>(null), Times.Once);
        }

        [Test,AutoMoqData]
        public void ForMemberItemsShouldExecuteConfigurationActionOnBuilder([Frozen] ValidatorBuilderContext context,
                                                                            [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                            [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                            ValidatorBuilder<ValidatedObject> sut,
                                                                            RuleBuilderContext ruleContext,
                                                                            IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                            ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,IEnumerable<char>>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForMemberItems(x => x.AProperty, c => c.AddRule<CharValueRule>());

            Mock.Get(valueBuilder)
                .Verify(x => x.AddRule<CharValueRule>(null), Times.Once);
        }

        [Test,AutoMoqData]
        public void ForValueShouldExecuteConfigurationActionOnBuilder([Frozen] ValidatorBuilderContext context,
                                                                      [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                      [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                      ValidatorBuilder<ValidatedObject> sut,
                                                                      RuleBuilderContext ruleContext,
                                                                      IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                      ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,string>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForValue(x => x.AProperty, c => c.AddRule<StringValueRule>());

            Mock.Get(valueBuilder)
                .Verify(x => x.AddRule<StringValueRule>(null), Times.Once);
        }

        [Test,AutoMoqData]
        public void ForValuesShouldExecuteConfigurationActionOnBuilder([Frozen] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                       [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                       ValidatorBuilder<ValidatedObject> sut,
                                                                       RuleBuilderContext ruleContext,
                                                                       IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                       ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,IEnumerable<char>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForValues(x => x.AProperty, c => c.AddRule<CharValueRule>());

            Mock.Get(valueBuilder)
                .Verify(x => x.AddRule<CharValueRule>(null), Times.Once);
        }
    }
}