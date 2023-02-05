using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class MustBeDefinedEnumConstantTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWhenValueIsADefinedConstantUsingType(MustBeDefinedEnumConstant sut, [RuleContext] RuleContext context)
        {
            sut.EnumType = typeof(MyFunkyEnum);
            Assert.That(() => sut.GetResultAsync(MyFunkyEnum.FunkyValueOne, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWhenValueIsANumbrWhichCorrespondsToADefinedConstant(MustBeDefinedEnumConstant sut, [RuleContext] RuleContext context)
        {
            sut.EnumType = typeof(MyFunkyEnum);
            Assert.That(() => sut.GetResultAsync(1, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWhenValueIsADefinedConstantUsingTypeName(MustBeDefinedEnumConstant sut, [RuleContext] RuleContext context)
        {
            sut.EnumType = null;
            sut.EnumTypeName = "CSF.Validation.Rules.MustBeDefinedEnumConstantTests+MyFunkyEnum, CSF.Validation.Tests";
            Assert.That(() => sut.GetResultAsync(MyFunkyEnum.FunkyValueOne, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWhenValueIsNull(MustBeDefinedEnumConstant sut, [RuleContext] RuleContext context)
        {
            sut.EnumType = typeof(MyFunkyEnum);
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWhenValueIsNotADefinedConstant(MustBeDefinedEnumConstant sut, [RuleContext] RuleContext context)
        {
            sut.EnumType = typeof(MyFunkyEnum);
            Assert.That(() => sut.GetResultAsync(2, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnErrorWhenNoEnumTypeProvided(MustBeDefinedEnumConstant sut, [RuleContext] RuleContext context)
        {
            sut.EnumType = null;
            sut.EnumTypeName = null;
            Assert.That(() => sut.GetResultAsync(2, context), Is.ErrorRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnErrorWhenEnumTypeIsNotFound(MustBeDefinedEnumConstant sut, [RuleContext] RuleContext context)
        {
            sut.EnumType = null;
            sut.EnumTypeName = "This.Type.Does.Not.Exist.ZZZ";
            Assert.That(() => sut.GetResultAsync(2, context), Is.ErrorRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnErrorWhenTypeIsNotAnEnumType(MustBeDefinedEnumConstant sut, [RuleContext] RuleContext context)
        {
            sut.EnumType = typeof(DateTime);
            Assert.That(() => sut.GetResultAsync(0, context), Is.ErrorRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncGenericShouldReturnPassWhenValueIsADefinedConstantUsingType(MustBeDefinedEnumConstant<MyFunkyEnum> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(MyFunkyEnum.FunkyValueOne, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncGenericShouldReturnPassWhenValueIsNull(MustBeDefinedEnumConstant<MyFunkyEnum> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncGenericShouldReturnFailWhenValueIsNotADefinedConstant(MustBeDefinedEnumConstant<MyFunkyEnum> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((MyFunkyEnum) 2, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessageGeneric(MustBeDefinedEnumConstant<MyFunkyEnum> sut, [RuleResult] ValidationRuleResult result)
        {
            Assert.That(() => sut.GetFailureMessageAsync((MyFunkyEnum)2, result),
                        Is.EqualTo("The value must be a defined constant of the enum CSF.Validation.Rules.MustBeDefinedEnumConstantTests+MyFunkyEnum, but it is not."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessagewhenIntendedTypeIsKnown(MustBeDefinedEnumConstant sut,
                                                                                            [RuleContext] RuleContext context,
                                                                                            IValidationLogic logic)
        {
            var data = new Dictionary<string, object> { { "Enum type", typeof(MyFunkyEnum) } };
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, data), context, logic);
            Assert.That(() => sut.GetFailureMessageAsync((MyFunkyEnum)2, result),
                        Is.EqualTo("The value must be a defined constant of the enum CSF.Validation.Rules.MustBeDefinedEnumConstantTests+MyFunkyEnum, but it is not."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessagewhenIntendedTypeIsNotKnown(MustBeDefinedEnumConstant sut,
                                                                                               [RuleResult] ValidationRuleResult result)
        {
            Assert.That(() => sut.GetFailureMessageAsync((MyFunkyEnum)2, result),
                        Is.EqualTo("The value must be a defined constant of the enum <unknown>, but it is not."));
        }

        public enum MyFunkyEnum { FunkyValueOne = 0, FunkyValueTwo = 1 }
    }
}