using System;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation.Tests.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class PolymorphicTypeMustDeriveFromParentValidatedTypeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTypesAreCompatible([ManifestModel] ManifestItem parentValue,
                                                                       [ManifestModel] ManifestItem grandparentValue,
                                                                       [RuleContext] RuleContext context,
                                                                       PolymorphicTypeMustDeriveFromParentValidatedType sut)
        {
            parentValue.ItemType = ManifestItemTypes.PolymorphicType;
            parentValue.Parent = grandparentValue;
            grandparentValue.ValidatedType = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(typeof(Cat), parentValue, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTypesAreNotCompatible([ManifestModel] ManifestItem parentValue,
                                                                          [ManifestModel] ManifestItem grandparentValue,
                                                                          [RuleContext] RuleContext context,
                                                                          PolymorphicTypeMustDeriveFromParentValidatedType sut)
        {
            parentValue.ItemType = ManifestItemTypes.PolymorphicType;
            parentValue.Parent = grandparentValue;
            grandparentValue.ValidatedType = typeof(Cat);
            Assert.That(() => sut.GetResultAsync(typeof(Pet), parentValue, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage([ManifestModel] ManifestItem parentValue,
                                                                     [ManifestModel] ManifestItem grandparentValue,
                                                                     [RuleResult] ValidationRuleResult result,
                                                                     PolymorphicTypeMustDeriveFromParentValidatedType sut)
        {
            parentValue.Parent = grandparentValue;
            grandparentValue.ValidatedType = typeof(Cat);
            Assert.That(async () => await sut.GetFailureMessageAsync(typeof(Pet), parentValue, result),
                        Is.EqualTo(@"Manifest items which include the type PolymorphicType must have a ValidatedType which derives from the ValidatedType of the parent manifest item.
ValidatedType = CSF.Validation.Tests.Rules.PolymorphicTypeMustDeriveFromParentValidatedTypeTests+Pet
Parent.ValidatedType = CSF.Validation.Tests.Rules.PolymorphicTypeMustDeriveFromParentValidatedTypeTests+Cat"));
        }

        public class Pet {}
        public class Cat : Pet {}
    }
}