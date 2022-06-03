using System;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ReflectionDelegateFactoryTests
    {
        [Test,AutoMoqData]
        public void GetAccessorFunctionShouldReturnCorrectDelegateForAProperty(ReflectionDelegateFactory sut,
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
        public void GetAccessorFunctionShouldReturnCorrectDelegateForAMethod(ReflectionDelegateFactory sut,
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
        public void GetAccessorFunctionShouldReturnCorrectDelegateForAField(ReflectionDelegateFactory sut,
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

        [Test,AutoMoqData]
        public void GetAccessorFunctionShouldThrowForInvalidMethodWithParameters(ReflectionDelegateFactory sut)
        {
            Assert.That(() => sut.GetAccessorFunction(typeof(SampleObject), nameof(SampleObject.InvalidMethodWithParams)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetAccessorFunctionShouldThrowForInvalidGenericMethod(ReflectionDelegateFactory sut)
        {
            Assert.That(() => sut.GetAccessorFunction(typeof(SampleObject), nameof(SampleObject.InvalidGenericMethod)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetAccessorFunctionShouldThrowForInvalidIndexer(ReflectionDelegateFactory sut)
        {
            Assert.That(() => sut.GetAccessorFunction(typeof(SampleObject), "Item"), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetAccessorFunctionShouldThrowForInvalidSetOnlyProperty(ReflectionDelegateFactory sut)
        {
            Assert.That(() => sut.GetAccessorFunction(typeof(SampleObject), nameof(SampleObject.InvalidSetOnlyProperty)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetAccessorFunctionShouldThrowForNonExistentMember(ReflectionDelegateFactory sut)
        {
            Assert.That(() => sut.GetAccessorFunction(typeof(SampleObject), "Nope"), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetSetterActionShouldReturnCorrectDelegateForAValidProperty(ReflectionDelegateFactory sut, SampleObject obj, string propertyValue)
        {
            var result = sut.GetSetterAction(typeof(SampleObject), nameof(SampleObject.ValidProperty));
            result(obj, propertyValue);
            Assert.That(obj.ValidProperty, Is.EqualTo(propertyValue));
        }

        [Test,AutoMoqData]
        public void GetSetterActionShouldThrowForAMethodWithNoParameters(ReflectionDelegateFactory sut)
        {
            Assert.That(() => sut.GetSetterAction(typeof(SampleObject), nameof(SampleObject.ValidMethod)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetSetterActionShouldThrowForAMethodWithOneParameter(ReflectionDelegateFactory sut)
        {
            Assert.That(() => sut.GetSetterAction(typeof(SampleObject), nameof(SampleObject.InvalidMethodWithParams)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetSetterActionShouldThrowForAnInvalidGetOnlyProperty(ReflectionDelegateFactory sut)
        {
            Assert.That(() => sut.GetSetterAction(typeof(SampleObject), nameof(SampleObject.InvalidGetOnlyProperty)), Throws.ArgumentException);
        }
    }

    public class SampleObject
    {
        public int ValidField;

        public string ValidProperty { get; set; }

        public string InvalidSetOnlyProperty { set { } }

        public string InvalidGetOnlyProperty { get => "Hello world"; }
        
        public string this[int i] { get => i.ToString(); }

        public DateTime ValidMethod() => DateTime.Today;

        public DateTime InvalidMethodWithParams(int days) => DateTime.Today.AddDays(days);

        public T InvalidGenericMethod<T>() => default(T);
    }
}