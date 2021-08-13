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
                                                                       [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<ObjectRule>(context, It.IsAny<Action<IConfiguresRule<ObjectRule>>>()))
                .Returns(ruleBuilder);
            Mock.Get(ruleBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);
            
            sut.AddRule<ObjectRule>();

            Assert.That(() => sut.GetManifestValue().Children.Single(), Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void GetManifestRulesShouldIterateOverEveryRuleAdded([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                    [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                    [Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    ValidatorBuilder<ValidatedObject> sut,
                                                                    [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<ObjectRule>(It.IsAny<ValidatorBuilderContext>(), It.IsAny<Action<IConfiguresRule<ObjectRule>>>()))
                .Returns(() => {
                    var ruleBuilder = new Mock<IBuildsRule<ObjectRule>>();
                    ruleBuilder
                        .Setup(x => x.GetManifestValue())
                        .Returns(() => value);
                    return ruleBuilder.Object;
                });

            sut.AddRule<ObjectRule>();
            sut.AddRule<ObjectRule>();

            sut.GetManifestValue();

            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<ObjectRule>(context, It.IsAny<Action<IConfiguresRule<ObjectRule>>>()),
                        Times.Exactly(2));
        }

        [Test,AutoMoqData]
        public void AddRulesShouldAddBuilderReturnedFromManifestFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                        [Frozen] IGetsValidatorManifest manifestFactory,
                                                                        ValidatorBuilder<ValidatedObject> sut,
                                                                        IGetsManifestValue manifest,
                                                                        [ManifestModel] ManifestValue value)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorManifest(typeof(ValidatedObjectValidator), context))
                .Returns(manifest);
            Mock.Get(manifest).Setup(x => x.GetManifestValue()).Returns(() => value);

            sut.AddRules<ValidatedObjectValidator>();

            Assert.That(sut.GetManifestValue().Children.Single(), Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void ForMemberShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                             [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                             [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                             ValidatorBuilder<ValidatedObject> sut,
                                                                             ValidatorBuilderContext ruleContext,
                                                                             IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                             [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,string>>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            sut.ForMember(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestValue().Children.Single(), Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void ForMemberItemsShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                  [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                                  [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                                  ValidatorBuilder<ValidatedObject> sut,
                                                                                  [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                                  IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                                  [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,IEnumerable<char>>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            sut.ForMemberItems(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestValue().Children.Single(), Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void ForValueShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                            [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                            [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                            ValidatorBuilder<ValidatedObject> sut,
                                                                            [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                            IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                            [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,string>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            sut.ForValue(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestValue().Children.Single(), Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void ForValuesShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                             [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                             [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                             ValidatorBuilder<ValidatedObject> sut,
                                                                             [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                             IBuildsValueAccessor<ValidatedObject,char> valueBuilder,
                                                                             [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,IEnumerable<char>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            sut.ForValues(x => x.AProperty, c => {});

            Assert.That(sut.GetManifestValue().Children.Single(), Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void ForMemberShouldPassConfigurationActionToBuilder([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                       [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                       ValidatorBuilder<ValidatedObject> sut,
                                                                       [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                       IBuildsValueAccessor<ValidatedObject,string> valueBuilder,
                                                                       [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,string>>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            Action<IConfiguresValueAccessor<ValidatedObject, string>> configAction = c => c.AddValueRule<StringValueRule>();
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
                                                                            [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,IEnumerable<char>>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            Action<IConfiguresValueAccessor<ValidatedObject, char>> configAction = c => c.AddValueRule<CharValueRule>();
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
                                                                      [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,string>>(), context, false))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            Action<IConfiguresValueAccessor<ValidatedObject, string>> configAction = c => c.AddValueRule<StringValueRule>();
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
                                                                       [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,IEnumerable<char>>>(), context, true))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(valueBuilder);
            Mock.Get(valueBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            Action<IConfiguresValueAccessor<ValidatedObject, char>> configAction = c => c.AddValueRule<CharValueRule>();
            sut.ForValues(x => x.AProperty, configAction);

            Mock.Get(valueBuilderFactory)
                .Verify(x => x.GetValueAccessorBuilder<ValidatedObject, char>(ruleContext, configAction), Times.Once);
        }
    }
}