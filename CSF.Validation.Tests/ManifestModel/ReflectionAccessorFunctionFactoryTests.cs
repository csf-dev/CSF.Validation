using System;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture,Parallelizable]
    public class ReflectionAccessorFunctionFactoryTests
    {
        [Test,AutoMoqData]
        public void GetAccessorFunctionShouldReturnCorrectDelegateForAProperty(ReflectionAccessorFunctionFactory sut,
                                                                               SampleObject obj)
        {
            var result = sut.GetAccessorFunction(typeof(SampleObject), nameof(SampleObject.ValidProperty));

            Assert.Multiple(() =>
            {
                Assert.That(result.ExpectedType, Is.EqualTo(typeof(string)), "Correct expected type");
                Assert.That(() => result.AccessorFunction(obj), Is.EqualTo(obj.ValidProperty), "Delegate gets correct value");
            });
        }

        [Test,AutoMoqData]
        public void GetAccessorFunctionShouldReturnCorrectDelegateForAMethod(ReflectionAccessorFunctionFactory sut,
                                                                               SampleObject obj)
        {
            var result = sut.GetAccessorFunction(typeof(SampleObject), nameof(SampleObject.ValidMethod));

            Assert.Multiple(() =>
            {
                Assert.That(result.ExpectedType, Is.EqualTo(typeof(DateTime)), "Correct expected type");
                Assert.That(() => result.AccessorFunction(obj), Is.EqualTo(obj.ValidMethod()), "Delegate gets correct value");
            });
        }

        [Test,AutoMoqData]
        public void GetAccessorFunctionShouldReturnCorrectDelegateForAField(ReflectionAccessorFunctionFactory sut,
                                                                            SampleObject obj,
                                                                            int fieldValue)
        {
            obj.ValidField = fieldValue;
            var result = sut.GetAccessorFunction(typeof(SampleObject), nameof(SampleObject.ValidField));

            Assert.Multiple(() =>
            {
                Assert.That(result.ExpectedType, Is.EqualTo(typeof(int)), "Correct expected type");
                Assert.That(() => result.AccessorFunction(obj), Is.EqualTo(fieldValue), "Delegate gets correct value");
            });
        }
    }

    public class SampleObject
    {
        public int ValidField;

        public string ValidProperty { get; set; }
        
        public string this[int i] { get => i.ToString(); }

        public DateTime ValidMethod() => DateTime.Today;

        public DateTime InvalidMethodWithParams(int days) => DateTime.Today.AddDays(days);

    }
}