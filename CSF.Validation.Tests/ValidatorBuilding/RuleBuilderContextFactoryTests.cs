using System;
using System.Linq.Expressions;
using AutoFixture.NUnit3;
using CSF.Reflection;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable]
    public class RuleBuilderContextFactoryTests
    {
        [Test,AutoMoqData]
        public void GetContextShouldReturnContextUsingValidationContext(ValidatorBuilderContext validationContext, RuleBuilderContextFactory sut)
        {
            var result = sut.GetContext(validationContext);
            Assert.That(result, Has.Property(nameof(RuleBuilderContext.ValidatorBuilderContext)).SameAs(validationContext));
        }

        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithMemberName([Frozen] IStaticallyReflects reflect,
                                                                         ValidatorBuilderContext validationContext,
                                                                         RuleBuilderContextFactory sut,
                                                                         bool enumerate)
        {
            Mock.Get(reflect)
                .Setup(x => x.Member(It.IsAny<Expression<Func<ValidatedObject, string>>>()))
                .Returns((Expression<Func<ValidatedObject, string>> accessor) => Reflect.Member(accessor));
            
            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext, enumerate);

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Property(nameof(RuleBuilderContext.ValidatorBuilderContext)).SameAs(validationContext));
                Assert.That(result, Has.Property(nameof(RuleBuilderContext.MemberName)).EqualTo(nameof(ValidatedObject.AProperty)));
                Assert.That(result, Has.Property(nameof(RuleBuilderContext.EnumerateValueItems)).EqualTo(enumerate));
            });
        }

        [Test,AutoMoqData]
        public void GetContextForValueShouldReturnContextWithoutMemberName(ValidatorBuilderContext validationContext,
                                                                           RuleBuilderContextFactory sut,
                                                                           bool enumerate)
        {
            var result = sut.GetContextForValue<ValidatedObject,string>(v => v.AProperty, validationContext, enumerate);

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Property(nameof(RuleBuilderContext.ValidatorBuilderContext)).SameAs(validationContext));
                Assert.That(result, Has.Property(nameof(RuleBuilderContext.MemberName)).Null);
                Assert.That(result, Has.Property(nameof(RuleBuilderContext.EnumerateValueItems)).EqualTo(enumerate));
            });
        }

        [Test,AutoMoqData]
        public void GetContextForMemberShouldReturnContextWithCorrectAccessor(ValidatorBuilderContext validationContext,
                                                                              RuleBuilderContextFactory sut,
                                                                              ValidatedObject obj)
        {
            var result = sut.GetContextForMember<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(() => result.ValueAccessor(obj), Is.EqualTo(obj.AProperty));
        }

        [Test,AutoMoqData]
        public void GetContextForValueShouldReturnContextWithCorrectAccessor(ValidatorBuilderContext validationContext,
                                                                             RuleBuilderContextFactory sut,
                                                                             ValidatedObject obj)
        {
            var result = sut.GetContextForValue<ValidatedObject,string>(v => v.AProperty, validationContext);

            Assert.That(() => result.ValueAccessor(obj), Is.EqualTo(obj.AProperty));
        }
    }
}