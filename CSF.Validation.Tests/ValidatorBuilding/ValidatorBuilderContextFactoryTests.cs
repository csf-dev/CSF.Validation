using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoFixture.NUnit3;
using CSF.Reflection;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValidatorBuilderContextFactoryTests
    {
        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithMemberName([Frozen] IStaticallyReflects reflect,
                                                                         [ManifestModel] ValidatorBuilderContext validationContext,
                                                                         ValidatorBuilderContextFactory sut)
        {
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            
            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(result.ManifestValue, Has.Property(nameof(ManifestItem.MemberName)).EqualTo(nameof(ValidatedObject.AProperty)));
        }

        [Test,AutoMoqData]
        public void GetContextForValueShouldReturnContextWithoutMemberName([ManifestModel] ValidatorBuilderContext validationContext,
                                                                           ValidatorBuilderContextFactory sut)
        {
            var result = sut.GetContextForValue<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(result.ManifestValue, Has.Property(nameof(ManifestItem.MemberName)).Null);
        }

        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithManifestValueIfNotEnumeratingItems([Frozen] IStaticallyReflects reflect,
                                                                                                 [ManifestModel] ValidatorBuilderContext validationContext,
                                                                                                 ValidatorBuilderContextFactory sut)
        {
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            
            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(result.ManifestValue, Is.InstanceOf<ManifestItem>());
        }

        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithManifestCollectionItemIfEnumeratingItems([Frozen] IStaticallyReflects reflect,
                                                                                                       [ManifestModel] ValidatorBuilderContext validationContext,
                                                                                                       ValidatorBuilderContextFactory sut)
        {
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            
            var result = sut.GetContextForCollection(validationContext, typeof(string));

            Assert.That(result.ManifestValue.IsCollectionItem, Is.True);
        }

        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithCorrectAccessor([ManifestModel] ValidatorBuilderContext validationContext,
                                                                              ValidatorBuilderContextFactory sut,
                                                                              ValidatedObject obj)
        {
            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(() => result.ManifestValue.AccessorFromParent(obj), Is.EqualTo(obj.AProperty));
        }

        [Test,AutoMoqData]
        public void GetContextForValueShouldReturnContextWithCorrectAccessor([ManifestModel] ValidatorBuilderContext validationContext,
                                                                             ValidatorBuilderContextFactory sut,
                                                                             ValidatedObject obj)
        {
            var result = sut.GetContextForValue<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(() => result.ManifestValue.AccessorFromParent(obj), Is.EqualTo(obj.AProperty));
        }

        [Test,AutoMoqData]
        public void GetContextForMemberThatAlreadyExistsShouldReturnSameManifestValue([Frozen] IStaticallyReflects reflect,
                                                                                      [ManifestModel] ValidatorBuilderContext validationContext,
                                                                                      ValidatorBuilderContextFactory sut,
                                                                                      [ManifestModel] ManifestItem aPropertyValue)
        {
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            aPropertyValue.MemberName = nameof(ValidatedObject.AProperty);
            validationContext.Contexts.Add(new ValidatorBuilderContext(aPropertyValue));

            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(result.ManifestValue, Is.SameAs(aPropertyValue));
        }

        [Test,AutoMoqData]
        public void GetContextForMemberThatAlreadyExistsAsCollectionItemShouldReturnSameManifestValue([Frozen] IStaticallyReflects reflect,
                                                                                                      [ManifestModel] ValidatorBuilderContext validationContext,
                                                                                                      ValidatorBuilderContextFactory sut,
                                                                                                      [ManifestModel] ManifestItem collectionValue)
        {
            collectionValue.ItemType = ManifestItemType.CollectionItem;
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            validationContext.ManifestValue.CollectionItemValue = collectionValue;

            var result = sut.GetContextForCollection(validationContext, typeof(string));

            Assert.That(result.ManifestValue, Is.SameAs(collectionValue));
        }

        [Test,AutoMoqData]
        public void GetPolymorphicContextShouldThrowIfManifestValueCannotHavePolymorphicTypes([ManifestModel] ManifestItem manifestModel,
                                                                                              ValidatorBuilderContextFactory sut)
        {
            manifestModel.ItemType = ManifestItemType.PolymorphicType;
            var validationContext = new ValidatorBuilderContext(manifestModel);
            Assert.That(() => sut.GetPolymorphicContext(validationContext, typeof(object)),
                        Throws.ArgumentException.And.Message.StartWith("The validation manifest value for the current context must not be ManifestItem"));
        }

        [Test,AutoMoqData]
        public void GetPolymorphicContextShouldReturnAContextFromAnExistingPolymorphicTypeIfItExists([ManifestModel] ManifestItem polymorphicValue,
                                                                                                     [ManifestModel] ManifestItem manifestValue,
                                                                                                     ValidatorBuilderContextFactory sut)
        {
            polymorphicValue.ItemType = ManifestItemType.PolymorphicType;
            manifestValue.PolymorphicTypes.Add(polymorphicValue);
            polymorphicValue.ValidatedType = typeof(string);
            var validationContext = new ValidatorBuilderContext(manifestValue);

            Assert.That(() => sut.GetPolymorphicContext(validationContext, typeof(string))?.ManifestValue,
                        Is.SameAs(polymorphicValue));
        }

        [Test,AutoMoqData]
        public void GetPolymorphicContextShouldReturnANewContextIfAnExistingPolymorphicTypeDoesNotExist([ManifestModel] ManifestItem manifestValue,
                                                                                                        ValidatorBuilderContextFactory sut)
        {
            manifestValue.PolymorphicTypes.Clear();
            var validationContext = new ValidatorBuilderContext(manifestValue);

            Assert.That(() => sut.GetPolymorphicContext(validationContext, typeof(string))?.ManifestValue?.IsPolymorphicType,
                        Is.True);
        }
    }
}