using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValidatedValueFactoryTests
    {
        [Test,AutoMoqData]
        public void GetValidatedValueShouldReturnSingleValueForManifestValueWithNoParentOrChildrenOrRules([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                          ValidatedValueFactory sut,
                                                                                                          object validatedValue,
                                                                                                          ResolvedValidationOptions validationOptions,
                                                                                                          [ExecutableModel] ValidatedValue value)
        {
            var manifestValue = new ManifestItem { ValidatedType = typeof(object) };
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
                                                                                                        ResolvedValidationOptions validationOptions,
                                                                                                        [ExecutableModel] ValidatedValue value,
                                                                                                        [ExecutableModel] ValidatedValue collectionValue,
                                                                                                        object item)
        {
            var manifestValue = new ManifestItem
            {
                ValidatedType = typeof(IEnumerable<object>),
                CollectionItemValue = new ManifestItem
                {
                    ValidatedType = typeof(object),
                    ItemType = ManifestItemTypes.CollectionItem,
                },
            };
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(value);
            value.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(validatedValue);
            Mock.Get(enumerableProvider)
                .Setup(x => x.GetEnumerableItems(validatedValue, manifestValue.CollectionItemValue.ValidatedType))
                .Returns(new[] { item });
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue.CollectionItemValue && b.GetActualValue() == item)))
                .Returns(collectionValue);

            sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue.CollectionItemValue)), Times.Once);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldCreateCollectionValueFromFactoryIfManifestHasACollectionItemWithinAChildItem([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                        [Frozen] IGetsEnumerableItemsToBeValidated enumerableProvider,
                                                                                                        ValidatedValueFactory sut,
                                                                                                        IEnumerable<object> validatedValue,
                                                                                                        ResolvedValidationOptions validationOptions,
                                                                                                        [ExecutableModel] ValidatedValue value,
                                                                                                        [ExecutableModel] ValidatedValue childValue,
                                                                                                        [ExecutableModel] ValidatedValue collectionValue,
                                                                                                        object item)
        {
            var manifestValue = new ManifestItem
            {
                ValidatedType = typeof(object),
                Children = new[] {
                    new ManifestItem
                    {
                        ValidatedType = typeof(IEnumerable<object>),
                        CollectionItemValue = new ManifestItem
                        {
                            ValidatedType = typeof(object),
                            ItemType = ManifestItemTypes.CollectionItem,
                        },
                    }
                }
            };
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue.Children.First())))
                .Returns(childValue);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(value);
            value.ManifestValue = manifestValue;
            childValue.ManifestValue = manifestValue.Children.First();
            collectionValue.ManifestValue = manifestValue.Children.First().CollectionItemValue;
            childValue.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(validatedValue);
            Mock.Get(enumerableProvider)
                .Setup(x => x.GetEnumerableItems(validatedValue, manifestValue.Children.First().CollectionItemValue.ValidatedType))
                .Returns(new[] { item });
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue.Children.First().CollectionItemValue && b.GetActualValue() == item)))
                .Returns(collectionValue);

            var result = sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Assert.Multiple(() =>
            {
                Assert.That(result.CollectionItems, Is.Empty, "Root value has no collection items");
                Assert.That(result.ChildValues, Has.Count.EqualTo(1), "Root value has one child value");
                Assert.That(result.ChildValues.FirstOrDefault()?.CollectionItems, Has.Count.EqualTo(1), "Root value has one collection item");
            });
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldCreateAValueValueFromFactoryForEachCollectionItem([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                             [Frozen] IGetsEnumerableItemsToBeValidated enumerableProvider,
                                                                                             ValidatedValueFactory sut,
                                                                                             IEnumerable<object> validatedValue,
                                                                                             ResolvedValidationOptions validationOptions,
                                                                                             [ExecutableModel] ValidatedValue value,
                                                                                             [ExecutableModel] ValidatedValue collectionValue,
                                                                                             object item1,
                                                                                             object item2,
                                                                                             object item3)
        {
            var manifestValue = new ManifestItem
            {
                ValidatedType = typeof(IEnumerable<object>),
                CollectionItemValue = new ManifestItem
                {
                    ValidatedType = typeof(object),
                    ItemType = ManifestItemTypes.CollectionItem,
                },
            };
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(value);
            value.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(validatedValue);
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
                                                                                                    ResolvedValidationOptions validationOptions,
                                                                                                    [ExecutableModel] ValidatedValue val,
                                                                                                    [ExecutableModel] ValidatedValue childVal)
        {
            var manifestValue = new ManifestItem
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestItem
            {
                Parent = manifestValue,
                ValidatedType = typeof(string),
            };
            manifestValue.Children.Add(childManifest);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(val);
            val.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(validatedValue);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest)))
                .Returns(childVal);
            childVal.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(childValue);
            object child = childValue;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(childManifest, validatedValue, validationOptions))
                .Returns(new SuccessfulGetValueToBeValidatedResponse(child));

            var result = sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest && b.GetActualValue() == child)), Times.Once);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldNotProcessAChildManifestValueIfTheActualValueCannotBeRetrieved([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                          [Frozen] IGetsValueToBeValidated valueProvider,
                                                                                                          ValidatedValueFactory sut,
                                                                                                          [NoAutoProperties] ComplexObject validatedValue,
                                                                                                          string childValue,
                                                                                                          ResolvedValidationOptions validationOptions,
                                                                                                          [ExecutableModel] ValidatedValue val,
                                                                                                          [ExecutableModel] ValidatedValue childVal)
        {
            var manifestValue = new ManifestItem
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestItem
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
                .Setup(x => x.GetValueToBeValidated(childManifest, validatedValue, validationOptions))
                .Returns(new IgnoredGetValueToBeValidatedResponse());

            var result = sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest && b.ValidatedValueResponse == child)), Times.Never);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldBeAbleToProcessAGrandchildManifestValue([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                    [Frozen] IGetsValueToBeValidated valueProvider,
                                                                                                    ValidatedValueFactory sut,
                                                                                                    [NoAutoProperties] ComplexObject validatedValue,
                                                                                                    [NoAutoProperties] ComplexObject childValue,
                                                                                                    string grandchildValue,
                                                                                                    ResolvedValidationOptions validationOptions,
                                                                                                    [ExecutableModel] ValidatedValue val,
                                                                                                    [ExecutableModel] ValidatedValue childVal,
                                                                                                    [ExecutableModel] ValidatedValue grandchildVal)
        {
            var manifestValue = new ManifestItem
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestItem
            {
                Parent = manifestValue,
                ValidatedType = typeof(ComplexObject),
            };
            var grandchildManifest = new ManifestItem
            {
                Parent = childManifest,
                ValidatedType = typeof(string),
            };
            manifestValue.Children.Add(childManifest);
            childManifest.Children.Add(grandchildManifest);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == manifestValue)))
                .Returns(val);
            val.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(validatedValue);
            val.ManifestValue = manifestValue;
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest)))
                .Returns(childVal);
            childVal.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(childValue);
            childVal.ManifestValue = childManifest;;
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == grandchildManifest)))
                .Returns(grandchildVal);
            grandchildVal.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(grandchildValue);
            grandchildVal.ManifestValue = grandchildManifest;
            object child = childValue;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(childManifest, validatedValue, validationOptions))
                .Returns(new SuccessfulGetValueToBeValidatedResponse(child));
            object grandchild = grandchildValue;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(grandchildManifest, childValue, validationOptions))
                .Returns(new SuccessfulGetValueToBeValidatedResponse(grandchild));

            var result = sut.GetValidatedValue(manifestValue, validatedValue, validationOptions);

            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == childManifest && b.GetActualValue() == child)), Times.Once);
            Mock.Get(valueFromBasisFactory)
                .Verify(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == grandchildManifest && b.GetActualValue() == grandchild)), Times.Once);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldBeAbleToProcessACollectionWithinACollection([Frozen] IGetsValidatedValueFromBasis valueFromBasisFactory,
                                                                                                    [Frozen] IGetsValueToBeValidated valueProvider,
                                                                                                    [Frozen] IGetsEnumerableItemsToBeValidated enumerableProvider,
                                                                                                    ValidatedValueFactory sut,
                                                                                                    [NoAutoProperties] ComplexObject validatedValue,
                                                                                                    ResolvedValidationOptions validationOptions,
                                                                                                    [ExecutableModel] ValidatedValue val,
                                                                                                    [ExecutableModel] ValidatedValue firstCollection,
                                                                                                    [ExecutableModel] ValidatedValue secondCollection,
                                                                                                    [ExecutableModel] ValidatedValue item)
        {
            var manifestValue = new ManifestItem
            {
                ValidatedType = typeof(ComplexObject),
                Children = new [] {
                    new ManifestItem
                    {
                        ValidatedType = typeof(ICollection<List<ComplexObject>>),
                        MemberName = nameof(ComplexObject.DoubleCollection),
                        AccessorFromParent = obj => ((ComplexObject) obj).DoubleCollection,
                        CollectionItemValue = new ManifestItem
                        {
                            ValidatedType = typeof(List<ComplexObject>),
                            ItemType = ManifestItemTypes.CollectionItem,
                            CollectionItemValue = new ManifestItem
                            {
                                ValidatedType = typeof(ComplexObject),
                                ItemType = ManifestItemTypes.CollectionItem,
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
            val.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(validatedValue);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == child)))
                .Returns(firstCollection);
            firstCollection.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(validatedValue.DoubleCollection);
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == child.CollectionItemValue)))
                .Returns(secondCollection);
            secondCollection.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(validatedValue.DoubleCollection.First());
            Mock.Get(valueFromBasisFactory)
                .Setup(x => x.GetValidatedValue(It.Is<ValidatedValueBasis>(b => b.ManifestValue == child.CollectionItemValue.CollectionItemValue)))
                .Returns(item);
            item.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(validatedValue.DoubleCollection.First().First());
            object validatedVal = validatedValue;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(manifestValue, validatedValue, validationOptions))
                .Returns(new SuccessfulGetValueToBeValidatedResponse(validatedValue));
            object doubleCollection = firstCollection.ValueResponse;
            Mock.Get(valueProvider)
                .Setup(x => x.GetValueToBeValidated(child, validatedValue, validationOptions))
                .Returns(new SuccessfulGetValueToBeValidatedResponse(doubleCollection));
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