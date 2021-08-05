using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture.NUnit3;
using CSF.Validation.Autofixture;
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
        public void UseObjectIdentityShouldAddIdentityAccessorToTheContext([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                           ValidatorBuilder<ValidatedObject> sut,
                                                                           ValidatedObject obj)
        {
            sut.UseObjectIdentity(x => x.Identity);

            Assert.That(() => context.ManifestValue.IdentityAccessor(obj), Is.EqualTo(obj.Identity));
        }

        [Test,AutoMoqData]
        public void AddRuleShouldAddRuleCreatedFromFactoryUsingContext([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                       [Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                       IBuildsRule<ObjectRule> ruleBuilder,
                                                                       ValidatorBuilder<ValidatedObject> sut,
                                                                       [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<ObjectRule>(context, It.IsAny<Action<IConfiguresRule<ObjectRule>>>()))
                .Returns(ruleBuilder);
            Mock.Get(ruleBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });
            
            sut.AddRule<ObjectRule>();

            Assert.That(() => sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void GetManifestRulesShouldReturnOneRulePerRuleAdded([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                    [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                    [Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    ValidatorBuilder<ValidatedObject> sut,
                                                                    [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<ObjectRule>(It.IsAny<ValidatorBuilderContext>(), It.IsAny<Action<IConfiguresRule<ObjectRule>>>()))
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
        public void AddRulesShouldAddBuilderReturnedFromManifestFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                        [Frozen] IGetsValidatorManifest manifestFactory,
                                                                        ValidatorBuilder<ValidatedObject> sut,
                                                                        IGetsManifestRules manifest,
                                                                        [ManifestModel] ManifestRule rule)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorManifest(typeof(ValidatedObjectValidator), context))
                .Returns(manifest);
            Mock.Get(manifest).Setup(x => x.GetManifestRules()).Returns(() => new[] { rule });

            sut.AddRules<ValidatedObjectValidator>();

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForMemberShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                             [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                             [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                             ValidatorBuilder<ValidatedObject> sut,
                                                                             ValidatorBuilderContext ruleContext,
                                                                             IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                             [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,string>>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForMember(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForMemberItemsShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                  [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                                  [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                                  ValidatorBuilder<ValidatedObject> sut,
                                                                                  [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                                  IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                                  [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,IEnumerable<char>>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForMemberItems(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForValueShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                            [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                            [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                            ValidatorBuilder<ValidatedObject> sut,
                                                                            [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                            IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                            [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,string>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForValue(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForValuesShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                             [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                             [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                             ValidatorBuilder<ValidatedObject> sut,
                                                                             [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                             IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                             [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,IEnumerable<char>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            sut.ForValues(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void ForMemberShouldPassConfigurationActionToBuilder([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                       [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                       ValidatorBuilder<ValidatedObject> sut,
                                                                       [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                       IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                       [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,string>>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            Action<IConfiguresValueAccessor<ValidatedObject, string>> configAction = c => c.AddRule<StringValueRule>();
            sut.ForMember(x => x.AProperty, configAction);

            Mock.Get(valueBuilderFactory)
                .Verify(x => x.GetValueAccessorBuilder<ValidatedObject, string>(ruleContext, configAction), Times.Once);
        }

        [Test,AutoMoqData]
        public void ForMemberItemsShouldPassConfigurationActionToBuilder([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                            [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                            [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                            ValidatorBuilder<ValidatedObject> sut,
                                                                            [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                            IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                            [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,IEnumerable<char>>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            Action<IConfiguresValueAccessor<ValidatedObject, char>> configAction = c => c.AddRule<CharValueRule>();
            sut.ForMemberItems(x => x.AProperty, configAction);

            Mock.Get(valueBuilderFactory)
                .Verify(x => x.GetValueAccessorBuilder<ValidatedObject, char>(ruleContext, configAction), Times.Once);
        }

        [Test,AutoMoqData]
        public void ForValueShouldPassConfigurationActionToBuilder([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                      [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                      [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                      ValidatorBuilder<ValidatedObject> sut,
                                                                      [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                      IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                      [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,string>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            Action<IConfiguresValueAccessor<ValidatedObject, string>> configAction = c => c.AddRule<StringValueRule>();
            sut.ForValue(x => x.AProperty, configAction);

            Mock.Get(valueBuilderFactory)
                .Verify(x => x.GetValueAccessorBuilder<ValidatedObject, string>(ruleContext, configAction), Times.Once);
        }

        [Test,AutoMoqData]
        public void ForValuesShouldPassConfigurationActionToBuilder([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                       [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                       ValidatorBuilder<ValidatedObject> sut,
                                                                       [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                       IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                       [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,IEnumerable<char>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            Action<IConfiguresValueAccessor<ValidatedObject, char>> configAction = c => c.AddRule<CharValueRule>();
            sut.ForValues(x => x.AProperty, configAction);

            Mock.Get(valueBuilderFactory)
                .Verify(x => x.GetValueAccessorBuilder<ValidatedObject, char>(ruleContext, configAction), Times.Once);
        }
    }
}