using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ModelValueToManifestValueConverterTests
    {
        [Test,AutoMoqData]
        public void ConvertAllValuesShouldSuccessfullyConvertASingleRootModelValue([Frozen] IGetsAccessorFunction accessorFactory,
                                                                                   [Frozen] IGetsValidatedType validatedTypeProvider,
                                                                                   ModelValueToManifestValueConverter sut,
                                                                                   [ManifestModel] ModelToManifestConversionContext context,
                                                                                   AccessorFunctionAndType accessor)
        {
            Mock.Get(accessorFactory)
                .Setup(x => x.GetAccessorFunction(context.ValidatedType, context.CurrentValue.IdentityMemberName))
                .Returns(accessor);
            Mock.Get(validatedTypeProvider)
                .Setup(x => x.GetValidatedType(It.IsAny<Type>(), It.IsAny<bool>()))
                .Returns((Type t, bool b) => t);

            var result = sut.ConvertAllValues(context);

            Assert.Multiple(() =>
            {
                Assert.That(result.ConvertedValues, Has.Count.EqualTo(1), "Count of converted values");
                var converted = result.ConvertedValues.Single();
                Assert.That(converted.ManifestValue, Has.Property(nameof(ManifestValue.AccessorFromParent)).SameAs(context.AccessorFromParent));
                Assert.That(converted.ManifestValue, Has.Property(nameof(ManifestValue.Children)).Empty);
                Assert.That(converted.ManifestValue, Has.Property(nameof(ManifestValue.IdentityAccessor)).SameAs(accessor.AccessorFunction));
                Assert.That(converted.ManifestValue, Has.Property(nameof(ManifestValue.MemberName)).EqualTo(context.MemberName));
                Assert.That(converted.ManifestValue, Has.Property(nameof(ManifestValue.Parent)).SameAs(context.ParentManifestValue));
                Assert.That(converted.ManifestValue, Has.Property(nameof(ManifestValue.Rules)).Empty);
            });
        }

        [Test,AutoMoqData]
        public void ConvertAllValuesShouldSuccessfullyConvertAMultiLevelValueHierarchy([Frozen] IGetsAccessorFunction accessorFactory,
                                                                                       [Frozen] IGetsValidatedType validatedTypeProvider,
                                                                                       ModelValueToManifestValueConverter sut,
                                                                                       [ManifestModel] ModelToManifestConversionContext context,
                                                                                       [ManifestModel] Value child1,
                                                                                       [ManifestModel] Value child2,
                                                                                       [ManifestModel] Value grandchild1,
                                                                                       [ManifestModel] Value grandchild2,
                                                                                       AccessorFunctionAndType accessor)
        {
            Mock.Get(accessorFactory)
                .Setup(x => x.GetAccessorFunction(It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(accessor);
            Mock.Get(validatedTypeProvider)
                .Setup(x => x.GetValidatedType(It.IsAny<Type>(), It.IsAny<bool>()))
                .Returns((Type t, bool b) => t);
            context.CurrentValue.Children.Add("Foo", child1);
            context.CurrentValue.Children.Add("Bar", child2);
            child1.Children.Add("Foo", grandchild1);
            child1.Children.Add("Bar", grandchild2);

            var result = sut.ConvertAllValues(context);

            Assert.Multiple(() =>
            {
                Assert.That(result.ConvertedValues, Has.Count.EqualTo(5), "Count of converted values");
                Assert.That(result.RootValue, Has.Property(nameof(ManifestValue.Parent)).SameAs(context.ParentManifestValue), "Correct parent for root value");
            });
        }

        [Test,AutoMoqData]
        public void ConvertAllValuesShouldCreateChildAccessorFromEnumeratedTypeWhereAppropriate([Frozen] IGetsAccessorFunction accessorFactory,
                                                                                                [Frozen] IGetsValidatedType validatedTypeProvider,
                                                                                                ModelValueToManifestValueConverter sut,
                                                                                                [ManifestModel] ModelToManifestConversionContext context,
                                                                                                [ManifestModel] Value collectionChild,
                                                                                                AccessorFunctionAndType accessor)
        {
            Mock.Get(accessorFactory)
                .Setup(x => x.GetAccessorFunction(It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(accessor);
            Mock.Get(validatedTypeProvider)
                .Setup(x => x.GetValidatedType(It.IsAny<Type>(), It.IsAny<bool>()))
                .Returns((Type t, bool b) => t);
            Mock.Get(validatedTypeProvider)
                .Setup(x => x.GetValidatedType(typeof(List<ComplexObject>), true))
                .Returns(typeof(ComplexObject));
            context.CurrentValue.CollectionItemValue = new CollectionItemValue { Children = new Dictionary<string,Value>{ { "Foo", collectionChild } }, };
            context.ValidatedType = typeof(List<ComplexObject>);

            var result = sut.ConvertAllValues(context);

            Mock.Get(accessorFactory).Verify(x => x.GetAccessorFunction(typeof(ComplexObject), "Foo"), Times.Once);
        }

        [Test,AutoMoqData]
        public void ConvertAllValuesShouldCreateIdentityAccessorFromEnumeratedTypeWhereAppropriate([Frozen] IGetsAccessorFunction accessorFactory,
                                                                                                   [Frozen] IGetsValidatedType validatedTypeProvider,
                                                                                                   ModelValueToManifestValueConverter sut,
                                                                                                   [ManifestModel] ModelToManifestConversionContext context,
                                                                                                   AccessorFunctionAndType accessor)
        {
            Mock.Get(accessorFactory)
                .Setup(x => x.GetAccessorFunction(It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(accessor);
            Mock.Get(validatedTypeProvider)
                .Setup(x => x.GetValidatedType(It.IsAny<Type>(), It.IsAny<bool>()))
                .Returns((Type t, bool b) => t);
            Mock.Get(validatedTypeProvider)
                .Setup(x => x.GetValidatedType(typeof(List<ComplexObject>), true))
                .Returns(typeof(ComplexObject));
            context.CurrentValue.CollectionItemValue = new CollectionItemValue { IdentityMemberName = "Foo" };
            context.ValidatedType = typeof(List<ComplexObject>);

            var result = sut.ConvertAllValues(context);

            Mock.Get(accessorFactory).Verify(x => x.GetAccessorFunction(typeof(ComplexObject), "Foo"), Times.Once);
        }
    }
}