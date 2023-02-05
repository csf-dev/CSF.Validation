using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Manifest
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class RecursiveManifestValueTests
    {
        [Test,AutoMoqData]
        public void ToStringShouldReturnAStringWithTheTypeAndMemberName(ManifestItem item)
        {
            Mock.Get(item).SetupGet(x => x.ValidatedType).Returns(typeof(string));

            var sut = new RecursiveManifestValue(item) { MemberName = "Foo" };

            Assert.That(() => sut.ToString(), Is.EqualTo("[RecursiveManifestValue: Type = String, MemberName = Foo]"));
        }

        [Test,AutoMoqData]
        public void SetValidatedTypeShouldThrow(ManifestItem item)
        {
            var sut = new RecursiveManifestValue(item);
            Assert.That(() => sut.ValidatedType = typeof(string), Throws.InstanceOf<NotSupportedException>())
        }

        [Test,AutoMoqData]
        public void SetIdentityAccessorShouldThrow(ManifestItem item)
        {
            var sut = new RecursiveManifestValue(item);
            Assert.That(() => sut.IdentityAccessor = x => "foo", Throws.InstanceOf<NotSupportedException>())
        }

        [Test,AutoMoqData]
        public void SetCollectionItemValueShouldThrow(ManifestItem item)
        {
            var sut = new RecursiveManifestValue(item);
            Assert.That(() => sut.CollectionItemValue = new ManifestCollectionItem(), Throws.InstanceOf<NotSupportedException>())
        }

        [Test,AutoMoqData]
        public void SetChildrenShouldThrow(ManifestItem item)
        {
            var sut = new RecursiveManifestValue(item);
            Assert.That(() => sut.Children = new List<ManifestValue>(), Throws.InstanceOf<NotSupportedException>())
        }

        [Test,AutoMoqData]
        public void SetRulesShouldThrow(ManifestItem item)
        {
            var sut = new RecursiveManifestValue(item);
            Assert.That(() => sut.Rules = new List<ManifestRule>(), Throws.InstanceOf<NotSupportedException>())
        }

        [Test,AutoMoqData]
        public void SetAccessorExceptionBehaviourShouldThrow(ManifestItem item)
        {
            var sut = new RecursiveManifestValue(item);
            Assert.That(() => sut.AccessorExceptionBehaviour = ValueAccessExceptionBehaviour.Throw, Throws.InstanceOf<NotSupportedException>())
        }
    }
}