using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class ValidatedValueFactoryTests
    {
        [Test,AutoMoqData]
        public void GetValidatedValueShouldReturnSingleValueForManifestValueWithNoParentOrChildrenOrRules([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                          ValidatedValueFactory sut,
                                                                                                          object validatedValue,
                                                                                                          ValidationOptions validationOptions,
                                                                                                          [ExecutableModel] ValidatedValue value)
        {
            var manifestValue = new ManifestValue { ValidatedType = typeof(object) };
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(value);

            var result = sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Assert.That(result, Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldCreateCollectionValueFromFactoryIfManifestHasACollectionItem([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                        [Frozen] IGetsEnumerableItemsToBeValidated enumerableProvider,
                                                                                                        ValidatedValueFactory sut,
                                                                                                        IEnumerable<object> validatedValue,
                                                                                                        ValidationOptions validationOptions,
                                                                                                        [ExecutableModel] ValidatedValue value,
                                                                                                        [ExecutableModel] ValidatedValue collectionValue,
                                                                                                        object item)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(IEnumerable<object>),
                CollectionItemValue = new ManifestCollectionItem
                {
                    ValidatedType = typeof(object)
                },
            };
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(value);
            value.ValueResponse = validatedValue;
            Mock.Get(enumerableProvider)
                .Setup(x => x.GetEnumerableItems(validatedValue, manifestValue.CollectionItemValue.ValidatedType))
                .Returns(new[] { item });
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue.CollectionItemValue && b.ActualValue == item)))
                .Returns(collectionValue);

            sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue.CollectionItemValue)), Times.Once);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldCreateAValueValueFromFactoryForEachCollectionItem([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                             [Frozen] IGetsEnumerableItemsToBeValidated enumerableProvider,
                                                                                             ValidatedValueFactory sut,
                                                                                             IEnumerable<object> validatedValue,
                                                                                             ValidationOptions validationOptions,
                                                                                             [ExecutableModel] ValidatedValue value,
                                                                                             [ExecutableModel] ValidatedValue collectionValue,
                                                                                             object item1,
                                                                                             object item2,
                                                                                             object item3)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(IEnumerable<object>),
                CollectionItemValue = new ManifestCollectionItem
                {
                    ValidatedType = typeof(object)
                },
            };
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(value);
            value.ValueResponse = validatedValue;
            Mock.Get(enumerableProvider)
                .Setup(x => x.GetEnumerableItems(validatedValue, manifestValue.CollectionItemValue.ValidatedType))
                .Returns(new[] { item1, item2, item3 });
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue.CollectionItemValue)))
                .Returns(collectionValue);

            sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue.CollectionItemValue)), Times.Exactly(3));
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldProcessAChildManifestValueIfTheActualValueCanBeRetrieved([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                    [Frozen] IGetsValueToBeValidated valueProvider,
                                                                                                    ValidatedValueFactory sut,
                                                                                                    [NoAutoProperties] ComplexObject validatedValue,
                                                                                                    string childValue,
                                                                                                    ValidationOptions validationOptions,
                                                                                                    [ExecutableModel] ValidatedValue val,
                                                                                                    [ExecutableModel] ValidatedValue childVal)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                ValidatedType = typeof(string),
            };
            manifestValue.Children.Add(childManifest);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(val);
            val.ValueResponse = validatedValue;
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest)))
                .Returns(childVal);
            childVal.ValueResponse = childValue;
            object child = childValue;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(childManifest, validatedValue, validationOptions, out child))
                .Returns(true);

            var result = sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest && b.ActualValue == child)), Times.Once);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldNotProcessAChildManifestValueIfTheActualValueCannotBeRetrieved([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                          [Frozen] IGetsValueToBeValidated valueProvider,
                                                                                                          ValidatedValueFactory sut,
                                                                                                          [NoAutoProperties] ComplexObject validatedValue,
                                                                                                          string childValue,
                                                                                                          ValidationOptions validationOptions,
                                                                                                          [ExecutableModel] ValidatedValue val,
                                                                                                          [ExecutableModel] ValidatedValue childVal)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                ValidatedType = typeof(string),
            };
            manifestValue.Children.Add(childManifest);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(val);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest)))
                .Returns(childVal);
            object child = childValue;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(childManifest, validatedValue, validationOptions, out child))
                .Returns(false);

            var result = sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest && b.ActualValue == child)), Times.Never);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldBeAbleToProcessAGrandchildManifestValue([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                    [Frozen] IGetsValueToBeValidated valueProvider,
                                                                                                    ValidatedValueFactory sut,
                                                                                                    [NoAutoProperties] ComplexObject validatedValue,
                                                                                                    [NoAutoProperties] ComplexObject childValue,
                                                                                                    string grandchildValue,
                                                                                                    ValidationOptions validationOptions,
                                                                                                    [ExecutableModel] ValidatedValue val,
                                                                                                    [ExecutableModel] ValidatedValue childVal,
                                                                                                    [ExecutableModel] ValidatedValue grandchildVal)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                ValidatedType = typeof(ComplexObject),
            };
            var grandchildManifest = new ManifestValue
            {
                Parent = childManifest,
                ValidatedType = typeof(string),
            };
            manifestValue.Children.Add(childManifest);
            childManifest.Children.Add(grandchildManifest);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(val);
            val.ValueResponse = validatedValue;
            val.ManifestValue = manifestValue;
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest)))
                .Returns(childVal);
            childVal.ValueResponse = childValue;
            childVal.ManifestValue = childManifest;;
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == grandchildManifest)))
                .Returns(grandchildVal);
            grandchildVal.ValueResponse = grandchildValue;
            grandchildVal.ManifestValue = grandchildManifest;
            object child = childValue;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(childManifest, validatedValue, validationOptions, out child))
                .Returns(true);
            object grandchild = grandchildValue;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(grandchildManifest, childValue, validationOptions, out grandchild))
                .Returns(true);

            var result = sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest && b.ActualValue == child)), Times.Once);
            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == grandchildManifest && b.ActualValue == grandchild)), Times.Once);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldBeAbleToProcessACollectionWithinACollection([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                    [Frozen] IGetsValueToBeValidated valueProvider,
                                                                                                    [Frozen] IGetsEnumerableItemsToBeValidated enumerableProvider,
                                                                                                    ValidatedValueFactory sut,
                                                                                                    [NoAutoProperties] ComplexObject validatedValue,
                                                                                                    ValidationOptions validationOptions,
                                                                                                    [ExecutableModel] ValidatedValue val,
                                                                                                    [ExecutableModel] ValidatedValue firstCollection,
                                                                                                    [ExecutableModel] ValidatedValue secondCollection,
                                                                                                    [ExecutableModel] ValidatedValue item)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
                Children = new [] {
                    new ManifestValue
                    {
                        ValidatedType = typeof(ICollection<List<ComplexObject>>),
                        MemberName = nameof(ComplexObject.DoubleCollection),
                        AccessorFromParent = obj => ((ComplexObject) obj).DoubleCollection,
                        CollectionItemValue = new ManifestCollectionItem
                        {
                            ValidatedType = typeof(List<ComplexObject>),
                            CollectionItemValue = new ManifestCollectionItem
                            {
                                ValidatedType = typeof(ComplexObject),
                            }
                        },
                    },
                },
            };
            var child = manifestValue.Children.Single();
            child.Parent = manifestValue;
            child.CollectionItemValue.Parent = manifestValue;
            child.CollectionItemValue.CollectionItemValue.Parent = manifestValue;
            validatedValue.DoubleCollection = new List<List<ComplexObject>>{new List<ComplexObject>{new ComplexObject()}};
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(val);
            val.ValueResponse = validatedValue;
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == child)))
                .Returns(firstCollection);
            firstCollection.ValueResponse = validatedValue.DoubleCollection;
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == child.CollectionItemValue)))
                .Returns(secondCollection);
            secondCollection.ValueResponse = validatedValue.DoubleCollection.First();
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == child.CollectionItemValue.CollectionItemValue)))
                .Returns(item);
            item.ValueResponse = validatedValue.DoubleCollection.First().First();
            object validatedVal = validatedValue;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(manifestValue, validatedValue, validationOptions, out validatedVal))
                .Returns(true);
            object doubleCollection = firstCollection.ValueResponse;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(child, validatedValue, validationOptions, out doubleCollection))
                .Returns(true);
            Mock.Get(enumerableProvider)
                .Setup(x => x.GetEnumerableItems(validatedValue.DoubleCollection, child.CollectionItemValue.ValidatedType))
                .Returns(validatedValue.DoubleCollection);
            Mock.Get(enumerableProvider)
                .Setup(x => x.GetEnumerableItems(validatedValue.DoubleCollection.First(), child.CollectionItemValue.CollectionItemValue.ValidatedType))
                .Returns(validatedValue.DoubleCollection.First());

            var result = sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == child.CollectionItemValue.CollectionItemValue)), Times.Once);
        }
    }
}