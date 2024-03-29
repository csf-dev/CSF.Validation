using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using NUnit.Framework;
using CSF.Specifications;
using System;

namespace CSF.Validation
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValueContextIsMatchForItemTests
    {
        [Test,AutoMoqData]
        public void GetExpressionGetsAnExpresssionWhichReturnsTrueForAManifestValueAndNoItem([ManifestModel] ManifestItem value,
                                                                                             object identity,
                                                                                             object actualValue)
        {
            var context = new ValueContext(identity, actualValue, value);
            var sut = new ValueContextIsMatchForItem(value);
            Assert.That(sut.Matches(context), Is.True);
        }

        [Test,AutoMoqData]
        public void GetExpressionGetsAnExpresssionWhichReturnsFalseForADifferentManifestValueAndNoItem([ManifestModel] ManifestItem value,
                                                                                                       [ManifestModel] ManifestItem otherValue,
                                                                                                       object identity,
                                                                                                       object actualValue)
        {
            var context = new ValueContext(identity, actualValue, value);
            var sut = new ValueContextIsMatchForItem(otherValue);
            Assert.That(sut.Matches(context), Is.False);
        }

        [Test,AutoMoqData]
        public void GetExpressionGetsAnExpresssionWhichReturnsTrueForAManifestValueAndAnItemWithTheCorrectIdentity([ManifestModel] ManifestItem value,
                                                                                                                   ObjectWithIdentity item)
        {
            value.IdentityAccessor = obj => ((ObjectWithIdentity)obj).Identity;
            var context = new ValueContext(item.Identity, item, value);
            var sut = new ValueContextIsMatchForItem(value, item);
            Assert.That(sut.Matches(context), Is.True);
        }

        [Test,AutoMoqData]
        public void GetExpressionGetsAnExpresssionWhichReturnsFalseForAManifestValueAndAnItemWithTheWrongIdentity([ManifestModel] ManifestItem value,
                                                                                                                  ObjectWithIdentity item)
        {
            value.IdentityAccessor = obj => ((ObjectWithIdentity)obj).Identity;
            var context = new ValueContext(Guid.NewGuid(), item, value);
            var sut = new ValueContextIsMatchForItem(value, item);
            Assert.That(sut.Matches(context), Is.False);
        }

        [Test,AutoMoqData]
        public void GetExpressionGetsAnExpresssionWhichReturnsTrueForAManifestValueAndTheSameItemWithoutIdentity([ManifestModel] ManifestItem value,
                                                                                                                 ObjectWithIdentity item)
        {
            value.IdentityAccessor = null;
            var context = new ValueContext(null, item, value);
            var sut = new ValueContextIsMatchForItem(value, item);
            Assert.That(sut.Matches(context), Is.True);
        }

        [Test,AutoMoqData]
        public void GetExpressionGetsAnExpresssionWhichReturnsFalseForAManifestValueAndADifferentItemWithoutIdentity([ManifestModel] ManifestItem value,
                                                                                                                     ObjectWithIdentity item,
                                                                                                                     ObjectWithIdentity otherItem)
        {
            value.IdentityAccessor = null;
            var context = new ValueContext(null, item, value);
            var sut = new ValueContextIsMatchForItem(value, otherItem);
            Assert.That(sut.Matches(context), Is.False);
        }

        public class ObjectWithIdentity
        {
            public Guid Identity { get; set; } = Guid.NewGuid();
        }
    }
}