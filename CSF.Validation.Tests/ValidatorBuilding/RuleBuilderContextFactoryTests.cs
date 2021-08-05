using System;
using System.Linq.Expressions;
using AutoFixture.NUnit3;
using CSF.Reflection;
using CSF.Validation.Autofixture;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable]
    public class RuleBuilderContextFactoryTests
    {
        [Test,AutoMoqData]
        public void GetRootContextShouldReturnANewContext([ManifestModel] ValidatorBuilderContext validationContext, ValidatorBuilderContextFactory sut)
        {
            Assert.That(() => sut.GetRootContext(), Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithMemberName([Frozen] IStaticallyReflects reflect,
                                                                         [ManifestModel] ValidatorBuilderContext validationContext,
                                                                         ValidatorBuilderContextFactory sut,
                                                                         bool enumerate)
        {
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            
            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext, enumerate);

            Assert.Multiple(() =>
            {
                Assert.That(result.ManifestValue, Has.Property(nameof(ManifestValue.MemberName)).EqualTo(nameof(ValidatedObject.AProperty)));
                Assert.That(result.ManifestValue, Has.Property(nameof(ManifestValue.EnumerateItems)).EqualTo(enumerate));
            });
        }

        [Test,AutoMoqData]
        public void GetContextForValueShouldReturnContextWithoutMemberName([ManifestModel] ValidatorBuilderContext validationContext,
                                                                           ValidatorBuilderContextFactory sut,
                                                                           bool enumerate)
        {
            var result = sut.GetContextForValue<ValidatedObject,string>(v => v.AProperty, validationContext, enumerate);

            Assert.Multiple(() =>
            {
                Assert.That(result.ManifestValue, Has.Property(nameof(ManifestValue.MemberName)).Null);
                Assert.That(result.ManifestValue, Has.Property(nameof(ManifestValue.EnumerateItems)).EqualTo(enumerate));
            });
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
    }
}