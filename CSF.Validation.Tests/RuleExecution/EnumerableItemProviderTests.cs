using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class EnumerableItemProviderTests
    {
        [Test,AutoMoqData]
        public void GetEnumerableItemsShouldThrowAneIfItemTypeIsNull(EnumerableItemProvider sut, object[] value)
        {
            Assert.That(() => sut.GetEnumerableItems(value, null), Throws.ArgumentNullException);
        }

        [Test,AutoMoqData]
        public void GetEnumerableItemsShouldReturnNullIfValueIsNull(EnumerableItemProvider sut, Type itemType)
        {
            Assert.That(() => sut.GetEnumerableItems(null, itemType), Is.Null);
        }

        [Test,AutoMoqData]
        public void GetEnumerableItemsShouldReturnEquivalentCollectionIfValueIsCorrectlyEnumerable(EnumerableItemProvider sut, string item1, string item2, string item3)
        {
            object value = new[] { item1, item2, item3 };
            Assert.That(() => sut.GetEnumerableItems(value, typeof(string)), Is.EqualTo(new [] {item1, item2, item3}));
        }

        [Test,AutoMoqData]
        public void GetEnumerableItemsShouldReturnACollectionBasedOnTheCorrectGenericImplementationOfEnumerable(EnumerableItemProvider sut, MultipleEnumerable value)
        {
            /* This test verifies that when dealing with a value which happens to implement IEnumerable<T> more than once, the correct
             * interface is used.  If we simply went to the implementation of non-generic IEnumerable then we're not sure that we're
             * actually going to get the result we expect.  This way, we do the right thing, even if/when faced with an object that
             * is enumerable for more than one generic type.
             */
            
            Assert.Multiple(() =>
            {
                Assert.That(() => sut.GetEnumerableItems(value, typeof(string)),
                            Is.EqualTo(new [] { "One", "Two", "Three" }),
                            "Correct items when requesting IEnumerable<string>");
                Assert.That(() => sut.GetEnumerableItems(value, typeof(int)),
                            Is.EqualTo(new [] { 1, 2, 3 }),
                            "Correct items when requesting IEnumerable<int>");
            });
        }

        [Test,AutoMoqData]
        public void GetEnumerableItemsShouldThrowValidationExceptionIfTheValueDoesNotImplementTheCorrectEnumerableInterface(EnumerableItemProvider sut, object value)
        {
            Assert.That(() => sut.GetEnumerableItems(value, typeof(string)), Throws.InstanceOf<ValidationException>());
        }

        /// <summary>
        /// This class implements both <see cref="IEnumerable{T}"/> for <see cref="String"/> as well as for <see cref="Int32"/>.
        /// Thus, when we want it for strings we must use the string-based version and not simply the non-generic
        /// <see cref="IEnumerable"/>.
        /// </summary>
        public class MultipleEnumerable : IEnumerable<string>, IEnumerable<int>
        {
            List<string> strings = new List<string> { "One", "Two", "Three" };
            List<int> numbers = new List<int> { 1, 2, 3 };

            IEnumerator<string> IEnumerable<string>.GetEnumerator() => strings.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => numbers.GetEnumerator();

            IEnumerator<int> IEnumerable<int>.GetEnumerator() => numbers.GetEnumerator();
        }
    }
}