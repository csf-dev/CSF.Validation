using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class NotEmptyTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWithAnEmptyCollection(NotEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string>(), context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWithANonEmptyCollection(NotEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string> {"Foo", "Bar"}, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWithAnEmptyArray(NotEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new string[0], context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWithANonEmptyArray(NotEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new [] {"Foo", "Bar"}, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWithAnEmptyString(NotEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(String.Empty, context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWithANonEmptyString(NotEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync("Foo".AsQueryable(), context), Is.PassingValidationResult);
        }
    }
}