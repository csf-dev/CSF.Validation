using System;
using System.Linq.Expressions;
using AutoFixture.NUnit3;
using CSF.Reflection;
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
        public void GetRootContextShouldReturnANewContext(ValidatorBuilderContext validationContext, ValidatorBuilderContextFactory sut)
        {
            Assert.That(() => sut.GetRootContext(), Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithMemberName([Frozen] IStaticallyReflects reflect,
                                                                         ValidatorBuilderContext validationContext,
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
        public void GetContextForValueShouldReturnContextWithoutMemberName(ValidatorBuilderContext validationContext,
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
        public void GetContextForMemberShouldReturnContextWithCorrectAccessor(ValidatorBuilderContext validationContext,
                                                                              ValidatorBuilderContextFactory sut,
                                                                              ValidatedObject obj)
        {
            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(() => result.ManifestValue.AccessorFromParent(obj), Is.EqualTo(obj.AProperty));
        }

        [Test,AutoMoqData]
        public void GetContextForValueShouldReturnContextWithCorrectAccessor(ValidatorBuilderContext validationContext,
                                                                             ValidatorBuilderContextFactory sut,
                                                                             ValidatedObject obj)
        {
            var result = sut.GetContextForValue<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(() => result.ManifestValue.AccessorFromParent(obj), Is.EqualTo(obj.AProperty));
        }
    }
}