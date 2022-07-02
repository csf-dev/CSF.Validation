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

            Assert.That(result.ManifestValue, Has.Property(nameof(ManifestValue.MemberName)).EqualTo(nameof(ValidatedObject.AProperty)));
        }

        [Test,AutoMoqData]
        public void GetContextForValueShouldReturnContextWithoutMemberName([ManifestModel] ValidatorBuilderContext validationContext,
                                                                           ValidatorBuilderContextFactory sut)
        {
            var result = sut.GetContextForValue<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(result.ManifestValue, Has.Property(nameof(ManifestValue.MemberName)).Null);
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

            Assert.That(result.ManifestValue, Is.InstanceOf<ManifestValue>());
        }

        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithManifestCollectionItemIfEnumeratingItems([Frozen] IStaticallyReflects reflect,
                                                                                                       [ManifestModel] ValidatorBuilderContext validationContext,
                                                                                                       ValidatorBuilderContextFactory sut)
        {
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            
            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext, true);

            Assert.That(result.ManifestValue, Is.InstanceOf<ManifestCollectionItem>());
        }

        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithCorrectAccessor([ManifestModel] ValidatorBuilderContext validationContext,
                                                                              ValidatorBuilderContextFactory sut,
                                                                              ValidatedObject obj)
        {
            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(() => ((ManifestValue) result.ManifestValue).AccessorFromParent(obj), Is.EqualTo(obj.AProperty));
        }

        [Test,AutoMoqData]
        public void GetContextForValueShouldReturnContextWithCorrectAccessor([ManifestModel] ValidatorBuilderContext validationContext,
                                                                             ValidatorBuilderContextFactory sut,
                                                                             ValidatedObject obj)
        {
            var result = sut.GetContextForValue<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(() => ((ManifestValue) result.ManifestValue).AccessorFromParent(obj), Is.EqualTo(obj.AProperty));
        }

        [Test,AutoMoqData]
        public void GetContextForMemberThatAlreadyExistsShouldReturnSameManifestValue([Frozen] IStaticallyReflects reflect,
                                                                                      [ManifestModel] ValidatorBuilderContext validationContext,
                                                                                      ValidatorBuilderContextFactory sut,
                                                                                      [ManifestModel] ManifestValue aPropertyValue)
        {
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            aPropertyValue.MemberName = nameof(ValidatedObject.AProperty);
            validationContext.ManifestValue.Children.Add(aPropertyValue);

            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(result.ManifestValue, Is.SameAs(aPropertyValue));
        }

        [Test,AutoMoqData]
        public void GetContextForMemberThatAlreadyExistsAsCollectionItemShouldReturnSameManifestValue([Frozen] IStaticallyReflects reflect,
                                                                                                      [ManifestModel] ValidatorBuilderContext validationContext,
                                                                                                      ValidatorBuilderContextFactory sut,
                                                                                                      [ManifestModel] ManifestCollectionItem collectionValue)
        {
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            validationContext.ManifestValue.CollectionItemValue = collectionValue;

            var result = sut.GetContextForMember<ValidatedObject,IEnumerable<string>>(v => v.Strings, validationContext, true);

            Assert.That(result.ManifestValue, Is.SameAs(collectionValue));
        }

        [Test,AutoMoqData]
        public void GetPolymorphicContextShouldThrowIfManifestValueCannotHavePolymorphicTypes([ManifestModel] ManifestPolymorphicType manifestModel,
                                                                                              ValidatorBuilderContextFactory sut)
        {
            var validationContext = new ValidatorBuilderContext(manifestModel);
            Assert.That(() => sut.GetPolymorphicContext(validationContext, typeof(object)),
                        Throws.ArgumentException.And.Message.StartWith("The validation manifest value for the current context must implement IHasPolymorphicTypes"));
        }

        [Test,AutoMoqData]
        public void GetPolymorphicContextShouldReturnAContextFromAnExistingPolymorphicTypeIfItExists([ManifestModel] ManifestPolymorphicType polymorphicValue,
                                                                                                     [ManifestModel] ManifestValue manifestValue,
                                                                                                     ValidatorBuilderContextFactory sut)
        {
            manifestValue.PolymorphicTypes.Add(polymorphicValue);
            polymorphicValue.ValidatedType = typeof(string);
            var validationContext = new ValidatorBuilderContext(manifestValue);

            Assert.That(() => sut.GetPolymorphicContext(validationContext, typeof(string))?.ManifestValue,
                        Is.SameAs(polymorphicValue));
        }

        [Test,AutoMoqData]
        public void GetPolymorphicContextShouldReturnANewContextIfAnExistingPolymorphicTypeDoesNotExist([ManifestModel] ManifestValue manifestValue,
                                                                                                        ValidatorBuilderContextFactory sut)
        {
            manifestValue.PolymorphicTypes.Clear();
            var validationContext = new ValidatorBuilderContext(manifestValue);

            Assert.That(() => sut.GetPolymorphicContext(validationContext, typeof(string))?.ManifestValue,
                        Is.InstanceOf<ManifestPolymorphicType>());
        }
    }
}