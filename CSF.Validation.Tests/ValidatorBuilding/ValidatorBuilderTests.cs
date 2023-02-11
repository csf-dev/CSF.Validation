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
    [TestFixture, NUnit.Framework.Parallelizable]
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
                                                                       ValidatorBuilder<ValidatedObject> sut)
        {
            sut.AddRule<ObjectRule>();

            Assert.That(() => sut.Context.ConfigurationCallbacks, Has.Count.EqualTo(1));
        }

        [Test,AutoMoqData]
        public void GetManifestRulesShouldIterateOverEveryRuleAdded([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                    [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                    [Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    ValidatorBuilder<ValidatedObject> sut,
                                                                    [ManifestModel] ManifestItem value)
        {
            sut.AddRule<ObjectRule>();
            sut.AddRule<ObjectRule>();

            sut.Context.GetManifestValue();

            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<ObjectRule>(context, It.IsAny<Action<IConfiguresRule<ObjectRule>>>()),
                        Times.Exactly(2));
        }

        [Test,AutoMoqData]
        public void AddRulesShouldAddBuilderReturnedFromManifestFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                        [Frozen] IGetsValidatorBuilderContextFromBuilder manifestFactory,
                                                                        ValidatorBuilder<ValidatedObject> sut,
                                                                        [ManifestModel] ValidatorBuilderContext subContext)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorBuilderContext(typeof(ValidatedObjectValidator), context))
                .Returns(subContext);

            sut.AddRules<ValidatedObjectValidator>();

            Assert.That(sut.Context.GetManifestValue().Children.Single(), Is.SameAs(subContext.ManifestValue));
        }

        [Test,AutoMoqData]
        public void ForMemberShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                             [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                             [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                             ValidatorBuilder<ValidatedObject> sut,
                                                                             [ManifestModel] ValidatorBuilderContext ruleContext)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,string>>>(), context))
                .Returns(ruleContext);

            sut.ForMember(x => x.AProperty, c => {});

            Assert.That(sut.Context.GetManifestValue().Children.Single(), Is.SameAs(ruleContext.ManifestValue));
        }

        [Test,AutoMoqData]
        public void ForMemberItemsShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                  [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                                  [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                                  ValidatorBuilder<ValidatedObject> sut,
                                                                                  [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                                  [ManifestModel] ValidatorBuilderContext itemContext,
                                                                                  IConfiguresValueAccessor<ValidatedObject,char> valueBuilder)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,IEnumerable<char>>>>(), context))
                .Returns(ruleContext);
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForCollection(ruleContext, typeof(char)))
                .Returns(itemContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(() => valueBuilder);
            itemContext.ManifestValue.ItemType = ManifestItemType.CollectionItem;

            sut.ForMemberItems(x => x.AProperty, c => {});

            Assert.That(sut.Context.GetManifestValue().Children.Single().CollectionItemValue, Is.SameAs(itemContext.ManifestValue));
        }

        [Test,AutoMoqData]
        public void ForValueShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                            [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                            [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                            ValidatorBuilder<ValidatedObject> sut,
                                                                            [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                            IConfiguresValueAccessor<ValidatedObject,string> valueBuilder)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,string>>(), context))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(() => valueBuilder);

            sut.ForValue(x => x.AProperty, c => {});

            Assert.That(sut.Context.GetManifestValue().Children.Single(), Is.SameAs(ruleContext.ManifestValue));
        }

        [Test,AutoMoqData]
        public void ForValuesShouldAddBuilderReturnedFromValueBuilderFactory([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                             [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                             [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                             ValidatorBuilder<ValidatedObject> sut,
                                                                             [ManifestModel] ValidatorBuilderContext valueContext,
                                                                             [ManifestModel] ValidatorBuilderContext itemContext,
                                                                             IConfiguresValueAccessor<ValidatedObject,char> valueBuilder)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,IEnumerable<char>>>(), context))
                .Returns(valueContext);
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForCollection(valueContext, typeof(char)))
                .Returns(itemContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(valueContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(() => valueBuilder);
            itemContext.ManifestValue.ItemType = ManifestItemType.CollectionItem;

            sut.ForValues(x => x.AProperty, c => {});

            Assert.That(sut.Context.GetManifestValue().Children.Single().CollectionItemValue, Is.SameAs(itemContext.ManifestValue));
        }

        [Test,AutoMoqData]
        public void ForMemberShouldPassConfigurationActionToBuilder([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                       [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                       ValidatorBuilder<ValidatedObject> sut,
                                                                       [ManifestModel] ValidatorBuilderContext ruleContext,
                                                                       Mock<IConfiguresValueAccessor<ValidatedObject,string>> valueBuilder,
                                                                       [ManifestModel] ManifestItem value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,string>>>(), context))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(() => valueBuilder.Object);
            valueBuilder
                .As<IHasValidationBuilderContext>()
                .SetupGet(x => x.Context)
                .Returns(() => new ValidatorBuilderContext(value));


            Action<IConfiguresValueAccessor<ValidatedObject, string>> configAction = c => c.AddRuleWithParent<StringValueRule>();
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
                                                                         Mock<IConfiguresValueAccessor<ValidatedObject,char>> valueBuilder,
                                                                            [ManifestModel] ManifestItem value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,IEnumerable<char>>>>(), context))
                .Returns(ruleContext);
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForCollection(ruleContext, typeof(char)))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(() => valueBuilder.Object);
            valueBuilder
                .As<IHasValidationBuilderContext>()
                .SetupGet(x => x.Context)
                .Returns(() => new ValidatorBuilderContext(value));

            Action<IConfiguresValueAccessor<ValidatedObject, char>> configAction = c => c.AddRuleWithParent<CharValueRule>();
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
                                                                       Mock<IConfiguresValueAccessor<ValidatedObject,string>> valueBuilder,
                                                                      [ManifestModel] ManifestItem value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,string>>(), context))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(() => valueBuilder.Object);
            valueBuilder
                .As<IHasValidationBuilderContext>()
                .SetupGet(x => x.Context)
                .Returns(() => new ValidatorBuilderContext(value));

            Action<IConfiguresValueAccessor<ValidatedObject, string>> configAction = c => c.AddRuleWithParent<StringValueRule>();
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
                                                                       Mock<IConfiguresValueAccessor<ValidatedObject,char>> valueBuilder,
                                                                       [ManifestModel] ManifestItem value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForValue(It.IsAny<Func<ValidatedObject,IEnumerable<char>>>(), context))
                .Returns(ruleContext);
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForCollection(ruleContext, typeof(char)))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,char>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,char>>>()))
                .Returns(() => valueBuilder.Object);
            valueBuilder
                .As<IHasValidationBuilderContext>()
                .SetupGet(x => x.Context)
                .Returns(() => new ValidatorBuilderContext(value));

            Action<IConfiguresValueAccessor<ValidatedObject, char>> configAction = c => c.AddRuleWithParent<CharValueRule>();
            sut.ForValues(x => x.AProperty, configAction);

            Mock.Get(valueBuilderFactory)
                .Verify(x => x.GetValueAccessorBuilder<ValidatedObject, char>(ruleContext, configAction), Times.Once);
        }

        [Test,AutoMoqData]
        public void ValidateAsAncestorShouldThrowIfForMemberHasAlreadyBeenUsed([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                               [Frozen] IGetsValidatorBuilderContext ruleContextFactory,
                                                                               [Frozen] IGetsValueAccessorBuilder valueBuilderFactory,
                                                                               ValidatorBuilder<ValidatedObject> sut,
                                                                               ValidatorBuilderContext ruleContext,
                                                                               Mock<IConfiguresValueAccessor<ValidatedObject,string>> valueBuilder,
                                                                               [ManifestModel] ManifestItem value)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContextForMember(It.IsAny<Expression<Func<ValidatedObject,string>>>(), context))
                .Returns(ruleContext);
            Mock.Get(valueBuilderFactory)
                .Setup(x => x.GetValueAccessorBuilder<ValidatedObject,string>(ruleContext, It.IsAny<Action<IConfiguresValueAccessor<ValidatedObject,string>>>()))
                .Returns(() => valueBuilder.Object);
            valueBuilder
                .As<IHasValidationBuilderContext>()
                .SetupGet(x => x.Context)
                .Returns(() => new ValidatorBuilderContext(value));

            sut.ForMember(x => x.AProperty, v => { });
            Assert.That(() => sut.ValidateAsAncestor(1), Throws.InvalidOperationException);
        }

        [Test,AutoMoqData]
        public void ValidateAsAncestorShouldThrowIfUsedTwice([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                             ValidatorBuilder<ValidatedObject> sut,
                                                             [ManifestModel] ManifestItem parent)
        {
            context.ManifestValue.Parent = parent;
            sut.ValidateAsAncestor(1);
            Assert.That(() => sut.ValidateAsAncestor(1), Throws.InvalidOperationException);
        }

        [Test,AutoMoqData]
        public void ForMemberShouldThrowIfValidateAsAncestorHasAlreadyBeenUsed([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                               ValidatorBuilder<ValidatedObject> sut,
                                                                               [ManifestModel] ManifestItem parent)
        {
            context.ManifestValue.Parent = parent;
            sut.ValidateAsAncestor(1);
            Assert.That(() => sut.ForMember(x => x.AProperty, v => { }), Throws.InvalidOperationException);
        }

        [Test,AutoMoqData]
        public void ValidateAsAncestorShouldSetupGetManifestValueToReturnARecursiveManifestValue([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                                 ValidatorBuilder<ValidatedObject> sut,
                                                                                                 [ManifestModel] ManifestItem parent,
                                                                                                 [ManifestModel] ManifestItem grandParent)
        {
            context.ManifestValue.Parent = parent;
            parent.Parent = grandParent;
            sut.ValidateAsAncestor(2);
            var result = sut.Context.GetManifestValue();
            Assert.Multiple(() =>
            {
                Assert.That(result.IsRecursive, Is.True, "Result is recursive");
                Assert.That(result.RecursiveAncestor, Is.SameAs(grandParent), "Result has correct ancestor");
            });
        }
    }
}